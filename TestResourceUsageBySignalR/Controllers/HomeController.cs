using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestResourceUsageBySignalR.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
namespace TestResourceUsageBySignalR.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;
    private readonly Process _process;
    public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
         _process = Process.GetCurrentProcess();

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




    public  IActionResult ShowSystemDetailsInfo()
    {
        var result = new SystemThreadInfoDto();

       // result.Data.ThreadInfoList = new List<ThreadInfoDto>();
        try
        {
            var listThread = new List<ThreadInfoDto>();
            // Memory and Threads
            var threadList = _process.Threads.ToDynamicList<ProcessThread>();
            foreach (ProcessThread threadItem in threadList.Where(x => x.ThreadState == System.Diagnostics.ThreadState.Wait))
            {
                listThread.Add(new ThreadInfoDto
                {
                    ThreadId = threadItem.Id,
                    ThreadState = threadItem.ThreadState.ToString(),
                    WaitReason = threadItem.WaitReason.ToString(),
                    StartTime = threadItem.StartTime.ToString(" HH:mm"),
                    StartTimeEn = threadItem.StartTime,
                });
            }
            int threadCount = _process.Threads.Count;
            long memoryUsage = _process.WorkingSet64 / (1024 * 1024); // MB
            ThreadPool.GetAvailableThreads(out int workerAvailable, out int ioAvailable);
            ThreadPool.GetMaxThreads(out int maxWorker, out int maxIo);
            int workerThreadsUsed = maxWorker - workerAvailable;
            int ioThreadsUsed = maxIo - ioAvailable;
            // CPU Usage
            double currentCpuTime = _process.TotalProcessorTime.TotalMilliseconds;
            double elapsedTime = 1000; // Interval in milliseconds
           // double cpuUsage = ((currentCpuTime - _lastCpuTime) / elapsedTime) / _cpuCount * 100;
           // _lastCpuTime = currentCpuTime;
            var gc = GC.GetTotalMemory(false) / (1024 * 1024);

            //Send data to clients
            //result.Data = new
            //{
            //    threadCount,
            //    memoryUsage,
            //    workerThreadsUsed,
            //    maxWorker,
            //    ioThreadsUsed,
            //    maxIo,
            //    cpuUsage,
            //    gc
            //};
            result.UserThreadCount = listThread.Where(x=>x.WaitReason== "UserRequest").Count();
            result.ThreadInfoList = listThread.OrderByDescending(x=>x.StartTimeEn).ToList();
        }
        catch (Exception ex)
        {
           // result.AddError("error", ex.Message);
        }
        //return await Task.FromResult(result);
       return View(result);
    }

  

}


public class SystemThreadInfoDto
{
    public SystemThreadInfoDto()
    {
        ThreadInfoList = new List<ThreadInfoDto>();
    }
    public List<ThreadInfoDto> ThreadInfoList { get; set; }
    public int UserThreadCount { get; internal set; }
}
public class ThreadInfoDto
{
    public int ThreadId { get; internal set; }
    public string ThreadState { get; internal set; }
    public string WaitReason { get; internal set; }
    public string StartTime { get; internal set; }
    public DateTime StartTimeEn { get; internal set; }
}