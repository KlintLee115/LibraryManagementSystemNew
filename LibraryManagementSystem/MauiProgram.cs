using LibraryManagementSystem.DatabaseManager;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace LibraryManagementSystem
{
    public static class MauiProgram
    {

        private static readonly string connectionString = "Host=localhost;Port=5432;Database=library;Username=postgres;Password=password;";

        public static readonly NpgsqlConnection connection = new(connectionString);

        static MauiProgram()
        {
            connection.Open();
            GetData();
        }

        private async static void GetData()
        {
            UsersManagement.users = await UsersManagement.SelectAllUsers();
            LibraryItemsManager.Games = await LibraryItemsManager.SelectAllGames();
            LibraryItemsManager.Dvds = await LibraryItemsManager.SelectAllDvDs();
            LibraryItemsManager.Books = await LibraryItemsManager.SelectAllBooks();
        }

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
