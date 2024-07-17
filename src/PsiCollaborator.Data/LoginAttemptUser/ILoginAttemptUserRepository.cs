namespace PsiCollaborator.Data.LoginAttemptUser
{
    public interface ILoginAttemptUserRepository
    {
        int Insert(ILoginAttemptUser loginUserAttempt);
    }
}