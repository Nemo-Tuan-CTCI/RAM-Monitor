using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualBasic.Devices;

public class SystemMemoryInfo
{
    public double TotalMemoryMB { get; set; }
    public double UsedMemoryMB { get; set; }
    public double FreeMemoryMB { get; set; }
}

public class ProcessMemoryInfo
{
    public string ProcessName { get; set; }
    public int PID { get; set; }
    public double MemoryMB { get; set; }
}

public static class MemorySampler
{
    public static SystemMemoryInfo GetSystemMemory()
    {
        var ci = new ComputerInfo();
        double total = ci.TotalPhysicalMemory / 1024.0 / 1024.0;
        double available = ci.AvailablePhysicalMemory / 1024.0 / 1024.0;
        return new SystemMemoryInfo
        {
            TotalMemoryMB = total,
            UsedMemoryMB = total - available,
            FreeMemoryMB = available
        };
    }

    public static List<ProcessMemoryInfo> GetTopProcesses(int topN = 10)
    {
        var list = new List<ProcessMemoryInfo>();
        foreach (var proc in Process.GetProcesses())
        {
            try
            {
                list.Add(new ProcessMemoryInfo
                {
                    ProcessName = proc.ProcessName,
                    PID = proc.Id,
                    MemoryMB = proc.WorkingSet64 / 1024.0 / 1024.0
                });
            }
            catch
            {
                // 權限不足或程序已結束，忽略
            }
        }
        return list.OrderByDescending(p => p.MemoryMB).Take(topN).ToList();
    }
}