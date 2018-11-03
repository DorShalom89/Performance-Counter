using System.Threading;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace WpfApp4
{
    //Class that uses the performance counter and run the tests
    class ConnectionTester
    {
        //Cpu usage test
        public static float TestCPUPerformance()
        {
            PerformanceCounter pCPU = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            float cpuUsage;
            //2 tests with sleep between so the performance counter will show real value and not 0
            cpuUsage = pCPU.NextValue();
            Thread.Sleep(30);
            cpuUsage = pCPU.NextValue();
            return cpuUsage;
        }
        //Ram usage test
        public static float TestRamPerformance()
        {
            PerformanceCounter pRAM = new PerformanceCounter("Memory", "% Committed Bytes In Use", true);
            return pRAM.NextValue();
        }
        //Processes test
        public static int CheckNumberofProcesses()
        {
            int processes = Process.GetProcesses().Length;
            return processes;
        }
        //Url reach test
        public static bool TestUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 15000;
            request.Method = "HEAD";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }
        //Path test
        public static bool TestDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
