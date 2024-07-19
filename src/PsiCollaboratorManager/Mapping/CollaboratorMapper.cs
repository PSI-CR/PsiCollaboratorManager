using PsiCollaborator.Data.Collaborator;
using PsiCollaboratorManager.Models.Collaborator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Mapping
{
    public class CollaboratorMapper
    {
        public CollaboratorDetailsModel Map(CollaboratorDetails source)
        {
            if (source == null) return null;

            var emergencyContacts = source.EmergencyContacts?
                .Select(record => new EmergencyContactModel(record))
                .ToList();

            return new CollaboratorDetailsModel
            {
                CollaboratorId = source.CollaboratorId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                OperatorNumber = source.OperatorNumber,
                Email = source.Email,
                DNICollaborator = source.DNICollaborator,
                DateOfBirth = source.DateOfBirth,
                Gender = source.Gender,
                Parent = source.Parent,
                MaritalStatusId = source.MaritalStatusId,
                Telephone1 = source.Telephone1,
                Telephone2 = source.Telephone2,
                RFIDCode = source.RFIDCode,
                DistrictName = source.DistrictName,
                CantonName = source.CantonName,
                ProvinceName = source.ProvinceName,
                AddressLine = source.AddressLine,
                BankName = source.BankName,
                CurrencyTypeName = source.CurrencyTypeName,
                NumberBankAccount = source.NumberBankAccount,
                IBANAccount = source.IBANAccount,
                Diseases = source.Diseases,
                TakingMedications = source.TakingMedications,
                Note = source.Note,
                Picture = source.Picture,
                EmergencyContacts = emergencyContacts
            };
        }

        public CollaboratorDetails Map(CollaboratorDetailsModel source)
        {
            if (source == null) return null;

            var emergencyContacts = source.EmergencyContacts?
                .Select(contact => $"{contact.FirstName},{contact.LastName},{contact.Relationship},{contact.Telephone},{contact.Telephone2}")
                .ToArray();

            return new CollaboratorDetails
            {
                CollaboratorId = source.CollaboratorId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                OperatorNumber = source.OperatorNumber,
                Email = source.Email,
                DNICollaborator = source.DNICollaborator,
                DateOfBirth = source.DateOfBirth,
                Gender = source.Gender,
                Parent = source.Parent,
                MaritalStatusId = source.MaritalStatusId,
                Telephone1 = source.Telephone1,
                Telephone2 = source.Telephone2,
                RFIDCode = source.RFIDCode,
                DistrictName = source.DistrictName,
                CantonName = source.CantonName,
                ProvinceName = source.ProvinceName,
                AddressLine = source.AddressLine,
                BankName = source.BankName,
                CurrencyTypeName = source.CurrencyTypeName,
                NumberBankAccount = source.NumberBankAccount,
                IBANAccount = source.IBANAccount,
                Diseases = source.Diseases,
                TakingMedications = source.TakingMedications,
                Note = source.Note,
                Picture = source.Picture,
                EmergencyContacts = emergencyContacts
            };
        }

    }
}