using GalaSoft.MvvmLight.Command;
using Poiesis.App.Base;
using Poiesis.Core;
using Poiesis.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poiesis.App.ViewModels.Workflow
{
    public class DatabaseAttributesViewModel : BaseViewModel
    {
        public Manager Manager { get => Locator.WorkflowShellViewModel.Manager; }

        public RelayCommand RunProcessCommand { get; set; }

        private string _newDatabaseName;
        public string NewDatabaseName
        {
            get { return _newDatabaseName; }
            set
            {
                _newDatabaseName = value;
                RaisePropertyChanged();
                Manager.UpdateNewDatabaseSQLServerSystemFiles(value);
                RaisePropertyChanged(nameof(SQLServerSystemFiles));
                CanRunProcess = !string.IsNullOrEmpty(value);
            }
        }

        public List<SQLServerSystemFile> SQLServerSystemFiles
        {
            get { return Manager.NewDatabase.SQLServerSystemFilesByTypes.Values.ToList(); }
        }

        private bool _canRunProcess;
        public bool CanRunProcess
        {
            get { return _canRunProcess; }
            set
            {
                if (_canRunProcess != value)
                {
                    _canRunProcess = value;
                    RaisePropertyChanged();
                }
            }
        }

        private List<ComboBoxEnumItem> _transferTypes;
        public List<ComboBoxEnumItem> TransferTypes
        {
            get => _transferTypes;
            set
            {
                _transferTypes = value;
                RaisePropertyChanged();
            }
        }

        private ComboBoxEnumItem _selectedTransferType;
        public ComboBoxEnumItem SelectedTransferType
        {
            get => _selectedTransferType;
            set
            {
                _selectedTransferType = value;
                RaisePropertyChanged();
                Manager.TransferType = (Manager.TransferTypes)value.Value;
            }
        }

        public DatabaseAttributesViewModel()
        {
            NewDatabaseName = Manager.NewDatabase.Name;

            RunProcessCommand = new RelayCommand(async () => await InitiateProcessAsync(Manager.InitiatePoiesis, RunProcessCommand, true), () => !IsBusy);

            TransferTypes = Enum.GetValues(typeof(Manager.TransferTypes))
                .Cast<Manager.TransferTypes>()
                .Select(tt => new ComboBoxEnumItem() { Value = (int)tt, Text = Manager.GetTransferTypeDisplayName(tt) })
                .ToList();

            SelectedTransferType = TransferTypes
                .Where(cbi => cbi.Value == (int)Manager.TransferType)
                .First();
        }
    }
}
