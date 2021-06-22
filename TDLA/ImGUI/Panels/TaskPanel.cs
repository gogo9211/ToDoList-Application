using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
using System.Linq;
using System.Text;
using TDLA.DBController;
using TDLA.Models;
using TDLA.Functions;

namespace TDLA.ImGUI.Panels
{
    static class TaskPanel
    {
        private static Tasks selected_task;
        private static bool completed = false;
        private static bool is_assigned_to_user = false;

        private static int current_assign_user_id;

        private static byte[] title = new byte[16];
        private static byte[] description = new byte[30];

        private static byte[] title_edit = new byte[16];
        private static byte[] description_edit = new byte[30];

        public static void DrawTasks(ToDoLists active_list)
        {
            if (selected_task != null)
            {
                completed = Convert.ToBoolean(selected_task.Completed);
            }

            if (ImGui.Begin("Task Information", ref Program.ui.window_array[4], ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                if (selected_task != null)
                {
                    ImGui.Text("Active List: " + active_list.Title);
                    ImGui.Text("Title: " + selected_task.Title);
                    ImGui.Text("Description: " + selected_task.Description);
                    ImGui.Text("Completed: " + completed);
                    ImGui.Text("Assigned To You: " + is_assigned_to_user);
                }
            }

            if (ImGui.Begin("Tasks", ref Program.ui.window_array[4]))
            {
                foreach (var task in Program.ui.tasks)
                {
                    if (task.ListID != active_list.ID)
                        continue;

                    if (ImGui.Selectable("Task: " + task.Title))
                    {
                        var current_task = task;
                        selected_task = current_task;

                        is_assigned_to_user = Program.ui.task_assigns.Any(item => item.UserID == Program.ui.logged_at_id && item.TaskID == current_task.ID);

                        title_edit = Encoding.ASCII.GetBytes(current_task.Title);
                        description_edit = Encoding.ASCII.GetBytes(current_task.Description);

                        Array.Resize(ref title_edit, 16);
                        Array.Resize(ref description_edit, 30);
                    }
                }
            }

            if (ImGui.Begin("Create Task", ref Program.ui.window_array[4], ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.PushItemWidth(300);

                ImGui.InputText("Title", title, 16);
                ImGui.InputText("Description", description, 30);

                ImGui.Spacing();
                if (ImGui.Button("Create", new Vector2(130, 20)))
                {
                    var name = Encoding.Default.GetString(title).Split('\0')[0];
                    var info = Encoding.Default.GetString(description).Split('\0')[0];

                    if (name == "" || info == "")
                    {
                        Console.WriteLine("Bad Field");
                        return;
                    }

                    TaskFunctions.CreateTask(active_list.ID, name, info);
                }
            }

            if (ImGui.Begin("Edit Task", ref Program.ui.window_array[4], ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.PushItemWidth(300);

                ImGui.InputText("Title", title_edit, 16);
                ImGui.InputText("Description", description_edit, 30);

                ImGui.Spacing();
                if (ImGui.Button("Edit", new Vector2(130, 20)))
                {
                    var name = Encoding.Default.GetString(title_edit).Split('\0')[0];
                    var info = Encoding.Default.GetString(description_edit).Split('\0')[0];

                    if (name == "" || info == "")
                    {
                        Console.WriteLine("Bad Field");
                        return;
                    }

                    if (selected_task == null)
                    {
                        Console.WriteLine("You Cant Edit This Task");
                        return;
                    }

                    TaskFunctions.EditTask(ref selected_task, name, info);
                }

                ImGui.Spacing();
                if (ImGui.Button("Delete", new Vector2(130, 20)))
                {
                    if (selected_task == null)
                    {
                        Console.WriteLine("You Cant Delete This Task");
                        return;
                    }

                    TaskFunctions.DeleteTask(selected_task);

                    selected_task = null;
                    Array.Clear(title_edit, 0, title_edit.Length);
                    Array.Clear(description_edit, 0, description_edit.Length);
                }

                ImGui.Spacing();
                if (ImGui.Checkbox("Completed", ref completed))
                {
                    if (selected_task == null)
                    {
                        Console.WriteLine("You Cant Complete This Task");
                        completed = !completed;
                        return;
                    }

                    TaskFunctions.CompleteTask(ref selected_task, Convert.ToInt32(completed));
                }

                ImGui.Spacing();
                ImGui.InputInt("User ID", ref current_assign_user_id);
                ImGui.Spacing();

                if (ImGui.Button("Assign", new Vector2(130, 20)))
                {
                    if (selected_task == null)
                    {
                        Console.WriteLine("Invalid Task");
                        return;
                    }

                    if (TaskFunctions.AssignTask(current_assign_user_id, active_list, selected_task) == true)
                    {
                        if (current_assign_user_id == Program.ui.logged_at_id)
                        {
                            is_assigned_to_user = true;
                        }
                    }
                }

            }
        }

        public static void Reset()
        {
            Array.Clear(title, 0, title.Length);
            Array.Clear(description, 0, description.Length);
            Array.Clear(title_edit, 0, title_edit.Length);
            Array.Clear(description_edit, 0, description_edit.Length);

            current_assign_user_id = 0;
            is_assigned_to_user = false;
            completed = false;
            selected_task = null;
        }
    }
}
