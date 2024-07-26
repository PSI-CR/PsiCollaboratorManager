using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PsiCollaboratorManager.Models.Collaborator
{
    public class CollaboratorFullModel
    {
        public int CollaboratorId { get; set; }

        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Display(Name = "Número de Operador")]
        public int OperatorNumber { get; set; }

        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Display(Name = "Cédula")]
        public string DNICollaborator { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Género")]
        public int Gender { get; set; }

        [Display(Name = "Hijos")]
        public bool Parent { get; set; }

        [Display(Name = "Casado")]
        public bool MaritalStatus { get; set; }

        [Display(Name = "Teléfono")]
        public string Telephone1 { get; set; }

        [Display(Name = "Teléfono 2")]
        public string Telephone2 { get; set; }

        [Display(Name = "Curriculum")]
        public string CurriculumFile { get; set; }

        [Display(Name = "Código RFID")]
        public string RFIDCode { get; set; }

        public bool NeedPasswordChange { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime LockOutEndTime { get; set; }

        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Crear cuenta de usuario al colaborador")]
        public bool CreateUserAccount { get; set; }

        [Display(Name = "Activo")]
        public bool IsActive { get; set; }

        [Display(Name = "Distrito")]
        public int DistrictId { get; set; }

        [Display(Name = "Cantón")]
        public int CantonId { get; set; }

        [Display(Name = "Provincia")]
        public int ProvinceId { get; set; }

        [Display(Name = "Dirección Exacta")]
        public string AddressLine { get; set; }

        [Display(Name = "Banco")]
        public int BankId { get; set; }

        [Display(Name = "Tipo de Moneda")]
        public int CurrencyTypeId { get; set; }

        [Display(Name = "Número de cuenta")]
        public string NumberBankAccount { get; set; }

        [Display(Name = "IBAN")]
        public string IBANAccount { get; set; }

        [Display(Name = "Padecimientos diagnosticados")]
        public string Diseases { get; set; }

        [Display(Name = "Está en medicación")]
        public bool TakingMedications { get; set; }

        [Display(Name = "Notas Médicas")]
        public string Note { get; set; }

        [Display(Name = "Foto")]
        public string Picture { get; set; }

        [Display(Name = "Contactos de Emergencia")]
        public EmergencyContactModel[] EmergencyContacts { get; set; }
    }
}