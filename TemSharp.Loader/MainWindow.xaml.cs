using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TemSharp.Loader.Core;

namespace TemSharp.Loader
{
    public class Status
    {
        private static String _text;
        public static String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnTextChanged(EventArgs.Empty);
            }
        }
        public static event EventHandler TextChanged;
        protected static void OnTextChanged(EventArgs e)
        {
            TextChanged?.Invoke(null, e);
        }
        static Status()
        {
            TextChanged += (sender, e) => { return; };
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Task.Run(() => { Init(); });
        }
        public void Init()
        {
            var gameProcName = "Temtem";
            var waitingFor = "Waiting for " + gameProcName + " to open";
            Status.Text = waitingFor;
            var gameProc = System.Diagnostics.Process.GetProcessesByName(gameProcName);
            if (gameProc.Length == 0)
                Console.WriteLine(waitingFor);
            while (gameProc.Length == 0)
            {
                Console.WriteLine(waitingFor);
                Thread.Sleep(5000);
                gameProc = System.Diagnostics.Process.GetProcessesByName(gameProcName);
            }
            Status.Text = "Game Open - Injecting";
            try
            {
                String randString = "aa" + Guid.NewGuid().ToString().Substring(0, 8);
                var q = gameProc[0].MainModule;
                var gameDir = System.IO.Path.GetDirectoryName(gameProc[0].MainModule.FileName);
                var gameName = System.IO.Path.GetFileName(gameDir);
                var unityDllPath = gameDir + @"\" + gameName + @"_Data\Managed\";
                Compiler.UnityDllPath = unityDllPath;
                Status.Text = "Injecting - Game @ " + unityDllPath;
                Compiler.UpdateSources();
                Injector.Inject(gameProcName, Compiler.CompileDll(randString), randString, "Init", "Load");
                Status.Text = "Injected, closing app shortly";
                //Thread.Sleep(10000);
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Status.Text = e.Message;
            }
        }
    }
}
