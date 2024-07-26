using System;

namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorBase
    {
        string AddressLine { get; set; }
        string CantonName { get; set; }
        int CollaboratorId { get; set; }
        DateTime DateOfBirth { get; set; }
        string DistrictName { get; set; }
        string DNICollaborator { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        int Gender { get; set; }
        string LastName { get; set; }
        int? MaritalStatusId { get; set; }
        int OperatorNumber { get; set; }
        bool Parent { get; set; }
        string ProvinceName { get; set; }
        string Telephone1 { get; set; }
        string Telephone2 { get; set; }
    }
}