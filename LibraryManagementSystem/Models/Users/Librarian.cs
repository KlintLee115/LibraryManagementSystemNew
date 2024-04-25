using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models.Users
{
    public class Librarian(int Id, string FirstName, string LastName, string email, string password) : Admin(Id, FirstName, LastName, email, password)
    {
        public override string ToString()
        {
            return $"{FirstName} {LastName}, Librarian";
        }


    }
}
