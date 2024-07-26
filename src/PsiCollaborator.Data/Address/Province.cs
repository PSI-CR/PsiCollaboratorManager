﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Address
{
    public class Province : IProvince
    {
        public int ProvinceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
