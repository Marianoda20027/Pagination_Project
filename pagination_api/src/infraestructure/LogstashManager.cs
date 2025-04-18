using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PaginationApp.Infraestucture
{
    public class LogstashManager
    {
        public async Task RunLogstashAsync()
        {
            var logstashProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "C:\\logstash-9.0.0\\bin\\logstash.bat", 
                    Arguments = "-f C:\\logstash-9.0.0\\config\\config.conf", 
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            if (!logstashProcess.Start())
                throw new InvalidOperationException("Failed to start Logstash process");

            string output = await logstashProcess.StandardOutput.ReadToEndAsync();
            string error = await logstashProcess.StandardError.ReadToEndAsync();

            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException($"Logstash error: {error}");

            if (!string.IsNullOrEmpty(output))
                Console.WriteLine("Logstash Output: " + output);

            // Espera a que el proceso termine
            logstashProcess.WaitForExit();
            
            if (logstashProcess.ExitCode != 0)
                throw new InvalidOperationException($"Logstash process exited with code: {logstashProcess.ExitCode}");
        }
    }
}