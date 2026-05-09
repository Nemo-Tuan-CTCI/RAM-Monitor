# RAM 監控 EXE 程式規劃（C#）

## 1. 目標
建立一個 Windows 可執行程式（EXE），讓使用者輸入監控時長後，程式能在該時長內持續記錄：

1. 電腦整體記憶體使用量
2. 電腦整體記憶體剩餘量
3. 當下記憶體用量最高的前 10 個程序（含程序名稱、PID、記憶體用量）

所有資料需寫入固定的 Excel 檔案，且每次執行都要保留舊資料（僅新增，不覆蓋）。

---

## 2. 功能需求

### 2.1 使用者輸入
1. 監控時長（建議輸入秒數，或支援 mm:ss 格式）
2. 可選：取樣間隔（預設 1 秒）

### 2.2 監控內容
每次取樣需取得：
1. 取樣時間（Timestamp）
2. 總實體記憶體（MB）
3. 已使用記憶體（MB）
4. 可用/剩餘記憶體（MB）
5. Top 10 程序清單：
   - 程序名稱
   - PID
   - Working Set（MB，建議使用 WorkingSet64）

### 2.3 Excel 寫入規則
1. 固定檔案路徑，例如：
   - MemoryUsageLog.xlsx（放在 EXE 同目錄）
2. 若檔案不存在：自動建立
3. 若檔案已存在：開啟後追加資料（Append）
4. 不刪除、不覆蓋舊紀錄

### 2.4 執行完成
1. 顯示完成訊息（含寫入筆數與檔案位置）
2. 若有錯誤（例如 Excel 被占用）：顯示明確錯誤原因

---

## 3. 資料設計（Excel）
建議使用兩張工作表，避免單列欄位過多且利於後續分析。

## 工作表 A：SystemMemoryLog（每次取樣一列）
欄位建議：
1. RunId（本次執行唯一識別）
2. SampleIndex（第幾次取樣）
3. Timestamp
4. ElapsedSeconds
5. TotalMemoryMB
6. UsedMemoryMB
7. FreeMemoryMB

## 工作表 B：TopProcessLog（每次取樣最多 10 列）
欄位建議：
1. RunId
2. SampleIndex
3. Timestamp
4. Rank（1~10）
5. ProcessName
6. PID
7. MemoryMB

> 好處：
> - 系統記憶體與程序明細分開，報表/樞紐分析更容易
> - 可完整保留每次取樣的 Top 10 明細

---

## 4. 技術規劃（C#）

## 4.1 開發框架
1. .NET 8 Console App（或 .NET 6 以上）
2. 封裝為單一 EXE（self-contained 可選）

## 4.2 主要套件
1. Excel 寫入：ClosedXML（推薦，API 直覺）
2. 系統資訊：
   - 電腦記憶體：可用 Microsoft.VisualBasic.Devices.ComputerInfo 或 PerformanceCounter / WMI
   - 程序記憶體：System.Diagnostics.Process

## 4.3 模組拆分
1. InputParser
   - 驗證時長、取樣間隔
2. MemorySampler
   - 取系統總量/已用/可用
   - 取 Top 10 程序記憶體
3. ExcelRepository
   - 建立 Excel
   - 取得最後一列
   - Append 新資料
4. MonitorService
   - 主流程控制（依時長迴圈取樣）

---

## 5. 流程設計
1. 啟動程式
2. 讀取使用者輸入（時長、間隔）
3. 產生 RunId（例如 GUID）
4. 開始迴圈直到時長結束：
   1. 讀取系統記憶體
   2. 讀取所有程序並取 Top 10
   3. 寫入 SystemMemoryLog（一列）
   4. 寫入 TopProcessLog（最多 10 列）
   5. 等待下一次取樣
5. 儲存 Excel
6. 顯示完成結果

---

## 6. 錯誤處理與邊界情況
1. 使用者輸入非數字或 <= 0
2. Excel 檔案被其他程式鎖住
3. 部分程序無權限讀取（需忽略該程序並繼續）
4. 程序在讀取過程中結束（需 try-catch）
5. 取樣耗時過長導致間隔飄移（可用 Stopwatch 校正）

---

## 7. 封裝與部署
1. 建議輸出命令（範例）：

```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

2. 產出位置：
   - bin/Release/.../publish/
3. 將 EXE 與 Excel 放在同資料夾即可執行

---

## 8. 驗收條件（Definition of Done）
1. 可輸入時長並正常啟動監控
2. 監控期間每個取樣點都有系統記憶體資料
3. 每個取樣點都有 Top 10 程序資料（若不足 10 個則以實際數量）
4. Excel 可重複執行後持續追加資料
5. 舊紀錄完整保留
6. EXE 在目標 Windows 環境可獨立執行

---

## 9. 後續可擴充項目
1. 增加 CSV 匯出
2. 增加圖表頁（趨勢線）
3. 增加記錄 CPU 使用率
4. 增加背景執行與最小化托盤
5. 增加設定檔（JSON）保存預設時長/間隔
