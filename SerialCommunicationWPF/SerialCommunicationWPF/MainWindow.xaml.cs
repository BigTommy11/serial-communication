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

namespace SerialConnectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Port p = new Port();

        public MainWindow()
        {
            InitializeComponent();

            btnOpen.IsEnabled = true;
            btnSend.IsEnabled = false;
            cmbComPort.ItemsSource = p.GetPortNum();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (cmbComPort.Text == "") return;

            if (p.PortOpen(cmbComPort.Text))
            {
                btnSend.IsEnabled = true;
            }
            else
            {
                btnSend.IsEnabled = false;
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            btnSend.IsEnabled = false;

            if (txtSendData.Text == "") return;

            p.Send(txtSendData.Text);
            //txtLog.Text = await p.ReciveAsync();

            btnSend.IsEnabled = true;
        }
    }
}
