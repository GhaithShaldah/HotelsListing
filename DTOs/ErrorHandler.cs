using Newtonsoft.Json;

namespace HotelsListing.DTOs
{
    public class ErrorHandler
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
