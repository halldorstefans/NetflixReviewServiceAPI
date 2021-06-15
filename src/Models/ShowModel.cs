using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NetflixReviewService.Models
{
    public class ShowModel
    {        
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("director")]
        public string Director { get; set; }
        
        [JsonPropertyName("cast")]
        public string Cast { get; set; }
        
        [JsonPropertyName("country")]
        public string Country { get; set; }
        
        [JsonPropertyName("dataAdded")]
        public string DateAdded { get; set; }
        
        [JsonPropertyName("releaseYear")]
        public int ReleaseYear { get; set; }
        
        [JsonPropertyName("rating")]
        public string Rating { get; set; }
        
        [JsonPropertyName("duration")]
        public string Duration { get; set; }
        
        [JsonPropertyName("listedIn")]
        public string ListedIn { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        public List<ShowReviewModel> Reviews { get; set; }
    }
}