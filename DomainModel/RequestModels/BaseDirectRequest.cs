using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainModel.RequestModels
{
    public abstract class BaseDirectRequest
    {
        public string PspId { get; set; }
        public string UserId { get; set; }
        public string Pswd { get; set; }
        public string PayId { get; set; }
        public string OrderId { get; set; }
    }
}
