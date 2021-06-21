using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
using System.Linq;
using System.Text;
using TDLA.DBController;
using TDLA.Functions;

namespace TDLA.ImGUI.Panels
{
    static class LoginPanel
    {
        private static byte[] username = new byte[16];
        private static byte[] password = new byte[16];

        public static void DrawLogin()
        {
            if (ImGui.Begin("Login", ref Program.ui.window_array[0], ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.PushItemWidth(300);

                ImGui.InputText("Username", username, 16);
                ImGui.InputText("Password", password, 16);

                ImGui.Spacing();
                if (ImGui.Button("Login", new Vector2(130, 20)))
                {
                    var name = Encoding.Default.GetString(username).Split('\0')[0];
                    var pass = Encoding.Default.GetString(password).Split('\0')[0];

                    LoginFunctions.Login(name, pass);
                }
            }
        }

        public static void Reset()
        {
            Array.Clear(username, 0, username.Length);
            Array.Clear(password, 0, username.Length);
        }
    }
}
