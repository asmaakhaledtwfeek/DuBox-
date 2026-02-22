using Dubox.Domain.Entities;
using Dubox.Domain.Specification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Application.Specifications
{
    public class ChecklistSpecification:Specification<Checklist>
    {
        public ChecklistSpecification()
        {
            AddCriteria(ck => ck.WIRCode != null && ck.IsActive);
            AddInclude(nameof(Checklist.Sections));
            AddInclude($"{nameof(Checklist.Sections)}.{nameof(ChecklistSection.Items)}");
            AddOrderBy(c => c.WIRCode);
            AddOrderBy(c => c.Name);

        }
        public ChecklistSpecification(string code , string targetWIRCode)
        {
            AddCriteria(c => c.Code ==code && c.WIRCode == targetWIRCode);
            AddInclude(nameof(Checklist.Sections));
            AddInclude($"{nameof(Checklist.Sections)}.{nameof(ChecklistSection.Items)}");
           
        }
        public ChecklistSpecification(string wircode)
        {
            AddCriteria(c=> c.WIRCode == wircode);
            AddInclude(nameof(Checklist.Sections));
            AddInclude($"{nameof(Checklist.Sections)}.{nameof(ChecklistSection.Items)}");

        }
    }
}
