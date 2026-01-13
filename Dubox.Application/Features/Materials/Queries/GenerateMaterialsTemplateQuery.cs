using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Materials.Queries;

public record GenerateMaterialsTemplateQuery() : IRequest<Result<byte[]>>;

