using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models.Users
{
    public class Librarian(int Id, string FirstName, string LastName) : Admin(Id, FirstName, LastName)
    {
        public override string ToString()
        {
            return $"{FirstName} {LastName}, Librarian";
        }


    }
}
