using System;
using System.Windows.Forms;
using RAMMonitor;

namespace RAMMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // 如果你沒在 Designer 綁定事件，這行才需要
            // this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            try
            {
                int duration = InputParser.ParseDuration(txtDuration.Text);
                int interval = InputParser.ParseInterval(txtInterval.Text);

                var repo = new ExcelRepository("MemoryUsageLog.xlsx");
                var monitor = new MonitorService(repo, duration, interval);

                int count = await System.Threading.Tasks.Task.Run(() => monitor.Start());

                MessageBox.Show($"監控完成！共寫入 {count} 筆資料。\n檔案位置：{AppDomain.CurrentDomain.BaseDirectory}MemoryUsageLog.xlsx");
            }
            catch (Exception ex)
            {
                MessageBox.Show("錯誤：" + ex.Message);
            }
            finally
            {
                btnStart.Enabled = true;
            }
        }
    }
}