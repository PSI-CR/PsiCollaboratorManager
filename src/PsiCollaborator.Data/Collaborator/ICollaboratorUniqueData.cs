namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorUniqueData
    {
        int CollaboratorId { get; set; }
        string DNICollaborator { get; set; }
        string Email { get; set; }
        int OperatorNumber { get; set; }
        string Telephone1 { get; set; }
    }
}