// Testy akcji Index() (GET i POST), w tym obs³uga poprawnych i b³êdnych odpowiedzi z API.

using Xunit;
using Moq;
using Moq.Protected;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TokenizerApp.Controllers;
using TokenizerApp.Models;

public class HomeControllerTest
{
    [Fact]
    public void Index_Get_ReturnsViewResult()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var controller = new HomeController(mockFactory.Object);

        var result = controller.Index();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Index_Post_SuccessfulResponse_SetsTokensAndReturnsViewWithModel()
    {
        // Arrange
        var mockFactory = new Mock<IHttpClientFactory>();
        var mockClient = new Mock<HttpMessageHandler>();
        var responseContent = "{\"Tokens\":[\"a\",\"b\"]}";
        mockClient
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            });

        var httpClient = new HttpClient(mockClient.Object);
        mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var controller = new HomeController(mockFactory.Object);
        var request = new TokenRequest { Text = "test", Model = "bert" };

        // Act
        var result = await controller.Index(request);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(request, viewResult.Model);
        Assert.Equal(new[] { "a", "b" }, (System.Collections.Generic.List<string>)controller.ViewBag.Tokens);
    }

    [Fact]
    public async Task Index_Post_FailedResponse_AddsModelErrorAndReturnsView()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var mockClient = new Mock<HttpMessageHandler>();
        mockClient
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var httpClient = new HttpClient(mockClient.Object);
        mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var controller = new HomeController(mockFactory.Object);
        var request = new TokenRequest { Text = "test", Model = "bert" };

        var result = await controller.Index(request);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
    }
}