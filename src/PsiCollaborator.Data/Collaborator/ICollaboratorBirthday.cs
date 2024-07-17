using System;

namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorBirthday
    {
        int CollaboratorId { get; set; }
        DateTime DateOfBirth { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}