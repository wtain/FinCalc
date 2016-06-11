using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCHA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FCHA.DataTypes;

namespace FCHA.Tests
{
    [TestClass()]
    public class CategoryViewModelTests
    {
        public AccountancyApplication Application
        {
            get { return CommonTestObjects.Application; }
        }

        public AccountancyDatabaseStub Database
        {
            get { return CommonTestObjects.Database; }
        }

        [TestMethod()]
        public void CategoryViewModelTest()
        {
            CategoryViewModel expense = Application.Categories.First(cwm => cwm.Type == CategoryType.Expense);
            Assert.IsNotNull(expense);
            CategoryViewModel newExpense = new CategoryViewModel(Database, expense, new Category("Test", -1, CategoryType.Expense, 1.0));
            Assert.IsNotNull(newExpense);
            Assert.AreEqual(newExpense.Name, "Test");
        }

        [TestMethod()]
        public void CategoryViewModelTest1()
        {
            CategoryViewModel newExpense = new CategoryViewModel(Database, new Category("Test 1", -1, CategoryType.Expense, 1.0));
            Assert.IsNotNull(newExpense);
            Assert.AreEqual(newExpense.Name, "Test 1");
        }

        [TestMethod()]
        public void ToStringTest()
        {
            CategoryViewModel expense = Application.Categories.First(cwm => cwm.Type == CategoryType.Expense);
            Assert.IsNotNull(expense);
            Assert.AreEqual(expense.ToString(), expense.Name);
        }

        [TestMethod()]
        public void UpdateUnderlyingDataTest()
        {
            CategoryViewModel newExpense = new CategoryViewModel(Database, new Category("Test 1", -1, CategoryType.Expense, 1.0));
            Assert.IsNotNull(newExpense);
            Assert.AreEqual(newExpense.Name, "Test 1");
            Assert.AreEqual(newExpense.UnderlyingData.name, "Test 1");
            newExpense.Name = "Test 2";
            Assert.AreEqual(newExpense.Name, "Test 2");
            Assert.AreEqual(newExpense.UnderlyingData.name, "Test 1");
            newExpense.UpdateUnderlyingData();
            Assert.AreEqual(newExpense.Name, "Test 2");
            Assert.AreEqual(newExpense.UnderlyingData.name, "Test 2");
        }

        [TestMethod()]
        public void IsCoversTest()
        {
            CategoryViewModel expense = Application.Categories.First(cwm => cwm.Type == CategoryType.Expense);
            Assert.IsNotNull(expense);
            CategoryViewModel firstChild = expense.Children.First();
            Assert.IsNotNull(firstChild);
            Assert.IsTrue(expense.IsCovers(firstChild));
        }
    }
}