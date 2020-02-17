using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckGoMobile.Interfaces
{
    public interface IDatabase
    {
        SQLiteConnection GetConnection();
    }
}
