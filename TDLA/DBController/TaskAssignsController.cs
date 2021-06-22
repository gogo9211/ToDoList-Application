using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDLA.Models;

namespace TDLA.DBController
{
    class TaskAssignsController
    {
        public List<TaskAssigns> GetAll()
        {
            using (DBEntities db = new DBEntities())
            {
                var assigns = db.TaskAssigns.ToList();
                return assigns;
            }
        }

        public void Insert(TaskAssigns assign)
        {
            using (DBEntities db = new DBEntities())
            {
                db.TaskAssigns.Add(assign);
                db.SaveChanges();
            }
        }

        public void Update(TaskAssigns assign)
        {
            using (DBEntities db = new DBEntities())
            {
                db.TaskAssigns.Attach(assign);
                db.Entry(assign).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                var assign = db.TaskAssigns.Where(c => c.ID == id).FirstOrDefault();

                if (assign != null)
                {
                    db.TaskAssigns.Remove(assign);
                    db.SaveChanges();
                }
            }
        }
    }
}
