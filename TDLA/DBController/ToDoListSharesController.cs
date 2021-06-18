using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDLA.DBController
{
    class ToDoListSharesController
    {
        public List<ToDoListShares> GetAll()
        {
            using (DBEntities db = new DBEntities())
            {
                var todo_list_shares = db.ToDoListShares.ToList();
                return todo_list_shares;
            }
        }

        public void Insert(ToDoListShares share_list)
        {
            using (DBEntities db = new DBEntities())
            {
                db.ToDoListShares.Add(share_list);
                db.SaveChanges();
            }
        }

        public void Update(ToDoListShares share_list)
        {
            using (DBEntities db = new DBEntities())
            {
                db.ToDoListShares.Attach(share_list);
                db.Entry(share_list).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                var share_list = db.ToDoListShares.Where(c => c.ID == id).FirstOrDefault();

                if (share_list != null)
                {
                    db.ToDoListShares.Remove(share_list);
                    db.SaveChanges();
                }
            }
        }
    }
}
