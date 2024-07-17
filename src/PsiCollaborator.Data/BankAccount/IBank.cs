namespace PsiCollaborator.Data.BankAccount
{
    public interface IBank
    {
        string AccountPattern { get; set; }
        string Acronym { get; set; }
        int BankId { get; set; }
        string Name { get; set; }
    }
}