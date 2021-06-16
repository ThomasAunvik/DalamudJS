using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DalamudJSLib
{
    public static class Misc
    {
        public static bool TryGetFile(string path, out string code)
        {
            try
            {
                code = File.ReadAllText(path);
                return true;
            }
            catch (Exception) { Console.WriteLine("Failed to load file: " + path); }

            code = "";
            return false;
        }
    }
}
