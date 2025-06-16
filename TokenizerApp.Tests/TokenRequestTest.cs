// Testy domy�lnych warto�ci i poprawno�ci ustawiania w�a�ciwo�ci.

using Xunit;
using TokenizerApp.Controllers;
using TokenizerApp.Models;

public class TokenRequestTest
{
	[Fact]
	public void Model_DefaultsToBert()
	{
		var req = new TokenRequest();
		Assert.Equal("bert", req.Model);
	}

	[Fact]
	public void CanSetTextAndModel()
	{
		var req = new TokenRequest { Text = "abc", Model = "other" };
		Assert.Equal("abc", req.Text);
		Assert.Equal("other", req.Model);
	}
}
