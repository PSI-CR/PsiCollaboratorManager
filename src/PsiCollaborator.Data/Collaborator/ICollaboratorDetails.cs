using System;

namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorDetails
    {
        string AddressLine { get; set; }
        string BankName { get; set; }
        string CantonName { get; set; }
        int CollaboratorId { get; set; }
        string CurrencyTypeName { get; set; }
        DateTime DateOfBirth { get; set; }
        string Diseases { get; set; }
        string DistrictName { get; set; }
        string DNICollaborator { get; set; }
        string Email { get; set; }
        string[] EmergencyContacts { get; set; }
        string FirstName { get; set; }
        int Gender { get; set; }
        string IBANAccount { get; set; }
        string LastName { get; set; }
        int? MaritalStatusId { get; set; }
        string Note { get; set; }
        string NumberBankAccount { get; set; }
        int OperatorNumber { get; set; }
        bool Parent { get; set; }
        string Picture { get; set; }
        string ProvinceName { get; set; }
        string RFIDCode { get; set; }
        bool TakingMedications { get; set; }
        string Telephone1 { get; set; }
        string Telephone2 { get; set; }
    }
}