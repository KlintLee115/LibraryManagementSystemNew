using LibraryManagementSystem.Exceptions;
using LibraryManagementSystem.Models.Interfaces;
using LibraryManagementSystem.Models.Items;
using LibraryManagementSystem.Models.Users;
using Npgsql;

namespace LibraryManagementSystem.DatabaseManager
{
    public class LibraryItemsManager
// start
    {

        public static List<Game> Games { get; set; } = [];
        public static List<DVD> Dvds { get; set; } = [];
        public static List<Book> Books { get; set; } = [];

        /*

        Helper function to get a library item based on item id and itemlist

        @param - id (int?) - item id
        @param - itemList (List<T>) - the list of items of which class implements ILibraryItem, the classes are Book, Game, and DvD

        @return - the library item
        */

        private static ILibraryItem? GetItem<T>(int? id, List<T> itemList) where T : ILibraryItem
        {
            if (!id.HasValue) throw new InputNotNumberError("Item ID");
            return itemList.FirstOrDefault(item => item.Id == id.Value);
        }


        /*
       * Get an item based on item id. Return null if there's no item
       * @param - id (int?) - the item id
       
       * @return - the library item, or null if there's none
       */

        public static ILibraryItem? GetItem(int? id)
        {
            if (!id.HasValue) throw new InputNotNumberError("Item ID");
            Book? book = GetBook(id.Value);
            if (book != null) return book;

            DVD? dvd = GetDVD(id.Value);
            if (dvd != null) return dvd;

            Game? game = GetGame(id.Value);
            if (game != null) return game;

            return null;
        }

        public static Book? GetBook(int? id) => (Book?)GetItem(id, Books);
        public static Game? GetGame(int? id) => (Game?)GetItem(id, Games);
        public static DVD? GetDVD(int? id) => (DVD?)GetItem(id, Dvds);

        /*
       * Handle the return process of an item. If there is penalty, return the penalty amount, otherwise return 0
       * @param - item (IBorrowable) - the item to be returned
       * @param - todayDate (DateTime) - today's date
       
       * @return - penalty fees
       */

        public static double HandleReturn(IBorrowable item, DateTime todayDate)
        {
            try
            {
                if (item is Book book) return book.CalculateLateFees(todayDate);
                if (item is DVD dvd) return dvd.CalculateLateFees(todayDate);
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
       * Get all dvds
       * @return - all dvds
       */

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

                        DVD dvd = new(id, title, director)
                        {
                            IsAvailable = true
                        };

                        if (borrowed)
                        {
                            DateTime borrowDate = reader.GetDateTime(reader.GetOrdinal("borrowdate"));
                            DateTime returnDate = reader.GetDateTime(reader.GetOrdinal("returndate"));
                            int userid = reader.GetInt32(reader.GetOrdinal("userid"));
                            string firstName = reader.GetString(reader.GetOrdinal("firstname"));
                            string lastName = reader.GetString(reader.GetOrdinal("lastname"));
                            string email = reader.GetString(reader.GetOrdinal("email"));
                            string password = reader.GetString(reader.GetOrdinal("password"));
                            int userType = reader.GetInt32(reader.GetOrdinal("usertype"));
                            User user = userType switch
                            {
                                1 => new Owner(userid, firstName, lastName, email, password),
                                2 => new Librarian(userid, firstName, lastName, email, password),
                                3 => new Patron(userid, firstName, lastName, email, password),
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

                    Book book = new(id, title, author)
                    {
                        IsAvailable = true
                    };

                    if (borrowed)
                    {
                        DateTime borrowDate = reader.GetDateTime(reader.GetOrdinal("borrowdate"));
                        DateTime returnDate = reader.GetDateTime(reader.GetOrdinal("returndate"));
                        int userid = reader.GetInt32(reader.GetOrdinal("userid"));
                        string firstName = reader.GetString(reader.GetOrdinal("firstname"));
                        string lastName = reader.GetString(reader.GetOrdinal("lastname"));
                        string email = reader.GetString(reader.GetOrdinal("email"));
                        string password = reader.GetString(reader.GetOrdinal("password"));
                        int userType = reader.GetInt32(reader.GetOrdinal("usertype"));
                        User user = userType switch
                        {
                            1 => new Owner(userid, firstName, lastName, email, password),
                            2 => new Librarian(userid, firstName, lastName, email, password),
                            3 => new Patron(userid, firstName, lastName, email, password),
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

        public static void AddGame(Game game) => Games.Add(game);
        public static void AddDVD(DVD dvd) => Dvds.Add(dvd);
        public static void AddBook(Book book) => Books.Add(book);

        public static void RemoveItem<T>(T item, List<T> itemList) where T : ILibraryItem => itemList.Remove(item);
    }
}


