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
        public CollaboratorBase Map(CollaboratorBaseModel source)
        {
            if (source == null) return null;

            return new CollaboratorBase
            {
                CollaboratorId = source.CollaboratorId,
                OperatorNumber = source.OperatorNumber,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DNICollaborator = source.DNICollaborator,
                DateOfBirth = source.DateOfBirth,
                Gender = source.Gender == "Femenino" ? 1 : 2,
                Parent = source.Parent,
                MaritalStatusId = source.MaritalStatus ? 1 : 2,
                Telephone1 = source.Telephone1,
                Telephone2 = source.Telephone2,
                ProvinceName = source.ProvinceName,
                CantonName = source.CantonName,
                DistrictName = source.DistrictName,
                AddressLine = source.AddressLine
            };
        }

        public CollaboratorBaseModel Map(CollaboratorBase source)
        {
            if (source == null) return null;

            return new CollaboratorBaseModel
            {
                CollaboratorId = source.CollaboratorId,
                OperatorNumber = source.OperatorNumber,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DNICollaborator = source.DNICollaborator,
                DateOfBirth = source.DateOfBirth,
                Gender = source.Gender == 1 ? "Femenino" : "Masculino",
                Parent = source.Parent,
                MaritalStatus = source.MaritalStatusId == 1,
                Telephone1 = source.Telephone1,
                Telephone2 = source.Telephone2,
                ProvinceName = source.ProvinceName,
                CantonName = source.CantonName,
                DistrictName = source.DistrictName,
                AddressLine = source.AddressLine
            };
        }
        public CollaboratorFull Map(CollaboratorFullModel source)
        {
            if (source == null) return null;
            var emergencyContacts = source.EmergencyContacts?
                .Select(contact => $"{contact.FirstName},{contact.LastName},{contact.Relationship},{contact.Telephone},{contact.Telephone2}")
                .ToArray();
            return new CollaboratorFull
            {
                CollaboratorId = source.CollaboratorId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                OperatorNumber = source.OperatorNumber,
                Email = source.Email,
                DNICollaborator = source.DNICollaborator,
                DateOfBirth = source.DateOfBirth == null ? new DateTime() : (DateTime)source.DateOfBirth,
                Gender = source.Gender,
                Parent = source.Parent,
                MaritalStatusId = source.MaritalStatus ? 1 : 2,
                Telephone1 = source.Telephone1,
                Telephone2 = source.Telephone2,
                CurriculumFile = source.CurriculumFile,
                RFIDCode = source.RFIDCode,
                NeedPasswordChange = source.NeedPasswordChange,
                IsLockedOut = source.IsLockedOut,
                LockOutEndTime = source.LockOutEndTime,
                Password = source.Password,
                CreateUserAccount = source.CreateUserAccount,
                IsActive = source.IsActive,
                DistrictId = source.DistrictId,
                CantonId = source.CantonId,
                ProvinceId = source.ProvinceId,
                AddressLine = source.AddressLine,
                BankId = source.BankId,
                CurrencyTypeId = source.CurrencyTypeId,
                NumberBankAccount = source.NumberBankAccount,
                IBANAccount = source.IBANAccount,
                Diseases = source.Diseases,
                TakingMedications = source.TakingMedications,
                Note = source.Note,
                Picture = source.Picture,
                EmergencyContacts = emergencyContacts
            };
        }

        public CollaboratorFullModel Map(CollaboratorFull source)
        {
            if (source == null) return null;
            var emergencyContacts = source.EmergencyContacts?
                .Select(record => new EmergencyContactModel(record))
                .ToArray();
            return new CollaboratorFullModel
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
                MaritalStatus = source.MaritalStatusId == 1,
                Telephone1 = source.Telephone1,
                Telephone2 = source.Telephone2,
                CurriculumFile = source.CurriculumFile,
                RFIDCode = source.RFIDCode,
                NeedPasswordChange = source.NeedPasswordChange,
                IsLockedOut = source.IsLockedOut,
                LockOutEndTime = source.LockOutEndTime,
                Password = source.Password,
                CreateUserAccount = source.CreateUserAccount,
                IsActive = source.IsActive,
                DistrictId = source.DistrictId,
                CantonId = source.CantonId,
                ProvinceId = source.ProvinceId,
                AddressLine = source.AddressLine,
                BankId = source.BankId,
                CurrencyTypeId = source.CurrencyTypeId,
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