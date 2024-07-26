class EmergencyContact {
    constructor(id, firstName, lastName, relationship, telephone, telephone2) {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Relationship = relationship;
        this.Telephone = telephone;
        this.Telephone2 = telephone2;
    }
    GetRelationshipName() {
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
        return RelationshipEnum[this.Relationship];
    }
}