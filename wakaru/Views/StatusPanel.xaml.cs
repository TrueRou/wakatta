using System;
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
using wakaru.Online;

namespace wakaru.Views
{
    /// <summary>
    /// StatusPanel.xaml 的交互逻辑
    /// </summary>
    public partial class StatusPanel : UserControl
    {
        public enum Status
        {
            IDLE, IN_CLASS, CLASS_OVER
        }

        private static StatusPanel? Instance;
        public StatusPanel()
        {
            Instance = this;
            InitializeComponent();
        }


        public static void UpdateStatus(Status status)
        {
            Instance?.Dispatcher.BeginInvoke(new Action(() => 
            {
                if (status == Status.IDLE)
                {
                    Instance.Label.Text = "闲置";
                    Instance.Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.PowerSleep;
                }
                if (status == Status.IN_CLASS)
                {
                    Instance.Label.Text = "上课";
                    Instance.Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.BookOpenPageVariant;
                }
                if (status == Status.CLASS_OVER)
                {
                    Instance.Label.Text = "下课";
                    Instance.Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.ShoeSneaker;
                }
            }));
        }
    }
}
