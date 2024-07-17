using System.Net;

namespace PsiCollaborator.Data.LoginAttemptCollaborator
{
    public interface ILoginAttemptCollaborator
    {
        string ApplicationName { get; set; }
        int CollaboratorId { get; set; }
        IPAddress IpAddress { get; set; }
        bool IsSuccess { get; set; }
    }
}