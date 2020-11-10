namespace Checkout.Gui
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.BarCodeTextBox = new System.Windows.Forms.TextBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.ScanButton = new System.Windows.Forms.Button();
            this.CancelItemButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.BillTextBox = new System.Windows.Forms.TextBox();
            this.LastScannedLabel = new System.Windows.Forms.Label();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SetLimitButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LimitUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LimitUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // BarCodeTextBox
            // 
            this.BarCodeTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.BarCodeTextBox.Enabled = false;
            this.BarCodeTextBox.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BarCodeTextBox.Location = new System.Drawing.Point(12, 84);
            this.BarCodeTextBox.MaxLength = 3;
            this.BarCodeTextBox.Name = "BarCodeTextBox";
            this.BarCodeTextBox.Size = new System.Drawing.Size(100, 50);
            this.BarCodeTextBox.TabIndex = 1;
            this.BarCodeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.StartButton.Enabled = false;
            this.StartButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.StartButton.Location = new System.Drawing.Point(12, 12);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(160, 66);
            this.StartButton.TabIndex = 2;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = false;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ScanButton
            // 
            this.ScanButton.BackColor = System.Drawing.Color.Aqua;
            this.ScanButton.Enabled = false;
            this.ScanButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ScanButton.Location = new System.Drawing.Point(118, 84);
            this.ScanButton.Name = "ScanButton";
            this.ScanButton.Size = new System.Drawing.Size(104, 50);
            this.ScanButton.TabIndex = 2;
            this.ScanButton.Text = "Scan";
            this.ScanButton.UseVisualStyleBackColor = false;
            this.ScanButton.Click += new System.EventHandler(this.ScanButton_Click);
            // 
            // CancelItemButton
            // 
            this.CancelItemButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CancelItemButton.Enabled = false;
            this.CancelItemButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CancelItemButton.ForeColor = System.Drawing.Color.Red;
            this.CancelItemButton.Location = new System.Drawing.Point(228, 84);
            this.CancelItemButton.Name = "CancelItemButton";
            this.CancelItemButton.Size = new System.Drawing.Size(124, 50);
            this.CancelItemButton.TabIndex = 2;
            this.CancelItemButton.Text = "Cancel";
            this.CancelItemButton.UseVisualStyleBackColor = false;
            this.CancelItemButton.Click += new System.EventHandler(this.CancelItemButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.BackColor = System.Drawing.Color.Red;
            this.StopButton.Enabled = false;
            this.StopButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.StopButton.ForeColor = System.Drawing.Color.White;
            this.StopButton.Location = new System.Drawing.Point(178, 12);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(174, 66);
            this.StopButton.TabIndex = 2;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = false;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // BillTextBox
            // 
            this.BillTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BillTextBox.Location = new System.Drawing.Point(12, 161);
            this.BillTextBox.Multiline = true;
            this.BillTextBox.Name = "BillTextBox";
            this.BillTextBox.ReadOnly = true;
            this.BillTextBox.Size = new System.Drawing.Size(497, 504);
            this.BillTextBox.TabIndex = 3;
            // 
            // LastScannedLabel
            // 
            this.LastScannedLabel.AutoSize = true;
            this.LastScannedLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LastScannedLabel.ForeColor = System.Drawing.Color.Blue;
            this.LastScannedLabel.Location = new System.Drawing.Point(12, 137);
            this.LastScannedLabel.Name = "LastScannedLabel";
            this.LastScannedLabel.Size = new System.Drawing.Size(20, 21);
            this.LastScannedLabel.TabIndex = 4;
            this.LastScannedLabel.Text = " -";
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MessageLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.MessageLabel.Location = new System.Drawing.Point(12, 668);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(22, 21);
            this.MessageLabel.TabIndex = 4;
            this.MessageLabel.Text = "   ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SetLimitButton);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.LimitUpDown);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.Location = new System.Drawing.Point(358, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(151, 132);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Price Limit";
            // 
            // SetLimitButton
            // 
            this.SetLimitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SetLimitButton.Enabled = false;
            this.SetLimitButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SetLimitButton.ForeColor = System.Drawing.Color.Black;
            this.SetLimitButton.Location = new System.Drawing.Point(6, 72);
            this.SetLimitButton.Name = "SetLimitButton";
            this.SetLimitButton.Size = new System.Drawing.Size(139, 50);
            this.SetLimitButton.TabIndex = 2;
            this.SetLimitButton.Text = "Set";
            this.SetLimitButton.UseVisualStyleBackColor = false;
            this.SetLimitButton.Click += new System.EventHandler(this.SetLimitButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 32);
            this.label1.TabIndex = 4;
            this.label1.Text = "€";
            // 
            // LimitUpDown
            // 
            this.LimitUpDown.Enabled = false;
            this.LimitUpDown.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LimitUpDown.Location = new System.Drawing.Point(39, 21);
            this.LimitUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.LimitUpDown.Name = "LimitUpDown";
            this.LimitUpDown.Size = new System.Drawing.Size(106, 35);
            this.LimitUpDown.TabIndex = 0;
            this.LimitUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 699);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.LastScannedLabel);
            this.Controls.Add(this.BillTextBox);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.CancelItemButton);
            this.Controls.Add(this.ScanButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.BarCodeTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Checkout";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LimitUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox BarCodeTextBox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button ScanButton;
        private System.Windows.Forms.Button CancelItemButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.TextBox BillTextBox;
        private System.Windows.Forms.Label LastScannedLabel;
        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown LimitUpDown;
        private System.Windows.Forms.Button SetLimitButton;
        private System.Windows.Forms.Label label1;
    }
}

