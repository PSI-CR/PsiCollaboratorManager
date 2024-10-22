using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Collaborator
{
    public class CollaboratorFull : ICollaboratorFull
    {
        public int CollaboratorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OperatorNumber { get; set; }
        public string Email { get; set; }
        public string DNICollaborator { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public bool Parent { get; set; }
        public int? MaritalStatusId { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string CurriculumFile { get; set; }
        public string RFIDCode { get; set; }
        public bool NeedPasswordChange { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime LockOutEndTime { get; set; }
        public string Password { get; set; }
        public bool CreateUserAccount { get; set; }
        public bool IsActive { get; set; }
        public int DistrictId { get; set; }
        public int CantonId { get; set; }
        public int ProvinceId { get; set; }
        public string AddressLine { get; set; }
        public int BankId { get; set; }
        public int CurrencyTypeId { get; set; }
        public string NumberBankAccount { get; set; }
        public string IBANAccount { get; set; }
        public string Diseases { get; set; }
        public bool TakingMedications { get; set; }
        public string Note { get; set; }
        public string Picture { get; set; }
        public string[] EmergencyContacts { get; set; }
        //public DateTime PasswordChangedDate { get; set; }
        public void SetDefaultValues()
        {
            CollaboratorId = CollaboratorId != 0 ? CollaboratorId : 0;
            OperatorNumber = OperatorNumber != 0 ? OperatorNumber : 0;
            FirstName = FirstName ?? "";
            LastName = LastName ?? "";
            Email = Email ?? "";
            DNICollaborator = DNICollaborator ?? "";
            DateOfBirth = DateOfBirth != DateTime.MinValue ? DateOfBirth : DateTime.MinValue;
            Gender = Gender != 0 ? Gender : 1;
            Parent = Parent;
            MaritalStatusId = MaritalStatusId ?? 1;
            Telephone1 = Telephone1 ?? "";
            Telephone2 = Telephone2 ?? "";
            CurriculumFile = CurriculumFile ?? "";
            RFIDCode = RFIDCode ?? "";
            NeedPasswordChange = NeedPasswordChange;
            IsLockedOut = IsLockedOut;
            LockOutEndTime = LockOutEndTime != DateTime.MinValue ? LockOutEndTime : DateTime.MinValue;
            IsActive = IsActive;
            DistrictId = DistrictId != 0 ? DistrictId : 1;
            CantonId = CantonId != 0 ? CantonId : 101;
            ProvinceId = ProvinceId != 0 ? ProvinceId : 10101;
            AddressLine = AddressLine ?? "";
            BankId = BankId != 0 ? BankId : 1;
            CurrencyTypeId = CurrencyTypeId != 0 ? CurrencyTypeId : 1;
            NumberBankAccount = NumberBankAccount ?? "";
            IBANAccount = IBANAccount ?? "";
            Diseases = Diseases ?? "";
            TakingMedications = TakingMedications;
            Note = Note ?? "";
            Picture = Picture ?? "";
            EmergencyContacts = EmergencyContacts ?? new string[0];
        }
    }
}
