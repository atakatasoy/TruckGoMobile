using System;
using System.Collections.Generic;
using System.Text;

namespace TruckGoMobile
{
    public class EntryEventArgs : EventArgs
    {
        public string Name { get; set; }
        public EntryEventArgs(string name)
        {
            Name = name;
        }
    }
}
