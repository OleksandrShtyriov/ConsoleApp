using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = "Data Source =.; Initial Catalog = StockManager; Integrated Security = True;";
            Database.DatabaseAccess db = new Database.DatabaseAccess(connectionString);
            Menu loginMenu = new Menu(db, "Stock manager menu", 2, new List<string> { "Login", "Register" }, new List<Action<Menu, Database.DatabaseAccess>> { Methods.HandleLogin, Methods.HandleRegister });

            loginMenu.Run();
        }
    }
}
