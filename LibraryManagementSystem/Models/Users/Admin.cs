using LibraryManagementSystem.Models.Interfaces;
using LibraryManagementSystem.Models.Items;

namespace LibraryManagementSystem.Models.Users
{
    public abstract class Admin(int id, string firstName, string lastName) : IUser
    {
        public int Id => id;

        public string FirstName => firstName;

        public string LastName => lastName;
        
        public void HandleBorrow<T>(T item) where T : IBorrowable
        {
            item.Borrower = this;
            item.BorrowDate = DateTime.Now;

            if (item is Book) item.ReturnDate = DateTime.Now.AddDays(10);

            else if (item is DVD) item.ReturnDate = DateTime.Now.AddDays(18);
        }
    }
}