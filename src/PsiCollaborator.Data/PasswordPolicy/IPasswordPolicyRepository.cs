namespace PsiCollaborator.Data.PasswordPolicy
{
    public interface IPasswordPolicyRepository
    {
        IPasswordPolicy GetById(int passwordPolicyId);
    }
}