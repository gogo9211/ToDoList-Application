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
    static class LoginFunctions
    {
        public static void Login(string name, string pass)
        {
            foreach (var user in Program.ui.users)
            {
                if (user.Username == name && user.Password == pass)
                {
                    Program.ui.logged_at_id = user.ID;
                    Program.ui.is_admin = Convert.ToBoolean(user.Admin);
                }
            }
        }
    }
}
