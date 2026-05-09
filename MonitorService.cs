using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public class MonitorService
{
    private readonly ExcelRepository _repo;
    private readonly int _duration;
    private readonly int _interval;

    public MonitorService(ExcelRepository repo, int duration, int interval)
    {
        _repo = repo;
        _duration = duration;
        _interval = interval;
    }

    public int Start()
    {
        var runId = Guid.NewGuid();
        int sampleIndex = 0;
        var sw = Stopwatch.StartNew();
        int writeCount = 0;

        while (sw.Elapsed.TotalSeconds < _duration)
        {
            sampleIndex++;
            var timestamp = DateTime.Now;
            var sysMem = MemorySampler.GetSystemMemory();
            var topProcs = MemorySampler.GetTopProcesses(10);

            try
            {
                _repo.Append(
                    timestamp,
                    sysMem,
                    topProcs
                );
                writeCount++;
            }
            catch (Exception ex)
            {
                throw new Exception("Excel 寫入失敗：" + ex.Message);
            }

            // 校正取樣間隔
            double elapsed = sw.Elapsed.TotalSeconds;
            double nextSample = sampleIndex * _interval;
            int sleepMs = (int)((nextSample - elapsed) * 1000);
            if (sleepMs > 0)
                Thread.Sleep(sleepMs);
        }
        return writeCount;
    }
}