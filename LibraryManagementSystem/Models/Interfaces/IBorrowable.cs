using LibraryManagementSystem.Models.Items;
using LibraryManagementSystem.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.Interfaces
{
    public interface IBorrowable : ILibraryItem
    {
        DateTime? BorrowDate { get; set; }
        DateTime? ReturnDate { get; set; }
        IUser? Borrower { get; set; }
        bool IsAvailable { get; set; }

        double CalculateLateFees(DateTime todayDate);
    }
}
