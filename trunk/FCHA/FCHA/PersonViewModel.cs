using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace FCHA
{
	public class PersonViewModel : DependencyObject
	{
		private Person m_underlyingData;
		private AccountsManager m_accountsManager;
		private UsersManager m_userManager;

		public Person UnderlyingData
		{
			get { return m_underlyingData; }
		}

		public static readonly DependencyProperty NameProperty =
			DependencyProperty.Register("Name", typeof(string), typeof(PersonViewModel));

		public static readonly DependencyProperty FullNameProperty =
			DependencyProperty.Register("FullName", typeof(string), typeof(PersonViewModel));

		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}

		public string FullName
		{
			get { return (string)GetValue(FullNameProperty); }
			set { SetValue(FullNameProperty, value); }
		}

		public PersonViewModel(Person person, AccountsManager accountsManager, UsersManager userManager)
		{
			m_underlyingData = person;
			Name = person.name;
			FullName = person.fullName;
			m_accountsManager = accountsManager;
			m_userManager = userManager;
		}

		public IEnumerable<AccountViewModel> GetUserAccounts()
		{
			return m_accountsManager.EnumUserAccounts(m_underlyingData).Select(a => new AccountViewModel(a, m_userManager, m_accountsManager));
		}

		public void UpdateUnderlyingData()
		{
			m_underlyingData = new Person(Name, FullName, m_underlyingData.personId);
		}
	}
}
