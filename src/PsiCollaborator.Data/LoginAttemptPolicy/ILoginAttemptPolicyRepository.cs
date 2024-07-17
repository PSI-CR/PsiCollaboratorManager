namespace PsiCollaborator.Data.LoginAttemptPolicy
{
    public interface ILoginAttemptPolicyRepository
    {
        ILoginAttemptPolicy GetById(int logginAttemptPolicyId);
    }
}