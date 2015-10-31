using FCHA.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCHA;

namespace FCHA.Tests
{
    public class AccountancyDatabaseStub : IAccountancyDatabase
    {
        private Dictionary<long, Person> m_persons = new Dictionary<long, Person>();
        private Dictionary<long, Account> m_accounts = new Dictionary<long, Account>();
        private Dictionary<long, AccountBalance> m_accountBalances = new Dictionary<long, AccountBalance>();
        private Dictionary<long, Expense> m_expenses = new Dictionary<long, Expense>();
        private Dictionary<long, Category> m_categories = new Dictionary<long, Category>();

        private long m_nextId = 0;

        private long m_user1id;
        private long m_user2id;
        private long m_user1cash;
        private long m_user1salary;
        private long m_user2cash;
        private long m_user2salary;
        private long m_catIncomesId;
        private long m_catIncomesSalaryId;
        private long m_catIncomesFreelanceId;
        private long m_catFoodId;
        private long m_catFoodstuffId;
        private long m_catRestaurantId;
        private long m_catFastfoodId;
        private long m_catTransportId;
        private long m_catTrainTicketId;
        private long m_catSubwayTicketId;

        public long User1Id { get { return m_user1id; } }
        public long User2Id { get { return m_user2id; } }
        public long User1Cash { get { return m_user1cash; } }
        public long User1Salary { get { return m_user1salary; } }
        public long User2Cash { get { return m_user2cash; } }
        public long User2Salary { get { return m_user2salary; } }
        public long CatIncomesId { get { return m_catIncomesId; } }
        public long CatIncomesSalaryId { get { return m_catIncomesSalaryId; } }
        public long CatIncomesFreelanceId { get { return m_catIncomesFreelanceId; } }
        public long CatFoodId { get { return m_catFoodId; } }
        public long CatFoodstuffId { get { return m_catFoodstuffId; } }
        public long CatRestaurantId { get { return m_catRestaurantId; } }
        public long CatFastfoodId { get { return m_catFastfoodId; } }
        public long CatTransportId { get { return m_catTransportId; } }
        public long CatTrainTicketId { get { return m_catTrainTicketId; } }
        public long CatSubwayTicketId { get { return m_catSubwayTicketId; } }

        public IEnumerable<OlapView> Reports { get { return new List<OlapView>(); } }

        public AccountancyDatabaseStub()
        {
            m_user1id = AddUser("User 1", "Test User 1");
            m_user2id = AddUser("User 2", "Test User 2");
            m_user1cash = AddAccount("Cash", "RUB", m_user1id, AccountType.Cash);
            m_user1salary = AddAccount("Salary", "RUB", m_user1id, AccountType.DebetCard);
            m_user2cash = AddAccount("Cash", "RUB", m_user2id, AccountType.Cash);
            m_user2salary = AddAccount("Salary", "RUB", m_user2id, AccountType.DebetCard);
            m_catIncomesId = Add("Incomes", true);
            m_catIncomesSalaryId = Add("Salary", m_catIncomesId, true);
            m_catIncomesFreelanceId = Add("Freelance", m_catIncomesId, true);
            m_catFoodId = Add("Food", false);
            m_catFoodstuffId = Add("Foodstuff", m_catFoodId, false);
            m_catRestaurantId = Add("Restaurant", m_catFoodId, false);
            m_catFastfoodId = Add("Fastfood", m_catFoodId, false);
            m_catTransportId = Add("Transport", false);
            m_catTrainTicketId = Add("Train Ticket", m_catTransportId, false);
            m_catSubwayTicketId = Add("Subway Ticket", m_catTransportId, false);
        }

        private long GetNewId()
        {
            return m_nextId++;
        }

        public void Add(ref Account account)
        {
            account.accountId = GetNewId();
            m_accounts.Add(account.accountId, account);
        }

        public long AddAccount(string name, string currency, long ownerPersonId, AccountType type)
        {
            Account account = new Account(0, currency, ownerPersonId, name, type);
            Add(ref account);
            return account.accountId;
        }

