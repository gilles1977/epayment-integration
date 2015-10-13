namespace DomainModel.RequestModels
{
    public class OrderQueryRequest
    {
        public string PspId { get; set; }
        public string UserId { get; set; }
        public string Pswd { get; set; }
        public string PayId { get; set; }
        public string OrderId { get; set; }
        public string PayIdSub { get; set; }
    }
}
