using ImGuiNET;
using System;
using System.Collections.Generic;
using TDLA.DBController;
using TDLA.ImGUI.Panels;
using TDLA.Models;

namespace TDLA.ImGUI
{
    class UI
    {
        private bool init = false;

        public UsersController uc = new UsersController();
        public ToDoListController tdc = new ToDoListController();
        public ToDoListSharesController tdsc = new ToDoListSharesController();
        public TaskController tc = new TaskController();

        public List<Users> users = new List<Users>();
        public List<ToDoLists> todo_lists = new List<ToDoLists>();
        public List<ToDoListShares> todo_list_shares = new List<ToDoListShares>();
        public List<Tasks> tasks = new List<Tasks>();

        public int logged_at_id = 0;
        public bool is_admin = false;

        public bool[] window_array = { true, true, true, true, true };

        public void DrawUI()
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
