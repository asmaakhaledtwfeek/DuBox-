using Dubox.Domain.Shared;
using MediatR;

namespace Dubox.Application.Features.Boxes.Queries;

public record GenerateBoxesTemplateQuery() : IRequest<Result<byte[]>>;

