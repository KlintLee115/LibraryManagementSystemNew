using LibraryManagementSystem.Models.Interfaces;
using LibraryManagementSystem.Models.Items;

namespace LibraryManagementSystem.Models.Users
{
    public abstract class Admin(int id, string firstName, string lastName, string email, string password) : User(id, firstName, lastName, email, password)
    {
        public override void HandleBorrow<T>(T item)
        {
            item.Borrower = this;
            item.BorrowDate = DateTime.Now;
            item.IsAvailable = false;

            if (item is Book) item.ReturnDate = DateTime.Now.AddDays(10);

            else if (item is DVD) item.ReturnDate = DateTime.Now.AddDays(18);
        }
    }
}