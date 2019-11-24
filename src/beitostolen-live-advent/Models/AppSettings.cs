using System;
namespace beitostolen_live_api.Models
{
    public class AppSettings
    {
        public string DefaultConnection { get; set; }
        public bool TestingMode { get; set; }
        public string BaseUrl { get; set; }
    }
}
