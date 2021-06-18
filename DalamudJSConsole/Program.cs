using DalamudJSLib;
using System;
using System.Threading.Tasks;

namespace DalamudJSConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DalamudJSEngine engine = new DalamudJSEngine("./PluginScript/build/index.js");
            await engine.Load();

            engine.ExecuteCode();
        }
    }
}
