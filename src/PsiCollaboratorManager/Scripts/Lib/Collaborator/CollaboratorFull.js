class CollaboratorFull {
    constructor(
        collaboratorId,
        firstName,
        lastName,
        operatorNumber,
        email,
        dniCollaborator,
        dateOfBirth,
        gender,
        parent,
        maritalStatus,
        telephone1,
        telephone2,
        curriculumFile,
        rfidCode,
        needPasswordChange,
        isLockedOut,
        lockOutEndTime,
        password,
        createUserAccount,
        isActive,
        districtId,
        cantonId,
        provinceId,
        addressLine,
        bankId,
        currencyTypeId,
        numberBankAccount,
        ibanAccount,
        diseases,
        takingMedications,
        note,
        picture,
        emergencyContacts
    ) {
        this.CollaboratorId = collaboratorId;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.OperatorNumber = operatorNumber;
        this.Email = email;
        this.DNICollaborator = dniCollaborator;
        this.DateOfBirth = dateOfBirth;
        this.Gender = gender;
        this.Parent = parent;
        this.MaritalStatus = maritalStatus;
        this.Telephone1 = telephone1;
        this.Telephone2 = telephone2;
        this.CurriculumFile = curriculumFile;
        this.RFIDCode = rfidCode;
        this.NeedPasswordChange = needPasswordChange;
        this.IsLockedOut = isLockedOut;
        this.LockOutEndTime = lockOutEndTime;
        this.Password = password;
        this.CreateUserAccount = createUserAccount;
        this.IsActive = isActive;
        this.DistrictId = districtId;
        this.CantonId = cantonId;
        this.ProvinceId = provinceId;
        this.AddressLine = addressLine;
        this.BankId = bankId;
        this.CurrencyTypeId = currencyTypeId;
        this.NumberBankAccount = numberBankAccount;
        this.IBANAccount = ibanAccount;
        this.Diseases = diseases;
        this.TakingMedications = takingMedications;
        this.Note = note;
        this.Picture = picture;
        this.EmergencyContacts = emergencyContacts;
    }
    static FromGui(emergencyContacts, base64Image) {
        const getValueById = (id, type = 'string') => {
            const element = document.getElementById(id);
            if (!element) return null;

            switch (type) {
                case 'int':
                    return parseInt(element.value, 10) || 0;
                case 'bool':
                    return element.checked;
                case 'date':
                    console.log(element.value);
                    console.log(new Date(element.value.split('-').join('/')));
                    return element.value ? new Date(element.value.split('-').join('/')) : null;
                default:
                    return element.value || '';
            }
        };

        return new CollaboratorFull(
            getValueById('CollaboratorId', 'int'),
            getValueById('FirstName'),
            getValueById('LastName'),
            getValueById('OperatorNumber', 'int'),
            getValueById('Email'),
            getValueById('DNICollaborator'),
            getValueById('DateOfBirth', 'date'),
            document.getElementById('rbtnFemale').checked ? 1 : 2,
            getValueById('rbtnYesMarital', 'bool'),
            getValueById('rbtnYesParent', 'bool'),
            getValueById('Telephone1'),
            getValueById('Telephone2'),
            "",
            getValueById('RFIDCode'),
            false,
            false,
            new Date(),
            getValueById('Password'),
            getValueById('CreateUserAccount', 'bool'),
            getValueById('IsActive', 'bool'),
            getValueById('DistrictId', 'int'),
            getValueById('CantonId', 'int'),
            getValueById('ProvinceId', 'int'),
            getValueById('AddressLine'),
            getValueById('BankId', 'int'),
            getValueById('CurrencyTypeId', 'int'),
            getValueById('NumberBankAccount'),
            getValueById('IBANAccount'),
            getValueById('Diseases'),
            getValueById('TakingMedications', 'bool'),
            getValueById('Note'),
            base64Image,
            emergencyContacts
        );
    }
}
