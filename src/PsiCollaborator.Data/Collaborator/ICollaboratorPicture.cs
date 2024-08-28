namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorPicture
    {
        int CollaboratorId { get; set; }
        string Firstname { get; set; }
        string Lastname { get; set; }
        string Picture { get; set; }
    }
}