using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Collaborator
{
    public class CollaboratorDetailsModel
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
        public string RFIDCode { get; set; }
        public string DistrictName { get; set; }
        public string CantonName { get; set; }
        public string ProvinceName { get; set; }
        public string AddressLine { get; set; }
        public string BankName { get; set; }
        public string CurrencyTypeName { get; set; }
        public string NumberBankAccount { get; set; }
        public string IBANAccount { get; set; }
        public string Diseases { get; set; }
        public bool TakingMedications { get; set; }
        public string Note { get; set; }
        public string Picture { get; set; }
        public List<EmergencyContactModel> EmergencyContacts { get; set; }
    }
}