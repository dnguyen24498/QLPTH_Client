using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMHelper;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Collections;
using GeneralClass;

namespace ViewModel
{
    public class UCClientInforViewModel : BaseViewModel
    {
        private string machineName;
        public string MachineName
        {
            get => machineName;
            set
            {
                machineName = value;
                OnPropertyChanged();
            }
        }

        private string machineStatus;
        public string MachineStatus
        {
            get => machineStatus;
            set
            {
                machineStatus = value;
                OnPropertyChanged();
            }
        }
        private string controlProcessStatus;
        public string ControlProcessStatus
        {
            get => controlProcessStatus;
            set
            {
                controlProcessStatus = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SettingProcessArgs> listProcess;
        public ObservableCollection<SettingProcessArgs> ListProcess
        {
            get => listProcess;
            set
            {
                listProcess = value;
                OnPropertyChanged();
            }
        }
        public UCClientInforViewModel()
        {
            MachineName = Environment.MachineName;
            MachineStatus = "Khả dụng";
            ControlProcessStatus = Properties.Settings.Default.ControlProcessStatus;
            Client.CustomClient.Instance.BroadcastOpenOnlyProcess += Instance_BroadcastOpenOnlyProcess;
            Client.CustomClient.Instance.BroadcastProhibitProcess += Instance_BroadcastProhibitProcess;
            Client.CustomClient.Instance.BroadcastDisableControl += Instance_BroadcastDisableControl;
            ListProcess = new ObservableCollection<SettingProcessArgs>(Client.ManageSettings.GetListSettingProcess()
                .Where(x => !x.ProcessName.ToLower().Contains("clientmanagement") && !x.ProcessName.ToLower().Contains("servermanagement"))) ?? null;
        }

        private void Instance_BroadcastDisableControl(object sender, EventArgs e)
        {
            Properties.Settings.Default.ControlProcessStatus=ControlProcessStatus = "Không có tiến trình nào bị cấm:";
            Properties.Settings.Default.Save();
            ListProcess = new ObservableCollection<SettingProcessArgs>();
        }

        private void Instance_BroadcastProhibitProcess(object sender, ObservableCollection<SettingProcessArgs> e)
        {
            Properties.Settings.Default.ControlProcessStatus = ControlProcessStatus = "Các tiến trình bị cấm:";
            Properties.Settings.Default.Save();
            ListProcess =new ObservableCollection<SettingProcessArgs>(e.Where(x=>!x.ProcessName.ToLower().Contains("clientmanagement") && !x.ProcessName.ToLower().Contains("servermanagement")));
        }

        private void Instance_BroadcastOpenOnlyProcess(object sender, ObservableCollection<SettingProcessArgs> e)
        {
            Properties.Settings.Default.ControlProcessStatus = ControlProcessStatus = "Chỉ được phép mở";
            Properties.Settings.Default.Save();
            ListProcess = ListProcess = new ObservableCollection<SettingProcessArgs>(e.Where(x => !x.ProcessName.ToLower().Contains("clientmanagement") && !x.ProcessName.ToLower().Contains("servermanagement")));
        }
    }
}
