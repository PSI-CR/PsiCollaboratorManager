using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.BankAccount
{
    public class Bank : IBank
    {
        public int BankId { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public string AccountPattern { get; set; }
    }
}
