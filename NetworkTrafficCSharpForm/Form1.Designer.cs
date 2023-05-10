namespace NetworkTrafficCSharpForm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnStartCapture = new System.Windows.Forms.Button();
            this.BtnStopCapture = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttviewpackets = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnStartCapture
            // 
            this.BtnStartCapture.Location = new System.Drawing.Point(42, 23);
            this.BtnStartCapture.Name = "BtnStartCapture";
            this.BtnStartCapture.Size = new System.Drawing.Size(140, 103);
            this.BtnStartCapture.TabIndex = 0;
            this.BtnStartCapture.Text = "BtnStartCapture";
            this.BtnStartCapture.UseVisualStyleBackColor = true;
            this.BtnStartCapture.Click += new System.EventHandler(this.BtnStartCapture_Click);
            // 
            // BtnStopCapture
            // 
            this.BtnStopCapture.Location = new System.Drawing.Point(262, 23);
            this.BtnStopCapture.Name = "BtnStopCapture";
            this.BtnStopCapture.Size = new System.Drawing.Size(140, 103);
            this.BtnStopCapture.TabIndex = 1;
            this.BtnStopCapture.Text = "BtnStopCapture";
            this.BtnStopCapture.UseVisualStyleBackColor = true;
            this.BtnStopCapture.Click += new System.EventHandler(this.BtnStopCapture_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(447, 23);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(140, 103);
            this.button1.TabIndex = 2;
            this.button1.Text = "Caputer EtherNet Packet";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // buttviewpackets
            // 
            this.buttviewpackets.Location = new System.Drawing.Point(667, 23);
            this.buttviewpackets.Name = "buttviewpackets";
            this.buttviewpackets.Size = new System.Drawing.Size(140, 103);
            this.buttviewpackets.TabIndex = 3;
            this.buttviewpackets.Text = "View Packets";
            this.buttviewpackets.UseVisualStyleBackColor = true;
            this.buttviewpackets.Click += new System.EventHandler(this.Buttviewpackets_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Location = new System.Drawing.Point(12, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1628, 845);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(1628, 845);
            this.dataGridView1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1652, 1007);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttviewpackets);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnStopCapture);
            this.Controls.Add(this.BtnStartCapture);
            this.Name = "Form1";
            this.Text = "NetTrafficAnalyzer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnStartCapture;
        private System.Windows.Forms.Button BtnStopCapture;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttviewpackets;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

