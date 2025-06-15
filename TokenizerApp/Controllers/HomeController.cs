using Microsoft.AspNetCore.Mvc;
using TokenizerApp.Models;
using System.Text;
using System.Text.Json;
using System.Net.Http;

namespace TokenizerApp.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Index(TokenRequest request)
    {
        // Get an HttpClient instance from the factory
        var httpClient = _httpClientFactory.CreateClient();

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Use the HttpClient instance
        var response = await httpClient.PostAsync("http://localhost:8000/tokenize", content);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Error contacting tokenizer API.");
            return View();
        }

        var resultJson = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TokenResponse>(resultJson);

        ViewBag.Tokens = result?.Tokens;
        return View(request);
    }
}