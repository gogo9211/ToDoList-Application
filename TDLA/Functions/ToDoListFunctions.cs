using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
using System.Linq;
using System.Text;
using TDLA.DBController;
using TDLA.ImGUI;
using TDLA.Models;

namespace TDLA.Functions
{
    static class ToDoListFunctions
    {
        public static void ShareList(int current_share_user_id, ToDoLists current_list_selected)
        {
            bool user_exist = Program.ui.users.Any(item => item.ID == current_share_user_id);
            if (user_exist == false)
            {
                Console.WriteLine("Invalid User ID");
                return;
            }

            var creator_of_list_id = Program.ui.users.Where(c => c.ID == current_list_selected.Creator).FirstOrDefault();
            if (creator_of_list_id.ID == current_share_user_id)
            {
                Console.WriteLine("Cant Share With This User");
                return;
            }

            var is_already_sharing = Program.ui.todo_list_shares.Where(c => c.ListID == current_list_selected.ID && c.UserID == current_share_user_id).FirstOrDefault();
            if (is_already_sharing != null)
            {
                Console.WriteLine("User Is Already Sharing This List");
                return;
            }

            ToDoListShares new_share = new ToDoListShares();

            new_share.ListID = current_list_selected.ID;
            new_share.UserID = current_share_user_id;

            Program.ui.tdsc.Insert(new_share);
            Program.ui.todo_list_shares = Program.ui.tdsc.GetAll();
            Console.WriteLine("Share Created");
        }

        public static void CreateList(string name)
        {
            bool list_exist = Program.ui.todo_lists.Any(item => item.Title == name);
            if (list_exist == true)
            {
                Console.WriteLine("List Already Created");
                return;
            }

            ToDoLists list = new ToDoLists();

            list.Title = name;
            list.Creator =  Program.ui.logged_at_id;
            list.ChangedBy =  Program.ui.logged_at_id;
            list.LastChange = DateTime.Now;
            list.CreatedAt = DateTime.Now;

            Program.ui.tdc.Insert(list);
            Program.ui.todo_lists = Program.ui.tdc.GetAll();
            Console.WriteLine("List Created");
        }

        public static void EditList(ToDoLists current_list_selected, string name)
        {
            bool list_exist = Program.ui.todo_lists.Any(item => item.Title == name);
            if (list_exist == true)
            {
                Console.WriteLine("Name already exist");
                return;
            }

            current_list_selected.Title = name;
            current_list_selected.LastChange = DateTime.Now;
            current_list_selected.ChangedBy = Program.ui.logged_at_id;

            Program.ui.tdc.Update(current_list_selected);
            Program.ui.todo_lists =  Program.ui.tdc.GetAll();

            Console.WriteLine("List Updated");
        }

        public static void DeleteList(bool list_is_shared, ToDoLists current_list_selected, int user_id)
        {
            if (list_is_shared)
            {
                var tasks = Program.ui.tasks.Where(c => c.ListID == current_list_selected.ID).ToList();
                foreach (var task in tasks)
                {
                    var assign = Program.ui.task_assigns.Where(c => c.TaskID == task.ID && c.UserID == user_id).FirstOrDefault();

                    if (assign != null)
                    {
                        Program.ui.tac.Delete(assign.ID);
                    }
                }

                var share = Program.ui.todo_list_shares.Where(c => c.ListID == current_list_selected.ID && c.UserID == user_id).FirstOrDefault();

                Program.ui.tdsc.Delete(share.ID);
                Program.ui.todo_list_shares = Program.ui.tdsc.GetAll();
                Program.ui.task_assigns = Program.ui.tac.GetAll();

                Console.WriteLine("Share Deleted");
            }
            else
            {
                var shares = Program.ui.todo_list_shares.Where(c => c.ListID == current_list_selected.ID).ToList();
                foreach (var share in shares)
                {
                     Program.ui.tdsc.Delete(share.ID);
                }

                var tasks = Program.ui.tasks.Where(c => c.ListID == current_list_selected.ID).ToList();
                foreach (var task in tasks)
                {
                    TaskFunctions.DeleteTask(task);
                }

                Program.ui.tdc.Delete(current_list_selected.ID);

                Program.ui.todo_lists = Program.ui.tdc.GetAll();
                Program.ui.todo_list_shares = Program.ui.tdsc.GetAll();
                Program.ui.tasks = Program.ui.tc.GetAll();
                Program.ui.task_assigns = Program.ui.tac.GetAll();

                Console.WriteLine("List Deleted");
            }
        }
    }
}
