using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace ShortCutMapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.reload();
        }

        private Data readFromDb(string path)
        {
            IDatabase databse = new DB_Xml(path);
            return databse.read(path);
        }

        void HandleRightClick(Data d, Object sender, MouseButtonEventArgs e)
        {

            if (e.RightButton == MouseButtonState.Pressed)
            {
                IDatabase db = new DB_Xml("");
                d.Path = "";
                db.save(d);
                this.reload();
            }           
        } 

        Button createButton(Data d)
        {
            Button b = new Button();
            b.ToolTip = d.Path;
            
            b.Height = 80;
            b.Width = 40;
            

            int pos = d.Path.LastIndexOf("\\");
            var title = pos != -1 ? d.Path.Substring(pos + 1) : d.Path;
            b.Content = new TextBlock()
            {
                FontSize = 10,
                Text = title,
                TextAlignment = TextAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                TextWrapping = TextWrapping.Wrap               
            };


            
            MouseButtonEventHandler handler = (s, e) => HandleRightClick(d, s, e);
            b.MouseDown += handler;

            RoutedEventHandler onClick= (s, e) => HandleLeftClick(d, s, e);
            b.Click += onClick;



            this.wrapPanel.Children.Add((UIElement)b);
            return b;             
        }

        private void reload()
        {
            this.wrapPanel.Children.Clear();
            if (!Directory.Exists(MainWindow.folder))
            {
                MainWindow.folder = Directory.GetCurrentDirectory() + "\\db";
                Directory.CreateDirectory(MainWindow.folder);
            }
            foreach (FileInfo fileInfo in ((IEnumerable<FileInfo>)new DirectoryInfo(MainWindow.folder).GetFiles("*.*", SearchOption.AllDirectories)).OrderBy<FileInfo, DateTime>((Func<FileInfo, DateTime>)(t => t.CreationTime)).ToList<FileInfo>())
            {
                FileInfo item = fileInfo;
                string str = File.ReadAllText(item.FullName);
                var data = readFromDb(item.FullName);
                if (data == null || data.Path.Length == 0)
                {
                    File.Delete(item.FullName);
                }
                else
                {
                    Button b = createButton(data);
                }
            }
        }
        const int WM_DROPFILES = 0x233;
        private static string folder;

        [DllImport("shell32.dll")]
        static extern void DragAcceptFiles(IntPtr hwnd, bool fAccept);

        [DllImport("shell32.dll")]
        static extern uint DragQueryFile(IntPtr hDrop, uint iFile, [Out] StringBuilder filename, uint cch);

        [DllImport("shell32.dll")]
        static extern void DragFinish(IntPtr hDrop);


        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var helper = new WindowInteropHelper(this);
            var hwnd = helper.Handle;

            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);

            DragAcceptFiles(hwnd, true);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_DROPFILES)
            {
                handled = true;
                return HandleDropFiles(wParam);
            }

            return IntPtr.Zero;
        }

        private IntPtr HandleDropFiles(IntPtr hDrop)
        {        

            const int MAX_PATH = 260;

            var count = DragQueryFile(hDrop, 0xFFFFFFFF, null, 0);

            for (uint i = 0; i < count; i++)
            {
                int size = (int)DragQueryFile(hDrop, i, null, 0);

                var filename = new StringBuilder(size + 1);
                DragQueryFile(hDrop, i, filename, MAX_PATH);


                HandleFileOpen(filename.ToString());
            }

            DragFinish(hDrop);

            return IntPtr.Zero;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void HandleFileOpen(string path)
        {
            Data d = new Data();
            d.Path = path;
            d.UniqueId = MainWindow.folder + "\\" + Data.GenerateUniqueID();
            IDatabase databse = new DB_Xml("");
            databse.save(d);

            createButton(d);
        }

        private void HandleLeftClick(Data d, object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", d.Path);            
        }
    }
}
