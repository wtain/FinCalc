using System.Collections.Generic;

namespace FCHA.Interfaces
{
    public interface IAccountsManager
    {
        void AddAccount(ref Account account);
        long AddAccount(string name, string currency, long ownerPersonId, AccountType type);
        void DeleteAccount(Account account);
        IEnumerable<Account> EnumAllAccounts();
        IEnumerable<Account> EnumUserAccounts(Person person);
        Account GetAccount(long accountId);
        AccountBalance GetAccountBalance(long accountId);
        void UpdateAccount(Account account);
    }
}