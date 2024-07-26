using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace PsiCollaboratorManager.Models.Collaborator
{
    public class EmergencyContactModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Relationship { get; set; }
        public string Telephone {  get; set; }
        public string Telephone2 {  get; set; }
        public EmergencyContactModel(string record) {
            string[] fields = record.Split(',');
            FirstName = fields[0];
            LastName = fields[1];
            Relationship = fields[2];
            Telephone = fields[3];
            Telephone2 = fields[4];
        }
        public string GetRecord()
        {
            return $"{FirstName},{LastName},{Relationship},{Telephone},{Telephone2}";
        }
        public string ToJson()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(this);

        }
        public EmergencyContactModel() { }
    }
}