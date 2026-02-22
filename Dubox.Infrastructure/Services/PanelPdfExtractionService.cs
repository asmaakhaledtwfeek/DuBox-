using Dubox.Domain.Services;
using Dubox.Domain.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Dubox.Infrastructure.Services;

/// <summary>
/// Service for extracting panel data from PDF files using Google Gemini AI
/// </summary>
public class PanelPdfExtractionService : IPanelPdfExtractionService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PanelPdfExtractionService> _logger;
    private readonly HttpClient _httpClient;

    public PanelPdfExtractionService(
        IConfiguration configuration,
        ILogger<PanelPdfExtractionService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.Timeout = TimeSpan.FromMinutes(2); // AI processing can take time
    }

    public async Task<Result<PanelExtractionResult>> ExtractPanelDataFromPdfAsync(
        Stream pdfStream,
        string fileName,
        Stream? boxTagsPdfStream = null,
        string? boxTagsFileName = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Starting PDF extraction for file: {fileName}");

            // Get configuration
            var apiKey = _configuration["GoogleGemini:ApiKey"];
            var model = _configuration["GoogleGemini:Model"] ?? "gemini-2.0-flash-exp";

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return Result.Failure<PanelExtractionResult>("Google Gemini API key not configured");
            }

            // Read PDF bytes
            pdfStream.Position = 0;
            byte[] pdfBytes;
            using (var memoryStream = new MemoryStream())
            {
                await pdfStream.CopyToAsync(memoryStream, cancellationToken);
                pdfBytes = memoryStream.ToArray();
            }

            _logger.LogInformation($"PDF file size: {pdfBytes.Length} bytes");

            // Convert to base64
            var base64Pdf = Convert.ToBase64String(pdfBytes);

            // Read second PDF if provided
            byte[]? boxTagsPdfBytes = null;
            string? base64BoxTagsPdf = null;
            if (boxTagsPdfStream != null)
            {
                _logger.LogInformation($"Starting box tags extraction for file: {boxTagsFileName}");
                boxTagsPdfStream.Position = 0;
                using (var memoryStream = new MemoryStream())
                {
                    await boxTagsPdfStream.CopyToAsync(memoryStream, cancellationToken);
                    boxTagsPdfBytes = memoryStream.ToArray();
                }
                _logger.LogInformation($"Box tags PDF file size: {boxTagsPdfBytes.Length} bytes");
                base64BoxTagsPdf = Convert.ToBase64String(boxTagsPdfBytes);
            }

            // Prepare Gemini API request
            var prompt = @"You are an expert estimation engineer analyzing precast concrete panel fabrication drawings.

**TASK**: Carefully examine this engineering drawing and extract ALL panel specifications.

**STEP-BY-STEP INSTRUCTIONS**:

1. **Locate the Element Tag Table**: Look for a table labeled ""ELEMENT TAG"", ""Element Tag"", ""PANEL SCHEDULE"", or similar. This table typically contains:
   - Panel ID/Tag (e.g., IW-230-1, SW-100-1, FF-S4-A, MGMT-GF-B1)
   - Volume (m³)
   - Weight (ton or kg - convert kg to tons by dividing by 1000)
   - Concrete Grade (e.g., C32/40, C40/50)
   - Cover (mm)
   - Quantity or QTY

2. **Locate the Embeds Table**: Look for a table labeled ""EMBEDS"", ""EMBECS"", ""HARDWARE"", or similar containing:
   - Symbol/Mark (e.g., F50, SLV, IN40)
   - Description (e.g., ""F50 FOR LIFTING 200"", ""50mm SLEEVE"")
   - Quantity per panel

3. **Locate each Box**: Look for Boxes, and see related Panel Ids/Tags, and add boxes related to each Panel in the list of Boxes

3. **Extract data for EACH panel found** and return in this EXACT JSON format:

