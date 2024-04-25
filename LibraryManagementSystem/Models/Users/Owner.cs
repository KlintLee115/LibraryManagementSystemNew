using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models.Users
{
    public class Owner(int id, string FirstName, string LastName, string email, string pasword) : Admin(id, FirstName, LastName, email, pasword)
    {
        public override string ToString()
        {
            return $"{FirstName} {LastName}, Owner";
        }
    }
}
