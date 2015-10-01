using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.Windows;

namespace FCHA
{
	public class AccountancyApplication : DependencyObject
		//: IDisposable
	{
		private SQLiteConnection m_connection;
		private CategoriesManager m_categoriesManager;
		private UsersManager m_usersManager;
		private AccountsManager m_accountsManager;
		private ExpensesManager m_expensesManager;

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

        public CbrClient LiveSource
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

        public AccountancyApplication(SQLiteConnection connection)
		{
			m_connection = connection;
			m_categoriesManager = new CategoriesManager(m_connection);
			m_usersManager = new UsersManager(m_connection);
			m_accountsManager = new AccountsManager(m_connection);
			m_expensesManager = new ExpensesManager(m_connection);

			m_personCache = new Dictionary<long, PersonViewModel>();
			m_accountCache = new Dictionary<long, AccountViewModel>();

			Users = new ObservableCollection<PersonViewModel>(m_usersManager.EnumAllUsers().Select(p => GetPerson(p)));
			Accounts = new ObservableCollection<AccountViewModel>(m_accountsManager.EnumAllAccounts().Select(a => GetAccount(a)));
			Categories = new ObservableCollection<CategoryViewModel>(m_categoriesManager.EnumAllCategories().Select(c => new CategoryViewModel(c)));
			Expenses = new ObservableCollection<ExpenseViewModel>(m_expensesManager.EnumAllExpenses().Select(e => new ExpenseViewModel(e, this)));
			VirtualRoot = new CategoryViewModel(m_categoriesManager, null, new Category("Virtual", 0));
			SelectedDate = DateTime.Now.Date;
			if (Users.Count > 0)
			{
				SelectedUser = Users[0];
				if (SelectedUser.UserAccounts.Count > 0)
					SelectedAccount = SelectedUser.UserAccounts[0];
			}

            Reports = new ObservableCollection<OlapView>();
            Reports.Add(new OlapView("Expense by Date & Category", m_connection, "ExpenseByCategory", new OlapStage("Date", "Category", "Amount")));
            Reports.Add(new OlapView("Expense by Month & Category", m_connection, "ExpenseByCategoryAndMonth", new OlapStage("Month", "Category", "Amount")));
            Reports.Add(new OlapView("Expense by Month & Top Category", m_connection, "ExpenseByTopLevelCategoryAndMonth", new OlapStage("Month", "Category", "Amount")));
            

            if (Reports.Count > 0)
                SelectedReport = Reports[0];

            LiveSource = new CbrClient();
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
				m_personCache[personId] = new PersonViewModel(m_usersManager.GetUser(personId), this);
			return m_personCache[personId];
		}

		public PersonViewModel GetPerson(Person p)
		{
			return GetPerson(p.personId);
		}

		public AccountViewModel GetAccount(long accountId)
		{
			if (!m_accountCache.ContainsKey(accountId))
				m_accountCache[accountId] = new AccountViewModel(m_accountsManager.GetAccount(accountId), this);
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

		public void AddCategory(string name)
		{
			long catId = m_categoriesManager.AddCategory(name);
			Category c = new Category(name, catId);
			VirtualRoot.Children.Add(new CategoryViewModel(m_categoriesManager, VirtualRoot, c));
			Categories.Add(new CategoryViewModel(c));
		}

		public void AddChildCategory(CategoryViewModel category, string name)
		{
			long catId = m_categoriesManager.AddCategory(name, category.CategoryId);
			Category c = new Category(name, catId, category.CategoryId);
			category.Children.Add(new CategoryViewModel(m_categoriesManager, category, c));
			Categories.Add(new CategoryViewModel(c));
		}

		public CategoryViewModel GetCategory(long categoryId)
		{
			return Categories.Where(c => c.CategoryId == categoryId).FirstOrDefault();
		}

		public void RenameCategory(CategoryViewModel category, string newName)
		{
			Category cat = category.UnderlyingData;
			cat.name = newName;
			m_categoriesManager.UpdateCategory(cat);
			category.Name = newName;
			GetCategory(category.CategoryId).Name = newName;
		}

		public void RemoveCategory(CategoryViewModel category)
		{
			m_categoriesManager.DeleteCategory(category.UnderlyingData);
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
			m_usersManager.AddUser(ref refPerson);
			person.UnderlyingData = refPerson;
			m_personCache.Add(refPerson.personId, person);
			Users.Add(person);
		}

		public void UpdatePerson(PersonViewModel person)
		{
			m_usersManager.UpdateUser(person.UnderlyingData);
		}

		public void RemovePerson(PersonViewModel person)
		{
			m_usersManager.DeleteUser(person.UnderlyingData);
			m_personCache.Remove(person.PersonId);
			Users.Remove(person);
		}

		public AccountViewModel CreateAccount(PersonViewModel person)
		{
			return new AccountViewModel(Account.CreateDefault(null != person ? person.PersonId : 0), this);
		}

		public IEnumerable<AccountViewModel> EnumUserAccounts(PersonViewModel person)
		{
			return m_accountsManager.EnumUserAccounts(person.UnderlyingData).Select(a => GetAccount(person, a));
		}

		public void AddAccount(AccountViewModel account)
		{
			Account refAccount = account.UnderlyingData;
			m_accountsManager.AddAccount(ref refAccount);
			account.UnderlyingData = refAccount;
			account.Owner.UserAccounts.Add(account);
			m_accountCache.Add(account.AccountId, account);
		}

		public void UpdateAccount(AccountViewModel account)
		{
			m_accountsManager.UpdateAccount(account.UnderlyingData);
		}

		public void DeleteAccount(AccountViewModel account)
		{
			m_accountsManager.DeleteAccount(account.UnderlyingData);
			account.Owner.UserAccounts.Remove(account);
			m_accountCache.Remove(account.AccountId);
		}

		public AccountBalance GetAccountState(AccountViewModel account)
		{
			return m_accountsManager.GetAccountBalance(account.AccountId);
		}

		public void AddExpense(ExpenseViewModel expense)
		{
			Expense refExpense = expense.UnderlyingData;
			m_expensesManager.AddExpense(ref refExpense);
			expense.UnderlyingData = refExpense;
			Expenses.Add(expense);
			expense.Account.UpdateAccountState();
            SelectedReport.RefreshView();
		}

		public void UpdateExpense(ExpenseViewModel expense)
		{
			AccountViewModel oldAccount = null;
			if (expense.Account.AccountId != expense.AccountId)
				oldAccount = GetAccount(expense.AccountId);
			expense.UpdateUnderlyingData();
			m_expensesManager.UpdateExpense(expense.UnderlyingData);
			expense.Account.UpdateAccountState();
			if (null != oldAccount)
				oldAccount.UpdateAccountState();
            SelectedReport.RefreshView();
		}

		public void DeleteExpense(ExpenseViewModel expense)
		{
			m_expensesManager.DeleteExpense(expense.UnderlyingData);
			Expenses.Remove(expense);
			expense.Account.UpdateAccountState();
            SelectedReport.RefreshView();
		}
	}
}
