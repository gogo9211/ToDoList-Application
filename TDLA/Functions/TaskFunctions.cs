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
    static class TaskFunctions
    {
        public static void CreateTask(int list_id, string title, string description)
        {
            Tasks task = new Tasks();

            task.Title = title;
            task.Description = description;
            task.ListID = list_id;
            task.Completed = 0;
            task.Creator = Program.ui.logged_at_id;
            task.ChangedBy = Program.ui.logged_at_id;
            task.LastChange = DateTime.Now;
            task.CreatedAt = DateTime.Now;

            Program.ui.tc.Insert(task);
            Program.ui.tasks = Program.ui.tc.GetAll();
            Console.WriteLine("Task Created");
        }

        public static void EditTask(ref Tasks task, string title, string description)
        {
            task.Title = title;
            task.Description = description;
            task.ChangedBy = Program.ui.logged_at_id;
            task.LastChange = DateTime.Now;

            Program.ui.tc.Update(task);
            Program.ui.tasks = Program.ui.tc.GetAll();

            Console.WriteLine("Task Updated");
        }

        public static void CompleteTask(ref Tasks task, int complete)
        {
            task.Completed = complete;
            task.ChangedBy = Program.ui.logged_at_id;
            task.LastChange = DateTime.Now;

            Program.ui.tc.Update(task);
            Program.ui.tasks = Program.ui.tc.GetAll();

            Console.WriteLine("Task Updated");
        }

        public static void DeleteTask(Tasks task)
        {
            var assigns = Program.ui.task_assigns.Where(c => c.TaskID == task.ID).ToList();
            foreach (var assign in assigns)
            {
                Program.ui.tac.Delete(assign.ID);
            }

            Program.ui.tc.Delete(task.ID);
            Program.ui.tasks = Program.ui.tc.GetAll();
            Program.ui.task_assigns = Program.ui.tac.GetAll();

            Console.WriteLine("Task Deleted");
        }

        public static bool AssignTask(int current_assign_user_id, ToDoLists current_list, Tasks current_task_selected)
        {
            bool user_exist = Program.ui.users.Any(item => item.ID == current_assign_user_id);
            if (user_exist == false)
            {
                Console.WriteLine("Invalid User ID");
                return false;
            }

            bool is_user_owner_of_list = current_list.Creator == current_assign_user_id;
            bool is_part_of_list = Program.ui.todo_list_shares.Any(item => item.UserID == current_assign_user_id && item.ListID == current_list.ID);
            if (is_user_owner_of_list == false && is_part_of_list == false)
            {
                Console.WriteLine("User Is Not Part Of The List");
                return false;
            }

            var is_already_assigned = Program.ui.task_assigns.Where(item => item.TaskID == current_task_selected.ID && item.UserID == current_assign_user_id).FirstOrDefault();
            if (is_already_assigned != null)
            {
                Console.WriteLine("User Is Already Assigned To This Task");
                return false;
            }

            TaskAssigns new_assign = new TaskAssigns();

            new_assign.TaskID = current_task_selected.ID;
            new_assign.UserID = current_assign_user_id;

            Program.ui.tac.Insert(new_assign);
            Program.ui.task_assigns = Program.ui.tac.GetAll();
            Console.WriteLine("Task Assigned");

            return true;
        }
    }
}
