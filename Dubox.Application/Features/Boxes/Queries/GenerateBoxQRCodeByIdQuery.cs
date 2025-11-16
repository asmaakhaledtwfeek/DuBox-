using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries
{

    public record GenerateBoxQRCodeByIdQuery(Guid BoxId) : IRequest<Result<string>>;
}
