namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorName
    {
        int CollaboratorId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}