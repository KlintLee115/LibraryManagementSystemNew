using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models.Users
{
    public class Owner(int id, string FirstName, string LastName) : Admin(id, FirstName, LastName)
    {
        public override string ToString()
        {
            return $"{FirstName} {LastName}, Owner";
        }
    }
}
