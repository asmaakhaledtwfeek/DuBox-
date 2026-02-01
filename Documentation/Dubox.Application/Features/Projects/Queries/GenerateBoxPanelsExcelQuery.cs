using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Projects.Queries;

public record GenerateBoxPanelsExcelQuery(Guid ProjectId) : IRequest<Result<byte[]>>;
