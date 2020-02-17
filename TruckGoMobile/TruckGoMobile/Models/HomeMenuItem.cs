using System;
using System.Collections.Generic;
using System.Text;

namespace TruckGoMobile.Models
{
    public enum MenuItemType
    {
        Login,
        Home,
        Company,
        Emergency,
        Profile,
        Friends,
        Route,
        Obidi,
        Camera,
        Weather,
        Logout
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
