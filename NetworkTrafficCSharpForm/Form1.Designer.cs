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
            this.SuspendLayout();
            // 
            // BtnStartCapture
            // 
            this.BtnStartCapture.Location = new System.Drawing.Point(68, 43);
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
            this.BtnStopCapture.Location = new System.Drawing.Point(215, 43);
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
            this.button1.Location = new System.Drawing.Point(68, 148);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 67);
            this.button1.TabIndex = 2;
            this.button1.Text = "Caputer EtherNet Packet";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 400);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnStopCapture);
            this.Controls.Add(this.BtnStartCapture);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnStartCapture;
        private System.Windows.Forms.Button BtnStopCapture;
        private System.Windows.Forms.Button button1;
    }
}

