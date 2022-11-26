using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Menu
    {
        string title;
        int len;
        List<string> options;
        List<Action<Menu, Database.DatabaseAccess>> functions;
        Database.DatabaseAccess db;

        public Menu() 
        {
            title = "";
            len = 0;
            options = new List<string>();
            functions = new List<Action<Menu, Database.DatabaseAccess>>();
            db = new Database.DatabaseAccess();
        }

        public Menu(Database.DatabaseAccess db, string title, int len, List<string> options, List<Action<Menu, Database.DatabaseAccess>> functions)
        {
            if ((options.Count != len) || (functions.Count != len))
            {
                throw new ArgumentException("Wrong length!");
            }

            this.db = db;
            this.title = title;
            this.len = len; 
            this.options = options;
            this.functions = functions;
        }

        public override string ToString()
        {
            string result = title + "\n\n";

            for (int i = 0; i < len; i++) 
            {
                result += (i + 1).ToString() + ". " + options[i] + "\n";
            }

            result += "\nSelect an option: ";
            return result;
        }

        public void HandleInput(int ans)
        {
            functions[ans - 1](this, db);
        }

        public void Run()
        {
            Console.Write(this);

            int ans = Convert.ToInt32(Console.ReadLine());

            if ((ans < 0) || (ans > len))
            {
                throw new Exception("Wrong answer!");
            }

            HandleInput(ans);
        }
    }
}
