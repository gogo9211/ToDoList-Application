using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TDLA.DBController;
using TDLA.ImGUI.Panels;

namespace TDLA.ImGUI
{
    class UI
    {
        private static bool init = false;

        public static UsersController uc = new UsersController();
        public static ToDoListController tdc = new ToDoListController();
        public static ToDoListSharesController tdsc = new ToDoListSharesController();

        public static List<Users> users = new List<Users>();
        public static List<ToDoLists> todo_lists = new List<ToDoLists>();
        public static List<ToDoListShares> todo_list_shares = new List<ToDoListShares>();

        public static int logged_at_id = 0;
        public static bool is_admin = false;

        public static bool[] window_array = { true, true, true, true, true };

        public static void DrawUI()
        {
            if (!init)
            {
                users = uc.GetAll();
                todo_lists = tdc.GetAll();
                todo_list_shares = tdsc.GetAll();

                init = true;
            }
                
            if (is_admin)
            {
                AdminPanel.DrawAdminPanel();
                LogOutPanel.DrawLogOut();
                return;
            }

            if (logged_at_id == 0)
            {
                LoginPanel.DrawLogin();
            }
            else
            {
                ToDoListPanel.DrawToDoLists();
                LogOutPanel.DrawLogOut();
            }
        }
    }
}
