using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Address
{
    public class District : IDistrict
    {
        public int DistrictId { get; set; }
        public int CantonId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
