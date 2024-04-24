using LibraryManagementSystem.Exceptions;
using LibraryManagementSystem.Models.Interfaces;
using LibraryManagementSystem.Models.Users;
using Npgsql;

namespace LibraryManagementSystem.DatabaseManager
{
    public class UsersManagement
    {
        public static List<IUser> users = [];

        public static void AddUser(IUser user)
        {
            users.Add(user);
        }

        /*
        Get a user based on user id

              @param - id (int?) - user id

              @return - the User
              */
        public static IUser GetUser(int id)
        {
            for (int i = 0; i < users.Count; i++)
            {
                IUser user = users[i];
                if (user.Id == id)
                {
                    return user;
                }
            }
            throw new ItemDoesntExistError("User ");
        }


        /*
         * Remove a user from the users list, add 1
         * @param - id (int) - the user id
         */

        public static void RemoveUser(IUser user)
        {
            try
            {
                users.Remove(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*

      Get all users

      @return - all users
      */

        public static async Task<List<IUser>> SelectAllUsers()
        {
            try
            {
                List<IUser> users = [];

                string query = "SELECT * FROM \"Users\"";

                using (var command = new NpgsqlCommand(query, MauiProgram.connection))
                {

                    using var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {

                        var id = reader.GetInt32(reader.GetOrdinal("id"));

                        var userType = reader.GetInt32(reader.GetOrdinal("usertype"));
                        var firstName = reader.GetString(reader.GetOrdinal("firstname"));
                        var lastName = reader.GetString(reader.GetOrdinal("lastname"));

                        IUser? newUser = userType switch
                        {
                            1 => new Owner(id, firstName, lastName),
                            2 => new Librarian(id, firstName, lastName),
                            3 => new Patron(id, firstName, lastName),
                            _ => null
                        };

                        if (newUser != null) users.Add(newUser);

                    }
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
