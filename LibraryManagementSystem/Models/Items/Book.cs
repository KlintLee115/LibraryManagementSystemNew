using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LibraryManagementSystem.Models.Interfaces;
using LibraryManagementSystem.Models.Users;

namespace LibraryManagementSystem.Models.Items
{
    public class Book(int id, string title, string author) : IBorrowable

    {
        public int Id { get; } = id;
        public string Title { get; } = title;
        public string Author { get; } = author;
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public IUser? Borrower { get; set; }
        public bool IsAvailable { get; set; }
        public double CalculateLateFees(DateTime todayDate)
        {
            double difference = (ReturnDate - todayDate).Value.TotalDays;

            IsAvailable = true;
            Borrower = null;
            BorrowDate = null;
            ReturnDate = null;

            if (difference < 0)
            {
                return 0.5 * Math.Abs(difference);
            }

            return 0;
        }
        public override string ToString()
        {
            return $"{Title}, {Author}";
        }
    }


}
