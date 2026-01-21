using System.Text.Json.Serialization; // Required for the attribute

namespace TokenizerApp.Models;

public class TokenRequest
{
    public string Text { get; set; }
    public string Model { get; set; } = "bert";
}

public class TokenResponse
{
    // Matches "tokens" in JSON
    [JsonPropertyName("tokens")]
    public List<string> Tokens { get; set; }

    // Matches "ids" OR "input_ids" depending on what your API sends. 
    // If your API sends "input_ids", change the string below to "input_ids".
    [JsonPropertyName("ids")]
    public List<int> Ids { get; set; }
}