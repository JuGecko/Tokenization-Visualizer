// Testy w³aœciwoœci ShowRequestId dla ró¿nych wartoœci RequestId.

using Xunit;
using TokenizerApp.Controllers;
using TokenizerApp.Models;

public class ErrorViewModelTest
{
    [Fact]
    public void ShowRequestId_ReturnsTrue_WhenRequestIdIsNotNullOrEmpty()
    {
        var model = new ErrorViewModel { RequestId = "abc" };
        Assert.True(model.ShowRequestId);
    }

    [Fact]
    public void ShowRequestId_ReturnsFalse_WhenRequestIdIsNull()
    {
        var model = new ErrorViewModel { RequestId = null };
        Assert.False(model.ShowRequestId);
    }

    [Fact]
    public void ShowRequestId_ReturnsFalse_WhenRequestIdIsEmpty()
    {
        var model = new ErrorViewModel { RequestId = "" };
        Assert.False(model.ShowRequestId);
    }
}