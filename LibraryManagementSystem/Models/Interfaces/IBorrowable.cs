using LibraryManagementSystem.Models.Users;

namespace LibraryManagementSystem.Models.Interfaces
{
    public interface IBorrowable : ILibraryItem
    {
        DateTime? BorrowDate { get; set; }
        DateTime? ReturnDate { get; set; }
        User? Borrower { get; set; }
        bool IsAvailable { get; set; }

        double CalculateLateFees(DateTime todayDate);
    }
}
