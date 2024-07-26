function GetCollaboratorDetails(collaboratorId, modalId) {
    $.ajax({
        url: '/Collaborator/GetDetails?collaboratorId=' + collaboratorId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            OpenModal(modalId);

            if (data.Picture) {
                if (data.Picture.startsWith('data:image/jpeg;base64,')) $('#' + modalId + ' .modalHeaderPhoto').attr('src', data.Picture);
                else $('#' + modalId + ' .modalHeaderPhoto').attr('src', 'data:image/jpeg;base64,' + data.Picture);
            } else {
                $('#' + modalId + '  .modalHeaderPhoto').attr('src', '/Images/DefaultCollaborator.jpg');
            }

            $('#' + modalId + '  .modalHeaderTitle').html(data.FirstName + ' ' + data.LastName);
            $('#' + modalId + '  #modal-DNICollaborator').html(data.DNICollaborator);
            $('#' + modalId + '  #modal-dateofbirth').html(data.DateOfBirth = new Date(data.DateOfBirth.match(/\d+/)[0] * 1).toLocaleDateString("en-US"));
            $('#' + modalId + '  #modal-parent').html(data.Parent ? "Si" : "No");
            $('#' + modalId + '  #modal-maritalstatusid').html(data.MaritalStatusId == 1 ? "Si" : "No");
            $('#' + modalId + '  #modal-gender').html(data.Gender == 1 ? "Femenino" : "Masculino");
            $('#' + modalId + '  #modal-telephone1').html(data.Telephone1);
            $('#' + modalId + '  #modal-telephone2').html(data.Telephone2);
            $('#' + modalId + '  #modal-email').html(data.Email);
            $('#' + modalId + '  #modal-province').html(data.ProvinceName);
            $('#' + modalId + '  #modal-canton').html(data.CantonName);
            $('#' + modalId + '  #modal-district').html(data.DistrictName);
            $('#' + modalId + '  #modal-addressline').html(data.AddressLine);
            $('#' + modalId + '  #modal-operatornum').html(data.OperatorNumber);
            $('#' + modalId + '  #modal-rfidcode').html(data.RFIDCode);
            $('#' + modalId + '  #modal-diseases').html(data.Diseases);
            $('#' + modalId + '  #modal-takingmedications').html(data.TakingMedications ? "Si" : "No");
            $('#' + modalId + '  #modal-note').html(data.Note);
            $('#' + modalId + '  #modal-bankname').html(data.BankName);
            $('#' + modalId + '  #modal-currencytypename').html(data.CurrencyTypeName);
            $('#' + modalId + '  #modal-numberbankaccount').html(data.NumberBankAccount);
            $('#' + modalId + '  #modal-ibanaccount').html(data.IBANAccount);

            displayEmergencyContacts(modalId, data.EmergencyContacts);

        },
        error: function () {
            alert('Ha ocurrido un error al obtener los detalles del empleado.');
        }
    });
}

function displayEmergencyContacts(modalId, emergencyContacts) {
    $('#' + modalId + ' .emergencyContactsTableBody').empty();
    for (let i = 0; i < emergencyContacts.length; i++) {
        let emergencyContact = emergencyContacts[i];
        var row = "<tr>";
        row += "<td>" + emergencyContact.FirstName + "</td>";
        row += "<td>" + emergencyContact.LastName + "</td>";
        row += "<td>" + GetRelationshipName(emergencyContact.Relationship) + "</td>";
        row += "<td>" + emergencyContact.Telephone + "</td>";
        row += "<td>" + emergencyContact.Telephone2 + "</td>";
        row += "</tr>";
        $('#' + modalId + ' .emergencyContactsTableBody').append(row);
    }
}

function GetRelationshipName(relationship) {
    const RelationshipEnum = {
        0: 'Otro',
        1: 'Madre',
        2: 'Padre',
        3: 'Hermana',
        4: 'Hermano',
        5: 'Hijo',
        6: 'Hija',
        7: 'Esposo',
        8: 'Esposa',
        9: 'Abuelo',
        10: 'Abuela'
    };
    return RelationshipEnum[relationship];
}

document.addEventListener("DOMContentLoaded", function () {
    var tabs = document.querySelectorAll('ul.nav.nav-tabs li a');

    tabs.forEach(function (tab) {
        tab.addEventListener('click', function (event) {
            event.preventDefault();
            var targetId = this.getAttribute('href');
            var tabContents = document.querySelectorAll('.tab-pane');
            tabContents.forEach(function (content) {
                content.classList.remove('active');
            });
            var targetTabContent = document.querySelector(targetId);
            targetTabContent.classList.add('active');
            tabs.forEach(function (tab) {
                tab.parentElement.classList.remove('active');
            });
            this.parentElement.classList.add('active');
        });
    });
});