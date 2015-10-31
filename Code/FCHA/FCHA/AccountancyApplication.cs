using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.Windows;
using FCHA.Interfaces;
using FCHA.WPFHelpers;

namespace FCHA
{
	public class AccountancyApplication : DependencyObject
	{
        private IAccountancyDatabase m_database;

        // todo: introduce cached database source
		private Dictionary<long, PersonViewModel> m_personCache;
		private Dictionary<long, AccountViewModel> m_accountCache;
		
		public static readonly DependencyProperty UsersProperty =
			DependencyProperty.Register("Users", typeof(ObservableCollection<PersonViewModel>), typeof(AccountancyApplication));

		public static readonly DependencyProperty AccountsProperty =
			DependencyProperty.Register("Accounts", typeof(ObservableCollection<AccountViewModel>), typeof(AccountancyApplication));

		public static readonly DependencyProperty CategoriesProperty =
			DependencyProperty.Register("Categories", typeof(ObservableCollection<CategoryViewModel>), typeof(AccountancyApplication));

		public static readonly DependencyProperty ExpensesProperty =
			DependencyProperty.Register("Expenses", typeof(ObservableCollection<ExpenseViewModel>), typeof(AccountancyApplication));

		public static readonly DependencyProperty VirtualRootProperty =
			DependencyProperty.Register("VirtualRoot", typeof(CategoryViewModel), typeof(AccountancyApplication));

		public static readonly DependencyProperty SelectedDateProperty =
			DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(AccountancyApplication));

		public static readonly DependencyProperty SelectedUserProperty =
			DependencyProperty.Register("SelectedUser", typeof(PersonViewModel), typeof(AccountancyApplication));

		public static readonly DependencyProperty SelectedAccountProperty =
			DependencyProperty.Register("SelectedAccount", typeof(AccountViewModel), typeof(AccountancyApplication));

		public static readonly DependencyProperty SelectedReportProperty =
			DependencyProperty.Register("SelectedReport", typeof(OlapView), typeof(AccountancyApplication), 
                new PropertyMetadata(OnSelectedReportChanged));

        public static readonly DependencyProperty ReportsProperty =
            DependencyProperty.Register("Reports", typeof(ObservableCollection<OlapView>), typeof(AccountancyApplication));

        public static readonly DependencyProperty LiveSourceProperty
            = DependencyProperty.Register("LiveSource", typeof(CbrClient), typeof(MainWindow));

        public IFXRateSource LiveSource
        {
            get { return (CbrClient)GetValue(LiveSourceProperty); }
            private set { SetValue(LiveSourceProperty, value); }
        }
        public CategoryViewModel VirtualRoot
		{
			get { return (CategoryViewModel)GetValue(VirtualRootProperty); }
			private set { SetValue(VirtualRootProperty, value); }
		}

		public ObservableCollection<PersonViewModel> Users
		{
			get { return (ObservableCollection<PersonViewModel>)GetValue(UsersProperty); }
			private set { SetValue(UsersProperty, value); }
		}

		public ObservableCollection<AccountViewModel> Accounts
		{
			get { return (ObservableCollection<AccountViewModel>)GetValue(AccountsProperty); }
			private set { SetValue(AccountsProperty, value); }
		}

		public ObservableCollection<CategoryViewModel> Categories
		{
			get { return (ObservableCollection<CategoryViewModel>)GetValue(CategoriesProperty); }
			private set { SetValue(CategoriesProperty, value); }
		}

		public ObservableCollection<ExpenseViewModel> Expenses
		{
			get { return (ObservableCollection<ExpenseViewModel>)GetValue(ExpensesProperty); }
			private set { SetValue(ExpensesProperty, value); }
		}

		public DateTime SelectedDate
		{
			get { return (DateTime)GetValue(SelectedDateProperty); }
			set { SetValue(SelectedDateProperty, value); }
		}

		public PersonViewModel SelectedUser
		{
			get { return (PersonViewModel)GetValue(SelectedUserProperty); }
			set { SetValue(SelectedUserProperty, value); }
		}

