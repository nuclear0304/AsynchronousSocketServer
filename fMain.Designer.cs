namespace AsynchronousSocketServer
{
    partial class fMain
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_ConnState = new System.Windows.Forms.TextBox();
            this.textBox_Exclog = new System.Windows.Forms.TextBox();
            this.label_Ip = new System.Windows.Forms.Label();
            this.label_Port = new System.Windows.Forms.Label();
            this.textBox_Ip = new System.Windows.Forms.TextBox();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Recvlog = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(326, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Listen";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox_ConnState
            // 
            this.textBox_ConnState.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_ConnState.Location = new System.Drawing.Point(12, 66);
            this.textBox_ConnState.Multiline = true;
            this.textBox_ConnState.Name = "textBox_ConnState";
            this.textBox_ConnState.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_ConnState.Size = new System.Drawing.Size(189, 294);
            this.textBox_ConnState.TabIndex = 2;
            // 
            // textBox_Exclog
            // 
            this.textBox_Exclog.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_Exclog.Location = new System.Drawing.Point(523, 66);
            this.textBox_Exclog.Multiline = true;
            this.textBox_Exclog.Name = "textBox_Exclog";
            this.textBox_Exclog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Exclog.Size = new System.Drawing.Size(306, 294);
            this.textBox_Exclog.TabIndex = 3;
            // 
            // label_Ip
            // 
            this.label_Ip.AutoSize = true;
            this.label_Ip.Location = new System.Drawing.Point(12, 12);
            this.label_Ip.Name = "label_Ip";
            this.label_Ip.Size = new System.Drawing.Size(20, 17);
            this.label_Ip.TabIndex = 4;
            this.label_Ip.Text = "Ip";
            // 
            // label_Port
            // 
            this.label_Port.AutoSize = true;
            this.label_Port.Location = new System.Drawing.Point(208, 12);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(34, 17);
            this.label_Port.TabIndex = 5;
            this.label_Port.Text = "Port";
            // 
            // textBox_Ip
            // 
            this.textBox_Ip.Location = new System.Drawing.Point(38, 8);
            this.textBox_Ip.MaxLength = 15;
            this.textBox_Ip.Name = "textBox_Ip";
            this.textBox_Ip.Size = new System.Drawing.Size(153, 25);
            this.textBox_Ip.TabIndex = 6;
            // 
            // textBox_Port
            // 
            this.textBox_Port.Location = new System.Drawing.Point(248, 8);
            this.textBox_Port.MaxLength = 5;
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(56, 25);
            this.textBox_Port.TabIndex = 7;
            this.textBox_Port.Text = "5001";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Connection state";
            // 
            // textBox_Recvlog
            // 
            this.textBox_Recvlog.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_Recvlog.Location = new System.Drawing.Point(211, 66);
            this.textBox_Recvlog.Multiline = true;
            this.textBox_Recvlog.Name = "textBox_Recvlog";
            this.textBox_Recvlog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Recvlog.Size = new System.Drawing.Size(306, 294);
            this.textBox_Recvlog.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(208, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "recv log";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(520, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "exc log";
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 372);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_Recvlog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Port);
            this.Controls.Add(this.textBox_Ip);
            this.Controls.Add(this.label_Port);
            this.Controls.Add(this.label_Ip);
            this.Controls.Add(this.textBox_Exclog);
            this.Controls.Add(this.textBox_ConnState);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fMain";
            this.Text = "非同步Socket Server";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_ConnState;
        private System.Windows.Forms.TextBox textBox_Exclog;
        private System.Windows.Forms.Label label_Ip;
        private System.Windows.Forms.Label label_Port;
        private System.Windows.Forms.TextBox textBox_Ip;
        private System.Windows.Forms.TextBox textBox_Port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Recvlog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

