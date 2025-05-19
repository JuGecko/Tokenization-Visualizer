namespace TokenizerApp.Models;
public class TokenRequest
{
	public string Text { get; set; }
	public string Model { get; set; } = "bert";
}

public class TokenResponse
{
	public List<string> Tokens { get; set; }
}
