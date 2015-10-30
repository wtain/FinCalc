using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCHA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCHA.Interfaces;

namespace FCHA.Tests
{
    [TestClass()]
    public class AccountancyApplicationTests
    {
        private AccountancyApplication m_app;
        private AccountancyDatabaseStub m_database;

        public AccountancyApplication Application
        {
            get
            {
                if (null == m_app)
                    m_app = new AccountancyApplication(Database, null);
                return m_app;
            }
        }

        public AccountancyDatabaseStub Database
        {
            get
            {
                if (null == m_database)
                    m_database = new AccountancyDatabaseStub();
                return m_database;
            }
        }

        [TestMethod()]
        public void AccountancyApplicationTest()
        {
            Assert.IsNotNull(Application.Users);
            Assert.IsNotNull(Application.Accounts);
            Assert.IsNotNull(Application.Categories);
            Assert.IsTrue(Application.Users.Count > 0);
            Assert.IsTrue(Application.Accounts.Count > 0);
            Assert.IsTrue(Application.Categories.Count > 0);
        }

        [TestMethod()]
        public void GetPersonTest()
        {
            PersonViewModel User1 = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(User1);
            Assert.AreEqual(User1.UserAccounts.Count, 2);

            PersonViewModel User2 = Application.GetPerson(Database.User2Id);
            Assert.IsNotNull(User2);
            Assert.AreEqual(User2.UserAccounts.Count, 2);
        }

        [TestMethod()]
        public void GetPersonTest1()
        {
            PersonViewModel User1 = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(User1);
            PersonViewModel User1Ref = Application.GetPerson(User1.UnderlyingData);
            Assert.IsNotNull(User1Ref);
            Assert.AreSame(User1, User1Ref);
        }

        [TestMethod()]
        public void GetAccountTest()
        {
            PersonViewModel User1 = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(User1);
            AccountViewModel User1Cash = Application.GetAccount(Database.User1Cash);
            Assert.IsNotNull(User1Cash);
            Assert.IsTrue(User1.UserAccounts.Contains(User1Cash));
        }

        [TestMethod()]
        public void GetAccountTest1()
        {
            AccountViewModel User1Cash = Application.GetAccount(Database.User1Cash);
            Assert.IsNotNull(User1Cash);
            AccountViewModel User1CashRef = Application.GetAccount(User1Cash.UnderlyingData);
            Assert.IsNotNull(User1CashRef);
            Assert.AreSame(User1Cash, User1CashRef);
        }

        [TestMethod()]
        public void GetAccountTest2()
        {
            PersonViewModel User1 = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(User1);
            AccountViewModel User1Cash = Application.GetAccount(Database.User1Cash);
            Assert.IsNotNull(User1Cash);
            AccountViewModel User1CashRef = Application.GetAccount(User1, User1Cash.UnderlyingData);
            Assert.IsNotNull(User1CashRef);
            Assert.AreSame(User1Cash, User1CashRef);
        }

        [TestMethod()]
        public void AddCategoryTest()
        {
            string testCategoryName = "TestCategory";
            Application.AddCategory(testCategoryName, false);
            Assert.IsTrue(Application.VirtualRoot.Children.Count(c => c.Name == testCategoryName) == 1);
        }

        [TestMethod()]
        public void AddChildCategoryTest()
        {
            string testCategoryName = "TestChildCategory";
            CategoryViewModel lastCategory = Application.VirtualRoot.Children.Last();
            Application.AddChildCategory(lastCategory, testCategoryName, false);
            Assert.IsTrue(lastCategory.Children.Count(c => c.Name == testCategoryName) == 1);
        }

        [TestMethod()]
        public void GetCategoryTest()
        {
            CategoryViewModel salary = Application.GetCategory(Database.CatIncomesSalaryId);
            Assert.IsNotNull(salary);
        }

        [TestMethod()]
        public void ChangeCategoryTest()
        {
            CategoryViewModel salary = Application.GetCategory(Database.CatIncomesSalaryId);
            Assert.IsNotNull(salary);
            string newName = "Primary salary";
            Application.ChangeCategory(salary, newName, true);
            Assert.IsTrue(Application.Categories.Count(c => c.Name == newName) == 1);
            CategoryViewModel transport = Application.GetCategory(Database.CatTransportId);
            string oldName = transport.Name;
            Assert.IsNotNull(transport);
            Assert.IsFalse(transport.IsIncome);
            Application.ChangeCategory(transport, transport.Name, true);
            Assert.IsTrue(transport.IsIncome);
            transport = Application.GetCategory(Database.CatTransportId);
            Assert.IsTrue(transport.IsIncome);
            Assert.AreEqual(oldName, transport.Name);
        }

