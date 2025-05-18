// Hubs/SystemHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Threading.Tasks;
using TestResourceUsageBySignalR;

public class SystemHub : Hub
{
    private readonly Process _process;
    private readonly int _cpuCount;
    private double _lastCpuTime;

    public SystemHub()
    {
        _process = Process.GetCurrentProcess();
        _cpuCount = Environment.ProcessorCount;
        _lastCpuTime = _process.TotalProcessorTime.TotalMilliseconds;
    }

    public async Task SendSystemInfo()
    {
        // Memory and Threads
        int threadCount = _process.Threads.Count;
        long memoryUsage = _process.WorkingSet64 / (1024 * 1024); // MB

        ThreadPool.GetAvailableThreads(out int workerAvailable, out int ioAvailable);
        ThreadPool.GetMaxThreads(out int maxWorker, out int maxIo);
        int workerThreadsUsed = maxWorker - workerAvailable;
        int ioThreadsUsed = maxIo - ioAvailable;

        // CPU Usage
        double currentCpuTime = _process.TotalProcessorTime.TotalMilliseconds;
        double elapsedTime = 1000; // Interval in milliseconds
        double cpuUsage = ((currentCpuTime - _lastCpuTime) / elapsedTime) / _cpuCount * 100;
        _lastCpuTime = currentCpuTime;

        // Active Requests
        int activeRequests = RequestCounterMiddleware.GetActiveRequests();

        // Send data to clients
        await Clients.All.SendAsync("ReceiveSystemInfo",
            threadCount,
            memoryUsage,
            workerThreadsUsed,
            maxWorker,
            ioThreadsUsed,
            maxIo,
            cpuUsage,
            activeRequests
        );
    }
}


public class SystemHub2 : Hub
{
    private readonly Process _process;

    public SystemHub2()
    {
        _process = Process.GetCurrentProcess();
    }

    public async Task SendSystemInfo2()
    {
        // Memory Usage
        long memoryUsage = _process.WorkingSet64 / (1024 * 1024); // MB

        // Send data to clients
        await Clients.All.SendAsync("ReceiveSystemInfo2", _process.Threads.Count, memoryUsage);
    }
}
