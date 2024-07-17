using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.BankAccount
{
    public class BankAccountRepository : DbMapper, IBankAccountRepository
    {
        public List<CurrencyType> GetAllCurrencyType()
        {
            return ExecuteList<CurrencyType>("Select_All_Currencytype").ToList();
        }
        public List<Bank> GetAllBank()
        {
            return ExecuteList<Bank>("Select_All_Bank").ToList();
        }
    }
}
