using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using TruckGoMobile.Droid.Database;
using TruckGoMobile.Interfaces;

[assembly:Xamarin.Forms.Dependency(typeof(DroidConnection))]
namespace TruckGoMobile.Droid.Database
{
    public class DroidConnection : IDatabase
    {
        public SQLiteConnection GetConnection()
        {
            try
            {
                var fileName = "TruckGoUserDb.db";
                var documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                var fullPath = Path.Combine(documentPath, fileName);
                if (!File.Exists(fullPath))
                {
                    using (var binaryReader = new BinaryReader(Android.App.Application.Context.Assets.Open(fileName)))
                    {
                        using (var binaryWriter = new BinaryWriter(new FileStream(fullPath, FileMode.Create)))
                        {
                            byte[] buffer = new byte[2048];
                            int length = 0;
                            while ((length = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                binaryWriter.Write(buffer, 0, length);
                            }
                        }
                    }
                }
                return new SQLiteConnection(fullPath);
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}