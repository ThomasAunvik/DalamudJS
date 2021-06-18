using Jint;
using Jint.Native;
using Jint.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DalamudJSLib
{
    public class DalamudJSEngine
    {
        public static ParserOptions EsprimaOptions = new ParserOptions
        {
            Tolerant = true
        };

        private readonly Engine _engine;
        private readonly string _jsPath;

        private Jint.Parser.Ast.Program tsScript;

        private string jsCode { get; set; }

        public DalamudJSEngine(string path)
        {
            _jsPath = path;

            _engine = new Engine()
                .SetValue("require", new Func<string, JsValue>(Require));
        }
        public async Task Load()
        {
            string js = await File.ReadAllTextAsync(_jsPath);
            jsCode = js;
        }

        public void ExecuteCode()
        {
            var dalamudInterface = new DalamudJSInterface()
            {
                Dalamud = new DalamudFunctions
                {
                    CallFunction = CallFunction
                }
            };

            _engine.Execute("var exports = {};", EsprimaOptions);
            _engine.Execute(jsCode);

            _engine.SetValue("dalamud_1", dalamudInterface);
            _engine.SetValue("dalamud", dalamudInterface.Dalamud);

            _engine.Invoke("intializeDalamud");
        }

        public void CallFunction(object value)
        {
            Console.WriteLine(value);
        }

        private JsValue Require(string filename)
        {
            var asText = filename.EndsWith("!text");
            if (asText)
            {
                filename = filename.Remove(filename.Length - "!text".Length);
            }
            var tsfilename = filename.Contains(".") ? filename : $"{filename}.ts";

            if (Misc.TryGetFile(tsfilename, out string code))
            {
                if (tsfilename.EndsWith(".ts") && !asText)
                {
                    _engine.Execute("var exports = {};", EsprimaOptions);
                    _engine.Execute(code, EsprimaOptions);
                    var result = _engine.GetValue("exports");
                    return result;
                }
                else
                {
                    var obj = new Dictionary<string, object>
                    {
                        ["default"] = code
                    };
                    return JsValue.FromObject(_engine, obj);
                }
            }

            var indexFileName = $"{filename}/index.ts";
            if (Misc.TryGetFile(indexFileName, out var indexCode))
            {
                if (tsfilename.EndsWith(".ts") && !asText)
                {
                    _engine.Execute("var exports = {};", EsprimaOptions);
                    _engine.Execute(indexCode, EsprimaOptions);
                    var result = _engine.GetValue("exports");
                    return result;
                }
                else
                {
                    var obj = new Dictionary<string, object>
                    {
                        ["default"] = indexCode
                    };
                    return JsValue.FromObject(_engine, obj);
                }
            }

            var jsTopFileName = $"{filename}.js";
            if (Misc.TryGetFile(jsTopFileName, out var jsTopIndexCode))
            {
                if (tsfilename.EndsWith(".js") && !asText)
                {
                    _engine.Execute("var exports = {};", EsprimaOptions);
                    _engine.Execute(jsTopIndexCode, EsprimaOptions);
                    var result = _engine.GetValue("exports");
                    return result;
                }
                else
                {
                    var obj = new Dictionary<string, object>
                    {
                        ["default"] = jsTopIndexCode
                    };
                    return JsValue.FromObject(_engine, obj);
                }
            }

            var tsTopFileName = $"{filename}.ts";
            if (Misc.TryGetFile(tsTopFileName, out var tsTopIndexCode))
            {
                if (tsfilename.EndsWith(".ts") && !asText)
                {
                    _engine.Execute("var exports = {};", EsprimaOptions);
                    _engine.Execute(tsTopIndexCode, EsprimaOptions);
                    var result = _engine.GetValue("exports");
                    return result;
                }
                else
                {
                    var obj = new Dictionary<string, object>
                    {
                        ["default"] = tsTopIndexCode
                    };
                    return JsValue.FromObject(_engine, obj);
                }
            }

            var jsfilename = filename.Contains(".") ? filename : $"{filename}.js";

            if (Misc.TryGetFile(jsfilename, out string jscode))
            {
                if (tsfilename.EndsWith(".js") && !asText)
                {
                    _engine.Execute("var exports = {};", EsprimaOptions);
                    _engine.Execute(code, EsprimaOptions);
                    var result = _engine.GetValue("exports");
                    return result;
                }
                else
                {
                    var obj = new Dictionary<string, object>
                    {
                        ["default"] = code
                    };
                    return JsValue.FromObject(_engine, obj);
                }
            }
            var indexJsFileName = $"{filename}/index.js";
            if (Misc.TryGetFile(indexJsFileName, out var indexJsCode))
            {
                if (tsfilename.EndsWith(".js") && !asText)
                {
                    _engine.Execute("var exports = {};", EsprimaOptions);
                    _engine.Execute(indexJsCode, EsprimaOptions);
                    var result = _engine.GetValue("exports");
                    return result;
                }
                else
                {
                    var obj = new Dictionary<string, object>
                    {
                        ["default"] = indexCode
                    };
                    return JsValue.FromObject(_engine, obj);
                }
            }
            return null;
        }

        private string CompileScriptInternal(string sourceCode)
        {
            if (tsScript == null)
            {
                var tsLib = File.ReadAllText("typescript.js");
                var parser = new JavaScriptParser(true);
                tsScript = parser.Parse(tsLib);
            }
            var _engine = new Jint.Engine();
            _engine.Execute(tsScript);
            _engine.SetValue("src", sourceCode);
            var transpileOptions = "{\"compilerOptions\": {\"target\":\"ES5\"}}";
            var output = _engine.Execute($"ts.transpileModule(src, {transpileOptions})", EsprimaOptions).GetCompletionValue().AsObject();
            return output.Get("outputText").AsString();
        }
    }
}
