using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDLA.DBController
{
    class UsersController
    {
        public List<Users> GetAll()
        {
            using (DBEntities db = new DBEntities())
            {
                var users = db.Users.ToList();
                return users;
            }
        }

        public void Insert(Users user)
        {
            using (DBEntities db = new DBEntities())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public void Update(Users user)
        {
            using (DBEntities db = new DBEntities())
            {
                db.Users.Attach(user);
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                var user = db.Users.Where(c => c.ID == id).FirstOrDefault();

                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
            }
        }
    }
}
