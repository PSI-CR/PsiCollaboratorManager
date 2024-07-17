using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Address
{
    public class Canton : ICanton
    {
        public int CantonId { get; set; }
        public int ProvinceId { get; set; }
        public string CantonName { get; set; }
        public string Description { get; set; }
    }
}
