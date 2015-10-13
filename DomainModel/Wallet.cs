using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DomainModel
{
    [DataContract]
    public class Wallet
    {
        [DataMember]
        public string WalletId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string CardHolderName { get; set; }
        [DataMember]
        public string PaymentMethod { get; set; }
        [DataMember]
        public string Brand { get; set; }
        [DataMember]
        public string HiddenCardNumber { get; set; }
        [DataMember]
        public string ExpiryDate { get; set; }
    }
}
