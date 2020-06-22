using System;
using System.Net.Sockets;
using System.Windows.Forms;
using EFlogger.Network.Commands;
using EFlogger.Network.Network;
using EFloggerApp.Views;

namespace EFloggerApp.Controllers
{
    public class MainFormController
    {
        private readonly IMainForm _view;

        private CommandListener _commandListener;

        private bool _acceptCommands = true;


        public MainFormController()
        {
            _view = new MainForm();
            InitCommandListener();
            SubscribeViewEvents();
            _view.EnableStartButton(false);
        }

        private void SubscribeViewEvents()
        {
            _view.OnClearToolStripButtonClick += _view_OnClearToolStripButtonClick;
            _view.OnStartToolStripButtonClick += _view_OnStartToolStripButtonClick;
            _view.OnStopToolStripButtonClick += _view_OnStopToolStripButtonClick;
            _view.OnCurrentCommandChanged += _view_OnCurrentCommandChanged;
            _view.FormClosing += _view_Closed;
        }

        private void _view_OnStopToolStripButtonClick()
        {
            _view.EnableStopButton(false);
            _view.EnableStartButton(true);
            _acceptCommands = false;
        }

        private void _view_OnStartToolStripButtonClick()
        {

            _view.EnableStopButton(true);
            _view.EnableStartButton(false);
            _acceptCommands = true;
        }

        private void _view_OnClearToolStripButtonClick()
        {
            _view.ClearCommandsGrid();
            _view.DetailQueryCommand = new QueryCommand();
        }

        private void _view_Closed(object sender, System.EventArgs e)
        {
            _commandListener.Stop();
        }

        private void _view_OnCurrentCommandChanged(QueryCommand queryCommand)
        {
            if (queryCommand == null) return;

            _view.DetailQueryCommand = queryCommand;
        }

        private void InitCommandListener()
        {
            _commandListener = new CommandListener();
            _commandListener.OnQueryCommand += CommandListenerOnQueryCommand;
            _commandListener.OnClearLogDataGrid += CommandListenerOnClearLogDataGrid;
        }

        private void CommandListenerOnQueryCommand(QueryCommand queryCommand, TcpClient tcpClient)
        {
            if (!_acceptCommands) return;

            queryCommand.CommandTextOriginal = queryCommand.CommandText;
            queryCommand.CommandText = queryCommand.CommandText.Replace("\r\n", " ").Replace("  ", " ");
            if (queryCommand.CommandText.Length > 100)
            {
                queryCommand.CommandText = queryCommand.CommandText.Substring(0, 100);
            }
            _view.AddQueryCommand(queryCommand);
        }

        private void CommandListenerOnClearLogDataGrid(TcpClient tcpClient)
        {
            _view.ClearCommandsGrid();
            _view.DetailQueryCommand = new QueryCommand();
        }

        public Form Run()
        {
            _view.Show();
            _commandListener.Start();
            return _view.GetForm();
        }
    }
}
