using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FCHA.Interfaces;
using FCHA.DataTypes;

namespace FCHA
{
    public interface IAccountancyApplication
    {
        ObservableCollection<AccountViewModel> Accounts { get; }
        ObservableCollection<CategoryViewModel> Categories { get; }
        ObservableCollection<ExpenseViewModel> Expenses { get; }
        IFXRateSource LiveSource { get; }
        ObservableCollection<OlapView> Reports { get; set; }
        AccountViewModel SelectedAccount { get; set; }
        DateTime SelectedDate { get; set; }
        OlapView SelectedReport { get; set; }
        PersonViewModel SelectedUser { get; set; }
        ObservableCollection<PersonViewModel> Users { get; }
        CategoryViewModel VirtualRoot { get; }

        void AddAccount(AccountViewModel account);
        CategoryViewModel AddCategory(string name, CategoryType type);
        CategoryViewModel AddChildCategory(CategoryViewModel category, string name, CategoryType type);
        long AddExpense(ExpenseViewModel expense);
        void AddPerson(PersonViewModel person);
        void ChangeCategory(CategoryViewModel category, string newName, CategoryType type);
        AccountViewModel CreateAccount(PersonViewModel person);
        void DeleteAccount(AccountViewModel account);
        void DeleteExpense(ExpenseViewModel expense);
        IEnumerable<AccountViewModel> EnumUserAccounts(PersonViewModel person);
        AccountViewModel GetAccount(long accountId);
        AccountViewModel GetAccount(Account a);
        AccountViewModel GetAccount(PersonViewModel person, Account a);
        AccountBalance GetAccountState(AccountViewModel account);
        CategoryViewModel GetCategory(long categoryId);
        PersonViewModel GetPerson(Person p);
        PersonViewModel GetPerson(long personId);
        PersonViewModel NewPerson();
        void RemoveCategory(CategoryViewModel category);
        void RemovePerson(PersonViewModel person);
        void UpdateAccount(AccountViewModel account);
        void UpdateExpense(ExpenseViewModel expense);
        void UpdatePerson(PersonViewModel person);
        void Transfer(AccountViewModel source, AccountViewModel target, long amount);
        void TransferWithConversion(AccountViewModel source, AccountViewModel target, long amount, double rate);
        void ChangeCategoryOrder(CategoryViewModel category, CategoryViewModel categoryBefore);
        void ChangeCategoryParent(CategoryViewModel category, CategoryViewModel newParent);
        CategoryViewModel DefaultIncomeCategory { get; }
        CategoryViewModel DefaultExpenseCategory { get; }
        CategoryViewModel DefaultTransferOutCategory { get; }
        CategoryViewModel DefaultTransferInCategory { get; }
    }
}