using System.Xml.Serialization;

namespace DomainModel.RequestModels
{
    [XmlRoot("ncresponse")]
    public class OrderQueryResponse
    {
        [XmlAttribute("orderID")]
        public string OrderId { get; set; }
        [XmlAttribute("PAYID")]
        public string PayId { get; set; }
        [XmlAttribute("PAYIDSUB")]
        public string PaySubId { get; set; }
        [XmlAttribute("NCERROR")]
        public string NcError { get; set; }
        [XmlAttribute("NCERRORPLUS")]
        public string NcErrorPlus { get; set; }
        [XmlAttribute("NCSTATUS")]
        public string NcStatus { get; set; }
        [XmlAttribute("ACCEPTANCE")]
        public string Acceptance { get; set; }
        [XmlAttribute("STATUS")]
        public string Status { get; set; }
        [XmlAttribute("ECI")]
        public string Eci { get; set; }
        [XmlAttribute("amount")]
        public string Amount { get; set; }
        [XmlAttribute("currency")]
        public string Currency { get; set; }
        [XmlAttribute("PM")]
        public string Pm { get; set; }
        [XmlAttribute("BRAND")]
        public string Brand { get; set; }
        [XmlAttribute("CARDNO")]
        public string CardNo { get; set; }
        [XmlAttribute("IP")]
        public string Ip { get; set; }
    }
}
