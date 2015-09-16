using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;

namespace FCHA
{
	[DebuggerDisplay("{Name}")]
	public class AccountViewModel : DependencyObject
	{
		private Account m_underlyingData;
		private AccountancyApplication m_accountancyApplication;

		public Account UnderlyingData
		{
			get { return m_underlyingData; }
			set { m_underlyingData = value; }
		}

		public static readonly DependencyProperty NameProperty =
			DependencyProperty.Register("Name", typeof(string), typeof(AccountViewModel));

		public static readonly DependencyProperty CurrencyProperty =
			DependencyProperty.Register("Currency", typeof(string), typeof(AccountViewModel));

		public static readonly DependencyProperty OwnerProperty =
			DependencyProperty.Register("Owner", typeof(PersonViewModel), typeof(AccountViewModel));

		public static readonly DependencyProperty AccountTypeProperty =
			DependencyProperty.Register("AccountType", typeof(AccountType), typeof(AccountViewModel));

		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}

		public string Currency
		{
			get { return (string)GetValue(CurrencyProperty); }
			set { SetValue(CurrencyProperty, value); }
		}

		public PersonViewModel Owner
		{
			get { return (PersonViewModel)GetValue(OwnerProperty); }
			set { SetValue(OwnerProperty, value); }
		}

		public AccountType AccountType
		{
			get { return (AccountType)GetValue(AccountTypeProperty); }
			set { SetValue(AccountTypeProperty, value); }
		}

		public AccountViewModel(Account account, AccountancyApplication accountancyApplication)
			: this(account, accountancyApplication, accountancyApplication.GetPerson(account.ownerPersonId))
		{
		}

		public AccountViewModel(Account account, AccountancyApplication accountancyApplication, PersonViewModel owner)
		{
			m_accountancyApplication = accountancyApplication;
			m_underlyingData = account;

			Name = account.name;
			Currency = account.currency;
			AccountType = account.type;
			Owner = owner;
		}

		public void UpdateUnderlyingData()
		{
			if (Owner.UnderlyingData.personId != UnderlyingData.ownerPersonId)
			{
				m_accountancyApplication.GetPerson(UnderlyingData.ownerPersonId).UserAccounts.Remove(this);
				Owner.UserAccounts.Add(this);
			}
			m_underlyingData = new Account(m_underlyingData.accountId, Currency, Owner.UnderlyingData.personId, Name, AccountType);
		}
	}
}
