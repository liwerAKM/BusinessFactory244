using OMSServer;
using PasS.Base.Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace OMSClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Task.Run(() => {
                InitializeBus();
            });
        }
        OperationMaintenanceServer OMServer = null;
        //定义Timer类
        System.Threading.Timer threadTimer;

        DataTable dtSLBRate = null;
        void InitializeBus()
        {
            try
            {
                bool IsMonitorService = MyPubConstant.IsMonitorService();
                if (IsMonitorService)
                {
                    this.InitializationOMServer();
                }
                else
                {
                    this.InitializationOMServer();
                }
                this.OMServer.ShowInfoEvent += Showrich_SysInfo;
                if (threadTimer != null)
                    threadTimer.Dispose();
                threadTimer = new System.Threading.Timer(new TimerCallback(TimerUp), null, 2000, 1000);
            }
            catch (Exception ex)
            {          
            }
        }
        private void InitializationOMServer()
        {
            if (this.OMServer == null)
            {
                bool flag2 = MyPubConstant.IsMonitorService();
                this.OMServer = new OperationMaintenanceServer();
            }
        }
        private void TimerUp(object state)
        {
         
            this.Dispatcher.Invoke(new Action(() =>
            {

                if (this.OMServer != null)
                {
                    this.OMServer.GetSLBRatesTable(ref this.dtSLBRate);
                   
                    //设置网格线
                    dgvSLBRate.GridLinesVisibility = DataGridGridLinesVisibility.All;
                    this.dgvSLBRate.ItemsSource = this.dtSLBRate.DefaultView;
                    dgvSLBRate.AutoGenerateColumns = true;
                }
            }));


        }

        void Showrich_SysInfo(string log)
        {
            ShowSysLog(log);
        }
        /// <summary>
        /// 将日志显示在文本框里
        /// </summary>
        /// <param name="log"></param>
        private void ShowSysLog(string log)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                txt_SysInfo.Text = DateTime.Now.ToString("HH:mm:ss.fff") + ": " + log + "\r\n" + txt_SysInfo.Text;
            }));

        }
    }
  


} 

