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
    class LoginFunctions
    {
        public static void Login(string name, string pass)
        {
            foreach (var user in UI.users)
            {
                if (user.Username == name && user.Password == pass)
                {
                    UI.logged_at_id = user.ID;
                    UI.is_admin = Convert.ToBoolean(user.Admin);
                }
            }
        }
    }
}
