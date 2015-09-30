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

		public long OwnerPersonId
		{
			get { return UnderlyingData.ownerPersonId; }
		}

		public long AccountId
		{
			get { return UnderlyingData.accountId; }
		}

		public static readonly DependencyProperty NameProperty =
			DependencyProperty.Register("Name", typeof(string), typeof(AccountViewModel));

		public static readonly DependencyProperty CurrencyProperty =
			DependencyProperty.Register("Currency", typeof(string), typeof(AccountViewModel));

		public static readonly DependencyProperty OwnerProperty =
			DependencyProperty.Register("Owner", typeof(PersonViewModel), typeof(AccountViewModel));

		public static readonly DependencyProperty AccountTypeProperty =
			DependencyProperty.Register("AccountType", typeof(AccountType), typeof(AccountViewModel));

		public static readonly DependencyProperty BalanceProperty =
			DependencyProperty.Register("Balance", typeof(long), typeof(AccountViewModel));

		public static readonly DependencyProperty LastUpdatedProperty =
			DependencyProperty.Register("LastUpdated", typeof(DateTime), typeof(AccountViewModel));

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

		public long Balance
		{
			get { return (long)GetValue(BalanceProperty); }
			set { SetValue(BalanceProperty, value); }
		}

		public DateTime LastUpdated
		{
			get { return (DateTime)GetValue(LastUpdatedProperty); }
			set { SetValue(LastUpdatedProperty, value); }
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

			UpdateAccountState();
		}

		public void UpdateUnderlyingData()
		{
			if (Owner.PersonId != OwnerPersonId)
			{
				m_accountancyApplication.GetPerson(OwnerPersonId).UserAccounts.Remove(this);
				Owner.UserAccounts.Add(this);
			}
			m_underlyingData = new Account(AccountId, Currency, Owner.PersonId, Name, AccountType);
		}

		public void UpdateAccountState()
		{
			if (0 == AccountId)
				return;
			AccountBalance state = m_accountancyApplication.GetAccountState(this);
			Balance = state.balance;
			LastUpdated = state.lastUpdated;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
