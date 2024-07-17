using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.BankAccount
{
    public class CurrencyType : ICurrencyType
    {
        public int CurrencyTypeId { get; set; }
        public string Name { get; set; }
    }
}
