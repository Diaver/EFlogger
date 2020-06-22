using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EFlogger.Network.Commands;
using SourceGrid;

namespace EFloggerApp.Views
{
    public partial class MainForm : Form, IMainForm
    {
        public event Action<QueryCommand> OnCurrentCommandChanged;
        public event Action OnStartToolStripButtonClick;
        public event Action OnStopToolStripButtonClick;
        public event Action OnClearToolStripButtonClick;

        public MainForm()
        {
            InitializeComponent();
            SubscribeEvents();
            InitCommandsGrid();
        }

        private void InitCommandsGrid()
        {
            _mBoundList = new DevAge.ComponentModel.BoundList<QueryCommand>(_queryCommands);

            commandsDataGrid.Columns.Add("Created", "Date", typeof(DateTime)).Width = 115;
            commandsDataGrid.Columns.Add("CommandText", "Text", typeof(string)).Width = 415;
            commandsDataGrid.Columns.Add("ResultRowsCount", "ResultRowsCount", typeof(string)).Width = 100;
            commandsDataGrid.Columns.Add("QueryMiliseconds", "Query Miliseconds", typeof(string)).Width = 100;
            commandsDataGrid.Columns.Add("MethodName", "Method Name", typeof(string)).Width = 150;
            commandsDataGrid.Columns.Add("ClassName", "Class Name", typeof(string)).Width = 200;
            commandsDataGrid.EnableSort = true;

            commandsDataGrid.Selection.FocusRowEntered += Selection_FocusRowEntered;
            commandsDataGrid.DataSource = _mBoundList;

            _mBoundList.AllowEdit = _mBoundList.AllowDelete = _mBoundList.AllowNew = false;
            commandsDataGrid.BorderStyle = BorderStyle.FixedSingle;
        }

        private void Selection_FocusRowEntered(object sender, RowEventArgs e)
        {
            if (!commandsDataGrid.SelectedDataRows.Any()) return;

            var queryCommand = commandsDataGrid.SelectedDataRows.First() as QueryCommand;
            OnCurrentCommandChanged(queryCommand);
        }

        private void SubscribeEvents()
        {
            startToolStripButton.Click += (sender, e) => OnStartToolStripButtonClick();
            stopToolStripButton.Click += (sender, e) => OnStopToolStripButtonClick();
            clearToolStripButton.Click += (sender, e) => OnClearToolStripButtonClick();
        }

        public Form GetForm()
        {
            return this;
        }

        public void EnableStopButton(bool isEnable)
        {
            stopToolStripButton.Enabled = isEnable;
        }

        public void EnableStartButton(bool isEnable)
        {
            startToolStripButton.Enabled = isEnable;
        }

        private DevAge.ComponentModel.BoundList<QueryCommand> _mBoundList;

        private readonly List<QueryCommand> _queryCommands = new List<QueryCommand>();

        public void AddQueryCommand(QueryCommand queryCommand)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<QueryCommand>(AddQueryCommand), queryCommand);
            }
            else
            {
                _queryCommands.Add(queryCommand);
                commandsDataGrid.OnCellsAreaChanged();
            }
        }

        public QueryCommand DetailQueryCommand
        {
            set { queryCommandBindingSource.DataSource = value; }
            get { return queryCommandBindingSource.DataSource as QueryCommand; }
        }

        public void ClearCommandsGrid()
        {
            _queryCommands.Clear();
            commandsDataGrid.OnCellsAreaChanged();
        }
       
    }
}
