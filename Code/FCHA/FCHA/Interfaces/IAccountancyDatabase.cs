
using System.Collections.Generic;

namespace FCHA.Interfaces
{
    public interface IAccountancyDatabase
        : IUsersManager
        , ICategoriesManager
        , IAccountsManager
        , IExpensesManager
    {
        // todo: To IReportsManager
        IEnumerable<OlapView> Reports { get; }
    }
}