		public AccountViewModel SelectedAccount
		{
			get { return (AccountViewModel)GetValue(SelectedAccountProperty); }
			set { SetValue(SelectedAccountProperty, value); }
		}

		public OlapView SelectedReport
        {
			get { return (OlapView)GetValue(SelectedReportProperty); }
			set { SetValue(SelectedReportProperty, value); }
		}

        public ObservableCollection<OlapView> Reports
        {
            get { return (ObservableCollection<OlapView>)GetValue(ReportsProperty); }
            set { SetValue(ReportsProperty, value); }
        }

        public AccountancyApplication(IAccountancyDatabase database, IFXRateSource liveSource)
        {
            m_database = database;
            LiveSource = liveSource;

            BuildCaches();
            BuildObjectCollections();

            AdjustSelections();
        }

        private void AdjustSelections()
        {
            SelectedDate = DateTime.Now.Date;
            if (Users.Count > 0)
            {
                SelectedUser = Users[0];
                if (SelectedUser.UserAccounts.Count > 0)
                    SelectedAccount = SelectedUser.UserAccounts[0];
            }

            if (Reports.Count > 0)
                SelectedReport = Reports[0];
        }

        private void BuildCaches()
        {
            m_personCache = new Dictionary<long, PersonViewModel>();
            m_accountCache = new Dictionary<long, AccountViewModel>();
        }

        private void BuildObjectCollections()
        {
            Users = new ObservableCollection<PersonViewModel>(m_database.EnumAllUsers().Select(p => GetPerson(p)));
            Accounts = new ObservableCollection<AccountViewModel>(m_database.EnumAllAccounts().Select(a => GetAccount(a)));
            Categories = new ObservableCollection<CategoryViewModel>(m_database.EnumAllCategories().Select(c => new CategoryViewModel(m_database, c)));
            Categories.ForEach(c => c.AdjustParent(this));
            Expenses = new ObservableCollection<ExpenseViewModel>(m_database.EnumAllExpenses().Select(e => new ExpenseViewModel(e, this)));
            VirtualRoot = new CategoryViewModel(m_database, null, new Category("Virtual", 0, false));
            Reports = new ObservableCollection<OlapView>(m_database.Reports);
        }

