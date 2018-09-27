using ServiceInterfaces.ViewModels.Common.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Cities
{
    public class SyncCityRequest
    {
        public DateTime? LastUpdatedAt { get; set; }
        public List<CityViewModel> UnSyncedCities { get; set; }
    }
}
