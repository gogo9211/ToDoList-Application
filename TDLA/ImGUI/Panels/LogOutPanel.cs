using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
using System.Linq;
using System.Text;
using TDLA.DBController;

namespace TDLA.ImGUI.Panels
{
    static class LogOutPanel
    {
        public static void DrawLogOut()
        {
            if (ImGui.Begin("Log Out", ref Program.ui.window_array[3], ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                if (ImGui.Button("Log Out", new Vector2(134, 20)))
                {
                    Program.ui.logged_at_id = 0;
                    Program.ui.is_admin = false;

                    ToDoListPanel.Reset();
                    LoginPanel.Reset();
                    AdminPanel.Reset();
                    TaskPanel.Reset();
                }
            }
        }
    }
}
