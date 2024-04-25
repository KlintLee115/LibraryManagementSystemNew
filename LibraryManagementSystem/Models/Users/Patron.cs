using LibraryManagementSystem.Models.Interfaces;
using LibraryManagementSystem.Models.Items;


namespace LibraryManagementSystem.Models.Users
{
    public class Patron(int id, string firstName, string lastName, string email, string password) : User(id, firstName, lastName, email, password)
    {
        public override void HandleBorrow<T>(T item)
        {
            item.Borrower = this;
            item.BorrowDate = DateTime.Now;
            item.IsAvailable = false;

            if (item is Book) item.ReturnDate = DateTime.Now.AddDays(7);
            else if (item is DVD) item.ReturnDate = DateTime.Now.AddDays(14);
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, Patron";
        }
    }
}
