using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using AddSharedParamter.DTO;
using AddSharedParamter.Infrastructure.Commands;
using AddSharedParamter.Models.RvtModels;
using AddSharedParamter.Models.XmlModel;
using AddSharedParamter.ViewModels.Base;
using Autodesk.Revit.UI;

namespace AddSharedParamter.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        

        #region Филды
        List<SharedParameterDto> sharedParamsDto;
        /// <summary>
        /// DTO список грапп параметров для добавления в семейство (Набирается из списка параметров из левого ListBox)
        /// </summary>
        List<SharedParameterDto> addSharedParamsDto = new List<SharedParameterDto>();
        List<ParametersSetDto> parametersSetDto = new List<ParametersSetDto>();
        private Brush selectedItemColor;

        /// <summary>
        /// DTO список групп параметров из cmd (Реализация в конструкторе)
        /// </summary>
        private List<SharedGroupDto> SharedGroupsDto;

        MainModel mainModel = new MainModel();
        #endregion

        #region Пропы

        /// <summary>
        /// ВнешнееСобытие ревита. Загрузка параметров в семейство
        /// </summary>
        public ExternalEvent exEventAddParameters { get; set; }
        #endregion

        #region Комманды
        public ICommand AddParametersCommand { get; set; }
        public ICommand AddPatamIntoProjectCommand { get; set; }
        public ICommand ClearParametersListCommand { get; set; }
        public ICommand AddParametersSetCommand { get; set; }










        public ICommand SelectGroups { get; set; }

        public ICommand RemoveSubjectCommand { get; set; }

        

        public ICommand RemoveSubjectParamSetItem { get; set; }
        #endregion

        public MainWindowViewModel(List<SharedGroupDto> sharedGroupsDto)
        {
            SharedGroupsDto = sharedGroupsDto;
            GetGroups("");

            #region Реализация коммандов
            AddParametersCommand = new RelayCommand(OnAddParametersCommandExecutde, CanAddParametersCommandExecute);
            AddPatamIntoProjectCommand = new RelayCommand(OnAddPatamIntoProjectCommandExecutde, CanAddPatamIntoProjectCommandExecute);
            ClearParametersListCommand = new RelayCommand(OnClearParametersListCommandExecutde, CanClearParametersListCommandExecute);
            AddParametersSetCommand = new RelayCommand(OnAddParametersSetCommandExecutde, CanAddParametersSetCommandExecute);
            #endregion
        }
        /*listBox списка групп параметров~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        #region КоллекшенВью для ComboBox списка групп параметров
        private ICollectionView sharedGroupDto;
        public ICollectionView SharedGroupDto
        {
            get => sharedGroupDto;
            set => Set(ref sharedGroupDto, value);
        }
        #endregion

        #region СелектедИтем комбобокса списка групп параметров
        private SharedGroupDto selectedItem;
        public SharedGroupDto SelectedItem
        {
            get { return selectedItem; }

            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
                GetParameters(selectedItem);
            }
        }
        #endregion

        #region Метод - обновление UI списка групп. (Реализация в конструкторе) (Удалить метод и вынесит логику в конструктор (тест))
        /// <summary>
        /// Отображение групп параметров из ФОПа в комбобоксе
        /// </summary>
        /// <param name="obj"></param>
        private void GetGroups(object obj)
        {
            SharedGroupDto = CollectionViewSource.GetDefaultView(SharedGroupsDto);
            SharedGroupDto.Refresh();
            //ApplyEvent.Raise();
        }
        #endregion
        /*listBox списка параметров~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        #region КоллекшенВтю для ListBox списка параметров
        private ICollectionView sharedParamDto;

        public ICollectionView SharedParamsDto
        {
            get => sharedParamDto;
            set => Set(ref sharedParamDto, value);
        }
        #endregion

        #region Метод - получение списка параметров из выбрраной группы. (Реализация - selectedItem листбокса Списка групп из ФОПа)
        private void GetParameters(SharedGroupDto group)
        {
            sharedParamsDto = group.SharedParametersDto;
            SharedParamsDto = CollectionViewSource.GetDefaultView(sharedParamsDto);
            SharedParamsDto.Refresh();
            //ApplyEvent.Raise();
        }
        #endregion

        #region Комманда - добавление параметра в список новых параметров
        private void OnAddParametersCommandExecutde(object obj)
        {


            if (addSharedParamsDto.Count == 0)
            {
                addSharedParamsDto.Add(obj as SharedParameterDto);
            }
            else
            {
                if (addSharedParamsDto?.FirstOrDefault(x => x.Name == (obj as SharedParameterDto).Name) == null)
                {
                    addSharedParamsDto.Add(obj as SharedParameterDto);
                }
            }

            AddSharedParamDto = CollectionViewSource.GetDefaultView(addSharedParamsDto);
            AddSharedParamDto.Refresh();
        }

        private bool CanAddParametersCommandExecute(object p) => true;
        #endregion
        /*listBox списка параметров для загрузки в семейство~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        #region КоллекшенВью для ListBox нового списка параметров для добавления в семейство
        private ICollectionView addSharedParamDto;
        

        public ICollectionView AddSharedParamDto
        {
            get => addSharedParamDto;
            set => Set(ref addSharedParamDto, value);
        }
        #endregion

        #region Цвет селектедИтема (не работает)
        public Brush SelectedItemColor
        {
            get => selectedItemColor;
            set => Set(ref selectedItemColor, value);
        } 
        #endregion
        /*Методы по добавлению и удалению параметров в семейсмтво и из списка~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        #region Метод - добавить параметры в семейство
        private void OnAddPatamIntoProjectCommandExecutde(object obj)
        {
            ExEventAddParameters.parameterDtos = addSharedParamsDto;
            exEventAddParameters.Raise();
        }

        private bool CanAddPatamIntoProjectCommandExecute(object p)
        {
            if (addSharedParamsDto.Count != 0)
                return true;
            else
                return false;

        }
        #endregion

        #region Метод - очистить список параметров

        private void OnClearParametersListCommandExecutde(object obj)
        {
            addSharedParamsDto.Clear();
            AddSharedParamDto = CollectionViewSource.GetDefaultView(addSharedParamsDto);
            AddSharedParamDto.Refresh();
        }

        private bool CanClearParametersListCommandExecute(object p)
        {
            if (addSharedParamsDto.Count != 0)
                return true;
            else
                return false;

        }
        #endregion
        /*Шапка выгрузки в XML наборов параметров~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        #region ТекстБлок. Имя нового набора параметров
        public string ParamSetName { get; set; }
        #endregion

        #region Метод - Добавить новый набор параметров
        private void OnAddParametersSetCommandExecutde(object obj)
        {
            ParametersSetDto _parametersSetDto = new ParametersSetDto();
            _parametersSetDto.Name = ParamSetName;
            try
            {
                foreach (var aa in addSharedParamsDto)
                {
                    _parametersSetDto.SharedParametersDtoList.Add(aa);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            parametersSetDto.Add(_parametersSetDto);
            //GetParametersSet("");
            File.Delete(@"C:\ProgramData\Autodesk\Revit\Addins\2021\ParametersList.xml");
            mainModel.ParamsXmlSerializer(parametersSetDto);
        }
        private bool CanAddParametersSetCommandExecute(object p)
        {
            if (ParamSetName != null && ParamSetName != "" && ParamSetName != " ")
                return true;
            else
                return false;

        }
        #endregion

        #region КоллекшенВью для списка набора параметров
        private ICollectionView parametersSet;
        public ICollectionView ParametersSet
        {
            get => parametersSet;
            set => Set(ref parametersSet, value);
        }
        #endregion

        #region Метод обновление UI для набора параметров
        public void ChangeUIParametersSet()
        {
            ParametersSet = CollectionViewSource.GetDefaultView(parametersSetDto);
            ParametersSet.Refresh();

        }
        #endregion

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    }
}