        [TestMethod()]
        public void RemoveCategoryTest()
        {
            CategoryViewModel transport = Application.GetCategory(Database.CatTransportId);
            Category catTransport = transport.UnderlyingData;
            Application.RemoveCategory(transport);
            transport = Application.GetCategory(Database.CatTransportId);
            Assert.IsNull(transport);
            Application.AddCategory(catTransport.name, catTransport.isIncome);
            transport = Application.Categories.Where(c => c.Name == catTransport.name && c.IsIncome == catTransport.isIncome).First();
            Assert.IsNotNull(transport);
        }

        [TestMethod()]
        public void NewPersonTest()
        {
            PersonViewModel person = Application.NewPerson();
            Assert.IsNotNull(person);
        }

        [TestMethod()]
        public void AddPersonTest()
        {
            PersonViewModel person = new PersonViewModel(new Person("User 3", "Test USer 3", 0), Application);
            Application.AddPerson(person);
            Assert.AreNotEqual(0, person.UnderlyingData.personId);
            PersonViewModel newPerson = Application.GetPerson(person.UnderlyingData.personId);
            Assert.IsNotNull(newPerson);
            Assert.AreSame(person, newPerson);
        }

        [TestMethod()]
        public void UpdatePersonTest()
        {
            PersonViewModel person = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(person);
            string newName = "User 1 New Name";
            Assert.AreNotEqual(newName, person.Name);
            person.Name = newName;
            Assert.AreNotEqual(newName, person.UnderlyingData.name);
            Assert.AreEqual(newName, person.Name);
            person.UpdateUnderlyingData();
            Assert.AreEqual(newName, person.UnderlyingData.name);
            Application.UpdatePerson(person);
            person = Application.GetPerson(Database.User1Id);
            Assert.AreEqual(newName, person.Name);
        }

        [TestMethod()]
        public void RemovePersonTest()
        {
            PersonViewModel person = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(person);
            Person undPerson = person.UnderlyingData;
            Application.RemovePerson(person);
            person = Application.GetPerson(Database.User1Id);
            Assert.IsNull(person);
            Application.AddPerson(new PersonViewModel(undPerson, Application));
            person = Application.Users.Where(p => p.UnderlyingData.name == undPerson.name && p.UnderlyingData.fullName == undPerson.fullName).First();
            Assert.IsNotNull(person);
        }

        [TestMethod()]
        public void CreateAccountTest()
        {
            PersonViewModel person = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(person);
            AccountViewModel account = Application.CreateAccount(person);
            Assert.IsNotNull(account);
            Assert.IsFalse(person.UserAccounts.Contains(account));
        }

        [TestMethod()]
        public void EnumUserAccountsTest()
        {
            PersonViewModel person = Application.GetPerson(Database.User2Id);
            Assert.IsNotNull(person);
            Assert.AreEqual(2, person.UserAccounts.Count);
        }

        [TestMethod()]
        public void AddAccountTest()
        {
            PersonViewModel person = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(person);
            AccountViewModel account = Application.CreateAccount(person);
            Assert.IsNotNull(account);
            Application.AddAccount(account);
            Assert.IsTrue(person.UserAccounts.Contains(account));
        }

        [TestMethod()]
        public void UpdateAccountTest()
        {
            AccountViewModel account = Application.GetAccount(Database.User1Cash);
            Assert.IsNotNull(account);
            Assert.AreNotEqual(AccountType.DebetCard, account.AccountType);
            account.AccountType = AccountType.DebetCard;
            account.UpdateUnderlyingData();
            Application.UpdateAccount(account);
            account = Application.GetAccount(Database.User1Cash);
            Assert.IsNotNull(account);
            Assert.AreEqual(AccountType.DebetCard, account.AccountType);
            AccountViewModel account2 = Application.GetAccount(Database.User2Cash);
            Assert.IsNotNull(account2);
            PersonViewModel person1 = Application.GetPerson(Database.User1Id);
            PersonViewModel person2 = Application.GetPerson(Database.User2Id);
            Assert.AreNotEqual(person1, account2.Owner);
            Assert.AreEqual(person2, account2.Owner);
            account2.Owner = person1;
            account2.UpdateUnderlyingData();
            Application.UpdateAccount(account2);
            account2 = Application.GetAccount(Database.User2Cash);
            Assert.AreNotEqual(person2, account2.Owner);
            Assert.AreEqual(person1, account2.Owner);
        }

