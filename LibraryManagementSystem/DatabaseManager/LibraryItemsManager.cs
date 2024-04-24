using LibraryManagementSystem.Exceptions;
using LibraryManagementSystem.Models.Interfaces;
using LibraryManagementSystem.Models.Items;
using LibraryManagementSystem.Models.Users;
using Npgsql;

namespace LibraryManagementSystem.DatabaseManager
{
    public class LibraryItemsManager

    {

        public static List<Game> Games { get; set; } = [];
        public static List<DVD> Dvds { get; set; } = [];
        public static List<Book> Books { get; set; } = [];

        /*

        Get a library item based on item id

        @param - id (int?) - item id

        @return - the library item
        */

        private static T GetItem<T>(int? id, IEnumerable<T> itemList) where T : ILibraryItem
        {
            if (!id.HasValue) throw new InputNotNumberError("Item ID");
            T item = itemList.FirstOrDefault(item => item.Id == id.Value) ?? throw new ItemDoesntExistError("Item");
            return item;
        }

        public static Book GetBook(int? id) => GetItem(id, Books);
        public static Game GetGame(int? id) => GetItem(id, Games);
        public static DVD GetDVD(int? id) => GetItem(id, Dvds);

        public static ILibraryItem? GetItem(int id)
        {
            Game? game = Games.FirstOrDefault(item => item.Id == id);
            if (game != null) return game;

            Book? book = Books.FirstOrDefault(item => item.Id == id);
            if (book != null) return book;

            DVD? dvd = Dvds.FirstOrDefault(item => item.Id == id);
            if (dvd != null) return dvd;

            throw new ItemDoesntExistError("Item");

        }

        public static double HandleReturn(int itemId, DateTime todayDate)
        {
            try
            {
                ILibraryItem? item = GetItem(itemId);
                if (item is Book book)
                {

                    return book.CalculateLateFees(todayDate);

                }
                else if (item is DVD dvd)
                {
                    return dvd.CalculateLateFees(todayDate);
                }

                throw new ItemDoesntExistError("Item");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<List<DVD>> SelectAllDvDs()
        {

            try
            {
                List<DVD> productList = [];

                string query = "SELECT dvd.itemid, dvd.title, dvd.director, items.borrowed, items.borrowdate, items.userid, items.returndate, users.firstname, users.lastname, users.usertype " +
                      "FROM \"DvDs\" dvd " +
                      "LEFT JOIN \"Items\" items ON dvd.itemid = items.id " +
                      "LEFT JOIN \"Users\" users ON users.id = items.userid";

                using (var command = new NpgsqlCommand(query, MauiProgram.connection))
                {

                    using var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("itemid"));
                        string title = reader.GetString(reader.GetOrdinal("title"));
                        string director = reader.GetString(reader.GetOrdinal("director"));
                        bool borrowed = reader.GetBoolean(reader.GetOrdinal("borrowed"));

                        DVD dvd = new(id, title, director);

                        if (borrowed)
                        {
                            DateTime borrowDate = reader.GetDateTime(reader.GetOrdinal("borrowdate"));
                            DateTime returnDate = reader.GetDateTime(reader.GetOrdinal("returndate"));
                            int userid = reader.GetInt32(reader.GetOrdinal("userid"));
                            string firstName = reader.GetString(reader.GetOrdinal("firstname"));
                            string lastName = reader.GetString(reader.GetOrdinal("lastname"));
                            int userType = reader.GetInt32(reader.GetOrdinal("usertype"));
                            IUser user = userType switch
                            {
                                1 => new Owner(userid, firstName, lastName),
                                2 => new Librarian(userid, firstName, lastName),
                                3 => new Patron(userid, firstName, lastName),
                                _ => throw new NotImplementedException()
                            };

                            dvd.Borrower = user;
                            dvd.IsAvailable = false;
                            dvd.BorrowDate = borrowDate;
                            dvd.ReturnDate = returnDate;
                        }

                        productList.Add(dvd);
                    }
                }
                return productList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public static async Task<List<Game>> SelectAllGames()
        {
            List<Game> productList = [];

            string query = "SELECT * FROM \"Games\"";

            using (var command = new NpgsqlCommand(query, MauiProgram.connection))
            {

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Console.WriteLine("OK for game");

                    int id = reader.GetInt32(reader.GetOrdinal("itemid"));
                    string name = reader.GetString(reader.GetOrdinal("name"));

                    productList.Add(new Game(id, name));
                }
            }

            return productList;
        }


        public static async Task<List<Book>> SelectAllBooks()
        {

            List<Book> productList = [];

            string query = "SELECT book.itemid, book.title, book.author, items.borrowed, items.borrowdate, items.userid, items.returndate, users.firstname, users.lastname, users.usertype " +
                  "FROM \"Books\" book " +
                  "LEFT JOIN \"Items\" items ON book.itemid = items.id " +
                  "LEFT JOIN \"Users\" users ON users.id = items.userid";

            using (var command = new NpgsqlCommand(query, MauiProgram.connection))
            {

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32(reader.GetOrdinal("itemid"));
                    string title = reader.GetString(reader.GetOrdinal("title"));
                    string author = reader.GetString(reader.GetOrdinal("author"));
                    bool borrowed = reader.GetBoolean(reader.GetOrdinal("borrowed"));

                    Book book = new(id, title, author);

                    if (borrowed)
                    {
                        DateTime borrowDate = reader.GetDateTime(reader.GetOrdinal("borrowdate"));
                        DateTime returnDate = reader.GetDateTime(reader.GetOrdinal("returndate"));
                        int userid = reader.GetInt32(reader.GetOrdinal("userid"));
                        string firstName = reader.GetString(reader.GetOrdinal("firstname"));
                        string lastName = reader.GetString(reader.GetOrdinal("lastname"));
                        int userType = reader.GetInt32(reader.GetOrdinal("usertype"));
                        IUser user = userType switch
                        {
                            1 => new Owner(userid, firstName, lastName),
                            2 => new Librarian(userid, firstName, lastName),
                            3 => new Patron(userid, firstName, lastName),
                            _ => throw new NotImplementedException()
                        };

                        book.Borrower = user;
                        book.IsAvailable = false;
                        book.BorrowDate = borrowDate;
                        book.ReturnDate = returnDate;
                    }

                    productList.Add(book);
                }
            }

            return productList;

        }

        public static async Task Return(int itemId)
        {

            string query = $"UPDATE \"Items\" SET borrowed=false, userid=null, borrowdate=null, returndate=null WHERE id={itemId}";

            using var command = new NpgsqlCommand(query, MauiProgram.connection);

            using var reader = await command.ExecuteReaderAsync();
        }

        public static void AddGame(Game game) => Games.Add(game);
        public static void AddDVD(DVD dvd) => Dvds.Add(dvd);
        public static void AddBook(Book book) => Books.Add(book);

        public static void RemoveItem<T>(T item, List<T> itemList) where T : ILibraryItem => itemList.Remove(item);
    }
}


