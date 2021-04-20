using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AddSharedParamter.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AddSharedParamter.Models.RvtModels
{
    class ExEventAddParameters : IExternalEventHandler
    {
        private UIApplication _uiapp;
        public Document doc;
        public Autodesk.Revit.ApplicationServices.Application app;
        public ExEventAddParameters(UIApplication uiapp)
        {
            _uiapp = uiapp;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            app = uiapp.Application;
            doc = uidoc.Document;
        }
        public static List<SharedParameterDto> _params;
        public static List<BuiltInParameterGroup> _paramsGroup;
        public static Dictionary<string, string> _paramsName;
        public void Execute(UIApplication _uiapp)
        {
            Transaction trans = new Transaction(doc, "Добавить параметры в семейства");
            trans.Start();
            foreach (var param in _params)
            {
                try
                {
                    DefinitionFile sharedParameterFile = app.OpenSharedParameterFile();
                    var RETRT = sharedParameterFile.Groups.FirstOrDefault(x => x.Name == param.GroupName);
                    ExternalDefinition externalDefinition = sharedParameterFile.Groups.FirstOrDefault(x => x.Name == param.GroupName).Definitions.get_Item(param.Name) as ExternalDefinition;
                    if (externalDefinition != null)
                    {
                        var dfhdnjfj = _paramsName.FirstOrDefault(x => x.Value == param.CurrentItem).Key;
                        var dgfdg = _paramsGroup.FirstOrDefault(x => x.ToString() == dfhdnjfj);
                        doc.FamilyManager.AddParameter(externalDefinition, dgfdg, param.IsExusting);
                    }
                }
                catch
                {

                }
               
            }
            trans.Commit();
            MessageBox.Show("Параметры успешно загруженны", "Статус");

        }

        public string GetName() => nameof(ExEventAddParameters);
    }
}
