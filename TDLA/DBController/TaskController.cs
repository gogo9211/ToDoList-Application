using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDLA.Models;

namespace TDLA.DBController
{
    class TaskController
    {
        public List<Tasks> GetAll()
        {
            using (DBEntities db = new DBEntities())
            {
                var tasks = db.Tasks.ToList();
                return tasks;
            }
        }

        public void Insert(Tasks task)
        {
            using (DBEntities db = new DBEntities())
            {
                db.Tasks.Add(task);
                db.SaveChanges();
            }
        }

        public void Update(Tasks task)
        {
            using (DBEntities db = new DBEntities())
            {
                db.Tasks.Attach(task);
                db.Entry(task).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                var task = db.Tasks.Where(c => c.ID == id).FirstOrDefault();

                if (task != null)
                {
                    db.Tasks.Remove(task);
                    db.SaveChanges();
                }
            }
        }
    }
}
