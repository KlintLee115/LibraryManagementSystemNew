using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models.Items
{
    public class DVD(int id, string title, string director) : IBorrowable
    {
        public int Id { get; } = id;
        public string Title { get; } = title;

        public string Director { get; } = director;
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public IUser? Borrower { get; set; }
        public bool IsAvailable { get; set; }

        public override string ToString()
        {
            return $"{Title}, {Director}";
        }

        public double CalculateLateFees(DateTime todayDate)
        {
            double difference = (ReturnDate - todayDate).Value.TotalDays;

            IsAvailable = true;
            Borrower = null;
            BorrowDate = null;
            ReturnDate = null;

            if (difference < 0)
            {
                return Math.Abs(difference);
            }

            return 0;
        }
    }
}

