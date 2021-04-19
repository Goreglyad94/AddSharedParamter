using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddSharedParamter.ViewModels.Base;
using Autodesk.Revit.DB;

namespace AddSharedParamter.DTO
{
    [Serializable]
    public class SharedParameterDto : ViewModel
    {
        public SharedParameterDto()
        {

        }
        public static List<string> BuiltInParamGroupName { get; set; }
        private string _currentItem;
        public string CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (_currentItem == value) return;
                _currentItem = value;
                OnPropertyChanged("CurrentItem");
            }
        }

        public string GroupName { get; set; }
        public string DescriptionDto { get; set; }
        public string GUID { get; set; }
        public string Name { get; set; }
        public BuiltInParameterGroup ParameterGroupDto { get; set; }
        public ParameterType ParameterTypeDto { get; set; }
        public bool IsExusting { get; set; }
    }
}
