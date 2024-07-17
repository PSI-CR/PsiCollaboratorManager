using System.Net;

namespace PsiCollaborator.Data.LoginAttemptUser
{
    public interface ILoginAttemptUser
    {
        string ApplicationName { get; set; }
        IPAddress IpAddress { get; set; }
        bool IsSuccess { get; set; }
        int UserAccountId { get; set; }
    }
}