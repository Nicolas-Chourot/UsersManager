using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsersManager.Models;

namespace UsersManager.Data
{
    // Data Access Layer
    public class UsersManagerContext : DbContext
    {
        public UsersManagerContext() : base("name=UsersManagerContext") { }
        public System.Data.Entity.DbSet<User> Users { get; set; }
        public System.Data.Entity.DbSet<Country> Countries { get; set; }

        public SelectList CountriesToSelectList()
        {
            var items = Countries.Select(c => new SelectListItem() { Value = c.Id.ToString(), Text = c.Name }).ToList();
            items.Insert(0, new SelectListItem { Value = "", Text = "" });
            return new SelectList(items, "Value", "Text");
        }
 
        public User FindUserByEmail(string email, int excludedUserId = 0)
        {
            if (excludedUserId != 0)
                return Users.Where(u => (u.Email == email) && (u.Id != excludedUserId)).FirstOrDefault();
            return Users.Where(u => u.Email == email).FirstOrDefault();
        }

        public bool AddUser(User user)
        {
            if (FindUserByEmail(user.Email) != null)
                return false;

            Users.Add(user);
            SaveChanges();
            return true;
        }

        public bool UpdateUser(User user)
        {
            if (FindUserByEmail(user.Email, user.Id) != null)
                return false;

            if (user.Password == "password_not_changed")
            {
                Entry(user).State = EntityState.Detached;
                User userToUpdate = Users.Find(user.Id);
                userToUpdate.CopyExceptPassword(user);
                Entry(userToUpdate).State = EntityState.Modified;
                SaveChanges();
            }
            else
            {
                Entry(user).State = EntityState.Modified;
                SaveChanges();
            }
            return true;
        }

        public void DeleteUser(int userId)
        {
            User user = Users.Find(userId);
            Users.Remove(user);
            SaveChanges();
        }
    }
}
