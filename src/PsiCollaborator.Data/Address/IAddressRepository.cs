using System.Collections.Generic;

namespace PsiCollaborator.Data.Address
{
    public interface IAddressRepository
    {
        List<Canton> GetAllCantonsByProvince(int provinceId);
        List<District> GetAllDistrictsByCanton(int cantonId);
        List<Province> GetAllProvinces();
    }
}