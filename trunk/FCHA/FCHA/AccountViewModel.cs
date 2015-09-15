﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace FCHA
{
	public class AccountViewModel : DependencyObject
	{
		private Account m_underlyingData;
		private UsersManager m_userManager;

		public Account UnderlyingData
		{
			get { return m_underlyingData; }
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

		public AccountViewModel(Account account, UsersManager userManager, AccountsManager accountsManager)
		{
			m_underlyingData = account;
			m_userManager = userManager;

			Name = account.name;
			Currency = account.currency;
			AccountType = account.type;
			if (0 != account.ownerPersonId)
				Owner = new PersonViewModel(userManager.GetUser(account.ownerPersonId), accountsManager, userManager);
		}

		public void UpdateUnderlyingData()
		{
			m_underlyingData = new Account(m_underlyingData.accountId, Currency, Owner.UnderlyingData.personId, Name, AccountType);
		}
	}
}