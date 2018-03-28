using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Mail;
using System.Net;
using System.Xml;
using System.IO;
using OpenPop.Pop3;
using OpenPop.Common.Logging;
using OpenPop.Mime;
using OpenPop.Mime.Decode;
using OpenPop.Mime.Header;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;

namespace final_project2
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isPressed = false;
        bool hidden = false;
        Point startPosition;
        Point p;
        int canvas_width = 40000;
        int timeline_band = 30;
        XmlDocument XmlDoc = new XmlDocument();
        XmlDocument XmlDoc2 = new XmlDocument();
        int maxnum = 0;
        int maxnum2 = 0;
        int selected = 0;
        Button dele_item;
        //var dele_item;
        System.Windows.Forms.NotifyIcon notify;
        public MainWindow()
        {
            InitializeComponent();
            
            timeline_construct();
            load_timeline();
            load_todo();
            set_default_time();
        }
        void timeline_construct()
        {
            front_canvas.Width = canvas_width;
            //Timeline show
            string hr_str = DateTime.Now.ToString("HH");
            string min_str = DateTime.Now.ToString("mm");
            int hr = int.Parse(hr_str);
            int min = int.Parse(min_str);
            Rectangle today = new Rectangle();
            today.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            today.Width = timeline_band * 24;
            today.Fill = new SolidColorBrush(Colors.White);
            today.Opacity = 0.2;
            Canvas.SetLeft(today,timeline_band *5 +10);
            front_canvas.Children.Add(today);
            for (int i = 0; i < canvas_width / timeline_band; i++)
            {
                Line timeline = new Line();
                timeline.X1 = timeline_band * i + 10 - min / 2;
                timeline.Y1 = System.Windows.SystemParameters.PrimaryScreenHeight - 150;
                timeline.Y1 = 0;
                timeline.X2 = timeline_band * i + 10 - min / 2;
                timeline.Y2 = System.Windows.SystemParameters.PrimaryScreenHeight - 50;
                timeline.StrokeThickness = 0.1;
                if ((i+hr) % 24 == 5)
                {
                    timeline.Y1 = 0;
                    TextBlock date = new TextBlock();
                    string date_str = DateTime.Now.AddDays((i + hr) / 24).ToString("M/d");
                    date_str = date_str + " 00:00";
                    date.Text = date_str;
                    date.FontSize = 20;
                    date.Foreground = new SolidColorBrush(Colors.White);
                    Canvas.SetLeft(date, timeline_band * (i) + 10 - min / 2);
                    timeline.StrokeThickness = 1;
                    front_canvas.Children.Add(date);
                }
                else if ((i + hr) % 24 == 17)
                {
                    TextBlock date = new TextBlock();
                    //string date_str = DateTime.Now.AddDays((i + hr) / 24).ToString("M/d");
                    string date_str = ((i + hr + 19) % 24).ToString();
                    date.Text = date_str;
                    date.FontSize = 16;
                    //date.FontSize = 20;
                    date.Foreground = new SolidColorBrush(Colors.Gray);
                    Canvas.SetLeft(date, timeline_band * (i) + 10 - min / 2);
                    timeline.StrokeThickness = 0.6;
                    front_canvas.Children.Add(date);
                }
                else
                {
                    TextBlock date = new TextBlock();
                    //string date_str = DateTime.Now.AddDays((i + hr) / 24).ToString("M/d");
                    string date_str = ((i + hr+19) % 24).ToString();
                    date.Text = date_str;
                    date.FontSize = 12;
                    //date.FontSize = 20;
                    date.Foreground = new SolidColorBrush(Colors.Gray);
                    Canvas.SetLeft(date, timeline_band * (i) + 10 - min / 2);
                    timeline.StrokeThickness = 0.1;
                    front_canvas.Children.Add(date);
                }
               

                SolidColorBrush whiteBrush = new SolidColorBrush();
                whiteBrush.Color = Colors.White;
                
                timeline.Stroke = whiteBrush;
                front_canvas.Children.Add(timeline);

            }
            Line now_time = new Line();
            now_time.X1 = timeline_band * 5 + 10;
            now_time.X2 = timeline_band * 5 + 10;
            now_time.Y2 = System.Windows.SystemParameters.PrimaryScreenHeight - 50;
            now_time.Y1 = 0;
            SolidColorBrush whiteBrush2 = new SolidColorBrush();
            whiteBrush2.Color = Colors.White;
            now_time.StrokeThickness = 2;
            now_time.Stroke = whiteBrush2;
            front_canvas.Children.Add(now_time);

            TextBlock date_now = new TextBlock();
            string date_str2 = DateTime.Now.ToString("M/d H:mm");
            date_now.Text = "Today";
            date_now.FontSize = 30;
            date_now.Foreground = new SolidColorBrush(Colors.White);
            Canvas.SetLeft(date_now, timeline_band * 5 + 10);
            Canvas.SetTop(date_now, 60);
            front_canvas.Children.Add(date_now);
        }
        
        void set_default_time()
        {
            //set calender default time
            //start_timepicker.Format = "Custom";
            //start_timepicker.Format = "Custom";
            start_timepicker.FormatString = "M/d H:mm";
            end_timepicker.FormatString = "M/d H:mm";
            start_timepicker2.FormatString = "M/d H:mm";
            end_timepicker2.FormatString = "M/d H:mm";

            start_timepicker.DefaultValue = DateTime.Now;
            end_timepicker.DefaultValue = DateTime.Now;
            start_timepicker.DisplayDefaultValueOnEmptyText = true;
            end_timepicker.DisplayDefaultValueOnEmptyText = true;
            start_timepicker2.DefaultValue = DateTime.Now;
            end_timepicker2.DefaultValue = DateTime.Now;
            start_timepicker2.DisplayDefaultValueOnEmptyText = true;
            end_timepicker2.DisplayDefaultValueOnEmptyText = true;
        }
         
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            // Configure and show a notification icon in the system tray
            this.notify = new System.Windows.Forms.NotifyIcon();
            this.notify.BalloonTipText = "Hello, NotifyIcon!";
            this.notify.Text = "Hello!";
            this.notify.Icon = new System.Drawing.Icon("123.ico");
            this.notify.Click += new EventHandler(Notify_Click);
            this.notify.ShowBalloonTip(1000);

            System.Windows.Forms.ContextMenu notifyIconMenu = new System.Windows.Forms.ContextMenu();

            System.Windows.Forms.MenuItem notifyIconMenuItem3 = new System.Windows.Forms.MenuItem();

            notifyIconMenuItem3.Index = 0;
            notifyIconMenuItem3.Text = "Exit";
            notifyIconMenuItem3.Click += new EventHandler(Exit_Click);
            notifyIconMenu.MenuItems.Add(notifyIconMenuItem3);
            
            this.notify.ContextMenu = notifyIconMenu;


            this.notify.Visible = true;

        }
        void Notify_Click(object sender, EventArgs e)
        {
            if (hidden)
            {
                this.Show();
                hidden = false;
            }
            else
            {
                this.Hide();
                hidden = true;
            }

        }

        void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void load_timeline()
        {
            ///////xml file load///////
            XmlDoc.Load("xml_test2.xml");
            XmlNodeList elemList = XmlDoc.SelectNodes("Root/Activity");

            for (int i = 0; i < elemList.Count; i++)
            {
                /*string iString = "2005-05-05 22:12";
                DateTime oDate = DateTime.ParseExact(iString, "yyyy-MM-dd HH:mm", null);
                */
                //MessageBox.Show(oDate.ToString("M/d H:mm"));
                string date_str2 = elemList[i].Attributes["start_time"].Value;
                DateTime date_rec = DateTime.ParseExact(date_str2, "M/d H:mm", null);
                TimeSpan sub = date_rec - DateTime.Now;
                double min_sub = sub.TotalMinutes;
                //if (hr_sub < 0) hr_sub++;
                //System.Windows.MessageBox.Show(hr_sub.ToString());

                elemList[i].Attributes["start_pos_x"].Value = ((int)(min_sub/2 + timeline_band * 5 + 10)).ToString();
                String len = elemList[i].Attributes["length"].Value;
                //elemList[i].
                int start_pos_x = int.Parse(elemList[i].Attributes["start_pos_x"].Value);
                int start_pos_y = int.Parse(elemList[i].Attributes["start_pos_y"].Value);
                string co = elemList[i].Attributes["color"].Value;



                String activity_name = elemList[i].InnerText;
                float timeline_len = float.Parse(len);
                Button g1 = new Button();
                g1.Height = 50;
                g1.Width = timeline_len;
                g1.HorizontalContentAlignment = HorizontalAlignment.Left;
                Canvas.SetLeft(g1, start_pos_x);
                Canvas.SetTop(g1, start_pos_y);
                //g1.Margin = new Thickness(p.X, p.Y, 0, 0);
                g1.Content = activity_name;
                g1.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(canvas_MouseLeftButtonUp);
                g1.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(canvas_MouseLeftButtonDown);
                g1.PreviewMouseMove += new MouseEventHandler(canvas_MouseMove);
                g1.PreviewMouseDoubleClick += new MouseButtonEventHandler(canvas_MouseDoubleClick);
                g1.PreviewMouseRightButtonUp += new MouseButtonEventHandler(activity_click);
                //g1.PreviewMouseRightButtonDown += new MouseButtonEventHandler(activity_click);
                var bc = new BrushConverter();

                if (co == "Red")
                    g1.Background = (Brush)bc.ConvertFrom("#FFFF6464");
                else if (co == "Blue")
                    g1.Background = (Brush)bc.ConvertFrom("#FF6464FF");
                else if (co == "Green")
                    g1.Background = (Brush)bc.ConvertFrom("#FF64FF64");
                else if (co == "Yellow")
                    g1.Background = (Brush)bc.ConvertFrom("#FFFFFF64");
                else
                    g1.Background = (Brush)bc.ConvertFrom("#FFFF64FF");
                g1.Uid = elemList[i].Attributes["id"].Value;
                int num = int.Parse(elemList[i].Attributes["id"].Value);
                if (num >= maxnum)
                    maxnum = num;
                maxnum++;
                front_canvas.Children.Add(g1);
                
            }
            XmlDoc.Save("xml_test2.xml");
            /*
            string iString = "2005-05-05 22:12";
            DateTime oDate = DateTime.ParseExact(iString, "yyyy-MM-dd HH:mm", null);
            MessageBox.Show(oDate.ToString("M/d H:mm"));
            */
        }
        void load_todo()
        {
            ///////xml file load///////
            XmlDoc2.Load("xml_todo.xml");
            XmlNodeList elemList = XmlDoc2.SelectNodes("Root/Activity");

            for (int i = 0; i < elemList.Count; i++)
            {
                String activity_name = elemList[i].InnerText;
                
                Button g1 = new Button();
                g1.Height = 50;
                g1.Width = 50;
                Canvas.SetLeft(g1, int.Parse(elemList[i].Attributes["id"].Value)*50);
                g1.Content = elemList[i].InnerText;
                var bc = new BrushConverter();
                g1.Background = (Brush)bc.ConvertFrom("#FFFF6464");
                g1.PreviewMouseDoubleClick += new MouseButtonEventHandler(canvas_MouseDoubleClick2);
                g1.Uid = elemList[i].Attributes["id"].Value;
                int num = int.Parse(elemList[i].Attributes["id"].Value);
                if (num >= maxnum2)
                    maxnum2 = num;
                maxnum2++;
                front_canvas2.Children.Add(g1);

            }
            XmlDoc2.Save("xml_todo.xml");
            
        }


        public string activity_name { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = e.Source as Button;
            b.Foreground = Brushes.Red;
        }

        private void mnuNew_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Shapes.Rectangle rect;
            // black_background.Visibility = Visibility.Visible;
            InputBox.Visibility = Visibility.Visible;
            
        }
        private void AddTodoList_Click(object sender, RoutedEventArgs e)
        {
            Todo_box.Visibility = Visibility.Visible;

        }
        private void grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {//MessageBox.Show(e.Source.GetType().Name);
            p = Mouse.GetPosition(front_canvas);

            int pos_to_min = (int)(p.X - timeline_band * 5 - 10) * 2;
            string date_str = DateTime.Now.AddMinutes(pos_to_min).ToString("M/d HH:mm");
            start_timepicker2.Value = DateTime.Now.AddMinutes(pos_to_min);
            end_timepicker2.Value = DateTime.Now.AddMinutes(pos_to_min + 120);
            start_timepicker2.DisplayDefaultValueOnEmptyText = true;
            end_timepicker2.DisplayDefaultValueOnEmptyText = true;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Shapes.Rectangle rect;
            // black_background.Visibility = Visibility.Visible;
            InputBox.Visibility = Visibility.Collapsed;
            EditBox.Visibility = Visibility.Collapsed;
        }
        //to do list cancel control
        

        ///////////////////
        private void create_Click(object sender, RoutedEventArgs e)
        {
            //int timeline_len = int.Parse(InputTextBox2.Text);
            Button g1 = new Button();
            g1.Height = 50;

            //time picker
            int pos_to_min = (int)(p.X - timeline_band * 5 - 10) *2;
            string date_str = DateTime.Now.AddMinutes(pos_to_min).ToString("M/d HH:mm");
           
            
            DateTime t1 = end_timepicker2.Value.Value;
            TimeSpan span = t1 - DateTime.Now.AddMinutes(pos_to_min);
            int timeline_len2 = (int)(span.TotalMinutes / 2);
            //System.Windows.MessageBox.Show(timeline_len2.ToString());


            g1.HorizontalContentAlignment = HorizontalAlignment.Left;
            g1.Width = timeline_len2;
            Canvas.SetLeft(g1, p.X);
            Canvas.SetTop(g1, p.Y);
            //g1.Margin = new Thickness(p.X, p.Y, 0, 0);
            g1.Content = InputTextBox.Text;
            g1.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(canvas_MouseLeftButtonUp);
            g1.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(canvas_MouseLeftButtonDown);
            g1.PreviewMouseMove += new MouseEventHandler(canvas_MouseMove);
            g1.PreviewMouseDoubleClick += new MouseButtonEventHandler(canvas_MouseDoubleClick);
            g1.PreviewMouseRightButtonUp += new MouseButtonEventHandler(activity_click);
            //g1.PreviewMouseRightButtonDown += new MouseButtonEventHandler(activity_click);

           //map position to timeline

            

            //set end time
            
            

            g1.Uid = maxnum.ToString();
            maxnum++; 
            var bc = new BrushConverter();
            if (color.Text == "Red")
                g1.Background = (Brush)bc.ConvertFrom("#FFFF6464");
            else if (color.Text == "Blue")
                g1.Background = (Brush)bc.ConvertFrom("#FF6464FF");
            else if (color.Text == "Green")
                g1.Background = (Brush)bc.ConvertFrom("#FF64FF64");
            else if (color.Text == "Yellow")
                g1.Background = (Brush)bc.ConvertFrom("#FFFFFF64");
            else
                g1.Background = (Brush)bc.ConvertFrom("#FFFF64FF");
            front_canvas.Children.Add(g1);

            //////save date//////
            XmlDoc.Load("xml_test2.xml");
            XmlNode root = XmlDoc.SelectSingleNode("Root");
            XmlElement elem = XmlDoc.CreateElement("Activity");
            elem.SetAttribute("length", timeline_len2.ToString());
            elem.SetAttribute("start_pos_x", p.X.ToString());
            elem.SetAttribute("start_pos_y", p.Y.ToString());
            elem.SetAttribute("id",g1.Uid);
            elem.SetAttribute("filelink", PathTextBox.Text);
            elem.SetAttribute("weblink", WebTextBox.Text);
            elem.SetAttribute("color", color.Text);
            elem.InnerText = InputTextBox.Text;
            elem.SetAttribute("start_time", date_str);
            elem.SetAttribute("end_time", end_timepicker2.Value.Value.ToString("M/d HH:mm"));
            root.AppendChild(elem);
            XmlDoc.Save("xml_test2.xml");
            

           
            //g1.Name = "test";
            
            /*
            Rectangle rect = new Rectangle();
            //rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Blue);
            rect.Fill = new SolidColorBrush(Colors.White);
            rect.Opacity = 0.5;
            rect.Width = timeline_len;
            rect.Height = 50;

            //Canvas.SetLeft(rect, p.X);
            //Canvas.SetTop(rect, p.Y);
            g1.Children.Add(rect);

            //add label on the rectangle
            Label label = new Label();
            label.Content = InputTextBox.Text;  */
            /*label.RenderTransform = new TranslateTransform
            {
                X = p.X,
                Y = p.Y
            };*/

            //g1.Children.Add(label);
            //MessageBox.Show(InputTextBox.Text);
          

            InputBox.Visibility = Visibility.Collapsed;
           
        }
        
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            front_canvas.Children.Remove(delete);
            XmlDoc.Load("xml_test2.xml");
            XmlNodeList elemList = XmlDoc.SelectNodes("Root/Activity");

            for (int i = 0; i < elemList.Count; i++)
            {
                if (elemList[i].Attributes["id"].Value == selected.ToString())
                {
                    elemList[i].InnerText = EditTextBox.Text;
                    //elemList[i].Attributes["length"].Value = EditTextBox2.Text;
                    elemList[i].Attributes["filelink"].Value = PathEditTextBox.Text;
                    elemList[i].Attributes["weblink"].Value = WebEditTextBox.Text;
                    elemList[i].Attributes["color"].Value = editcolor.Text;
                    elemList[i].Attributes["start_time"].Value = start_timepicker.Value.Value.ToString("M/d HH:mm");
                    elemList[i].Attributes["end_time"].Value = end_timepicker.Value.Value.ToString("M/d HH:mm");
                    
                    //set start position
                    //DateTime oDate = DateTime.ParseExact(iString, "yyyy-MM-dd HH:mm", null);
                    DateTime start_time = DateTime.ParseExact(elemList[i].Attributes["start_time"].Value,"M/d HH:mm",null);
                    DateTime end_time = DateTime.ParseExact(elemList[i].Attributes["end_time"].Value, "M/d HH:mm", null);

                    TimeSpan start_pos = start_time - DateTime.Now;
                    TimeSpan end_pos = end_time - DateTime.Now;
                    double start_min = start_pos.TotalMinutes;
                    double end_min = end_pos.TotalMinutes;
                    //set length
                    elemList[i].Attributes["length"].Value = ((int)((end_min - start_min) / 2)).ToString();

                    elemList[i].Attributes["start_pos_x"].Value = ((int)(start_min / 2 + timeline_band * 5 + 10)).ToString();
                    String len = elemList[i].Attributes["length"].Value;
                    //elemList[i].
                    int start_pos_x = int.Parse(elemList[i].Attributes["start_pos_x"].Value);
                    int start_pos_y = int.Parse(elemList[i].Attributes["start_pos_y"].Value);
                    string co = elemList[i].Attributes["color"].Value;

                    //set time
                    //start_time.Value = DateTime.Now;

                    String activity_name = elemList[i].InnerText;
                    float timeline_len = float.Parse(len);
                    Button g1 = new Button();
                    g1.Height = 50;
                    g1.Width = timeline_len;
                    g1.HorizontalContentAlignment = HorizontalAlignment.Left;
                    Canvas.SetLeft(g1, start_pos_x);
                    Canvas.SetTop(g1, start_pos_y);
                    //g1.Margin = new Thickness(p.X, p.Y, 0, 0);
                    g1.Content = activity_name;
                    g1.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(canvas_MouseLeftButtonUp);
                    g1.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(canvas_MouseLeftButtonDown);
                    g1.PreviewMouseMove += new MouseEventHandler(canvas_MouseMove);
                    g1.PreviewMouseDoubleClick += new MouseButtonEventHandler(canvas_MouseDoubleClick);
                    g1.PreviewMouseRightButtonUp += new MouseButtonEventHandler(activity_click);
                    //g1.PreviewMouseRightButtonDown += new MouseButtonEventHandler(activity_click);
                    var bc = new BrushConverter();
                     g1.HorizontalContentAlignment = HorizontalAlignment.Left;
                    if (co == "Red")
                        g1.Background = (Brush)bc.ConvertFrom("#FFFF6464");
                    else if (co == "Blue")
                        g1.Background = (Brush)bc.ConvertFrom("#FF6464FF");
                    else if (co == "Green")
                        g1.Background = (Brush)bc.ConvertFrom("#FF64FF64");
                    else if (co == "Yellow")
                        g1.Background = (Brush)bc.ConvertFrom("#FFFFFF64");
                    else
                        g1.Background = (Brush)bc.ConvertFrom("#FFFF64FF");
                    g1.Uid = elemList[i].Attributes["id"].Value;
                    
                    front_canvas.Children.Add(g1);
                    XmlDoc.Save("xml_test2.xml");
                }
            }

            
            EditBox.Visibility = Visibility.Collapsed;

        }
        Button delete;
        private void canvas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            p = Mouse.GetPosition(front_canvas);
            //MessageBox.Show(e.Source.GetType().Name);
            if (e.Source is Button)
            {
                Button ClickedRectangle = (Button)e.Source;
                delete = ClickedRectangle;
                selected = int.Parse(ClickedRectangle.Uid);
                ActivityBox.Visibility = Visibility.Visible;
                
                dele_item = (Button)e.Source;
                //dele_item = e.OriginalSource;
            }
        }

        private void activity_click(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is Button)
            {
                Button ClickedRectangle = (Button)e.Source;
                XmlDoc.Load("xml_test2.xml");
                XmlNodeList elemList = XmlDoc.SelectNodes("Root/Activity");
                for (int i = 0; i < elemList.Count; i++)
                {
                    if (elemList[i].Attributes["id"].Value == ClickedRectangle.Uid)
                    {
                        if (elemList[i].Attributes["filelink"].Value.Length !=0)
                        {
                            Process Process = new Process();
                            string link = elemList[i].Attributes["filelink"].Value;
                            int len = link.Length - 1;
                            while (link[len] != '.')
                            {
                                len--;
                            }
                            string file = link.Substring(len + 1, link.Length - 1 - len);
                            //MessageBox.Show(file);
                            if (file == "docx")
                            {
                                Process.StartInfo.FileName = "winword.EXE";
                                Process.StartInfo.Arguments = link;
                                Process.Start();
                            }
                            else if (file == "pptx")
                            {
                                Process.StartInfo.FileName = "powerpnt.EXE";
                                Process.StartInfo.Arguments = link;
                                Process.Start();
                            }
                            else if (file == "xlsx")
                            {
                                Process.StartInfo.FileName = "excel.EXE";
                                Process.StartInfo.Arguments = link;
                                Process.Start();
                            }
                            else
                            {
                                Process.Start(link);
                            }
                        }
                        if (elemList[i].Attributes["weblink"].Value.Length != 0)
                        {
                            Process Process = new Process();
                            string link = elemList[i].Attributes["weblink"].Value;
                            Process.Start(link);
                        }
                        
                    }
                }
            }
        }
        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            //MessageBox.Show(e.Source.GetType().Name);
            if (e.Source is Button)
            {
                
                //MessageBox.Show(InputTextBox.Text);
                Button ClickedRectangle = (Button)e.Source;
                
                //ClickedRectangle.Opacity = 0.5;
                //ClickedRectangle.Visibility = Visibility.Collapsed;
                p = Mouse.GetPosition(front_canvas);
                XmlDoc.Load("xml_test2.xml");
                XmlNodeList elemList = XmlDoc.SelectNodes("Root/Activity");
                for (int i = 0; i < elemList.Count; i++)
                {
                    if (elemList[i].Attributes["id"].Value == ClickedRectangle.Uid)
                    {
                        int pos_to_min = (int)(Canvas.GetLeft(ClickedRectangle) - timeline_band * 5 - 10 )*2;
                        string date_str = DateTime.Now.AddMinutes(pos_to_min).ToString("M/d HH:mm");
                        elemList[i].Attributes["start_time"].Value = date_str;
                        elemList[i].Attributes["end_time"].Value = DateTime.ParseExact(date_str, "M/d HH:mm", null).AddMinutes(int.Parse(elemList[i].Attributes["length"].Value)*2).ToString("M/d HH:mm");
                        elemList[i].Attributes["start_pos_x"].Value = ((int)Canvas.GetLeft(ClickedRectangle)).ToString();
                        elemList[i].Attributes["start_pos_y"].Value = ((int)Canvas.GetTop(ClickedRectangle)).ToString();
                        XmlDoc.Save("xml_test2.xml");
                    }
                }
                ClickedRectangle.ReleaseMouseCapture();
                isPressed = false;

            }
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {//MessageBox.Show(e.Source.GetType().Name);
            if (e.Source is Button)
            {
                Button ClickedRectangle = (Button)e.Source;
                
                ClickedRectangle.Opacity = 1;
                if (e.ClickCount == 2)
                {
                    //front_canvas.Children.Remove(ClickedRectangle);
                   
                }
                else
                {
                    isPressed = true;
                    startPosition = e.GetPosition(front_canvas);
                    ClickedRectangle.CaptureMouse();
                }
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(e.Source.GetType().Name);
            if (e.Source is Button)
            {
                Button ClickedRectangle = (Button)e.Source;
                //MessageBox.Show(e.Source.GetType().Name);
                if (isPressed)
                {
                    Point position = e.GetPosition(front_canvas);
                    double daltaY = position.Y - startPosition.Y;
                    double daltaX = position.X - startPosition.X;
                    double pos_y = (double)ClickedRectangle.GetValue(Canvas.TopProperty) + daltaY;
                    if (pos_y> SystemParameters.PrimaryScreenHeight - 150)
                    {
                        pos_y = SystemParameters.PrimaryScreenHeight - 150;
                    }
                    Point relativePoint = ClickedRectangle.TransformToAncestor(front_canvas)
                          .Transform(new Point(0, 0));
                    //ClickedRectangle.Margin = new Thickness(relativePoint.X + daltaX, relativePoint.Y + daltaY, 0, 0);
                    //MessageBox.Show(daltaY.ToString());
                    ClickedRectangle.SetValue(Canvas.TopProperty, pos_y);
                    ClickedRectangle.SetValue(Canvas.LeftProperty, (double)ClickedRectangle.GetValue(Canvas.LeftProperty) + daltaX);
                    // Canvas.SetLeft(ClickedRectangle, 100);
                    //Canvas.SetLeft(ClickedRectangle, 100);
                   // MessageBox.Show(daltaY.ToString());
                    startPosition = position;
                }
            }
        }


        bool canvas_press = false;
        Point startPosition2;

        private void zoomAndPanControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            front_canvas.Focus();
            Keyboard.Focus(front_canvas);
           // e.Source.Parent
            //MessageBox.Show(e.Source.GetType().Name);
            if (e.Source is Canvas)
            {
                Canvas canvas_click = (Canvas)e.Source;
                canvas_press = true;

                startPosition2 = e.GetPosition(front_canvas);
                canvas_click.CaptureMouse();

            }
            else if (e.Source is Rectangle)
            {
                canvas_press = true;
                //System.Windows.MessageBox.Show(e.OriginalSource.GetType().Name);
                Rectangle rect_click = (Rectangle)e.Source;
                Canvas canvas_click = (Canvas)rect_click.Parent;
                startPosition2 = e.GetPosition(front_canvas);
                rect_click.CaptureMouse();

            }
            
        }
        
        private void zoomAndPanControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            if (e.Source is Canvas)
            {

                //MessageBox.Show(InputTextBox.Text);
                Canvas ClickedRectangle = (Canvas)e.Source;         
                ClickedRectangle.ReleaseMouseCapture();
                canvas_press = false;

            }
            else if (e.Source is Rectangle)
            {
                //System.Windows.MessageBox.Show(e.OriginalSource.GetType().Name);
                Rectangle rect_click = (Rectangle)e.Source;
                Canvas canvas_click = (Canvas)rect_click.Parent;

                rect_click.ReleaseMouseCapture();
                canvas_press = false;
            }
            

        }

        /// <summary>
        /// Event raised on mouse move in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source is Canvas)
            {
                Canvas ClickedRectangle = (Canvas)e.Source;
                //MessageBox.Show(e.Source.GetType().Name);
                if (canvas_press)
                {
                    
                    Point position = e.GetPosition(grid1);
                    if (position.X - startPosition2.X > 0) front_canvas.Margin = new Thickness(0, 0, 0, 0);
                    else if (position.X - startPosition2.X < -canvas_width + SystemParameters.PrimaryScreenWidth) front_canvas.Margin = new Thickness(-canvas_width + SystemParameters.PrimaryScreenWidth, 0, 0, 0);
                    else front_canvas.Margin = new Thickness(position.X-startPosition2.X, 0, 0, 0);

                }
            }
            else if (e.Source is Rectangle)
            {
                //System.Windows.MessageBox.Show(e.OriginalSource.GetType().Name);
                Rectangle rect_click = (Rectangle)e.Source;
                Canvas canvas_click = (Canvas)rect_click.Parent;
                if (canvas_press)
                {

                    Point position = e.GetPosition(grid1);
                    if (position.X - startPosition2.X > 0) front_canvas.Margin = new Thickness(0, 0, 0, 0);
                    else if (position.X - startPosition2.X < -canvas_width + SystemParameters.PrimaryScreenWidth) front_canvas.Margin = new Thickness(-canvas_width + SystemParameters.PrimaryScreenWidth, 0, 0, 0);
                    else front_canvas.Margin = new Thickness(position.X - startPosition2.X, 0, 0, 0);

                }
            }
        }

        const double ScaleRate = 1.1;
        int times = 0;
        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                st.ScaleX *= ScaleRate;
                st.ScaleY *= ScaleRate;
                times++;
            }
            else
            {
                if (times > 0)
                {
                    st.ScaleX /= ScaleRate;
                    st.ScaleY /= ScaleRate;
                    times--;
                }
            }
        }
        private void Edit_Act(object sender, MouseButtonEventArgs e)
        {
            EditBox.Visibility = Visibility.Visible;

            XmlDoc.Load("xml_test2.xml");
            XmlNodeList elemList = XmlDoc.SelectNodes("Root/Activity");
            for (int i = 0; i < elemList.Count; i++)
            {
                if (elemList[i].Attributes["id"].Value == selected.ToString())
                {
                    EditTextBox.Text = elemList[i].InnerText;
                    //EditTextBox2.Text = elemList[i].Attributes["length"].Value;
                    PathEditTextBox.Text = elemList[i].Attributes["filelink"].Value;
                    WebEditTextBox.Text = elemList[i].Attributes["weblink"].Value;
                    editcolor.Text = elemList[i].Attributes["color"].Value;
                    start_timepicker.Value = DateTime.ParseExact(elemList[i].Attributes["start_time"].Value, "M/d H:mm", null);
                    end_timepicker.Value = DateTime.ParseExact(elemList[i].Attributes["end_time"].Value, "M/d H:mm", null);
                }
            }
            ActivityBox.Visibility = Visibility.Collapsed;
        }
       
        private void Dele_Act(object sender, MouseButtonEventArgs e)
        {
            XmlDoc.Load("xml_test2.xml");
            string idd = selected.ToString();            
            //System.Windows.MessageBox.Show(ClickedRectangle.GetType().Name);
            
            XmlNodeList nodes = XmlDoc.SelectNodes("//Activity[@id=" + idd + "]");
            for (int i = 0;i<nodes.Count;i++)
            {
                nodes[i].ParentNode.RemoveChild(nodes[i]);

            }
            XmlDoc.Save("xml_test2.xml");
            ActivityBox.Visibility = Visibility.Collapsed;
            front_canvas.Children.Remove(delete);
        }
        private void E_Act(object sender, MouseButtonEventArgs e)
        {
            ActivityBox.Visibility = Visibility.Collapsed;
        }

        private void btnOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|Word files (*.docx)|*.docx|PowerPoint files (*.pptx)|*.pptx|Excel files (*.xlsx)|*.xlsx|PDF files (*.pdf)|*.pdf|Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    PathTextBox.Text = System.IO.Path.GetFullPath(filename);
            }
        }

        

        private void btnOpenWeb_Click(object sender, RoutedEventArgs e)
        {
            Process Process = new Process();
            Process.Start("chrome.exe");
        }
        

        private void btnEditOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|Word files (*.docx)|*.docx|PowerPoint files (*.pptx)|*.pptx|Excel files (*.xlsx)|*.xlsx|PDF files (*.pdf)|*.pdf|Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    PathEditTextBox.Text = System.IO.Path.GetFullPath(filename);
            }
        }
        private void btnEditOpenWeb_Click(object sender, RoutedEventArgs e)
        {
            Process Process = new Process();
            Process.Start("chrome.exe");
        }



        private void cancel_Click2(object sender, RoutedEventArgs e)
        {
            Todo_box.Visibility = Visibility.Collapsed;
        }


        //to do list create control
        private void create_Click2(object sender, RoutedEventArgs e)
        {
            //int timeline_len = int.Parse(InputTextBox2.Text);
            Button g1 = new Button();
            g1.Height = 50;
            var bc = new BrushConverter();
            g1.Background = (Brush)bc.ConvertFrom("#FFFF6464");
            
            g1.Width = 50;
            Canvas.SetLeft(g1, maxnum2 * 50);
            //g1.Margin = new Thickness(p.X, p.Y, 0, 0);
            g1.Content = InputTextBox2.Text;
            g1.Uid = maxnum2.ToString();

            g1.PreviewMouseDoubleClick += new MouseButtonEventHandler(canvas_MouseDoubleClick2);

            maxnum2++;

            //////save date//////

            XmlDoc2.Load("xml_todo.xml");
            XmlNode root = XmlDoc2.SelectSingleNode("Root");
            XmlElement elem = XmlDoc2.CreateElement("Activity");

            elem.SetAttribute("id", g1.Uid);
            elem.SetAttribute("filelink", PathTextBox2.Text);
            elem.SetAttribute("weblink", WebTextBox2.Text);
            //elem.SetAttribute("color", color.Text);
            elem.InnerText = InputTextBox2.Text;

            root.AppendChild(elem);
            XmlDoc2.Save("xml_todo.xml");

            front_canvas2.Children.Add(g1);

            Todo_box.Visibility = Visibility.Collapsed;

        }
        private void btnOpenWeb_Click2(object sender, RoutedEventArgs e)
        {
            Process Process = new Process();
            Process.Start("chrome.exe");
        }
        private void btnOpenFiles_Click2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|Word files (*.docx)|*.docx|PowerPoint files (*.pptx)|*.pptx|Excel files (*.xlsx)|*.xlsx|PDF files (*.pdf)|*.pdf|Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    PathTextBox2.Text = System.IO.Path.GetFullPath(filename);
            }
        }

        //Point startposition2;
        private void canvas_MouseDoubleClick2(object sender, MouseButtonEventArgs e)
        {
            //System.Windows.MessageBox.Show("ttt");
            if (e.Source is Button)
            {
                Button ClickedRectangle = (Button)e.Source;
                front_canvas2.Children.Remove(ClickedRectangle);
                //p = Mouse.GetPosition(front_canvas);
                XmlDoc2.Load("xml_todo.xml");
                XmlNode root2 = XmlDoc2.SelectSingleNode("Root");
                XmlNodeList elemList2 = XmlDoc2.SelectNodes("Root/Activity");

                XmlDoc.Load("xml_test2.xml");
                XmlNode root = XmlDoc.SelectSingleNode("Root");
                XmlElement elem = XmlDoc.CreateElement("Activity");
                
                
                for (int i = 0; i < elemList2.Count; i++)
                {
                    if (elemList2[i].Attributes["id"].Value == ClickedRectangle.Uid)
                    {
                        Button g1 = new Button();
                        g1.Height = 50;

                        g1.Width = 60;
                        Canvas.SetLeft(g1, 160);
                        Canvas.SetTop(g1, 400);
                        //g1.Margin = new Thickness(p.X, p.Y, 0, 0);
                        g1.Content = elemList2[i].InnerText;
                        g1.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(canvas_MouseLeftButtonUp);
                        g1.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(canvas_MouseLeftButtonDown);
                        g1.PreviewMouseMove += new MouseEventHandler(canvas_MouseMove);
                        g1.PreviewMouseDoubleClick += new MouseButtonEventHandler(canvas_MouseDoubleClick);
                        g1.PreviewMouseRightButtonUp += new MouseButtonEventHandler(activity_click);
                        g1.HorizontalContentAlignment = HorizontalAlignment.Left;
                        g1.Uid = maxnum.ToString();
                        maxnum++;
                        var bc = new BrushConverter();
                        g1.Background = (Brush)bc.ConvertFrom("#FFFF6464");
                        
                        front_canvas.Children.Add(g1);
                        //int pos_to_min = (int)(Canvas.GetLeft(ClickedRectangle) - timeline_band * 5 - 10) * 2;
                        //string date_str = DateTime.Now.AddMinutes(pos_to_min).ToString("M/d HH:mm");
                        elem.SetAttribute("length", "60");
                        elem.SetAttribute("start_pos_x", "160");
                        elem.SetAttribute("start_pos_y", "400");
                        elem.SetAttribute("id", g1.Uid);
                        elem.SetAttribute("filelink", elemList2[i].Attributes["filelink"].Value);
                        elem.SetAttribute("weblink", elemList2[i].Attributes["weblink"].Value);
                        elem.SetAttribute("color", "Red");
                        elem.InnerText = elemList2[i].InnerText;
                        elem.SetAttribute("start_time", DateTime.Now.ToString("M/d HH:mm"));
                        elem.SetAttribute("end_time", DateTime.Now.AddHours(2).ToString("M/d HH:mm"));
                        root.AppendChild(elem);
                        XmlDoc.Save("xml_test2.xml");

                        elemList2[i].ParentNode.RemoveChild(elemList2[i]);
                        XmlDoc2.Save("xml_todo.xml");
                    }
                }
            }
        }
    }
}
