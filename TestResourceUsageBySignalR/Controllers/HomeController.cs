using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestResourceUsageBySignalR.Models;

namespace TestResourceUsageBySignalR.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;

    public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Index2()
    {
        return View();
    }



    public IActionResult Privacy()
    {

        Task.Run(() =>
        {
            Task.Delay(100);
        });

        return View();
    }

    public async Task<IActionResult> CheckGoogle()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("https://www.google.com");
        response.EnsureSuccessStatusCode(); // Throws exception if not 200-299
        string data = await response.Content.ReadAsStringAsync();
         data = await response.Content.ReadAsStringAsync();
         data = await response.Content.ReadAsStringAsync();
         data = await response.Content.ReadAsStringAsync();
         data = await response.Content.ReadAsStringAsync();
         data = await response.Content.ReadAsStringAsync();
         data = await response.Content.ReadAsStringAsync();
        return Json("");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
