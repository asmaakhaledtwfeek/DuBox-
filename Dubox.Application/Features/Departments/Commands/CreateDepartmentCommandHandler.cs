using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using Dubox.Domain.Shared;
using MapsterMapper;
using MediatR;

namespace Dubox.Application.Features.Departments.Commands
{
    public class CreateDepartmentCommandHandler
     : IRequestHandler<CreateDepartmentCommand, Result<DepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDepartmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<DepartmentDto>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var nameExists = await _unitOfWork.Repository<Department>()
            .IsExistAsync(d => d.DepartmentName.ToLower() == request.DepartmentName.ToLower(), cancellationToken);

            if (nameExists)
                return Result.Failure<DepartmentDto>($"Department name '{request.DepartmentName}' already exists.");

            var codeExists = await _unitOfWork.Repository<Department>()
              .IsExistAsync(d => d.Code.ToLower() == request.Code.ToLower(), cancellationToken);

            if (codeExists)
                return Result.Failure<DepartmentDto>($"Department code '{request.Code}' already exists.");

            var department = _mapper.Map<Department>(request);

            department.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<Department>().AddAsync(department, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var dto = _mapper.Map<DepartmentDto>(department);

            return Result.Success(dto);
        }
    }
}
