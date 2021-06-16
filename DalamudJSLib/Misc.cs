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
            catch (Exception) { }

            code = "";
            return false;
        }
    }
}
