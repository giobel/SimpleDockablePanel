using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RevitAddinWPF.ViewModels
{
    public class viewmodelRevitBridge : SimpleDockablePanel.ViewModels.BaseViewModel
    {
        private Dictionary<string, int> _dicWallType;
        private int _selectedWallType;
        private ObservableCollection<string> _listParameters;

        internal Models.modelRevitBridge RevitModel { get; set; }

        public Dictionary<string, int> DicWallType
        {
            get
            {
                return _dicWallType;
            }
            set
            {
                _dicWallType = value;
            }
        }

        public int SelectedWallType
        {
            get
            {
                return _selectedWallType;
            }
            set
            {
                _selectedWallType = value;
            }
        }

        public ObservableCollection<string> ListParameters
        {
            get
            {
                return _listParameters;
            }
            set
            {
                _listParameters = value;
                NotifyPropertyChanged();

            }
        }

        public ICommand RetrieveParametersValuesCommand
        {
            get
            {
                return new SimpleDockablePanel.ViewModels.RelayCommand(RetrieveParametersValuesAction, CanExecute);
            }
        }

        private void RetrieveParametersValuesAction(object obj)
        {
            if(SelectedWallType != -1)
            {
                ListParameters = new ObservableCollection<string>(RevitModel.GenerateParametersAndValues(SelectedWallType));
            }
        }

        private bool CanExecute(object obj)
        {
            return true;
        }

        //constructor

        public viewmodelRevitBridge()
        {

        }

    }
}
