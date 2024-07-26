namespace PsiCollaborator.Data.Address
{
    public interface IDistrict
    {
        int CantonId { get; set; }
        string Description { get; set; }
        int DistrictId { get; set; }
        string Name { get; set; }
    }
}