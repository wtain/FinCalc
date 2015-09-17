using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace FCHA
{
	[DebuggerDisplay("{Name}")]
	public class PersonViewModel : DependencyObject
	{
		private Person m_underlyingData;
		private AccountancyApplication m_accountancyApplication;

		public Person UnderlyingData
		{
			get { return m_underlyingData; }
			set { m_underlyingData = value; }
		}

		public static readonly DependencyProperty NameProperty =
			DependencyProperty.Register("Name", typeof(string), typeof(PersonViewModel));

		public static readonly DependencyProperty FullNameProperty =
			DependencyProperty.Register("FullName", typeof(string), typeof(PersonViewModel));

		public static readonly DependencyProperty UserAccountsProperty =
			DependencyProperty.Register("UserAccounts", typeof(ObservableCollection<AccountViewModel>), typeof(PersonViewModel));

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

		public ObservableCollection<AccountViewModel> UserAccounts
		{
			get { return (ObservableCollection<AccountViewModel>)GetValue(UserAccountsProperty); }
			set { SetValue(UserAccountsProperty, value); }
		}

		public PersonViewModel(Person person, AccountancyApplication accountancyApplication)
		{
			m_underlyingData = person;
			Name = person.name;
			FullName = person.fullName;
			m_accountancyApplication = accountancyApplication;
			UserAccounts = new ObservableCollection<AccountViewModel>(m_accountancyApplication.EnumUserAccounts(this));
		}

		public void UpdateUnderlyingData()
		{
			m_underlyingData = new Person(Name, FullName, m_underlyingData.personId);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
