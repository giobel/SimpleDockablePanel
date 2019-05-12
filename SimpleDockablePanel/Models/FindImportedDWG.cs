using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SimpleDockablePanel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDockablePanel.Models
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]    
    public class FindImportedDWG : IExternalEventHandler
        {

        private ObservableCollection<ImportedDWG> importedDWGlist { get; set; }

        public void Execute(UIApplication uiapp)
            {
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            FilteredElementCollector col = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance));

            

            importedDWGlist = new ObservableCollection<ImportedDWG>();

            //NameValueCollection listOfViewSpecificImports = new NameValueCollection();

            //NameValueCollection listOfModelImports = new NameValueCollection();

            //NameValueCollection listOfUnidentifiedImports = new NameValueCollection();


            /*

            string info = null;
              ICollection<ElementId> ids = ExternalFileUtils.GetAllExternalFileReferences(doc);
              foreach(ElementId id in ids)
              {
              Element elem = doc.GetElement(id);
              if(elem.Category!= null)
              info += elem.Category.Name + "\n";
              }
              TaskDialog.Show("result",info);
                   TaskDialog.Show("result", ids.Count().ToString()); */

            foreach (Element e in col)
            {


                if (e.Category != null)
                {
                    if (e.ViewSpecific)
                    {
                        string viewName = null;

                        try
                        {
                            Element viewElement = doc.GetElement(e.OwnerViewId);
                            viewName = viewElement.Name;
                        }
                        catch (Autodesk.Revit.Exceptions
                          .ArgumentNullException) // just in case
                        {
                            viewName = String.Concat("Invalid View ID: ",e.OwnerViewId.ToString());
                        }


                        if (null != e.Category)


                            importedDWGlist.Add(new ImportedDWG { ImportType = "View Specific", CategoryName = e.Category.Name, ViewName = viewName });
                            //listOfViewSpecificImports.Add(importCategoryNameToFileName(e.Category.Name), viewName);

                        else
                        {
                            importedDWGlist.Add(new ImportedDWG { ImportType = "Unidentified", CategoryName = e.Id.ToString(), ViewName = viewName });
                            //listOfUnidentifiedImports.Add(e.Id.ToString(), viewName);
                        }

                    }

                    else
                    {
                        importedDWGlist.Add(new ImportedDWG { ImportType = "Model Imports", CategoryName = e.Category.Name, ViewName = e.Name });
                        //listOfModelImports.Add(importCategoryNameToFileName(e.Category.Name), e.Name);
                    }

                }
                else
                {
                    //TaskDialog.Show("result",e.Id.ToString());
                }

            }

            
            string result = "";
            foreach (ImportedDWG nvc in importedDWGlist)
            {
                result += String.Format("{0} : {1}\n", nvc.CategoryName, nvc.ImportType);
            }


            TaskDialog.Show("View Specific", result);

            //ViewModels.ImportDWGViewModel vim = new ViewModels.ImportDWGViewModel();
            //vim.pierino = new ObservableCollection<ImportedDWG>();
            //vim.pierino = importedDWGlist;

            


        }//close method

        public ObservableCollection<ImportedDWG> PierinoDWG()
        {
            return importedDWGlist;
        }

      
        private string importCategoryNameToFileName(string catName)
        {

            string fileName = catName;
            fileName = fileName.Trim();

            if (fileName.EndsWith(")"))
            {
                int lastLeftBracket = fileName.LastIndexOf("(");

                if (-1 != lastLeftBracket)
                    fileName = fileName.Remove(lastLeftBracket); // remove left bracket
            }

            return fileName.Trim();


        }//close method


        public string GetName()
        {
            return "External Event Find Imported DWG";
        }
    }
    
}
