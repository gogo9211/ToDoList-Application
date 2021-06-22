using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
using System.Linq;
using System.Text;
using TDLA.DBController;
using TDLA.Functions;
using TDLA.Models;

namespace TDLA.ImGUI.Panels
{
    static class ToDoListPanel
    {
        private static ToDoLists current_list_selected;
        private static bool list_is_shared = false;

        private static int current_share_user_id = 0;

        private static byte[] title = new byte[16];
        private static byte[] title_edit = new byte[16];
        private static byte[] title_share = new byte[16];
        public static void DrawToDoLists()
        {
            if (current_list_selected != null)
            {
                TaskPanel.DrawTasks(current_list_selected);
            }

            if (ImGui.Begin("Lists", ref Program.ui.window_array[2]))
            {
                foreach (var list in Program.ui.todo_lists)
                {
                    if (list.Creator != Program.ui.logged_at_id)
                        continue;

                    if (ImGui.Selectable("List: " + list.Title))
                    {
                        var current_list = list;
                        current_list_selected = current_list;
                        list_is_shared = false;

                        title_edit = Encoding.ASCII.GetBytes(current_list.Title);
                        title_share = Encoding.ASCII.GetBytes(current_list.Title);

                        Array.Resize(ref title_edit, 16);
                        Array.Resize(ref title_share, 16);

                        TaskPanel.Reset();
                    }
                }

                if (ImGui.Begin("Shared Lists", ref Program.ui.window_array[2]))
                {
                    foreach (var share in Program.ui.todo_list_shares)
                    {
                        if (share.UserID != Program.ui.logged_at_id)
                            continue;

                        var list = Program.ui.todo_lists.Where(c => c.ID == share.ListID).FirstOrDefault();
                        if (ImGui.Selectable("List: " + list.Title))
                        {
                            var current_list = list;
                            current_list_selected = current_list;
                            list_is_shared = true;

                            title_edit = Encoding.ASCII.GetBytes(current_list.Title);
                            title_share = Encoding.ASCII.GetBytes(current_list.Title);

                            Array.Resize(ref title_edit, 16);
                            Array.Resize(ref title_share, 16);

                            TaskPanel.Reset();
                        }
                    }
                }
            }

            if (ImGui.Begin("Share List", ref Program.ui.window_array[2], ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.PushItemWidth(300);

                var name = Encoding.Default.GetString(title_share).Split('\0')[0];
                ImGui.Text("List: " + name);

                ImGui.InputInt("User ID", ref current_share_user_id);

                ImGui.Spacing();
                if (ImGui.Button("Share", new Vector2(130, 20)))
                {
                    if (current_list_selected == null)
                    {
                        Console.WriteLine("Invalid List");
                        return;
                    }

                    ToDoListFunctions.ShareList(current_share_user_id, current_list_selected);
                }
            }

            if (ImGui.Begin("Create List", ref Program.ui.window_array[2], ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.PushItemWidth(300);

                ImGui.InputText("Title", title, 16);

                ImGui.Spacing();
                if (ImGui.Button("Create", new Vector2(130, 20)))
                {
                    var name = Encoding.Default.GetString(title).Split('\0')[0];

                    if (name == "")
                    {
                        Console.WriteLine("Bad Field");
                        return; 
                    }

                    ToDoListFunctions.CreateList(name);
                }
            }

            if (ImGui.Begin("Edit List", ref Program.ui.window_array[2], ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.PushItemWidth(300);

                ImGui.InputText("Title", title_edit, 16);

                ImGui.Spacing();
                if (ImGui.Button("Edit", new Vector2(130, 20)))
                {
                    var name = Encoding.Default.GetString(title_edit).Split('\0')[0];

                    if (name == "")
                    {
                        Console.WriteLine("Bad Field");
                        return; 
                    }

                    if (current_list_selected == null)
                    {
                        Console.WriteLine("You Cant Edit This List");
                        return;
                    }

                    ToDoListFunctions.EditList(current_list_selected, name);

                    title_share = Encoding.ASCII.GetBytes(name);
                    Array.Resize(ref title_share, 16);
                }

                ImGui.SameLine();
                if (ImGui.Button("Delete", new Vector2(130, 20)))
                {
                    if (current_list_selected == null)
                    {
                        Console.WriteLine("You Cant Edit This List");
                        return;
                    }

                    ToDoListFunctions.DeleteList(list_is_shared, current_list_selected, Program.ui.logged_at_id);

                    current_list_selected = null;
                    Array.Clear(title_edit, 0, title_edit.Length);
                    Array.Clear(title_share, 0, title_edit.Length);

                    TaskPanel.Reset();
                }
            }
        }
        public static void Reset()
        {
            Array.Clear(title, 0, title.Length);
            Array.Clear(title_edit, 0, title_edit.Length);
            Array.Clear(title_share, 0, title_edit.Length);

            current_share_user_id = 0;
            list_is_shared = false;
            current_list_selected = null;
        }
    }
}
