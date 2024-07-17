namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorOperator
    {
        int CollaboratorId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        int OperatorNumber { get; set; }
    }
}