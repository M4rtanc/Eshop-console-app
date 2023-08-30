using HW02.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW02.Events
{
    public class CommandEvent : EventArgs
    {
        public Command Command { get; set; }
    }
}
