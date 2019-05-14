using Autodesk.Revit.UI;
using SimpleDockablePanel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace SimpleDockablePanel.ViewModels
{
    public class ImportDWGViewModel : BaseViewModel
    {

        private string textValue;
        public string TextValue
        {
            get {
                
                return textValue; }
            set {
                
                TextValue = value;
                NotifyPropertyChanged();
            }
        }

        

        private ObservableCollection<ImportedDWG> _listImportedDWGs { get; set; }
        
        public ObservableCollection<ImportedDWG> pierino
        {
            get
            {
                return _listImportedDWGs;
            }
            set
            {
                _listImportedDWGs = value;
                NotifyPropertyChanged();
            }
        }

        public ImportDWGViewModel()
        {
            pierino = new ObservableCollection<ImportedDWG>(); //will keep all the values
            //DWGeventHandler();
            //AggiungiClick = new RelayCommand(DWGRaise);
            
        }

        public RelayCommand AggiungiClick
        {
            get
            {
                
                return new RelayCommand(DWGRaise);
            }
        }

        private RevitAddinWPF.Command _DWGHandler;
        private ExternalEvent _DWGEvent;

        public void DWGeventHandler()
        {
            _DWGHandler = new RevitAddinWPF.Command();
            _DWGEvent = ExternalEvent.Create(_DWGHandler);
            
        }

        public void DWGRaise(object obj)
        {
            _DWGEvent.Raise();

        }

        

        private void ExecuteMethod(object obj)
        {
            MessageBox.Show("ciao");
            
        }
    }
}
