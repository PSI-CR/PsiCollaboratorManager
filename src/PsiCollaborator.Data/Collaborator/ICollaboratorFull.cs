using System;

namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorFull
    {
        string AddressLine { get; set; }
        int BankId { get; set; }
        int CantonId { get; set; }
        int CollaboratorId { get; set; }
        bool CreateUserAccount { get; set; }
        int CurrencyTypeId { get; set; }
        string CurriculumFile { get; set; }
        DateTime DateOfBirth { get; set; }
        string Diseases { get; set; }
        int DistrictId { get; set; }
        string DNICollaborator { get; set; }
        string Email { get; set; }
        string[] EmergencyContacts { get; set; }
        string FirstName { get; set; }
        int Gender { get; set; }
        string IBANAccount { get; set; }
        bool IsActive { get; set; }
        bool IsLockedOut { get; set; }
        string LastName { get; set; }
        DateTime LockOutEndTime { get; set; }
        int? MaritalStatusId { get; set; }
        bool NeedPasswordChange { get; set; }
        string Note { get; set; }
        string NumberBankAccount { get; set; }
        int OperatorNumber { get; set; }
        bool Parent { get; set; }
        string Password { get; set; }
        string Picture { get; set; }
        int ProvinceId { get; set; }
        string RFIDCode { get; set; }
        bool TakingMedications { get; set; }
        string Telephone1 { get; set; }
        string Telephone2 { get; set; }

        void SetDefaultValues();
        //DateTime PasswordChangedDate { get; set; }
    }
}