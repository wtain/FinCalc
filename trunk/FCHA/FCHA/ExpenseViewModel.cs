using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace FCHA
{
	public class ExpenseViewModel : DependencyObject
	{
		private Expense m_underlyingData;

		public static readonly DependencyProperty AccountProperty
			= DependencyProperty.Register("Account", typeof(AccountViewModel), typeof(ExpenseViewModel));

		public static readonly DependencyProperty AmountProperty
			= DependencyProperty.Register("Amount", typeof(long), typeof(ExpenseViewModel));

		public static readonly DependencyProperty DateProperty
			= DependencyProperty.Register("Date", typeof(DateTime), typeof(ExpenseViewModel));

		public static readonly DependencyProperty CategoryProperty
			= DependencyProperty.Register("Category", typeof(CategoryViewModel), typeof(ExpenseViewModel));

		public static readonly DependencyProperty DescriptionProperty
			= DependencyProperty.Register("Description", typeof(string), typeof(ExpenseViewModel));

		public AccountViewModel Account
		{
			get { return (AccountViewModel)GetValue(AccountProperty); }
			set { SetValue(AccountProperty, value); }
		}

		public long Amount
		{
			get { return (long)GetValue(AmountProperty); }
			set { SetValue(AmountProperty, value); }
		}

		public DateTime Date
		{
			get { return (DateTime)GetValue(DateProperty); }
			set { SetValue(DateProperty, value); }
		}

		public CategoryViewModel Category
		{
			get { return (CategoryViewModel)GetValue(CategoryProperty); }
			set { SetValue(CategoryProperty, value); }
		}

		public string Description
		{
			get { return (string)GetValue(DescriptionProperty); }
			set { SetValue(DescriptionProperty, value); }
		}

		public Expense UnderlyingData
		{
			get { return m_underlyingData; }
			set { m_underlyingData = value; }
		}

		public long AccountId
		{
			get { return UnderlyingData.accountId; }
		}

		public ExpenseViewModel(Expense e, AccountancyApplication app)
		{
			Account = app.GetAccount(e.accountId);
			Amount = e.amount;
			Date = e.date;
			Category = app.GetCategory(e.categoryId);
			Description = e.description;

			m_underlyingData = e;
		}

		public void UpdateUnderlyingData()
		{
			m_underlyingData = new Expense(m_underlyingData.expenseId, Account.AccountId, Amount, Category.CategoryId, Date, Description);
		}
	}
}
