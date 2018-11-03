using System;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Threading;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        // Boolean value for test threads control
        private bool running = false;

        //Buttons actions
        // Start / Stop Button functions
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!running)
            {
                btnStart.Dispatcher.Invoke(() => btnStart.Content = "Stop");
                //Sets running to true to keep threads while loop alive
                running = true;
                //New thread for not blocking the UI thread
                Thread newThread;
                //Determins which test to run 
                if (!props.advanced) newThread = new Thread(RunBasicTests);
                else newThread = new Thread(RunAdvancedTests);
                newThread.IsBackground = true;
                newThread.Start();
            }
            else
            {   
                //Sets running to false to stop threads while loop
                running = false;
                //Reset window values
                btnStart.Dispatcher.Invoke(() => btnStart.Content = "Start Tests");
                lblRunning.Dispatcher.Invoke(new Action(() => lblRunning.Text = "Done"));
                lblCpuUsage.Dispatcher.Invoke(new Action(() => lblCpuUsage.Text = ""));
                lblRamUsage.Dispatcher.Invoke(new Action(() => lblRamUsage.Text = ""));
                lblProcesses.Dispatcher.Invoke(new Action(() => lblProcesses.Text = ""));
                lblURL.Dispatcher.Invoke(new Action(() => lblURL.Text = ""));
                lblDir.Dispatcher.Invoke(new Action(() => lblDir.Text = ""));

            }

        }
        //Advanced button shows AdvancedWindow
        private void btnStart_Copy_Click(object sender, RoutedEventArgs e)
        {
            AdvancedWindow advancedWindow = new AdvancedWindow();
            advancedWindow.ShowDialog();
        }

        //Properties class
        //Singleton class - holds the test properties
        private static Props props = Props.PropsInstance();

        //Update props after import file
        public static void OverrideProps()
        {
            props = Props.PropsInstance();
        }

        //Data Binding
        //Value for binding basic url text box
        public String UrlCheck
        {
            get { return props.basicURL; }
            set
            {
                props.basicURL = value;
                OnPropertyChanged("UrlCheck");
            }
        }
        //Value for binding basic path text box
        public String PathCheck
        {
            get { return props.basicPath; }
            set
            {
                props.basicPath = value;
                OnPropertyChanged("PathCheck");
            }
        }
        //Value for binding basic interval text box
        public int Interval
        {
            get { return props.basicInterval; }
            set
            {
                props.basicInterval = value;
                OnPropertyChanged("Interval");
            }
        }

        //Tests
        //Basic test parallel
        private void RunBasicTests()
        {
            while (running)
            {
                lblRunning.Dispatcher.Invoke(new Action(() => lblRunning.Text = "Running Tests"));
                Parallel.Invoke(
                    //Cpu usage test - invoke the action for updating result to the UI
                    () =>
                    {
                        lblCpuUsage.Dispatcher.Invoke(new Action(() => lblCpuUsage.Text = ConnectionTester.TestCPUPerformance().ToString() + "%"));
                    },
                    //Ram usage test - invoke the action for updating result to the UI
                    () =>
                    {
                        lblRamUsage.Dispatcher.Invoke(new Action(() => lblRamUsage.Text = ConnectionTester.TestRamPerformance().ToString()+"%"));
                    },
                    //Processes running test - invoke the action for updating result to the UI
                    () =>
                    {
                        lblProcesses.Dispatcher.Invoke(new Action(() => lblProcesses.Text = ConnectionTester.CheckNumberofProcesses().ToString()));
                    },
                    //Url reach test - invoke the action for updating result to the UI
                    () =>
                    {
                        if (ConnectionTester.TestUrl(UrlCheck)) lblURL.Dispatcher.Invoke(new Action(() => lblURL.Text = "Reachable"));
                        else lblURL.Dispatcher.Invoke(new Action(() => lblURL.Text = "Unreachable"));
                    },
                    // Path exists test - invoke the action for updating result to the UI
                    () =>
                    {
                        if (ConnectionTester.TestDirectory(PathCheck)) lblDir.Dispatcher.Invoke(new Action(() => lblDir.Text = "Exists"));
                        else lblDir.Dispatcher.Invoke(new Action(() => lblDir.Text = "Does not exsist"));
                    }
                );
                lblRunning.Dispatcher.Invoke(new Action(() => lblRunning.Text = "Done"));
                //Sleep until next test interval
                Thread.Sleep(Interval * 1000);

            }

        }
        //Advanced test seperated to tasks
        private void RunAdvancedTests()
        {
            lblRunning.Dispatcher.Invoke(new Action(() => lblRunning.Text = "Advanced Tests"));
            //Cpu usage test - invoke the action for updating result to the UI and sleep accordnig to user input
            Task.Run(() =>
                {
                    while(running)
                    {
                        lblCpuUsage.Dispatcher.Invoke(new Action(() => lblCpuUsage.Text = ConnectionTester.TestCPUPerformance().ToString() + "%"));
                        Thread.Sleep(props.cpuTestInterval * 1000);
                    }
                });
            //Ram usage test - invoke the action for updating result to the UI and sleep accordnig to user input
            Task.Run(() =>
                {
                    while (running)
                    {
                        lblRamUsage.Dispatcher.Invoke(new Action(() => lblRamUsage.Text = ConnectionTester.TestRamPerformance().ToString() + "%"));
                        Thread.Sleep(props.ramTestInterval * 1000);
                    }
                });
            //Processes running test - invoke the action for updating result to the UI and sleep accordnig to user input
            Task.Run(() =>
                {
                    while (running)
                    {
                        lblProcesses.Dispatcher.Invoke(new Action(() => lblProcesses.Text = ConnectionTester.CheckNumberofProcesses().ToString()));
                        Thread.Sleep(props.processesTestInterval * 1000);
                    }
                });
            //Url reach test - invoke the action for updating result to the UI and sleep accordnig to user input
            Task.Run(() =>
                {
                    while (running)
                    {
                        if (ConnectionTester.TestUrl(UrlCheck)) lblURL.Dispatcher.Invoke(new Action(() => lblURL.Text = "Reachable"));
                        else lblURL.Dispatcher.Invoke(new Action(() => lblURL.Text = "Unreachable"));
                        Thread.Sleep(props.urlTestInterval * 1000);
                    }   
                });
            //Path exists test - invoke the action for updating result to the UI and sleep accordnig to user input
            Task.Run(() =>
                {
                    while (running)
                    {
                        if (ConnectionTester.TestDirectory(PathCheck)) lblDir.Dispatcher.Invoke(new Action(() => lblDir.Text = "Exists"));
                        else lblDir.Dispatcher.Invoke(new Action(() => lblDir.Text = "Does not exsist"));
                        Thread.Sleep(props.pathTestInterval * 1000);
                    }
                });
        }

        //Implementation of INotifyPropertyChanged interface for live update values
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }

}
