namespace PsiCollaborator.Data.Address
{
    public interface ICanton
    {
        int CantonId { get; set; }
        string CantonName { get; set; }
        string Description { get; set; }
        int ProvinceId { get; set; }
    }
}