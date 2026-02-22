namespace Dubox.Api.Configurations;

/// <summary>
/// Configuration for Google Gemini AI service
/// </summary>
public class GeminiConfig
{
    public const string Section = "GoogleGemini";

    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "gemini-2.0-flash-exp";
    public int MaxTokens { get; set; } = 8000;
}






