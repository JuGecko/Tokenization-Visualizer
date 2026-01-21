using Microsoft.AspNetCore.Mvc;
using TokenizerApp.Models;
using System.Text;
using System.Text.Json;

namespace TokenizerApp.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult AboutTokenization()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Index(TokenRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Assuming API is running at localhost:8000
        var response = await httpClient.PostAsync("http://localhost:8000/tokenize", content);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "API Error");
            return View(request);
        }

        var resultJson = await response.Content.ReadAsStringAsync();

        // Case insensitive is safer
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var result = JsonSerializer.Deserialize<TokenResponse>(resultJson, options);

        // Pass data to View
        ViewBag.Tokens = result?.Tokens;
        ViewBag.TokenIds = result?.Ids;

        return View(request);
    }
}