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

		public PersonViewModel(Person person, AccountancyApplication accountancyApplication)
		{
			m_underlyingData = person;
			Name = person.name;
			FullName = person.fullName;
			m_accountancyApplication = accountancyApplication;
		}

		public IEnumerable<AccountViewModel> UserAccounts
		{
			get { return m_accountancyApplication.EnumUserAccounts(this); }
		}

		public void UpdateUnderlyingData()
		{
			m_underlyingData = new Person(Name, FullName, m_underlyingData.personId);
		}
	}
}
