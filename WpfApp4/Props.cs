using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4
{
    //Class that holds the basic and advanced tests properties values
    public class Props
    {
        private static Props instance = null;
        public Props()
        {

        }
        public static Props PropsInstance()
        {
            if (instance == null)
            {
                instance = new Props();
            }
            return instance;
        }

        public static void ImportProps(Props props)
        {
            instance = props;
        }

        // Properties for basic test
        public String basicURL = "http://google.com";
        public String basicPath = "C:/Program Files";
        public int basicInterval = 1;
        public bool advanced = false;


        // Properties for advanced test
        //Cpu test properties:
        public int cpuTestInterval = 1;
        public bool cpuTestRun = true;

        //Ram test properties:
        public int ramTestInterval = 1;
        public bool ramTestRun = true;

        //Processes test properties:
        public int processesTestInterval = 1;
        public bool processesTestRun = true;

        //Url test properties:
        public String urlTestValue = "Http://google.com";
        public int urlTestInterval = 1;
        public bool urlTestRun = true;

        //Path test properties:
        public String pathTestValue = "C:/Program Files";
        public int pathTestInterval = 1;
        public bool pathTestRun = true;

    }


}
