using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleDockablePanel.ViewModels
{
    class DWGtableViewModel : BaseViewModel
    {

        private ObservableCollection<int> _numberDWG;

        public ObservableCollection<int> NumberDWG
        {
            get
            {
                return _numberDWG;
            }
            set
            {
                _numberDWG = value;
                NotifyPropertyChanged();

            }
        }

        public RelayCommand ActivateClick { get; set; }


        public DWGtableViewModel()
        {

            SelectAllInView();

            NumberDWG = new ObservableCollection<int>();

            ActivateClick = new RelayCommand(ActivateMethod);

        }

        private void ActivateMethod(object obj)
        {
            _selectAllInViewEvent.Raise();
            //co.Execute(ShowDockableWindow._cachedUiApp); _cachedUiApp is null
            _selectAllInViewHandler.PassValue(NumberDWG);
            
            MessageBox.Show("activate clicked");
        }

        private Models.DWGtableModelCommand _selectAllInViewHandler;
        private ExternalEvent _selectAllInViewEvent;

        public void SelectAllInView()
        {
            _selectAllInViewHandler = new Models.DWGtableModelCommand();
            _selectAllInViewEvent = ExternalEvent.Create(_selectAllInViewHandler);
        }

        public void SelectAllInViewRaise(object obj)
        {
            _selectAllInViewEvent.Raise();
        }
    }
}
