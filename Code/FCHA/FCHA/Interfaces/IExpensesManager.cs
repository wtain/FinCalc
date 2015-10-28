using System;
using System.Collections.Generic;

namespace FCHA.Interfaces
{
    public interface IExpensesManager
    {
        void AddExpense(ref Expense expense);
        long AddExpense(long accountId, long amount, long categoryId, DateTime date, string description);
        void DeleteExpense(Expense expense);
        IEnumerable<Expense> EnumAllExpenses();
        void UpdateExpense(Expense expense);
    }
}