using System;
using System.Collections.Generic;
using System.Text;

namespace TruckGoMobile
{
    public class ButtonEventArgs : EventArgs
    {
        public string Name { get; set; }
        public ButtonEventArgs(string name)
        {
            Name = name;
        }
    }
}
