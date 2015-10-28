
using FCHA.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace FCHA
{
    public class AccountancyDatabase
        : IDisposable
        , IAccountancyDatabase
    {
        private SQLiteConnection m_connection;
        private List<OlapView> m_Reports;

        private CategoriesManager m_categoriesManager;
        private UsersManager m_usersManager;
        private AccountsManager m_accountsManager;
        private ExpensesManager m_expensesManager;

        public AccountancyDatabase(SQLiteConnection connection)
        {
            m_connection = connection;

            m_categoriesManager = new CategoriesManager(m_connection);
            m_usersManager = new UsersManager(m_connection);
            m_accountsManager = new AccountsManager(m_connection);
            m_expensesManager = new ExpensesManager(m_connection);
        }

        public IEnumerable<OlapView> Reports
        {
            get
            {
                if (null == m_Reports)
                {
                    m_Reports = new List<OlapView>();
                    m_Reports.Add(new OlapView("ExpenseByCategory", m_connection, "ExpenseByCategory", new OlapStage("Date", "Category", "Amount")));
                    m_Reports.Add(new OlapView("ExpenseByCategoryAndMonth", m_connection, "ExpenseByCategoryAndMonth", new OlapStage("Month", "Category", "Amount")));
                    m_Reports.Add(new OlapView("ExpenseByTopLevelCategoryAndMonth", m_connection, "ExpenseByTopLevelCategoryAndMonth", new OlapStage("Month", "Category", "Amount")));
                }
                return m_Reports;
            }
        }

        public void Dispose()
        {
            if (null != m_connection)
                m_connection.Dispose();
        }

        public void AddUser(ref Person person)
        {
            m_usersManager.AddUser(ref person);
        }

        public long AddUser(string name, string fullName)
        {
            return m_usersManager.AddUser(name, fullName);
        }

        public void DeleteUser(Person person)
        {
            m_usersManager.DeleteUser(person);
        }

        public IEnumerable<Person> EnumAllUsers()
        {
            return m_usersManager.EnumAllUsers();
        }

        public Person GetUser(long personId)
        {
            return m_usersManager.GetUser(personId);
        }

        public void UpdateUser(Person person)
        {
            m_usersManager.UpdateUser(person);
        }

        public long AddCategory(string name, bool isIncome)
        {
            return m_categoriesManager.AddCategory(name, isIncome);
        }

        public long AddCategory(string name, long parentId, bool isIncome)
        {
            return m_categoriesManager.AddCategory(name, parentId, isIncome);
        }

        public void DeleteCategory(Category cat)
        {
            m_categoriesManager.DeleteCategory(cat);
        }

        public IEnumerable<Category> EnumAllCategories()
        {
            return m_categoriesManager.EnumAllCategories();
        }

        public IEnumerable<Category> EnumCategoriesByParent(long parentId)
        {
            return m_categoriesManager.EnumCategoriesByParent(parentId);
        }

        public void UpdateCategory(Category cat)
        {
            m_categoriesManager.UpdateCategory(cat);
        }

        public void AddAccount(ref Account account)
        {
            m_accountsManager.AddAccount(ref account);
        }

        public long AddAccount(string name, string currency, long ownerPersonId, AccountType type)
        {
            return m_accountsManager.AddAccount(name, currency, ownerPersonId, type);
        }

        public void DeleteAccount(Account account)
        {
            m_accountsManager.DeleteAccount(account);
        }

        public IEnumerable<Account> EnumAllAccounts()
        {
            return m_accountsManager.EnumAllAccounts();
        }

        public IEnumerable<Account> EnumUserAccounts(Person person)
        {
            return m_accountsManager.EnumUserAccounts(person);
        }

        public Account GetAccount(long accountId)
        {
            return m_accountsManager.GetAccount(accountId);
        }

        public AccountBalance GetAccountBalance(long accountId)
        {
            return m_accountsManager.GetAccountBalance(accountId);
        }

        public void UpdateAccount(Account account)
        {
            m_accountsManager.UpdateAccount(account);
        }

        public void AddExpense(ref Expense expense)
        {
            m_expensesManager.AddExpense(ref expense);
        }

        public long AddExpense(long accountId, long amount, long categoryId, DateTime date, string description)
        {
            return m_expensesManager.AddExpense(accountId, amount, categoryId, date, description);
        }

        public void DeleteExpense(Expense expense)
        {
            m_expensesManager.DeleteExpense(expense);
        }

        public IEnumerable<Expense> EnumAllExpenses()
        {
            return m_expensesManager.EnumAllExpenses();
        }

        public void UpdateExpense(Expense expense)
        {
            m_expensesManager.UpdateExpense(expense);
        }
    }
}