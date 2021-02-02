using IP_Change_Wpf.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Management;
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

namespace IP_Change_Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<SelectableViewModel> Items2 { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cb_target.ItemsSource = GetLocalIP();
            cb_target_his.ItemsSource = GetHisIP();
            DG_his.ItemsSource = CreateData();
        }
        Dictionary<int, DataRow> dic = new Dictionary<int, DataRow>();
        private List<string> GetHisIP()
        {
            List<string> list = new List<string>();
            DataTable dt = Sql_Service.GetHis_IP();

            if (dt != null && dt.Rows.Count != 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dic[i] = dt.Rows[i];
                    list.Add(dt.Rows[i]["IP"].ToString());
                }
            }
            return list;
        }
        private List<string> GetLocalIP()
        {
            List<string> list = new List<string>();
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;
                string[] addresses = (string[])mo["IPAddress"];
                if (addresses[0].Contains("xxx.xxx.xxx.xxx"))
                {
                    Console.WriteLine("不修改保护的地址");
                    continue;
                }
                list.Add(addresses[0]);
            }

            return list;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetNetworkAdapter(tb_IP.Text, tb_zym.Text, tb_mrwg.Text, tb_dns.Text);
        }

        private void cb_target_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var aa = sender as ComboBox;
            if (sender as ComboBox != null && (sender as ComboBox).SelectedIndex != -1)
            {
                tb_dns.Text = dic[(sender as ComboBox).SelectedIndex]["dns"].ToString();
                tb_IP.Text = dic[(sender as ComboBox).SelectedIndex]["IP"].ToString();
                tb_mrwg.Text = dic[(sender as ComboBox).SelectedIndex]["gateway"].ToString();
                tb_zym.Text = dic[(sender as ComboBox).SelectedIndex]["subnet"].ToString();
            }
        }
        public void SetNetworkAdapter(string ip, string mask, string host, string DNS)
        {
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;
                string[] addresses = (string[])mo["IPAddress"];
                if (addresses[0].Contains("xxx.xxx.xxx.xxx") || addresses[0] != cb_target.Text)
                {
                    Console.WriteLine("不修改保护的地址");
                    continue;
                }
                Console.WriteLine("开始修改");
                try
                {
                    //设置ip地址和子网掩码 
                    inPar = mo.GetMethodParameters("EnableStatic");
                    inPar["IPAddress"] = new string[] { ip };
                    inPar["SubnetMask"] = new string[] { mask };//"255.255.255.0"
                    outPar = mo.InvokeMethod("EnableStatic", inPar, null);
                    //设置网关地址 
                    inPar = mo.GetMethodParameters("SetGateways");
                    inPar["DefaultIPGateway"] = new string[] { host };
                    outPar = mo.InvokeMethod("SetGateways", inPar, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                //设置DNS 

                inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                inPar["DNSServerSearchOrder"] = new string[] { DNS };//可设置第二个dns
                outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
                MessageBox.Show("修改成功！");
                Window_Loaded(null, null);
                break;

            }
        }
        private static ObservableCollection<SelectableViewModel> CreateData()
        {
            DataTable dt = Sql_Service.GetHis_IP();

            ObservableCollection<SelectableViewModel> lsos = new ObservableCollection<SelectableViewModel>();
            foreach (DataRow dr in dt.Rows)
            {
                SelectableViewModel aa = new SelectableViewModel();
                aa.ID = dr["ID"].ToString();
                aa.IP = dr["IP"].ToString();
                aa.默认网关 = dr["gateway"].ToString();
                aa.子网掩码 = dr["subnet"].ToString();
                aa.DNS服务器 = dr["dns"].ToString();
                lsos.Add(aa);
            }
            return lsos;
        }

        private void CountingButton_OnClick(object sender, RoutedEventArgs e)
        {
            //Sql_Service.InsertIP("", "", "", "");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Sql_Service.InsertIP(tb_IP.Text, tb_zym.Text, tb_mrwg.Text, tb_dns.Text);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window_Loaded(null, null);
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            cb_target.Text = "";
            cb_target_his.Text = "";
            tb_dns.Text = "";
            tb_IP.Text = "";
            tb_mrwg.Text = "";
            tb_zym.Text = "";
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SelectableViewModel sv = DG_his.SelectedItem as SelectableViewModel;
            if (sv == null)
            {
                return;
            }
            Sql_Service.DeletIP(sv.ID);
            DG_his.ItemsSource = CreateData();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Window_Loaded(null, null);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"]) continue;
                string[] addresses = (string[])mo["IPAddress"];
                if (addresses[0].Contains("xxx.xxx.xxx.xxx") || addresses[0] != cb_target.Text)
                {
                    Console.WriteLine("不修改保护的地址");
                    continue;
                }
                mo.InvokeMethod("SetDNSServerSearchOrder", null);
                mo.InvokeMethod("EnableStatic", null);
                mo.InvokeMethod("SetGateways", null);
                mo.InvokeMethod("EnableDHCP", null);
                break;
            }
            Window_Loaded(null, null);
            MessageBox.Show("修改成功！");
        }
    }
}
