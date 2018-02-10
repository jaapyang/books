using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ToolPlat
{
    public static class ToolMapping
    {
        private static readonly Dictionary<string, ToolInfo> Dic;
        static ToolMapping()
        {
            Dic = new Dictionary<string, ToolInfo>();
        }

        public static void Init()
        {
            
            var baseDirPath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDirPath, "Views");
            var files = Directory.GetFiles(path);
            var pattern = @"\w+.html";

            var handlerBaseNameSpace = typeof(HandlerBase).Namespace;

            foreach (var file in files)
            {
                var match = Regex.Match(file, pattern);

                if (!match.Success) continue;

                var fileName = match.Value;
                var toolName = fileName.Substring(0, fileName.Length - 5);

                var toolInfo = new ToolInfo()
                {
                    HandlerName = $"{toolName}Handler",
                    ViewPath = file,
                    ToolName = toolName,
                    HandlerFullName = $"{handlerBaseNameSpace}.Handlers.{toolName}Handler"
                };

                Dic.Add(toolName, toolInfo);
            }
        }

        public static string[] GetToolNames()
        {
            return Dic.Select(x => x.Key).ToArray();
        }

        public static ToolInfo GetViewPath(string toolName)
        {
            return Dic[toolName];
        }
    }
}