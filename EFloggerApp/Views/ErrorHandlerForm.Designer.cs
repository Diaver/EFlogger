namespace EFloggerApp.Views
{
    partial class ErrorHandlerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorHandlerForm));
            this.exceptionInfoTextBox = new System.Windows.Forms.TextBox();
            this.detailInfoTextBox = new System.Windows.Forms.TextBox();
            this.showErrorLink = new System.Windows.Forms.LinkLabel();
            this.sendButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.logFileLink = new System.Windows.Forms.LinkLabel();
            this.debugButton = new System.Windows.Forms.Button();
            this.replyEmailTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // exceptionInfoTextBox
            // 
            this.exceptionInfoTextBox.Location = new System.Drawing.Point(12, 226);
            this.exceptionInfoTextBox.Multiline = true;
            this.exceptionInfoTextBox.Name = "exceptionInfoTextBox";
            this.exceptionInfoTextBox.ReadOnly = true;
            this.exceptionInfoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.exceptionInfoTextBox.Size = new System.Drawing.Size(456, 243);
            this.exceptionInfoTextBox.TabIndex = 0;
            this.exceptionInfoTextBox.Visible = false;
            this.exceptionInfoTextBox.WordWrap = false;
            // 
            // detailInfoTextBox
            // 
            this.detailInfoTextBox.Location = new System.Drawing.Point(16, 118);
            this.detailInfoTextBox.Multiline = true;
            this.detailInfoTextBox.Name = "detailInfoTextBox";
            this.detailInfoTextBox.Size = new System.Drawing.Size(453, 55);
            this.detailInfoTextBox.TabIndex = 1;
            // 
            // showErrorLink
            // 
            this.showErrorLink.AutoSize = true;
            this.showErrorLink.Location = new System.Drawing.Point(13, 197);
            this.showErrorLink.Name = "showErrorLink";
            this.showErrorLink.Size = new System.Drawing.Size(111, 13);
            this.showErrorLink.TabIndex = 2;
            this.showErrorLink.TabStop = true;
            this.showErrorLink.Text = "Error detail information";
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(321, 179);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(148, 31);
            this.sendButton.TabIndex = 3;
            this.sendButton.Text = "Send To Developer";
            this.sendButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(457, 52);
            this.label1.TabIndex = 4;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(221, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Field for additional information about the error:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // logFileLink
            // 
            this.logFileLink.AutoSize = true;
            this.logFileLink.Location = new System.Drawing.Point(144, 197);
            this.logFileLink.Name = "logFileLink";
            this.logFileLink.Size = new System.Drawing.Size(44, 13);
            this.logFileLink.TabIndex = 6;
            this.logFileLink.TabStop = true;
            this.logFileLink.Text = "Log File";
            // 
            // debugButton
            // 
            this.debugButton.Location = new System.Drawing.Point(206, 180);
            this.debugButton.Name = "debugButton";
            this.debugButton.Size = new System.Drawing.Size(109, 30);
            this.debugButton.TabIndex = 7;
            this.debugButton.Text = "Debug";
            this.debugButton.UseVisualStyleBackColor = true;
            // 
            // replyEmailTextBox
            // 
            this.replyEmailTextBox.Location = new System.Drawing.Point(117, 66);
            this.replyEmailTextBox.Name = "replyEmailTextBox";
            this.replyEmailTextBox.Size = new System.Drawing.Size(352, 20);
            this.replyEmailTextBox.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Your email for reply:";
            // 
            // ErrorHandlerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 477);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.replyEmailTextBox);
            this.Controls.Add(this.debugButton);
            this.Controls.Add(this.logFileLink);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.showErrorLink);
            this.Controls.Add(this.detailInfoTextBox);
            this.Controls.Add(this.exceptionInfoTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorHandlerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox exceptionInfoTextBox;
        private System.Windows.Forms.TextBox detailInfoTextBox;
        private System.Windows.Forms.LinkLabel showErrorLink;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel logFileLink;
        private System.Windows.Forms.Button debugButton;
        private System.Windows.Forms.TextBox replyEmailTextBox;
        private System.Windows.Forms.Label label3;
    }
}