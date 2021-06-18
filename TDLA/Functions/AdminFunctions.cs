using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
using System.Linq;
using System.Text;
using TDLA.DBController;
using TDLA.ImGUI;

namespace TDLA.Functions
{
    class AdminFunctions
    {
        public static void RegisterUser(string name, string pass, string first_n, string last_n)
        {
            bool user_exist = UI.users.Any(item => item.Username == name);
            if (user_exist == true)
            {
                Console.WriteLine("Username Already Registered");
                return;
            }
            Users user = new Users();

            user.Username = name;
            user.Password = pass;
            user.FirstName = first_n;
            user.LastName = last_n;
            user.Admin = 0;
            user.Creator = UI.logged_at_id;
            user.CreatedAt = DateTime.Now;
            user.LastChange = DateTime.Now;
            user.ChangedBy = UI.logged_at_id;

            UI.uc.Insert(user);
            UI.users = UI.uc.GetAll();

            Console.WriteLine("Account Created");
        }
        
        public static void EditUser(string name, string pass, string first_n, string last_n, Users current_user_selected)
        {
            current_user_selected.Username = name;
            current_user_selected.Password = pass;
            current_user_selected.FirstName = first_n;
            current_user_selected.LastName = last_n;
            current_user_selected.LastChange = DateTime.Now;
            current_user_selected.ChangedBy = UI.logged_at_id;

            UI.uc.Update(current_user_selected);
            UI.users = UI.uc.GetAll();

            Console.WriteLine("User Updated");
        }

        public static void DeleteUser(Users current_user_selected)
        {
            var user_lists = UI.todo_lists.Where(c => c.Creator == current_user_selected.ID).ToList();
            foreach (var list in user_lists)
            {
                ToDoListFunctions.DeleteList(false, list, UI.logged_at_id);
            }

            var user_shares = UI.todo_list_shares.Where(c => c.UserID == current_user_selected.ID).ToList();
            foreach (var share in user_shares)
            {
                var list = UI.todo_lists.Where(c => c.ID == share.ListID).FirstOrDefault();
                ToDoListFunctions.DeleteList(true, list, share.UserID);
            }

            UI.uc.Delete(current_user_selected.ID);
            UI.users = UI.uc.GetAll();

            Console.WriteLine("User Deleted");
        }
    }
}
