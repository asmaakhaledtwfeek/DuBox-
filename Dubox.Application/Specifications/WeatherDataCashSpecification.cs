using Dubox.Domain.Entities;
using Dubox.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dubox.Application.Specifications
{
    public  class WeatherDataCashSpecification:Specification<WeatherDataCache>
    {
        public WeatherDataCashSpecification(decimal roundedLon,decimal roundedLat)
        {
            AddCriteria(c => c.Latitude == (decimal)roundedLat && c.Longitude == (decimal)roundedLon);
            AddOrderByDescending(c => c.LastUpdated);
        }
    }
}
