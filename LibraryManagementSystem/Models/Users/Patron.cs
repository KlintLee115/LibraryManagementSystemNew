using LibraryManagementSystem.Models.Interfaces;
using LibraryManagementSystem.Models.Items;


namespace LibraryManagementSystem.Models.Users
{
    public class Patron(int id, string firstName, string lastName) : IUser
    {
        public int Id => id;

        public string FirstName => firstName;

        public string LastName => lastName;

        public void HandleBorrow<T>(T item) where T : IBorrowable
        {
            item.Borrower = this;
            item.BorrowDate = DateTime.Now;

            if (item is Book) item.ReturnDate = DateTime.Now.AddDays(7);
            else if (item is DVD) item.ReturnDate = DateTime.Now.AddDays(14);
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, Patron";
        }
    }
}
