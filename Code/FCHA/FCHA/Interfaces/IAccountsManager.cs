using System.Collections.Generic;

namespace FCHA.Interfaces
{
    public interface IAccountsManager
    {
        void Add(ref Account account);
        long AddAccount(string name, string currency, long ownerPersonId, AccountType type);
        void Delete(Account account);
        IEnumerable<Account> EnumAllAccounts();
        IEnumerable<Account> EnumUserAccounts(Person person);
        Account Get(long accountId);
        AccountBalance GetBalance(long accountId);
        void Update(Account account);
    }
}