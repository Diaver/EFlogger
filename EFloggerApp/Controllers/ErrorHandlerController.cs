using System;
using System.Diagnostics;
using System.Windows.Forms;
using EFloggerApp.Components;
using EFloggerApp.Properties;
using EFloggerApp.Views;

namespace EFloggerApp.Controllers
{
    public class ErrorHandlerController
    {

        private readonly Exception _exception;
        private readonly ErrorHandlerForm _view = new ErrorHandlerForm();

        private const string FromEmail = "no_reply@ef-logger.com";

        private bool _isDetailShow;

        public ErrorHandlerController(Exception exception)
        {
            _exception = exception;
            SubscribeEvents();
        }


        private void SubscribeEvents()
        {
            _view.OnSendButtonClick += View_OnSendButtonClick;
            _view.OnShowErrorLinkClick += View_OnShowErrorLink;
            _view.OnLogFileLinkClick += View_OnLogFileLinkClick;
            _view.OnDebugButtonClick += View_OnDebugButtonClick;
        }

        private void View_OnSendButtonClick()
        {
            try
            {
                string bodyString = string.Format("Дополнительная информация: {0} \n Ошибка: {1} \n Reply Email: {2}",
                                              _view.ExceptionDetailText,
                                              _view.ExceptionInfoText,
                                              _view.ReplyEmail);

                MailSender.SendEmail(Settings.Default.ProgrammerEmail, string.Empty, FromEmail, bodyString, "EFlogger Exception!", 0, string.Empty);
                MessageBox.Show("Email sended!", "Email..",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                Program.Logger.Error(exception);
                MessageBox.Show(
                    "Error, оimpossible to send a error to the developer. \n " +
                    "Please send the log file to the developer's email. " + Settings.Default.ProgrammerEmail + "\n" + 
                    "Log File:" + Environment.CurrentDirectory + " \n \n" + exception);

                if (MessageBox.Show("Attach debugger? \n Only for developer!!!", "Debugging...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Debugger.Launch();
                    throw;
                }
            }
        }

        private void View_OnShowErrorLink()
        {
            _isDetailShow = !_isDetailShow;
            ShowDetailInfoException(_isDetailShow);
        }

        private void View_OnDebugButtonClick()
        {
            if (MessageBox.Show("Attach debugger? \n Only for developer!!!", "Debugging...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Debugger.Launch();
            }
        }

        private void View_OnLogFileLinkClick()
        {
            Process.Start(Environment.CurrentDirectory);
        }

        private void ShowDetailInfoException(bool isShow)
        {
            const int minimazedHeight = 256;
            const int maximazedHeight = 515;

            _view.Height = isShow
                               ? maximazedHeight
                               : minimazedHeight;
            _view.ShowExceptionInfoTextBox(isShow);

        }

        public void Run()
        {
            ShowDetailInfoException(_isDetailShow);
           
            
            string exceptionInfoText = string.Format(
                "An unexpected error occurred: {0}" + Environment.NewLine +
                "Time: {1} " + Environment.NewLine +
                "{2}" + Environment.NewLine +
                "InnerException: \n {3}" + Environment.NewLine +
                "InnerException StackTrace: \n  {4}" + Environment.NewLine, 
                _exception.Message, 
                DateTime.Now, 
                _exception, 
                _exception.InnerException, 
                _exception.InnerException != null 
                    ? _exception.InnerException.StackTrace 
                    : string.Empty
            );

            Program.Logger.Error(exceptionInfoText);
            _view.ExceptionInfoText = exceptionInfoText;

            _view.ShowDialog();

        }


    }
}
