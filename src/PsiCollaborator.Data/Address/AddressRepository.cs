using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Address
{
    public class AddressRepository : DbMapper, IAddressRepository
    {
        public List<Province> GetAllProvinces()
        {
            return ExecuteList<Province>("Select_All_Provinces").ToList();
        }
        public List<Canton> GetAllCantonsByProvince(int provinceId)
        {
            return ExecuteListWithParameters<Canton>("Select_All_Cantons_By_Province", new List<DbParameter>() { new DbParameter("param_provinceId", ParameterDirection.Input, provinceId) }).ToList();
        }
        public List<District> GetAllDistrictsByCanton(int cantonId)
        {
            return ExecuteListWithParameters<District>("Select_All_Districts_By_Canton", new List<DbParameter>() { new DbParameter("param_cantonId", ParameterDirection.Input, cantonId) }).ToList();
        }
    }
}
