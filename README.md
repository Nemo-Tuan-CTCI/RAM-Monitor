# RAM Monitor

Windows Forms 記憶體監控工具（.NET Framework 4.8），可在指定時長內定期取樣系統記憶體與高記憶體程序，並將結果持續追加到 Excel 檔案。

## 功能摘要

- 輸入監控持續時間（秒數，或 `mm:ss` 格式）
- 輸入取樣間隔秒數（空白預設 1 秒）
- 每次取樣記錄：
  - 系統總記憶體（MB）
  - 系統可用記憶體（MB）
  - 記憶體用量最高前 10 個程序（名稱、PID、用量）
- 監控結束後顯示寫入筆數與輸出檔案位置
- Excel 採用追加寫入（不覆蓋舊資料）

## 技術與相依套件

- 語言與框架：C# / .NET Framework 4.8
- UI：Windows Forms
- Excel：ClosedXML
- 系統資訊來源：
  - `Microsoft.VisualBasic.Devices.ComputerInfo`（實體記憶體）
  - `System.Diagnostics.Process`（程序記憶體）

主要套件版本請參考 `packages.config`。

## 專案結構（核心檔案）

- `Program.cs`：程式進入點，啟動 `Form1`
- `Form1.cs`：UI 事件與啟動監控流程
- `InputParser.cs`：輸入解析（持續時間、間隔）
- `MemorySampler.cs`：記憶體與程序取樣
- `MonitorService.cs`：監控迴圈、取樣節奏校正
- `ExcelRepository.cs`：Excel 建立與追加寫入

## 執行流程

1. 在 UI 輸入持續時間與取樣間隔
2. 點擊「開始監控」
3. `MonitorService` 在指定時間內重複取樣
4. 每次取樣將資料寫入 `MemoryUsageLog.xlsx`
5. 完成後顯示成功訊息

## 輸出檔案

- 檔名：`MemoryUsageLog.xlsx`
- 位置：程式執行目錄（通常在 `bin/Debug/` 或 `bin/Release/`）

### 工作表 1：SystemMemoryLog

欄位：

- `Timestamp`
- `TotalMemoryMB`
- `FreeMemoryMB`

### 工作表 2：TopProcessLog

欄位：

- `ProcessName`
- `Timestamp`
- `Rank`
- `PID`
- `UsedMemoryMB`

## 建置與執行

### Visual Studio

1. 使用 Visual Studio 開啟 `RAMMonitor.sln`
2. 先還原 NuGet 套件（若尚未自動還原）
3. 建置並執行（F5）

### 命令列（Developer PowerShell）

```powershell
nuget restore RAMMonitor.sln
msbuild RAMMonitor.sln /p:Configuration=Release
```

執行檔預設在 `bin/Release/`。

## 錯誤與例外處理

- 輸入格式錯誤會跳出錯誤訊息
- 若 Excel 檔案被鎖定或寫入失敗，會顯示「Excel 寫入失敗」訊息
- 無法讀取的程序（權限不足或程序已結束）會略過，不中斷整體監控

## 已知限制

- 目前 `SystemMemoryLog` 未寫入 `UsedMemoryMB` 欄位（程式可計算，但尚未輸出該欄）
- 每次按下開始都寫入同一份 `MemoryUsageLog.xlsx`
- 監控執行時按鈕會停用，避免重複啟動

## 後續建議

- 在 `SystemMemoryLog` 增加 `UsedMemoryMB` 欄位
- 新增執行批次識別（RunId）與取樣序號（SampleIndex）
- 新增 CPU / 磁碟 I/O 指標
- 提供 CSV 匯出與圖表頁
