using System.Text.Json.Serialization;

namespace Domain;

public class Photo
{
    public string PhotoId { get; set; }

    public string Url { get; set; }

    public bool IsMain { get; set; }

    [JsonIgnore]
    public AppUser User { get; set; }

    [JsonIgnore]
    public string UserId { get; set; }
}
