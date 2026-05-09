using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // 加入此行
using ClosedXML.Excel;

namespace RAMMonitor // 建議與其他類別同命名空間
{
    public class ExcelRepository
    {
        private readonly string _filePath;

        public ExcelRepository(string filePath)
        {
            _filePath = filePath;
        }

        public void Append(
            Guid runId,
            int sampleIndex,
            DateTime timestamp,
            double elapsedSeconds,
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
                    wsSys.Cell(1, 1).Value = "RunId";
                    wsSys.Cell(1, 2).Value = "SampleIndex";
                    wsSys.Cell(1, 3).Value = "Timestamp";
                    wsSys.Cell(1, 4).Value = "ElapsedSeconds";
                    wsSys.Cell(1, 5).Value = "TotalMemoryMB";
                    wsSys.Cell(1, 6).Value = "UsedMemoryMB";
                    wsSys.Cell(1, 7).Value = "FreeMemoryMB";
                }
                int sysRow = wsSys.LastRowUsed()?.RowNumber() ?? 1;
                wsSys.Cell(sysRow + 1, 1).Value = runId.ToString();
                wsSys.Cell(sysRow + 1, 2).Value = sampleIndex;
                wsSys.Cell(sysRow + 1, 3).Value = timestamp;
                wsSys.Cell(sysRow + 1, 4).Value = elapsedSeconds;
                wsSys.Cell(sysRow + 1, 5).Value = sysMem.TotalMemoryMB;
                wsSys.Cell(sysRow + 1, 6).Value = sysMem.UsedMemoryMB;
                wsSys.Cell(sysRow + 1, 7).Value = sysMem.FreeMemoryMB;

                // TopProcessLog
                var wsProc = wb.Worksheets.FirstOrDefault(ws => ws.Name == "TopProcessLog") ??
                             wb.AddWorksheet("TopProcessLog");
                if (wsProc.LastRowUsed() == null)
                {
                    wsProc.Cell(1, 1).Value = "RunId";
                    wsProc.Cell(1, 2).Value = "SampleIndex";
                    wsProc.Cell(1, 3).Value = "Timestamp";
                    wsProc.Cell(1, 4).Value = "Rank";
                    wsProc.Cell(1, 5).Value = "ProcessName";
                    wsProc.Cell(1, 6).Value = "PID";
                    wsProc.Cell(1, 7).Value = "MemoryMB";
                }
                int procRow = wsProc.LastRowUsed()?.RowNumber() ?? 1;
                for (int i = 0; i < topProcesses.Count; i++)
                {
                    wsProc.Cell(procRow + 1 + i, 1).Value = runId.ToString();
                    wsProc.Cell(procRow + 1 + i, 2).Value = sampleIndex;
                    wsProc.Cell(procRow + 1 + i, 3).Value = timestamp;
                    wsProc.Cell(procRow + 1 + i, 4).Value = i + 1;
                    wsProc.Cell(procRow + 1 + i, 5).Value = topProcesses[i].ProcessName;
                    wsProc.Cell(procRow + 1 + i, 6).Value = topProcesses[i].PID;
                    wsProc.Cell(procRow + 1 + i, 7).Value = topProcesses[i].MemoryMB;
                }

                wb.SaveAs(_filePath);
            }
        }
    }
}