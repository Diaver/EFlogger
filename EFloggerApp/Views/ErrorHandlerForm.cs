using System;
using System.Windows.Forms;

namespace EFloggerApp.Views
{
    public partial class ErrorHandlerForm : Form
    {

        public Action OnSendButtonClick;

        public Action OnShowErrorLinkClick;
        public Action OnLogFileLinkClick;
        public Action OnDebugButtonClick;

        public ErrorHandlerForm()
        {
            InitializeComponent();
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            sendButton.Click += (sender, e) => OnSendButtonClick();
            debugButton.Click += (sender, e) => OnDebugButtonClick();
            showErrorLink.Click += (sender, e) => OnShowErrorLinkClick();
            logFileLink.Click += (sender, e) => OnLogFileLinkClick();
        }

        public string ExceptionInfoText
        {
            get { return exceptionInfoTextBox.Text; }
            set { exceptionInfoTextBox.Text = value; }
        }

        public string ExceptionDetailText
        {
            get { return detailInfoTextBox.Text; }
            set { detailInfoTextBox.Text = value; }
        }

        public void ShowExceptionInfoTextBox(bool isShow)
        {
            exceptionInfoTextBox.Visible = isShow;
        }

        public string ReplyEmail {
            get { return replyEmailTextBox.Text; }
        }
    }
}
