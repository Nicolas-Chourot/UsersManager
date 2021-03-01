using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using UsersManager.Data;
using UsersManager.Models;

namespace UsersManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //migrateCoutriesDataToDataBase();
            //migrateCoutriesDataToJSON();
            //migrateUsersToJson();
        }

        public void migrateCoutriesDataToDataBase()
        {
            UsersManagerContext db = new UsersManagerContext();
            Country country = db.Countries.FirstOrDefault();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Countries.txt");
            if (country == null)
            {
                StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/Countries.txt"));
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] tokens = line.Split(';');
                    Country newCountry = new Country();
                    newCountry.Name = tokens[0];
                    db.Countries.Add(newCountry);
                    db.SaveChanges();
                }

                sr.Dispose();
            }
            db.Dispose();
        }

        public void migrateCoutriesDataToJSON()
        {
            List<Country> countries = new List<Country>();
            int index = 0;
            StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/Countries.txt"));
            while (!sr.EndOfStream)
            {
                index++;
                string line = sr.ReadLine();
                string[] tokens = line.Split(';');
                Country newCountry = new Country();
                newCountry.Id = index;
                newCountry.Name = tokens[0];
                countries.Add(newCountry);
            }
            sr.Dispose();
            using (StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~/App_Data/Countries.json")))
            {
                sw.WriteLine(JsonConvert.SerializeObject(countries));
            }
        }

        public void migrateUsersToJson()
        {
            UsersManagerContext db = new UsersManagerContext();
            using (StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~/App_Data/Users.json")))
            {
                List<User> users = new List<User>();
                foreach(User user in db.Users)
                {
                    User u = new User();
                    u.Copy(user);
                    users.Add(u);
                }
                sw.WriteLine(JsonConvert.SerializeObject(users));
            }
            db.Dispose();
        }
    }
}
