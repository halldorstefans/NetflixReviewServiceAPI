using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NetflixReviewService.Models
{
    public class NetflixData
    { 
        [JsonPropertyName("data")]
        public List<ShowModel> Data { get; set; }
    }
}
