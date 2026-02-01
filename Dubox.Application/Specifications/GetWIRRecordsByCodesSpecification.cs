using System.Collections.Generic;
using System.Linq;
using Dubox.Domain.Entities;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications;

public class GetWIRRecordsByCodesSpecification : Specification<WIRRecord>
{
    public GetWIRRecordsByCodesSpecification(IEnumerable<string> wirCodes)
    {
        var codes = wirCodes
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Select(code => code.ToLower().Trim())
            .Distinct()
            .ToList();

        AddCriteria(record => codes.Contains(record.WIRCode.ToLower()));
        AddInclude(nameof(WIRRecord.BoxActivity));
        AddInclude($"{nameof(WIRRecord.BoxActivity)}.{nameof(WIRRecord.BoxActivity.Box)}");
    }
}

