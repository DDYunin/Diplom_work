namespace TestBLE
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Scan = new System.Windows.Forms.Button();
            this.StopScan = new System.Windows.Forms.Button();
            this.Connect = new System.Windows.Forms.Button();
            this.Disconnect = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lbldata = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.all_data = new System.Windows.Forms.Button();
            this.online_data = new System.Windows.Forms.Button();
            this.PasswordPanel = new System.Windows.Forms.Panel();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.InputPassword = new System.Windows.Forms.TextBox();
            this.ButtonCheck = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.PasswordPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 51);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(449, 295);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Device name";
            this.columnHeader1.Width = 206;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Device Id";
            this.columnHeader2.Width = 212;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(12, 696);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(449, 228);
            this.listBox1.TabIndex = 1;
            // 
            // Scan
            // 
            this.Scan.Location = new System.Drawing.Point(12, 12);
            this.Scan.Name = "Scan";
            this.Scan.Size = new System.Drawing.Size(75, 23);
            this.Scan.TabIndex = 2;
            this.Scan.Text = "Scanning";
            this.Scan.UseVisualStyleBackColor = true;
            this.Scan.Click += new System.EventHandler(this.Scan_Click);
            // 
            // StopScan
            // 
            this.StopScan.Location = new System.Drawing.Point(93, 12);
            this.StopScan.Name = "StopScan";
            this.StopScan.Size = new System.Drawing.Size(107, 23);
            this.StopScan.TabIndex = 3;
            this.StopScan.Text = "StopScanning";
            this.StopScan.UseVisualStyleBackColor = true;
            this.StopScan.Click += new System.EventHandler(this.StopScan_Click);
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(297, 12);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(75, 23);
            this.Connect.TabIndex = 4;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // Disconnect
            // 
            this.Disconnect.Location = new System.Drawing.Point(378, 12);
            this.Disconnect.Name = "Disconnect";
            this.Disconnect.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Disconnect.Size = new System.Drawing.Size(83, 23);
            this.Disconnect.TabIndex = 5;
            this.Disconnect.Text = "Disconnect";
            this.Disconnect.UseVisualStyleBackColor = true;
            this.Disconnect.Click += new System.EventHandler(this.Disconnect_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(640, 19);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(44, 16);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Status";
            // 
            // lbldata
            // 
            this.lbldata.AutoSize = true;
            this.lbldata.Location = new System.Drawing.Point(736, 19);
            this.lbldata.Name = "lbldata";
            this.lbldata.Size = new System.Drawing.Size(44, 16);
            this.lbldata.TabIndex = 7;
            this.lbldata.Text = "label1";
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Location = new System.Drawing.Point(12, 365);
            this.chart1.Name = "chart1";
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Black;
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(1032, 295);
            this.chart1.TabIndex = 8;
            this.chart1.Text = "chart1";
            title2.Name = "Title1";
            title2.Text = "Sin(x)";
            this.chart1.Titles.Add(title2);
            // 
            // all_data
            // 
            this.all_data.BackColor = System.Drawing.Color.Red;
            this.all_data.Cursor = System.Windows.Forms.Cursors.Hand;
            this.all_data.Location = new System.Drawing.Point(530, 285);
            this.all_data.Name = "all_data";
            this.all_data.Size = new System.Drawing.Size(128, 49);
            this.all_data.TabIndex = 9;
            this.all_data.Text = "All Data";
            this.all_data.UseVisualStyleBackColor = false;
            this.all_data.Click += new System.EventHandler(this.all_data_Click);
            // 
            // online_data
            // 
            this.online_data.BackColor = System.Drawing.Color.Lime;
            this.online_data.Cursor = System.Windows.Forms.Cursors.Hand;
            this.online_data.ForeColor = System.Drawing.Color.Black;
            this.online_data.Location = new System.Drawing.Point(820, 285);
            this.online_data.Name = "online_data";
            this.online_data.Size = new System.Drawing.Size(159, 49);
            this.online_data.TabIndex = 10;
            this.online_data.Text = "Online Data";
            this.online_data.UseVisualStyleBackColor = false;
            this.online_data.Click += new System.EventHandler(this.online_data_Click);
            // 
            // PasswordPanel
            // 
            this.PasswordPanel.BackColor = System.Drawing.SystemColors.Info;
            this.PasswordPanel.Controls.Add(this.ButtonCheck);
            this.PasswordPanel.Controls.Add(this.InputPassword);
            this.PasswordPanel.Controls.Add(this.PasswordLabel);
            this.PasswordPanel.Location = new System.Drawing.Point(559, 63);
            this.PasswordPanel.Name = "PasswordPanel";
            this.PasswordPanel.Size = new System.Drawing.Size(400, 203);
            this.PasswordPanel.TabIndex = 11;
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PasswordLabel.Location = new System.Drawing.Point(123, 13);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(161, 25);
            this.PasswordLabel.TabIndex = 0;
            this.PasswordLabel.Text = "Введите пароль";
            // 
            // InputPassword
            // 
            this.InputPassword.Location = new System.Drawing.Point(84, 69);
            this.InputPassword.Multiline = true;
            this.InputPassword.Name = "InputPassword";
            this.InputPassword.Size = new System.Drawing.Size(238, 69);
            this.InputPassword.TabIndex = 1;
            // 
            // ButtonCheck
            // 
            this.ButtonCheck.Location = new System.Drawing.Point(167, 161);
            this.ButtonCheck.Name = "ButtonCheck";
            this.ButtonCheck.Size = new System.Drawing.Size(75, 23);
            this.ButtonCheck.TabIndex = 2;
            this.ButtonCheck.Text = "Check";
            this.ButtonCheck.UseVisualStyleBackColor = true;
            this.ButtonCheck.Click += new System.EventHandler(this.ButtonCheck_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 1031);
            this.Controls.Add(this.PasswordPanel);
            this.Controls.Add(this.online_data);
            this.Controls.Add(this.all_data);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.lbldata);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.Disconnect);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.StopScan);
            this.Controls.Add(this.Scan);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.listView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.PasswordPanel.ResumeLayout(false);
            this.PasswordPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button Scan;
        private System.Windows.Forms.Button StopScan;
        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.Button Disconnect;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lbldata;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button all_data;
        private System.Windows.Forms.Button online_data;
        private System.Windows.Forms.Panel PasswordPanel;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox InputPassword;
        private System.Windows.Forms.Button ButtonCheck;
    }
}

