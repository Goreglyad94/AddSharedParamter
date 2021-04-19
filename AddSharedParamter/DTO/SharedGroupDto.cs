using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddSharedParamter.DTO
{
    [Serializable]
    public class SharedGroupDto
    {
        public SharedGroupDto()
        {

        }
        public string Name { get; set; }
        public List<SharedParameterDto> SharedParametersDto { get; set; } = new List<SharedParameterDto>();

        public bool Checked { get; set; }
    }
}