        public long Add(string name, bool isIncome)
        {
            Category category = new Category(name, GetNewId(), isIncome);
            m_categories.Add(category.categoryId, category);
            return category.categoryId;
        }

        public long Add(string name, long parentId, bool isIncome)
        {
            Category category = new Category(name, GetNewId(), parentId, isIncome);
            m_categories.Add(category.categoryId, category);
            return category.categoryId;
        }

        private Category GetCategory(long categoryId)
        {
            return m_categories[categoryId];
        }

        public void AddExpense(ref Expense expense)
        {
            expense.expenseId = GetNewId();
            m_expenses.Add(expense.expenseId, expense);
            AccountBalance accountBalance = GetBalance(expense.accountId);
            if (GetCategory(expense.categoryId).isIncome)
                accountBalance.balance += expense.amount;
            else
                accountBalance.balance -= expense.amount;
        }

        public long AddExpense(long accountId, long amount, long categoryId, DateTime date, string description)
        {
            Expense expense = new Expense(0, accountId, amount, categoryId, date, description);
            AddExpense(ref expense);
            return expense.expenseId;
        }

        public void AddUser(ref Person person)
        {
            person.personId = GetNewId();
            m_persons.Add(person.personId, person);
        }

        public long AddUser(string name, string fullName)
        {
            Person person = new Person(name, fullName, 0);
            AddUser(ref person);
            return person.personId;
        }

        public void Delete(Account account)
        {
            m_accounts.Remove(account.accountId);
        }

        public void Delete(Category cat)
        {
            m_categories.Remove(cat.categoryId);
        }

        public void DeleteExpense(Expense expense)
        {
            m_expenses.Remove(expense.expenseId);
            AccountBalance accountBalance = GetBalance(expense.accountId);
            if (!GetCategory(expense.categoryId).isIncome)
                accountBalance.balance += expense.amount;
            else
                accountBalance.balance -= expense.amount;
        }

        public void DeleteUser(Person person)
        {
            m_persons.Remove(person.personId);
        }

        public IEnumerable<Account> EnumAllAccounts()
        {
            return m_accounts.Values;
        }

        public IEnumerable<Category> EnumAllCategories()
        {
            return m_categories.Values;
        }

        public IEnumerable<Expense> EnumAllExpenses()
        {
            return m_expenses.Values;
        }

        public IEnumerable<Person> EnumAllUsers()
        {
            return m_persons.Values;
        }

        public IEnumerable<Category> EnumByParent(long parentId)
        {
            return EnumAllCategories().Where(c => c.parentId == parentId);
        }

        public IEnumerable<Account> EnumUserAccounts(Person person)
        {
            return EnumAllAccounts().Where(a => a.ownerPersonId == person.personId);
        }

        public Account Get(long accountId)
        {
            if (!m_accounts.ContainsKey(accountId))
                return null;
            return m_accounts[accountId];
        }

        public AccountBalance GetBalance(long accountId)
        {
            if (!m_accountBalances.ContainsKey(accountId))
                m_accountBalances[accountId] = new AccountBalance();
            return m_accountBalances[accountId];
        }

        public Person GetUser(long personId)
        {
            if (!m_persons.ContainsKey(personId))
                return null;
            return m_persons[personId];
        }

        public void Update(Account account)
        {
            m_accounts[account.accountId] = account;
        }

        public void Update(Category cat)
        {
            m_categories[cat.categoryId] = cat;
        }

        public void UpdateExpense(Expense expense)
        {
            AccountBalance accountBalance1 = GetBalance(m_expenses[expense.expenseId].accountId);
            AccountBalance accountBalance2 = GetBalance(expense.accountId);
            if (GetCategory(expense.categoryId).isIncome)
                accountBalance1.balance -= m_expenses[expense.expenseId].amount;
            else
                accountBalance1.balance += m_expenses[expense.expenseId].amount;
            m_expenses[expense.expenseId] = expense;
            if (GetCategory(expense.categoryId).isIncome)
                accountBalance2.balance += expense.amount;
            else
                accountBalance2.balance -= expense.amount;
        }

        public void UpdateUser(Person person)
        {
            m_persons[person.personId] = person;
        }
    }
}
