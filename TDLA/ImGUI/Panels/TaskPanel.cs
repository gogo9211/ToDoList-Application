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
        private static Task current_task;
        private static byte[] title = new byte[16];
        private static byte[] description = new byte[16];

        public static void DrawTasks(ToDoLists active_list)
        {
            if (ImGui.Begin("Create Task", ref Program.ui.window_array[4], ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.PushItemWidth(300);

                ImGui.InputText("Title", title, 16);
                ImGui.InputText("Description", description, 16);

                ImGui.Spacing();
                if (ImGui.Button("Create Task", new Vector2(130, 20)))
                {
                    var name = Encoding.Default.GetString(title).Split('\0')[0];
                    var info = Encoding.Default.GetString(description).Split('\0')[0];

                    TaskFunctions.CreateTask(active_list.ID, name, info);
                }
            }
        }

        public static void Reset()
        {
            Array.Clear(title, 0, title.Length);
            Array.Clear(description, 0, description.Length);
        }
    }
}
