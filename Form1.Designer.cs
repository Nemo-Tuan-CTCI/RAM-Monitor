namespace RAMMonitor
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtDuration = new System.Windows.Forms.TextBox();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblInterval = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtDuration
            // 
            this.txtDuration.Location = new System.Drawing.Point(82, 30);
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Size = new System.Drawing.Size(120, 22);
            this.txtDuration.TabIndex = 0;
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(82, 70);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(120, 22);
            this.txtInterval.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(82, 107);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 30);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "開始監控";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(12, 33);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(53, 12);
            this.lblDuration.TabIndex = 3;
            this.lblDuration.Text = "持續時間";
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(12, 73);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(53, 12);
            this.lblInterval.TabIndex = 4;
            this.lblInterval.Text = "檢查間隔";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 332);
            this.Controls.Add(this.lblInterval);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.txtDuration);
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "RAM 監控";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDuration;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblInterval;
    }
}

