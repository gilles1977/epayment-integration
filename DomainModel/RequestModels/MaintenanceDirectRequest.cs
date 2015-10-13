using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainModel.RequestModels
{
    public class MaintenanceDirectRequest : BaseDirectRequest
    {
        public string Amount { get; set; }
        public string Operation { get; set; }
    }
}
