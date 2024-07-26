class EmergencyContactExtension {
    constructor(contacts) {
        this.contacts = contacts;
        this.currentId = 1;
    }
    generateId() {
        return this.currentId++;
    }
    AddContact(firstName, lastName, relationship, telephone, telephone2) {
        const id = this.generateId();
        const newContact = new EmergencyContact(id, firstName, lastName, relationship, telephone, telephone2);
        this.contacts.push(newContact);
        return newContact;
    }
    GetContactById(id) {
        return this.contacts.find(contact => contact.Id === id) || null;
    }
    DeleteContact(id) {
        this.contacts = this.contacts.filter(contact => { return contact.Id != id; });
    }
    GetContacts() {
        return this.contacts;
    }
}