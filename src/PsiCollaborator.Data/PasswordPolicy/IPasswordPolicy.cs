namespace PsiCollaborator.Data.PasswordPolicy
{
    public interface IPasswordPolicy
    {
        bool CheckIfContainsUserInfo { get; set; }
        bool CheckIfIsCommonlyUsed { get; set; }
        bool CheckIfIsRecent { get; set; }
        int MinLength { get; set; }
        int PasswordDuration { get; set; }
        int PasswordPolicyId { get; set; }
        bool UseLowerCase { get; set; }
        bool UseNumbers { get; set; }
        bool UseSymbols { get; set; }
        bool UseUpperCase { get; set; }
    }
}