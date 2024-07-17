using System.Collections.Generic;

namespace PsiCollaborator.Data.BankAccount
{
    public interface IBankAccountRepository
    {
        List<Bank> GetAllBank();
        List<CurrencyType> GetAllCurrencyType();
    }
}