using System;
using System.Windows;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for AdvancedWindow.xaml
    /// </summary>
    public partial class AdvancedWindow : INotifyPropertyChanged
    {
        public AdvancedWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        //Buttons
        //Close button function
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //Export button using Newtonsoft.Json for serializing props object to json
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            String path = fbd.SelectedPath;
            StreamWriter sw = new StreamWriter(path + "/TestProps.txt");
            String jsonProperties = JsonConvert.SerializeObject(props);
            sw.Write(jsonProperties);
            sw.Close();
            this.Close();

        }
        //Import button using Newtonsoft.Json for deserializing json to props object
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.ShowDialog();
            String path = ofd.FileName;
            StreamReader sr = new StreamReader(path);
            String jsonProperties = sr.ReadToEnd();
            sr.Close();
            Props newProps = JsonConvert.DeserializeObject<Props>(jsonProperties);
            //Importing the new props to the singleton class
            Props.ImportProps(newProps);
            //Overriding props object in main window
            MainWindow.OverrideProps();
            this.Close();
        }


        Props props = Props.PropsInstance();
        //Value for binding advanced tests checkbox
        public bool Advanced
        {
            get { return props.advanced; }
            set
            {
                props.advanced = value;
                OnPropertyChanged("Advanced");
            }
        }
        //Value for binding advanced cpu interval text box
        public int CpuTestInterval
        {
            get { return props.cpuTestInterval; }
            set
            {
                props.cpuTestInterval = value;
                OnPropertyChanged("CpuTestInterval");
            }
        }
        //Value for binding advanced cpu test checkbox
        public bool CpuTestRun
        {
            get { return props.cpuTestRun; }
            set
            {
                props.cpuTestRun = value;
                OnPropertyChanged("CpuTestRun");
            }
        }
        //Value for binding advanced ram interval text box
        public int RamTestInterval
        {
            get { return props.ramTestInterval; }
            set
            {
                props.ramTestInterval = value;
                OnPropertyChanged("RamTestInterval");
            }
        }
        //Value for binding advanced ram test checkbox
        public bool RamTestRun
        {
            get { return props.ramTestRun; }
            set
            {
                props.ramTestRun = value;
                OnPropertyChanged("RamTestRun");
            }
        }
        //Value for binding advanced processes interval text box
        public int ProcessesTestInterval
        {
            get { return props.processesTestInterval; }
            set
            {
                props.processesTestInterval = value;
                OnPropertyChanged("ProcessesTestInterval");
            }
        }
        //Value for binding advanced processes test checkbox
        public bool ProcessesTestRun
        {
            get { return props.processesTestRun; }
            set
            {
                props.processesTestRun = value;
                OnPropertyChanged("ProcessesTestRun");
            }
        }
        //Value for binding advanced url value text box
        public String UrlTestValue
        {
            get { return props.urlTestValue; }
            set
            {
                props.urlTestValue = value;
                OnPropertyChanged("UrlTestValue");
            }
        }
        //Value for binding advanced url interval text box
        public int UrlTestInterval
        {
            get { return props.urlTestInterval; }
            set
            {
                props.urlTestInterval = value;
                OnPropertyChanged("UrlTestInterval");
            }
        }
        //Value for binding advanced url test checkbox
        public bool UrlTestRun
        {
            get { return props.urlTestRun; }
            set
            {
                props.urlTestRun = value;
                OnPropertyChanged("UrlTestRun");
            }
        }
        //Value for binding advanced path value text box
        public String PathTestValue
        {
            get { return props.pathTestValue; }
            set
            {
                props.pathTestValue = value;
                OnPropertyChanged("PathTestValue");
            }
        }
        //Value for binding advanced path interval text box
        public int PathTestInterval
        {
            get { return props.pathTestInterval; }
            set
            {
                props.pathTestInterval = value;
                OnPropertyChanged("PathTestInterval");
            }
        }
        //Value for binding advanced path test checkbox
        public bool PathTestRun
        {
            get { return props.pathTestRun; }
            set
            {
                props.pathTestRun = value;
                OnPropertyChanged("PathTestRun");
            }
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
