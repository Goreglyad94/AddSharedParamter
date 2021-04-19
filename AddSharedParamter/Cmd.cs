using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddSharedParamter.DTO;
using AddSharedParamter.Models.RvtModels;
using AddSharedParamter.ViewModels;
using AddSharedParamter.Views.Windows;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AddSharedParamter
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd : IExternalCommand
    {
        List<SharedGroupDto> sharedGroupsDtos = new List<SharedGroupDto>();
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Application app = uiapp.Application;

            if (commandData.Application.ActiveUIDocument.Document.IsFamilyDocument)
            {
                DefinitionFile sharedParameterFile = app.OpenSharedParameterFile();
                DefinitionGroups sharGroups = sharedParameterFile.Groups;

                foreach (var sharGroup in sharGroups)
                {
                    SharedGroupDto sharGrupDto = new SharedGroupDto();
                    sharGrupDto.Name = sharGroup.Name;
                    foreach (var sharParam in sharGroup.Definitions)
                    {
                        SharedParameterDto sharedParameterDto = new SharedParameterDto();
                        sharedParameterDto.Name = (sharParam as ExternalDefinition).Name;
                        sharedParameterDto.ParameterGroupDto = (sharParam as ExternalDefinition).ParameterGroup;
                        sharedParameterDto.ParameterTypeDto = (sharParam as ExternalDefinition).ParameterType;
                        sharedParameterDto.GUID = (sharParam as ExternalDefinition).GUID.ToString();
                        sharedParameterDto.DescriptionDto = (sharParam as ExternalDefinition).Description;
                        sharedParameterDto.GroupName = sharGroup.Name;
                        sharGrupDto.SharedParametersDto.Add(sharedParameterDto);

                    }
                    sharedGroupsDtos.Add(sharGrupDto);
                }

                
            }

            var builtInPramGroup = (BuiltInParameterGroup[])Enum.GetValues(typeof(BuiltInParameterGroup));
            List<BuiltInParameterGroup> bipg = new List<BuiltInParameterGroup>();
            foreach (var _param in builtInPramGroup)
            {
                var dsfdf = doc.FamilyManager.IsUserAssignableParameterGroup(_param);
                if (dsfdf == true)
                {
                    bipg.Add(_param);
                }
            }

            List<string> builtInPramGroupValue = new List<string>();
            Dictionary<string, string> dicParamGroup = new Dictionary<string, string>();
            foreach (var item in builtInPramGroup)
            {
                if (doc.FamilyManager.IsUserAssignableParameterGroup(item) == true)
                {
                    dicParamGroup.Add(item.ToString(), LabelUtils.GetLabelFor(item));
                }
            }
            foreach (var dir in dicParamGroup)
            {
                builtInPramGroupValue.Add(dir.Value);
            }
            SharedParameterDto.BuiltInParamGroupName = builtInPramGroupValue;


            MainWindow mainWindow = new MainWindow();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(sharedGroupsDtos);
            mainWindowViewModel.exEventAddParameters = ExternalEvent.Create(new ExEventAddParameters());
            mainWindow.DataContext = mainWindowViewModel;
            mainWindow.Show();


            return Result.Succeeded;
        }
    }
}