        [TestMethod()]
        public void DeleteAccountTest()
        {
            PersonViewModel person1 = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(person1);
            AccountViewModel account = Application.GetAccount(Database.User1Cash);
            Account undAccount = account.UnderlyingData;
            Assert.IsNotNull(account);
            Assert.IsTrue(person1.UserAccounts.Contains(account));
            Application.DeleteAccount(account);
            account = Application.GetAccount(Database.User1Cash);
            Assert.IsNull(account);
            Assert.IsFalse(person1.UserAccounts.Contains(account));
            account = new AccountViewModel(undAccount, Application);
            Application.AddAccount(account);
            Assert.IsTrue(person1.UserAccounts.Contains(account));
        }

        [TestMethod()]
        public void GetAccountStateTest()
        {
            PersonViewModel person1 = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(person1);
            AccountViewModel account = person1.UserAccounts.First();
            AccountBalance accState = Application.GetAccountState(account);
        }

        // todo: no copypastes

        [TestMethod()]
        public void AddExpenseTest()
        {
            PersonViewModel person1 = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(person1);
            AccountViewModel account = person1.UserAccounts.First();
            AccountBalance accState = Application.GetAccountState(account);
            long oldBalance = accState.balance;
            long amount = 100;
            Application.AddExpense(new ExpenseViewModel(new Expense(0, account.AccountId, amount, Database.CatFoodId, DateTime.Now, string.Empty), Application));
            accState = Application.GetAccountState(account);
            long newBalance = accState.balance;
            Assert.AreEqual(oldBalance - amount, newBalance);
        }

        [TestMethod()]
        public void UpdateExpenseTest()
        {
            PersonViewModel person1 = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(person1);
            AccountViewModel account = person1.UserAccounts.First();
            long amount = 100;
            ExpenseViewModel expense = new ExpenseViewModel(new Expense(0, account.AccountId, amount, Database.CatFoodId, DateTime.Now, string.Empty), Application);
            long expenseId = Application.AddExpense(expense);
            AccountBalance accState = Application.GetAccountState(account);
            long oldBalance = accState.balance;
            expense.Amount = 150;
            Application.UpdateExpense(expense);
            accState = Application.GetAccountState(account);
            long newBalance = accState.balance;
            Assert.AreEqual(newBalance + 50, oldBalance);
            PersonViewModel person2 = Application.GetPerson(Database.User2Id);
            AccountViewModel account2 = person2.UserAccounts.First();
            AccountBalance accState2 = Application.GetAccountState(account2);
            long accBalance2 = accState2.balance;
            expense.Account = account2;
            Application.UpdateExpense(expense);
            accState = Application.GetAccountState(account);
            accState2 = Application.GetAccountState(account2);
            newBalance = accState.balance;
            Assert.AreEqual(newBalance, 0);
            Assert.AreEqual(accState2.balance + 150, accBalance2);
        }

        [TestMethod()]
        public void DeleteExpenseTest()
        {
            PersonViewModel person1 = Application.GetPerson(Database.User1Id);
            Assert.IsNotNull(person1);
            AccountViewModel account = person1.UserAccounts.First();
            AccountBalance accState = Application.GetAccountState(account);
            long oldBalance = accState.balance;
            long amount = 100;
            ExpenseViewModel expense = new ExpenseViewModel(new Expense(0, account.AccountId, amount, Database.CatFoodId, DateTime.Now, string.Empty), Application);
            Application.AddExpense(expense);
            accState = Application.GetAccountState(account);
            long newBalance = accState.balance;
            Assert.AreEqual(oldBalance - amount, newBalance);
            Application.DeleteExpense(expense);
            accState = Application.GetAccountState(account);
            newBalance = accState.balance;
            Assert.AreEqual(oldBalance, newBalance);
        }
    }
}