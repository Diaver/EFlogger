using System;
using System.Windows.Forms;
using EFlogger.Network.Commands;

namespace EFloggerApp.Views
{
    public interface IMainForm
    {
        void AddQueryCommand(QueryCommand queryCommand);

        event Action<QueryCommand> OnCurrentCommandChanged;

        event Action OnStartToolStripButtonClick;

        event Action OnStopToolStripButtonClick;

        event Action OnClearToolStripButtonClick;

        QueryCommand DetailQueryCommand { set; get; }

        event EventHandler Closed;

        event FormClosingEventHandler FormClosing;

        void ClearCommandsGrid();

        void Close();

        void Show();

        Form GetForm();

        void EnableStopButton(bool isEnable);

        void EnableStartButton(bool isEnable);
    }
}