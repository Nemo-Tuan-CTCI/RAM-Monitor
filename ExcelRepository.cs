using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;

public class ExcelRepository
{
    private readonly string _filePath;

    public ExcelRepository(string filePath)
    {
        _filePath = filePath;
    }

    public void Append(
        DateTime timestamp,
        SystemMemoryInfo sysMem,
        List<ProcessMemoryInfo> topProcesses)
    {
        bool fileExists = File.Exists(_filePath);
        using (var wb = fileExists ? new XLWorkbook(_filePath) : new XLWorkbook())
        {
            // SystemMemoryLog
            var wsSys = wb.Worksheets.FirstOrDefault(ws => ws.Name == "SystemMemoryLog") ??
                        wb.AddWorksheet("SystemMemoryLog");
            if (wsSys.LastRowUsed() == null)
            {
                wsSys.Cell(1, 1).Value = "Timestamp";
                wsSys.Cell(1, 2).Value = "TotalMemoryMB";
                wsSys.Cell(1, 3).Value = "FreeMemoryMB";
            }
            int sysRow = wsSys.LastRowUsed()?.RowNumber() ?? 1;
            wsSys.Cell(sysRow + 1, 1).Value = timestamp.ToString("yyyy/MM/dd HH:mm:ss");
            wsSys.Cell(sysRow + 1, 2).Value = sysMem.TotalMemoryMB;
            wsSys.Cell(sysRow + 1, 3).Value = sysMem.FreeMemoryMB;

            // TopProcessLog
            var wsProc = wb.Worksheets.FirstOrDefault(ws => ws.Name == "TopProcessLog") ??
                         wb.AddWorksheet("TopProcessLog");
            if (wsProc.LastRowUsed() == null)
            {
                wsProc.Cell(1, 1).Value = "ProcessName";
                wsProc.Cell(1, 2).Value = "Timestamp";
                wsProc.Cell(1, 3).Value = "Rank";
                wsProc.Cell(1, 4).Value = "PID";
                wsProc.Cell(1, 5).Value = "UsedMemoryMB";
            }
            int procRow = wsProc.LastRowUsed()?.RowNumber() ?? 1;
            for (int i = 0; i < topProcesses.Count; i++)
            {
                wsProc.Cell(procRow + 1 + i, 1).Value = topProcesses[i].ProcessName;
                wsProc.Cell(procRow + 1 + i, 2).Value = timestamp.ToString("yyyy/MM/dd HH:mm:ss");
                wsProc.Cell(procRow + 1 + i, 3).Value = i + 1;
                wsProc.Cell(procRow + 1 + i, 4).Value = topProcesses[i].PID;
                wsProc.Cell(procRow + 1 + i, 5).Value = topProcesses[i].MemoryMB;
            }

            wb.SaveAs(_filePath);
        }
    }
}
