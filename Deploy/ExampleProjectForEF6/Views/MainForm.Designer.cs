namespace EFloggerTestAppEF6.Views
{
    partial class MainForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.startSendToClientButton = new System.Windows.Forms.Button();
            this.stopSendToClientButton = new System.Windows.Forms.Button();
            this.startSaveToLogButton = new System.Windows.Forms.Button();
            this.stopSaveToLogButton = new System.Windows.Forms.Button();
            this.clearLogButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(86, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Execute Command";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.executeButton_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(187, 50);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(135, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Add log message";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.writeMessageButton_Click);
            // 
            // startSendToClientButton
            // 
            this.startSendToClientButton.Location = new System.Drawing.Point(29, 130);
            this.startSendToClientButton.Name = "startSendToClientButton";
            this.startSendToClientButton.Size = new System.Drawing.Size(132, 23);
            this.startSendToClientButton.TabIndex = 3;
            this.startSendToClientButton.Text = "Start Send to Client";
            this.startSendToClientButton.UseVisualStyleBackColor = true;
            this.startSendToClientButton.Click += new System.EventHandler(this.startSendToClientButton_Click);
            // 
            // stopSendToClientButton
            // 
            this.stopSendToClientButton.Location = new System.Drawing.Point(187, 130);
            this.stopSendToClientButton.Name = "stopSendToClientButton";
            this.stopSendToClientButton.Size = new System.Drawing.Size(135, 23);
            this.stopSendToClientButton.TabIndex = 4;
            this.stopSendToClientButton.Text = "Stop Send to Client";
            this.stopSendToClientButton.UseVisualStyleBackColor = true;
            this.stopSendToClientButton.Click += new System.EventHandler(this.stopSendToClientButton_Click);
            // 
            // startSaveToLogButton
            // 
            this.startSaveToLogButton.Location = new System.Drawing.Point(29, 191);
            this.startSaveToLogButton.Name = "startSaveToLogButton";
            this.startSaveToLogButton.Size = new System.Drawing.Size(132, 23);
            this.startSaveToLogButton.TabIndex = 5;
            this.startSaveToLogButton.Text = "Start Save to Log";
            this.startSaveToLogButton.UseVisualStyleBackColor = true;
            this.startSaveToLogButton.Click += new System.EventHandler(this.startSaveToLogButton_Click);
            // 
            // stopSaveToLogButton
            // 
            this.stopSaveToLogButton.Location = new System.Drawing.Point(187, 191);
            this.stopSaveToLogButton.Name = "stopSaveToLogButton";
            this.stopSaveToLogButton.Size = new System.Drawing.Size(135, 23);
            this.stopSaveToLogButton.TabIndex = 6;
            this.stopSaveToLogButton.Text = "Stop Save to Log";
            this.stopSaveToLogButton.UseVisualStyleBackColor = true;
            this.stopSaveToLogButton.Click += new System.EventHandler(this.stopSaveToLogButton_Click);
            // 
            // clearLogButton
            // 
            this.clearLogButton.Location = new System.Drawing.Point(361, 163);
            this.clearLogButton.Name = "clearLogButton";
            this.clearLogButton.Size = new System.Drawing.Size(75, 23);
            this.clearLogButton.TabIndex = 7;
            this.clearLogButton.Text = "Clear Log";
            this.clearLogButton.UseVisualStyleBackColor = true;
            this.clearLogButton.Click += new System.EventHandler(this.clearLogButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 366);
            this.Controls.Add(this.clearLogButton);
            this.Controls.Add(this.stopSaveToLogButton);
            this.Controls.Add(this.startSaveToLogButton);
            this.Controls.Add(this.stopSendToClientButton);
            this.Controls.Add(this.startSendToClientButton);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button startSendToClientButton;
        private System.Windows.Forms.Button stopSendToClientButton;
        private System.Windows.Forms.Button startSaveToLogButton;
        private System.Windows.Forms.Button stopSaveToLogButton;
        private System.Windows.Forms.Button clearLogButton;
    }
}