        private static void OnSelectedReportChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (null == e.NewValue)
                return;
            ((OlapView)e.NewValue).RefreshView();
        }

        public PersonViewModel GetPerson(long personId)
		{
            if (!m_personCache.ContainsKey(personId))
            {
                Person person = m_database.GetUser(personId);
                if (null == person)
                    return null;
                m_personCache.Add(personId, new PersonViewModel(person, this));
            }
			return m_personCache[personId];
		}

		public PersonViewModel GetPerson(Person p)
		{
			return GetPerson(p.personId);
		}

		public AccountViewModel GetAccount(long accountId)
		{
            if (!m_accountCache.ContainsKey(accountId))
            {
                Account account = m_database.Get(accountId);
                if (null == account)
                    return null;
                m_accountCache.Add(accountId, new AccountViewModel(account, this));
            }
			return m_accountCache[accountId];
		}

		public AccountViewModel GetAccount(Account a)
		{
			return GetAccount(a.accountId);
		}

		public AccountViewModel GetAccount(PersonViewModel person, Account a)
		{
			if (!m_accountCache.ContainsKey(a.accountId))
				m_accountCache[a.accountId] = new AccountViewModel(a, this, person);
			return m_accountCache[a.accountId];
		}

		public void AddCategory(string name, bool bIsIncome)
		{
			long catId = m_database.Add(name, bIsIncome);
			Category c = new Category(name, catId, bIsIncome);
            CategoryViewModel newCategory = new CategoryViewModel(m_database, VirtualRoot, c);
            VirtualRoot.Children.Add(newCategory);
			Categories.Add(newCategory);
		}

		public void AddChildCategory(CategoryViewModel category, string name, bool bIsIncome)
		{
			long catId = m_database.Add(name, category.CategoryId, bIsIncome);
			Category c = new Category(name, catId, category.CategoryId, bIsIncome);
            CategoryViewModel newCategory = new CategoryViewModel(m_database, category, c);
            category.Children.Add(newCategory);
			Categories.Add(newCategory);
		}

		public CategoryViewModel GetCategory(long categoryId)
		{
            if (0 == categoryId)
                return VirtualRoot;
            return Categories.Where(c => c.CategoryId == categoryId).FirstOrDefault();
		}

		public void ChangeCategory(CategoryViewModel category, string newName, bool isincome)
		{
			Category cat = category.UnderlyingData;
			cat.name = newName;
            cat.isIncome = isincome;
            m_database.Update(cat);
            category.Name = newName;
            category.IsIncome = isincome;
            category.UpdateUnderlyingData();
        }

		public void RemoveCategory(CategoryViewModel category)
		{
            m_database.Delete(category.UnderlyingData);
			category.Parent.Children.Remove(category);
			Categories.Remove(GetCategory(category.CategoryId));
		}

		public PersonViewModel NewPerson()
		{
			return new PersonViewModel(Person.DefaultPerson, this);
		}

		public void AddPerson(PersonViewModel person)
		{
			Person refPerson = person.UnderlyingData;
            m_database.AddUser(ref refPerson);
			person.UnderlyingData = refPerson;
			m_personCache.Add(refPerson.personId, person);
			Users.Add(person);
		}

		public void UpdatePerson(PersonViewModel person)
		{
            m_database.UpdateUser(person.UnderlyingData);
		}

		public void RemovePerson(PersonViewModel person)
		{
            m_database.DeleteUser(person.UnderlyingData);
			m_personCache.Remove(person.PersonId);
			Users.Remove(person);
		}

		public AccountViewModel CreateAccount(PersonViewModel person)
		{
			return new AccountViewModel(Account.CreateDefault(null != person ? person.PersonId : 0), this);
		}

		public IEnumerable<AccountViewModel> EnumUserAccounts(PersonViewModel person)
		{
			return m_database.EnumUserAccounts(person.UnderlyingData).Select(a => GetAccount(person, a));
		}

		public void AddAccount(AccountViewModel account)
		{
            // todo: add account to particular person specified
			Account refAccount = account.UnderlyingData;
            m_database.Add(ref refAccount);
			account.UnderlyingData = refAccount;
			account.Owner.UserAccounts.Add(account);
			m_accountCache.Add(account.AccountId, account);
		}

		public void UpdateAccount(AccountViewModel account)
		{
            m_database.Update(account.UnderlyingData);
		}

		public void DeleteAccount(AccountViewModel account)
		{
            m_database.Delete(account.UnderlyingData);
			account.Owner.UserAccounts.Remove(account);
			m_accountCache.Remove(account.AccountId);
		}

		public AccountBalance GetAccountState(AccountViewModel account)
		{
			return m_database.GetBalance(account.AccountId);
		}

		public long AddExpense(ExpenseViewModel expense)
		{
			Expense refExpense = expense.UnderlyingData;
            m_database.AddExpense(ref refExpense);
			expense.UnderlyingData = refExpense;
			Expenses.Add(expense);
			expense.Account.UpdateAccountState();
            if (null != SelectedReport)
                SelectedReport.RefreshView();
            return refExpense.expenseId;
		}

		public void UpdateExpense(ExpenseViewModel expense)
		{
			AccountViewModel oldAccount = null;
			if (expense.Account.AccountId != expense.AccountId)
				oldAccount = GetAccount(expense.AccountId);
			expense.UpdateUnderlyingData();
            m_database.UpdateExpense(expense.UnderlyingData);
			expense.Account.UpdateAccountState();
			if (null != oldAccount)
				oldAccount.UpdateAccountState();
            if (null != SelectedReport)
                SelectedReport.RefreshView();
		}

		public void DeleteExpense(ExpenseViewModel expense)
		{
            m_database.DeleteExpense(expense.UnderlyingData);
			Expenses.Remove(expense);
			expense.Account.UpdateAccountState();
            if (null != SelectedReport)
                SelectedReport.RefreshView();
		}
	}
}
