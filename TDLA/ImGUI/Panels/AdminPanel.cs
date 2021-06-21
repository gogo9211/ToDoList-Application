using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
using System.Linq;
using System.Text;
using TDLA.Functions;
using TDLA.Models;

namespace TDLA.ImGUI.Panels
{
    static class AdminPanel
    {
        private static Users current_user_selected;

        private static byte[] username_edit = new byte[16];
        private static byte[] password_edit = new byte[16];
        private static byte[] first_name_edit = new byte[16];
        private static byte[] last_name_edit = new byte[16];

        private static byte[] username = new byte[16];
        private static byte[] password = new byte[16];
        private static byte[] first_name = new byte[16];
        private static byte[] last_name = new byte[16];
        public static void DrawAdminPanel()
        {
            if (ImGui.Begin("Users", ref Program.ui.window_array[1]))
            {
                foreach (var user in Program.ui.users)
                {
                    if (ImGui.Selectable("user: " + user.FirstName))
                    {
                        var current_user = user;
                        current_user_selected = current_user;

                        username_edit = Encoding.ASCII.GetBytes(current_user.Username);
                        password_edit = Encoding.ASCII.GetBytes(current_user.Password);
                        first_name_edit = Encoding.ASCII.GetBytes(current_user.FirstName);
                        last_name_edit = Encoding.ASCII.GetBytes(current_user.LastName);

                        Array.Resize(ref username_edit, 16);
                        Array.Resize(ref password_edit, 16);
                        Array.Resize(ref first_name_edit, 16);
                        Array.Resize(ref last_name_edit, 16);
                    }
                }
            }

            if (ImGui.Begin("Create Account", ref Program.ui.window_array[1], ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.PushItemWidth(300);

                ImGui.InputText("Username", username, 16);
                ImGui.InputText("Password", password, 16);

                ImGui.InputText("First Name", first_name, 16);
                ImGui.InputText("Last Name", last_name, 16);

                ImGui.Spacing();
                if (ImGui.Button("Register", new Vector2(130, 20)))
                {
                    var name = Encoding.Default.GetString(username).Split('\0')[0];
                    var pass = Encoding.Default.GetString(password).Split('\0')[0];

                    var first_n = Encoding.Default.GetString(first_name).Split('\0')[0];
                    var last_n = Encoding.Default.GetString(last_name).Split('\0')[0];

                    if (name == "" || pass == "" || first_n == "" || last_n == "")
                    {
                        Console.WriteLine("Bad Field");
                        return; 
                    }

                    AdminFunctions.RegisterUser(name, pass, first_n, last_n);
                }
            }

            if (ImGui.Begin("Edit User", ref Program.ui.window_array[1], ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.PushItemWidth(300);

                ImGui.InputText("Username", username_edit, 16);
                ImGui.InputText("Password", password_edit, 16);

                ImGui.InputText("First Name", first_name_edit, 16);
                ImGui.InputText("Last Name", last_name_edit, 16);

                ImGui.Spacing();
                if (ImGui.Button("Edit", new Vector2(130, 20)))
                {
                    var name = Encoding.Default.GetString(username_edit).Split('\0')[0];
                    var pass = Encoding.Default.GetString(password_edit).Split('\0')[0];

                    var first_n = Encoding.Default.GetString(first_name_edit).Split('\0')[0];
                    var last_n = Encoding.Default.GetString(last_name_edit).Split('\0')[0];

                    if (name == "" || pass == "" || first_n == "" || last_n == "")
                    {
                        Console.WriteLine("Bad Field");
                        return; 
                    }

                    if (current_user_selected != null && current_user_selected.ID != 1003)
                    {
                        AdminFunctions.EditUser(name, pass, first_n, last_n, current_user_selected);
                    }
                    else
                    {
                        Console.WriteLine("You Cant Edit This User");
                    }
                }

                ImGui.SameLine();
                if (ImGui.Button("Delete", new Vector2(130, 20)))
                {
                    if (current_user_selected != null && current_user_selected.ID != 1003) //main admin account
                    {
                        AdminFunctions.DeleteUser(current_user_selected);

                        //reset everything after user gets deleted
                        current_user_selected = null;
                        Array.Clear(username_edit, 0, username_edit.Length);
                        Array.Clear(password_edit, 0, username_edit.Length);
                        Array.Clear(first_name_edit, 0, username_edit.Length);
                        Array.Clear(last_name_edit, 0, username_edit.Length);
                    }
                    else
                    {
                        Console.WriteLine("You Cant Delete This User");
                    }
                }
            }
        }
        public static void Reset()
        {
            Array.Clear(username, 0, username.Length);
            Array.Clear(password, 0, username.Length);
            Array.Clear(first_name, 0, username.Length);
            Array.Clear(last_name, 0, username.Length);

            Array.Clear(username_edit, 0, username_edit.Length);
            Array.Clear(password_edit, 0, username_edit.Length);
            Array.Clear(first_name_edit, 0, username_edit.Length);
            Array.Clear(last_name_edit, 0, username_edit.Length);

            current_user_selected = null;
        }
    }
}
