using nginxconfigurator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nginxconfigurator.Controllers
{
    public class Helper
    {
        public static async Task WriteFile(string file, string text)
        {
            await File.WriteAllTextAsync(file, text);
        }

        public static string CreateServerConfigPart(NginxConfigModel nginxConfigModel)
        {
            string text = @"
                server {
                    server_name: " + nginxConfigModel.server_name + @";
                    location / {
                        proxy_set_header Host $host;
                        proxy_pass "+ nginxConfigModel.ip+@";
                        proxy_redirect off;
                    }
                }"; 
            return text;
        }

        public static void RestartNginx()
        {
            Console.WriteLine(ShellHelper.Bash("service nginx restart"));
        }

    }

    public class ShellHelper { 
        public static string Bash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c\"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }
    }
}
