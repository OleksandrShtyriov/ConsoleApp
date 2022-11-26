using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Methods
    {
        private static (string username, string password) getInput()
        {
            string? username, password;

            Console.Write("Input your username: ");
            username = Console.ReadLine();

            Console.Write("Input your password: ");
            password = Console.ReadLine();

            if (username == null || password == null)
            {
                throw new Exception("Invalid login or password!");
            }

            return (username, password);
        }

        public static void HandleLogin(Menu menu, Database.DatabaseAccess db)
        {
            (string username, string password) = getInput();
            string user = db.FindEqual("dbo.Users", new List<(string, string)> { ("Username", username), ("Password", password) });
            string[] userFields = user.Split(' ');

            if (user != "")
            {
                if (userFields[userFields.Length - 2] == "manager")
                {
                    ToMainMenu(menu, db);
                }
                else
                {
                    Console.Clear();

                    (new Menu(db, "Main menu", 1, new List<string> { "Logout" }, new List<Action<Menu, Database.DatabaseAccess>> { HandleLogout })).Run();
                }
            }
            else
            {
                throw (new Exception("Wrong username or password!"));
            }
        }

        public static void HandleRegister(Menu menu, Database.DatabaseAccess db)
        {
            (string username, string password) = getInput();

            db.Insert("dbo.Users", new List<string> { username, password, "guest" });

            ToMainMenu(menu, db);
        }

        public static void ToMainMenu(Menu menu, Database.DatabaseAccess db)
        {
            Menu mainMenu = new Menu(db, "Main menu", 4,
                                     new List<string> { "View actions", "Add an action", "Modify an action", "Logout" },
                                     new List<Action<Menu, Database.DatabaseAccess>> { HandleView, HandleAdd, HandleModify, HandleLogout });

            mainMenu.Run();
        }

        public static void HandleView(Menu menu, Database.DatabaseAccess db)
        {
            Menu actionsMenu = new Menu(db, "Actions menu", 6,
                                        new List<string> { "View all actions", "View active actions", "View past actions", "View future actions", "Back to main menu", "Logout" },
                                        new List<Action<Menu, Database.DatabaseAccess>> { HandleAll, HandleActive, HandlePast, HandleFuture, ToMainMenu, HandleLogout });
            Console.Clear();

            actionsMenu.Run();
        }

        public static void HandleAll(Menu menu, Database.DatabaseAccess db)
        {
            string result = db.FindAll("Actions");

            if (result != "")
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("There are no actions yet.");
            }

            menu.Run();
        }

        public static void HandleActive(Menu menu, Database.DatabaseAccess db)
        {
            string result = db.FindEqual("Actions", new List<(string, string)> { ("State", "active")});

            if (result != "")
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("There are no active actions yet.");
            }

            menu.Run();
        }

        public static void HandlePast(Menu menu, Database.DatabaseAccess db)
        {
            string result = db.FindEqual("Actions", new List<(string, string)> { ("State", "past") });

            if (result != "")
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("There are no past actions yet.");
            }

            menu.Run();
        }

        public static void HandleFuture(Menu menu, Database.DatabaseAccess db)
        {
            string result = db.FindEqual("Actions", new List<(string, string)> { ("State", "future") });

            if (result != "")
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("There are no future actions yet.");
            }

            menu.Run();
        }



        public static void HandleAdd(Menu menu, Database.DatabaseAccess db)
        {
            string? state, article;
            int discount;

            Console.Write("Input the state of the action: ");
            state = Console.ReadLine();            
            
            Console.Write("Input the article: ");
            article = Console.ReadLine();  
            
            if (state == null || article == null) 
            {
                throw new Exception("Wrong values!");
            }
            
            Console.Write("Input the discount: ");
            discount = Convert.ToInt32(Console.ReadLine());

            db.Insert("Actions", new List<string> { state, article, discount.ToString() });

            menu.Run();
        }

        public static void HandleModify(Menu menu, Database.DatabaseAccess db)
        {
            string? article, discount;
            string? newState, newArticle, newDiscount;

            Console.Write("Input the article: ");
            article = Console.ReadLine();

            Console.Write("Input the discount: ");
            discount = Console.ReadLine();

            Console.Write("\nInput the new state: ");
            newState = Console.ReadLine();

            Console.Write("Input the new article: ");
            newArticle = Console.ReadLine();

            Console.Write("Input the new discount: ");
            newDiscount = Console.ReadLine();

            if (article == null || discount == null || newState == null || newArticle == null || newDiscount == null) 
            {
                throw new Exception("Wrong values!");
            }

            db.Update("Actions", new List<(string, string)> { ("State", newState), ("Article", newArticle), ("Discount", newDiscount) },
                      new List<(string, string)> { ("State", "future"), ("Article", article), ("Discount", discount) });

            menu.Run();
        }

        public static void HandleLogout(Menu menu, Database.DatabaseAccess db)
        {
            Environment.Exit(0);
        }
    }
}
