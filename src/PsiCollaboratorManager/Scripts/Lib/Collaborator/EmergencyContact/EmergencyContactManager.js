function DisplayEmergencyContacts() {
    let emergencyContacts = extension.GetContacts();
    let tableBody = $("#EmergencyContactTable tbody");
    tableBody.empty();
    emergencyContacts.forEach((contact, index) => {
        let row = '<tr>';
        if(index % 2 == 0) row = '<tr class="selectedRow">'
        row += '<td>' + contact.FirstName + '</td>';
        row += '<td>' + contact.LastName + '</td>';
        row += '<td>' + contact.GetRelationshipName() + '</td>';
        row += '<td>' + contact.Telephone + '</td>';
        row += '<td>' + contact.Telephone2 + '</td>';
        row += '<td><button onclick="EditEmergencyContact(' + contact.Id + ')" type="button" class="ecButton editButton">Editar</button></td>';
        row += '<td><button onclick="DeleteEmergencyContact(' + contact.Id + ')" type="button" class="ecButton deleteButton"> Eliminar</button></td>';
        row += '</tr>';
        tableBody.append(row);
    })
}
function AddEmergencyContact() {
    let firstName = document.getElementById("EmergencyContactFirstName").value;
    let lastName = document.getElementById("EmergencyContactLastName").value;
    let relationship = document.getElementById("EmergencyContactRelationship").value;
    let telephone = document.getElementById("EmergencyContactTelephone").value;
    let telephone2 = document.getElementById("EmergencyContactTelephone2").value;
    let errorMessage = validateEmergencyContact(firstName, lastName, relationship, telephone, telephone2);
    if (errorMessage != '') {
        new Messi(errorMessage, { title: 'Error', titleClass: 'anim error', modal: true }).show();
        return;
    }
    extension.AddContact(firstName, lastName, relationship, telephone, telephone2);
    DisplayEmergencyContacts();
    setInputs("", "", 0, "", "");
}
function EditEmergencyContact(id) {
    let contact = extension.GetContactById(id);
    if (contact == null) return;
    DeleteEmergencyContact(id);
    setInputs(contact.FirstName, contact.LastName, contact.Relationship, contact.Telephone, contact.Telephone2);
}
function DeleteEmergencyContact(id) {
    extension.DeleteContact(id);
    DisplayEmergencyContacts();
}
function validateEmergencyContact(firstName, lastName, relationship, telephone, telephone2) {
    let errorMessage = '';
    if (!firstName.trim()) errorMessage += "First name is required.\n";
    if (!lastName.trim()) errorMessage += "Last name is required.\n";
    if (!telephone.trim()) errorMessage += "Primary telephone number is required.\n";

    let phoneRegex = /^\d{8}$/;
    if (!phoneRegex.test(telephone)) errorMessage += "Primary telephone number must be exactly 8 digits.\n";
    if (telephone2 && !phoneRegex.test(telephone2)) errorMessage += "Secondary telephone number must be exactly 8 digits if provided.\n";

    let relationshipValue = parseInt(relationship);
    if (isNaN(relationshipValue) || relationshipValue < 0 || relationshipValue > 10) errorMessage += "Relationship must be a number between 0 and 10.\n";

    return errorMessage;
}
function setInputs(firstName, lastName, relationship, telephone, telephone2) {
    document.getElementById("EmergencyContactFirstName").value = firstName;
    document.getElementById("EmergencyContactLastName").value = lastName;
    document.getElementById("EmergencyContactRelationship").value = relationship;
    document.getElementById("EmergencyContactTelephone").value = telephone;
    document.getElementById("EmergencyContactTelephone2").value = telephone2;

}
let addButton = document.getElementById("AddContactButton");
addButton.addEventListener("click", e => {
    AddEmergencyContact();
});
