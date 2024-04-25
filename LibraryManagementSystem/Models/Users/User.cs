using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models.Users
{
    public abstract class User(int id, string firstName, string lastName, string email, string password)
    {

        public int Id { get; } = id;
        public string FirstName { get; } = firstName;
        public string LastName { get; } = lastName;
        public string Email { get; } = email;
        public string Password { get; } = password;

        public abstract void HandleBorrow<T>(T item) where T : IBorrowable;
    }
}
