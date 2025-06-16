// Testy poprawnoœci ustawiania i odczytu listy tokenów.
using Xunit;
using TokenizerApp.Controllers;
using TokenizerApp.Models;

public class TokenResponseTest
{
    [Fact]
    public void CanSetTokens()
    {
        var resp = new TokenResponse { Tokens = new System.Collections.Generic.List<string> { "a", "b" } };
        Assert.Equal(new[] { "a", "b" }, resp.Tokens);
    }
}