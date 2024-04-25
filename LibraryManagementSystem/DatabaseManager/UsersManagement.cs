using LibraryManagementSystem.Exceptions;
using LibraryManagementSystem.Models.Users;
using Npgsql;

namespace LibraryManagementSystem.DatabaseManager
{
    public class UsersManagement
    {
        public static List<User> users = [];

        /*
        * Add a user to the users list
        * @param - user (User) - the user
        */

        public static void AddUser(User user) => users.Add(user);

        /*
         * Remove a user from the users list
         * @param - user (User) - the user
         */

        public static void RemoveUser(User user) => users.Remove(user);

        /*
        * Get all users
        * @return - all users
        */

        public static async Task<List<User>> SelectAllUsers()
        {
            try
            {
                List<User> users = [];

                string query = "SELECT * FROM \"Users\"";

                using var command = new NpgsqlCommand(query, MauiProgram.connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {

                    int id = reader.GetInt32(reader.GetOrdinal("id"));

                    int userType = reader.GetInt32(reader.GetOrdinal("usertype"));
                    string firstName = reader.GetString(reader.GetOrdinal("firstname"));
                    string lastName = reader.GetString(reader.GetOrdinal("lastname"));
                    string email = reader.GetString(reader.GetOrdinal("email"));
                    string password = reader.GetString(reader.GetOrdinal("password"));

                    User? newUser = userType switch
                    {
                        1 => new Owner(id, firstName, lastName, email, password),
                        2 => new Librarian(id, firstName, lastName, email, password),
                        3 => new Patron(id, firstName, lastName, email, password),
                        _ => null
                    };

                    if (newUser != null) users.Add(newUser);

                }


                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
