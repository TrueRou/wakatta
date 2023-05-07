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
    /// OnlinePanel.xaml 的交互逻辑
    /// </summary>
    public partial class OnlinePanel : UserControl
    {
        private static OnlinePanel? Instance;
        public OnlinePanel()
        {
            Instance = this;
            InitializeComponent();
        }

        public static void UpdateStatus()
        {
            var client = WakattaClient.CurrentClient;
            if (client == null) return;
            Instance?.Dispatcher.BeginInvoke(new Action(() =>
            {
                Instance.TextClientName.Text = client.Identifier;
                Instance.TextBeginRingtone.Text = client.ClassBeginRingtone.Replace(".wav", "");
                Instance.TextOverRingtone.Text = client.ClassOverRingtone.Replace(".wav", "");
                if (client.SubscribeSchedule != null)
                    Instance.TextSubscribeSchedule.Text = client.SubscribeSchedule.Label;
                else
                    Instance.TextSubscribeSchedule.Text = "未订阅";
            }));
        }

    }
}
