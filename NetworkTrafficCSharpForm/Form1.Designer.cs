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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.BtnStartCapture = new System.Windows.Forms.Button();
            this.BtnStopCapture = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttviewpackets = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.DataConnButt = new System.Windows.Forms.Button();
            this.ComboBox1 = new System.Windows.Forms.ComboBox();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBox3 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnStartCapture
            // 
            this.BtnStartCapture.Location = new System.Drawing.Point(28, 15);
            this.BtnStartCapture.Margin = new System.Windows.Forms.Padding(2);
            this.BtnStartCapture.Name = "BtnStartCapture";
            this.BtnStartCapture.Size = new System.Drawing.Size(93, 67);
            this.BtnStartCapture.TabIndex = 0;
            this.BtnStartCapture.Text = "BtnStartCapture";
            this.BtnStartCapture.UseVisualStyleBackColor = true;
            this.BtnStartCapture.Click += new System.EventHandler(this.BtnStartCapture_Click);
            // 
            // BtnStopCapture
            // 
            this.BtnStopCapture.Location = new System.Drawing.Point(175, 15);
            this.BtnStopCapture.Margin = new System.Windows.Forms.Padding(2);
            this.BtnStopCapture.Name = "BtnStopCapture";
            this.BtnStopCapture.Size = new System.Drawing.Size(93, 67);
            this.BtnStopCapture.TabIndex = 1;
            this.BtnStopCapture.Text = "BtnStopCapture";
            this.BtnStopCapture.UseVisualStyleBackColor = true;
            this.BtnStopCapture.Click += new System.EventHandler(this.BtnStopCapture_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(298, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 67);
            this.button1.TabIndex = 2;
            this.button1.Text = "Caputer EtherNet Packet";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // buttviewpackets
            // 
            this.buttviewpackets.Location = new System.Drawing.Point(445, 15);
            this.buttviewpackets.Margin = new System.Windows.Forms.Padding(2);
            this.buttviewpackets.Name = "buttviewpackets";
            this.buttviewpackets.Size = new System.Drawing.Size(93, 67);
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
            this.panel1.Location = new System.Drawing.Point(8, 98);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1085, 549);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(1085, 549);
            this.dataGridView1.TabIndex = 5;
            // 
            // DataConnButt
            // 
            this.DataConnButt.Location = new System.Drawing.Point(553, 15);
            this.DataConnButt.Name = "DataConnButt";
            this.DataConnButt.Size = new System.Drawing.Size(93, 67);
            this.DataConnButt.TabIndex = 5;
            this.DataConnButt.Text = "Enter Database Connection String";
            this.DataConnButt.UseVisualStyleBackColor = true;
            this.DataConnButt.Click += new System.EventHandler(this.DataConnButt_Click);
            // 
            // ComboBox1
            // 
            this.ComboBox1.FormattingEnabled = true;
            this.ComboBox1.Location = new System.Drawing.Point(652, 15);
            this.ComboBox1.Name = "ComboBox1";
            this.ComboBox1.Size = new System.Drawing.Size(437, 21);
            this.ComboBox1.TabIndex = 6;
            this.ComboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // TextBox1
            // 
            this.TextBox1.Location = new System.Drawing.Point(652, 62);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(100, 20);
            this.TextBox1.TabIndex = 7;
            this.TextBox1.Text = "192.168.1.";
            this.TextBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(652, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Local Network";
            // 
            // TextBox2
            // 
            this.TextBox2.Enabled = false;
            this.TextBox2.Location = new System.Drawing.Point(773, 62);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(100, 20);
            this.TextBox2.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(770, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "My Internet IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(890, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "My Network IP";
            // 
            // TextBox3
            // 
            this.TextBox3.Enabled = false;
            this.TextBox3.Location = new System.Drawing.Point(893, 62);
            this.TextBox3.Name = "TextBox3";
            this.TextBox3.Size = new System.Drawing.Size(100, 20);
            this.TextBox3.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 655);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TextBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TextBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this.ComboBox1);
            this.Controls.Add(this.DataConnButt);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttviewpackets);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnStopCapture);
            this.Controls.Add(this.BtnStartCapture);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "NetTrafficAnalyzer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnStartCapture;
        private System.Windows.Forms.Button BtnStopCapture;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttviewpackets;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button DataConnButt;
        private System.Windows.Forms.ComboBox ComboBox1;
        private System.Windows.Forms.TextBox TextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBox3;
    }
}

