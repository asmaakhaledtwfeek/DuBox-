using Dubox.Application.DTOs;
using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

/// <summary>
/// Query to get QR code image along with the public URL for a box
/// </summary>
public record GetBoxQRCodeWithUrlQuery(Guid BoxId) : IRequest<Result<BoxQRCodeDto>>;

