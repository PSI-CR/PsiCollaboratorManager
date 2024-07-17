namespace PsiCollaborator.Data.LoginAttemptCollaborator
{
    public interface ILoginAttemptCollaboratorRepository
    {
        int Save(ILoginAttemptCollaborator loginCollaboratorAttempt);
    }
}