{
  ""panels"": [
    {
      ""tag"": ""IW-230-1"",
      ""volume_m3"": 3.02,
      ""weight_ton"": 7.57,
      ""concrete_grade"": ""C32/40"",
      ""cover_mm"": 25,
      ""quantity_in_this_box"": 2,
      ""embeds"": [
        {""symbol"": ""F50"", ""description"": ""F50 FOR LIFTING 200"", ""qty"": 2},
        {""symbol"": ""SLV"", ""description"": ""50mm SLEEVE"", ""qty"": 2}
      ]
    }
  ]
}

**CRITICAL RULES**:
✓ Look for tables anywhere in the drawing - they may be in corners, margins, or separate pages
✓ Extract ALL panels - there may be 1 to 20+ panels per drawing
✓ Panel tags are usually alphanumeric codes like: IW-230-1, SW-100-1, FF-S4-A, MGMT-GF-B1-1, etc.
✓ If weight is in kg, divide by 1000 to convert to tons
✓ If a field is truly not present in the drawing, use null (not empty string)
✓ If quantity is not specified, use 1
✓ Return ONLY valid JSON - no markdown blocks, no explanations, no extra text
✓ Embeds array can be empty [] if no embeds table is found

**IMPORTANT**: Read the drawing carefully. The tables contain critical data. Do not return empty panels.";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { text = prompt },
                            new
                            {
                                inline_data = new
                                {
                                    mime_type = "application/pdf",
                                    data = base64Pdf
                                }
                            }
                        }
                    }
                }
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            _logger.LogInformation($"Calling Gemini API with model: {model}");

            // Create tasks for parallel execution
            var panelExtractionTask = CallGeminiApiAsync(url, content, cancellationToken);
            
            Task<HttpResponseMessage>? boxTagsExtractionTask = null;
            if (base64BoxTagsPdf != null)
            {
                var boxTagsPrompt = @"Analyze this architectural floor plan.
Please extract the unique module identification tags from the floor plan.

module identification tags are formatted as white text on a black rectangular background.
take only the tags that are above/below red outline rectangles.

Return the result strictly as a JSON array in this exact format:
[""158-FF-S1"", ""158-FF-S4-A"", ""158-FF-S4-C""]

IMPORTANT: Return ONLY the JSON array - no markdown blocks, no explanations, no extra text.";

                var boxTagsRequestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new object[]
                            {
                                new { text = boxTagsPrompt },
                                new
                                {
                                    inline_data = new
                                    {
                                        mime_type = "application/pdf",
                                        data = base64BoxTagsPdf
                                    }
                                }
                            }
                        }
                    }
                };

                var boxTagsJsonRequest = JsonSerializer.Serialize(boxTagsRequestBody);
                var boxTagsContent = new StringContent(boxTagsJsonRequest, Encoding.UTF8, "application/json");
                
                _logger.LogInformation("Calling Gemini API for box tags extraction");
                boxTagsExtractionTask = CallGeminiApiAsync(url, boxTagsContent, cancellationToken);
            }

            // Wait for all tasks to complete
            HttpResponseMessage response;
            HttpResponseMessage? boxTagsResponse = null;

            if (boxTagsExtractionTask != null)
            {
                var responses = await Task.WhenAll(panelExtractionTask, boxTagsExtractionTask);
                response = responses[0];
                boxTagsResponse = responses[1];
            }
            else
            {
                response = await panelExtractionTask;
            }

            // Process panel extraction response
            var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Gemini API error: {response.StatusCode} - {responseText}");
                return Result.Failure<PanelExtractionResult>($"AI extraction failed: {response.StatusCode}");
            }

            _logger.LogInformation("Gemini API response received, parsing...");

            // Parse Gemini response
            var textResponse = ParseGeminiResponse(responseText);
            
            if (string.IsNullOrWhiteSpace(textResponse))
            {
                return Result.Failure<PanelExtractionResult>("AI returned empty response");
            }

            _logger.LogInformation($"AI response text: {textResponse.Substring(0, Math.Min(500, textResponse.Length))}...");

            // Clean up response (remove markdown code blocks if present)
            textResponse = CleanJsonResponse(textResponse);

            // Parse extracted data
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var extractedData = JsonSerializer.Deserialize<ExtractedDataWrapper>(textResponse, options);

            if (extractedData?.Panels == null || !extractedData.Panels.Any())
            {
                return Result.Failure<PanelExtractionResult>("No panels found in the drawing");
            }

            var result = new PanelExtractionResult
            {
                Panels = extractedData.Panels,
                Warnings = new List<string>(),
                BoxTags = new List<string>()
            };

            // Process box tags response if available
            if (boxTagsResponse != null)
            {
                var boxTagsResponseText = await boxTagsResponse.Content.ReadAsStringAsync(cancellationToken);
                
                if (boxTagsResponse.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Box tags Gemini API response received, parsing...");
                    
                    var boxTagsTextResponse = ParseGeminiResponse(boxTagsResponseText);
                    
                    if (!string.IsNullOrWhiteSpace(boxTagsTextResponse))
                    {
                        boxTagsTextResponse = CleanJsonResponse(boxTagsTextResponse);
                        
                        // Parse as JSON array
                        var boxTags = JsonSerializer.Deserialize<List<string>>(boxTagsTextResponse, options);
                        
                        if (boxTags != null && boxTags.Any())
                        {
                            result.BoxTags = boxTags;
                            
                            // Serialize and print the result
                            var serializedBoxTags = JsonSerializer.Serialize(boxTags, new JsonSerializerOptions 
                            { 
                                WriteIndented = true 
                            });
                            _logger.LogInformation($"Box Tags Extraction Result:\n{serializedBoxTags}");
                            Console.WriteLine($"Box Tags Extraction Result:\n{serializedBoxTags}");
                        }
                    }
                }
                else
                {
                    _logger.LogError($"Box tags Gemini API error: {boxTagsResponse.StatusCode} - {boxTagsResponseText}");
                    result.Warnings.Add("Box tags extraction failed");
                }
            }

            // Validate and add warnings
            foreach (var panel in result.Panels)
            {
                if (string.IsNullOrWhiteSpace(panel.Tag))
                {
                    result.Warnings.Add("Panel found without tag/ID");
                }
                if (!panel.VolumeM3.HasValue)
                {
                    result.Warnings.Add($"Panel {panel.Tag}: Volume not found");
                }
                if (!panel.WeightTon.HasValue)
                {
                    result.Warnings.Add($"Panel {panel.Tag}: Weight not found");
                }
            }

            _logger.LogInformation($"Successfully extracted {result.Panels.Count} panel(s) with {result.Warnings.Count} warning(s)");

            return Result.Success(result);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse AI response as JSON");
            return Result.Failure<PanelExtractionResult>($"Failed to parse AI response: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error extracting panel data from PDF: {fileName}");
            return Result.Failure<PanelExtractionResult>($"Extraction error: {ex.Message}");
        }
    }

    private async Task<HttpResponseMessage> CallGeminiApiAsync(
        string url, 
        StringContent content, 
        CancellationToken cancellationToken)
    {
        return await _httpClient.PostAsync(url, content, cancellationToken);
    }

    private string? ParseGeminiResponse(string responseText)
    {
        var geminiResponse = JsonNode.Parse(responseText);
        var candidates = geminiResponse?["candidates"]?.AsArray();
        
        if (candidates == null || candidates.Count == 0)
        {
            return null;
        }

        return candidates[0]?["content"]?["parts"]?[0]?["text"]?.ToString();
    }

    private string CleanJsonResponse(string textResponse)
    {
        textResponse = textResponse.Trim();
        if (textResponse.StartsWith("```json"))
        {
            textResponse = textResponse.Substring(7);
        }
        if (textResponse.StartsWith("```"))
        {
            textResponse = textResponse.Substring(3);
        }
        if (textResponse.EndsWith("```"))
        {
            textResponse = textResponse.Substring(0, textResponse.Length - 3);
        }
        return textResponse.Trim();
    }

    private class ExtractedDataWrapper
    {
        [System.Text.Json.Serialization.JsonPropertyName("panels")]
        public List<ExtractedPanelData> Panels { get; set; } = new();
    }
}

