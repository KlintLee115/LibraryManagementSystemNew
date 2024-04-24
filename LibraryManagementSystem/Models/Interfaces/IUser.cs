using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.Interfaces
{
    public interface IUser
    {

        public int Id { get;}
        public string FirstName { get; }
        public string LastName { get; }
        public void HandleBorrow<T>(T item) where T : IBorrowable;
    }
}
