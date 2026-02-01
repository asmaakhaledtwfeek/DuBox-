using Dubox.Domain.Entities;
using Dubox.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Application.Specifications
{
    public class GetFactoryByIdwithIncludesSpecification:Specification<Factory>
    {
        public GetFactoryByIdwithIncludesSpecification(Guid factoryId)
        {
            AddCriteria(f => f.FactoryId == factoryId);
        }
    }
}
