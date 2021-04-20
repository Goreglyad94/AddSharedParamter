using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using AddSharedParamter.DTO;

namespace AddSharedParamter.Models.XmlModel
{
    class MainModel
    {
        /// <summary>
        /// Выгрузка в XML
        /// </summary>
        /// <param name="ParamsSetDto"></param>
        public void ParamsXmlSerializer(List<ParametersSetDto> ParamsSetDto)
        {
            try
            {
                var xml = new XmlSerializer(typeof(List<ParametersSetDto>));

                using (var fs = new FileStream(@"C:\ProgramData\Autodesk\Revit\Addins\2021\ParametersList.xml", FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, ParamsSetDto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
        /// <summary>
        /// Чтение из XML
        /// </summary>
        /// <param name="ParamsSetDto"></param>
        public List<ParametersSetDto> ParamsXmlDeserializer()
        {
            try
            {
                var xml = new XmlSerializer(typeof(List<ParametersSetDto>));

                using (var fs = new FileStream(@"C:\ProgramData\Autodesk\Revit\Addins\2021\ParametersList.xml", FileMode.OpenOrCreate))
                {
                    List<ParametersSetDto> prs = (List<ParametersSetDto>)xml.Deserialize(fs);
                    return prs;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }

        }
    }
}
