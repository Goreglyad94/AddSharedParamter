using System.Collections.Generic;
using AddSharedParamter.DTO;
using Autodesk.Revit.UI;

namespace AddSharedParamter.Models.RvtModels
{
    class ExEventAddParameters : IExternalEventHandler
    {
        public static List<SharedParameterDto> parameterDtos = new List<SharedParameterDto>();
        public void Execute(UIApplication app)
        {
            TaskDialog.Show("Статус", "Работает!");
        }

        public string GetName() => nameof(ExEventAddParameters);
    }
}
