using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TruckGoMobile.Interfaces;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public class User
    {
        [PrimaryKey]
        public string AccessToken { get; set; } = "asd";

        public string Username { get; set; }
        public string UserNameSurname { get; set; }
        public bool LoggedIn { get; set; }
        public DateTime LastLoggedInDate { get; set; }

        List<PropertyInfo> propertyList;

        public void SetProperty<T>(string nameOfProp, T value)
        {
            if (propertyList == null)
                propertyList = GetType().GetProperties().ToList();

            var prop = propertyList.FirstOrDefault(each => each.Name == nameOfProp);

            if (prop == null)
                throw new NullReferenceException();

            //It will throw expection if the T value is wrong type regarding to the nameOfProp
            var check = (T)prop.GetValue(this);

            prop.SetValue(this, value);

            using (var con = DependencyService.Get<IDatabase>().GetConnection())
            {
                con.BeginTransaction();
                con.Update(this);
                con.Commit();
            }
        }

        public void SetProperty<T>(string nameOfProp, T value, SQLiteConnection con)
        {
            if (propertyList == null)
                propertyList = GetType().GetProperties().ToList();

            var prop = propertyList.FirstOrDefault(each => each.Name == nameOfProp);

            if (prop == null)
                throw new NullReferenceException();

            //It will throw expection if the T value is wrong type regarding to the nameOfProp
            var check = (T)prop.GetValue(this);

            prop.SetValue(this, value);

            con.BeginTransaction();
            con.Update(this);
            con.Commit();
        }
    }
}
