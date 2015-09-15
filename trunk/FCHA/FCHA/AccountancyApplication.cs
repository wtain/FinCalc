using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace FCHA
{
	public class AccountancyApplication
		//: IDisposable
	{
		private SQLiteConnection m_connection;
		private CategoriesManager m_mgr;
		private UsersManager m_usersManager;
		private AccountsManager m_accountsManager;

		private CategoryViewModel m_categoriesVirtualRoot;
		private List<PersonViewModel> m_users;

		private Dictionary<long, PersonViewModel> m_personCache;

		public AccountancyApplication(SQLiteConnection connection)
		{
			m_connection = connection;
			m_mgr = new CategoriesManager(m_connection);
			m_usersManager = new UsersManager(m_connection);
			m_accountsManager = new AccountsManager(m_connection);

			m_personCache = new Dictionary<long, PersonViewModel>();
		}

		public CategoryViewModel CategoriesRoot
		{
			get
			{
				if (null == m_categoriesVirtualRoot)
					m_categoriesVirtualRoot = new CategoryViewModel(m_mgr, null, new Category("Virtual", 0));
				return m_categoriesVirtualRoot;
			}
		}

		private PersonViewModel GetPersonFromCache(Person p)
		{
			if (!m_personCache.ContainsKey(p.personId))
				m_personCache[p.personId] = new PersonViewModel(p, this);
			return m_personCache[p.personId];
		}

		public List<PersonViewModel> Users
		{
			get
			{
				if (null == m_users)
					m_users = new List<PersonViewModel>(m_usersManager.EnumAllUsers().Select(p => GetPersonFromCache(p)));
				return m_users;
			}
		}

		public void AddCategory(string name)
		{
			m_mgr.AddCategory(name);
			CategoriesRoot.RefreshChildren();
		}

		public void AddChildCategory(CategoryViewModel category, string name)
		{
			m_mgr.AddCategory(name, category.UnderlyingData.categoryId);
			category.RefreshChildren();
		}

		public void RenameCategory(CategoryViewModel category, string newName)
		{
			Category cat = category.UnderlyingData;
			cat.name = newName;
			m_mgr.UpdateCategory(cat);
			category.RefreshParentChildren();
		}

		public void RemoveCategory(CategoryViewModel category)
		{
			m_mgr.DeleteCategory(category.UnderlyingData);
			category.RefreshParentChildren();
		}

		public PersonViewModel NewPerson()
		{
			return new PersonViewModel(Person.DefaultPerson, this);
		}

		public PersonViewModel GetPerson(long personId)
		{
			// ?? GetPersonFromCache
			if (!m_personCache.ContainsKey(personId))
				m_personCache[personId] = new PersonViewModel(m_usersManager.GetUser(personId), this);
			return m_personCache[personId];
		}

		public PersonViewModel GetPerson(Person p)
		{
			return GetPerson(p.personId);
		}

		public void AddPerson(PersonViewModel person)
		{
			Person refPerson = person.UnderlyingData;
			m_usersManager.AddUser(ref refPerson);
			person.UnderlyingData = refPerson;
			m_personCache.Add(refPerson.personId, person);
			m_users.Add(person);
		}

		public void UpdatePerson(PersonViewModel person)
		{
			m_usersManager.UpdateUser(person.UnderlyingData);
		}

		public void RemovePerson(PersonViewModel person)
		{
			m_usersManager.DeleteUser(person.UnderlyingData);
			m_personCache.Remove(person.UnderlyingData.personId);
			m_users.Remove(person);
		}

		public AccountViewModel CreateAccount(PersonViewModel person)
		{
			return new AccountViewModel(Account.CreateDefault(null != person ? person.UnderlyingData.personId : 0), this);
		}

		public IEnumerable<AccountViewModel> EnumUserAccounts(PersonViewModel person)
		{
			// todo: cache
			return m_accountsManager.EnumUserAccounts(person.UnderlyingData).Select(a => new AccountViewModel(a, this));
		}

		public void AddAccount(AccountViewModel account)
		{
			Account refAccount = account.UnderlyingData;
			m_accountsManager.AddAccount(ref refAccount);
			account.UnderlyingData = refAccount;
		}

		public void UpdateAccount(AccountViewModel account)
		{
			m_accountsManager.UpdateAccount(account.UnderlyingData);
		}

		public void DeleteAccount(AccountViewModel account)
		{
			m_accountsManager.DeleteAccount(account.UnderlyingData);
		}
	}
}
