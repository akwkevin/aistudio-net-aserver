using Coldairarrow.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;

namespace AIStudio.WpfApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RichTextBox RichTextBox { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            RichTextBox = rtb;
        }


        public static void LogOut(LogLevel level, string logtxt)
        {        

            if (string.IsNullOrEmpty(logtxt))
            {
                return;
            }

            //if (isPause == true)
            //{
            //    return;
            //}


            try
            {
                App.Current.Dispatcher.Invoke((Action)delegate ()
                {
                    if (RichTextBox == null || !RichTextBox.IsLoaded) return;

                    if (RichTextBox.Document.Blocks.Count > 300)
                    {
                        RichTextBox.Document.Blocks.Clear();
                    }
                    //日志详细内容
                    Run content = new Run(logtxt);

                    //根据日志级别，设置内容字体在控件上显示的颜色
                    switch (level)
                    {
                        case LogLevel.Critical:
                            content.Foreground = Brushes.Red;
                            content.FontWeight = FontWeights.ExtraBold;
                            break;
                        case LogLevel.Error:
                            content.Foreground = Brushes.Red;
                            content.FontWeight = FontWeights.Bold;
                            break;
                        case LogLevel.Warning:
                            content.Foreground = Brushes.DarkOrange;
                            content.FontWeight = FontWeights.Bold;
                            break;
                        case LogLevel.Information:
                            content.Foreground = Brushes.Green;
                            break;
                        case LogLevel.Debug:
                            content.Foreground = Brushes.Green;
                            break;
                        default:
                            break;
                    }

                    bool isAtBottom = IsVerticalScrollBarAtBottom;
                    Paragraph p = new Paragraph();

                    //时间
                    Run title = new Run(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff ")) { Foreground = Brushes.Gray };
                    p.Inlines.Add(title);
                    p.Inlines.Add(content);
                    RichTextBox.Document.Blocks.Add(p);

                    if (isAtBottom)
                        RichTextBox.ScrollToEnd();
                });
            }
            catch { }
        }

        public static bool IsVerticalScrollBarAtBottom
        {
            get
            {
                bool atBottom = false;

                double dVer = RichTextBox.VerticalOffset;       //获取竖直滚动条滚动位置
                double dViewport = RichTextBox.ViewportHeight;  //获取竖直可滚动内容高度
                double dExtent = RichTextBox.ExtentHeight;      //获取可视区域的高度

                if (dVer + dViewport >= dExtent)
                    atBottom = true;
                else
                    atBottom = false;
                return atBottom;
            }
        }
    }
}
