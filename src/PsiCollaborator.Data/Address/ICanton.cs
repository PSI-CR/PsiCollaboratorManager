namespace PsiCollaborator.Data.Address
{
    public interface ICanton
    {
        int CantonId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int ProvinceId { get; set; }
    }
}