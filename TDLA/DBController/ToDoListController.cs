using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDLA.Models;

namespace TDLA.DBController
{
    class ToDoListController
    {
        public List<ToDoLists> GetAll()
        {
            using (DBEntities db = new DBEntities())
            {
                var todo_lists = db.ToDoLists.ToList();
                return todo_lists;
            }
        }

        public void Insert(ToDoLists list)
        {
            using (DBEntities db = new DBEntities())
            {
                db.ToDoLists.Add(list);
                db.SaveChanges();
            }
        }

        public void Update(ToDoLists list)
        {
            using (DBEntities db = new DBEntities())
            {
                db.ToDoLists.Attach(list);
                db.Entry(list).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                var list = db.ToDoLists.Where(c => c.ID == id).FirstOrDefault();

                if (list != null)
                {
                    db.ToDoLists.Remove(list);
                    db.SaveChanges();
                }
            }
        }
    }
}
