using System.Collections.Generic;

namespace FCHA.Interfaces
{
    public interface IUsersManager
    {
        void AddUser(ref Person person);
        long AddUser(string name, string fullName);
        void DeleteUser(Person person);
        IEnumerable<Person> EnumAllUsers();
        Person GetUser(long personId);
        void UpdateUser(Person person);
    }
}