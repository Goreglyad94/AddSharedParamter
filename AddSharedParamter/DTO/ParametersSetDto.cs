using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddSharedParamter.DTO
{
    [Serializable]
    public class ParametersSetDto
    {
        public ParametersSetDto()
        {

        }
        public string Name { get; set; }
        public ParametersSetDto(string name)
        {
            Name = name;
        }
        public List<SharedParameterDto> SharedParametersDtoList { get; set; } = new List<SharedParameterDto>();
    }
}
