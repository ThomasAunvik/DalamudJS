using System;
using System.Collections.Generic;
using System.Text;

namespace DalamudJSLib
{
    public class DalamudFunctions
    {
        public Action<object> CallFunction { get; set; }
    }
    public class DalamudJSInterface
    {
        public DalamudFunctions Dalamud { get; set; }
    }
}
