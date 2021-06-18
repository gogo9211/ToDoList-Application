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
    class ToDoListFunctions
    {
        public static void ShareList(int current_share_user_id, ToDoLists current_list_selected)
        {
            bool user_exist = UI.users.Any(item => item.ID == current_share_user_id);
            if (user_exist == false)
            {
                Console.WriteLine("Invalid User ID");
                return;
            }

            var creator_of_list_id = UI.users.Where(c => c.ID == current_list_selected.Creator).FirstOrDefault();
            if (creator_of_list_id.ID == current_share_user_id)
            {
                Console.WriteLine("Cant Share With This User");
                return;
            }

            var is_already_sharing = UI.todo_list_shares.Where(c => c.ListID == current_list_selected.ID && c.UserID == current_share_user_id).FirstOrDefault();
            if (is_already_sharing != null)
            {
                Console.WriteLine("User Is Already Sharing This List");
                return;
            }

            ToDoListShares new_share = new ToDoListShares();

            new_share.ListID = current_list_selected.ID;
            new_share.UserID = current_share_user_id;

            UI.tdsc.Insert(new_share);
            UI.todo_list_shares = UI.tdsc.GetAll();
            Console.WriteLine("Share Created");
        }

        public static void CreateList(string name)
        {
            bool list_exist = UI.todo_lists.Any(item => item.Title == name);
            if (list_exist == true)
            {
                Console.WriteLine("List Already Created");
                return;
            }

            ToDoLists list = new ToDoLists();

            list.Title = name;
            list.Creator = UI.logged_at_id;
            list.ChangedBy = UI.logged_at_id;
            list.LastChange = DateTime.Now;
            list.CreatedAt = DateTime.Now;

            UI.tdc.Insert(list);
            UI.todo_lists = UI.tdc.GetAll();
            Console.WriteLine("List Created");
        }

        public static void EditList(ToDoLists current_list_selected, string name)
        {
            current_list_selected.Title = name;
            current_list_selected.LastChange = DateTime.Now;
            current_list_selected.ChangedBy = UI.logged_at_id;

            UI.tdc.Update(current_list_selected);
            UI.todo_lists = UI.tdc.GetAll();

            Console.WriteLine("List Updated");
        }

        public static void DeleteList(bool list_is_shared, ToDoLists current_list_selected, int user_id)
        {
            if (list_is_shared)
            {
                var share = UI.todo_list_shares.Where(c => c.ListID == current_list_selected.ID && c.UserID == user_id).FirstOrDefault();

                UI.tdsc.Delete(share.ID);
                UI.todo_list_shares = UI.tdsc.GetAll();

                Console.WriteLine("Share Deleted");
            }
            else
            {
                var shares = UI.todo_list_shares.Where(c => c.ListID == current_list_selected.ID).ToList();
                foreach (var share in shares)
                {
                    UI.tdsc.Delete(share.ID);
                }

                UI.tdc.Delete(current_list_selected.ID);
                UI.todo_lists = UI.tdc.GetAll();
                UI.todo_list_shares = UI.tdsc.GetAll();

                Console.WriteLine("List Deleted");
            }
        }
    }
}
