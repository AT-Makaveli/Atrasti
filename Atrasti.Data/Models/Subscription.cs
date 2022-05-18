namespace Atrasti.Data.Models
{
    public class Subscription
    {
        public string PlaceHolder { get; set; }

        public int CompanyId { get; set; }

        public string Data { get; set; }

        public AtrastiUser User { get; set; }
    }
}
