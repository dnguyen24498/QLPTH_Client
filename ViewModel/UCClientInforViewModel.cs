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
            Client.CustomClient.Instance.BroadcastOpenOnlyProcess += Instance_BroadcastOpenOnlyProcess;
            Client.CustomClient.Instance.BroadcastProhibitProcess += Instance_BroadcastProhibitProcess;
            Client.CustomClient.Instance.BroadcastDisableControl += Instance_BroadcastDisableControl;
            ListProcess = Client.ManageSettings.GetListSettingProcess() ?? null;
        }

        private void Instance_BroadcastDisableControl(object sender, EventArgs e)
        {
            ControlProcessStatus = "Không có tiến trình nào bị cấm:";
            ListProcess = new ObservableCollection<SettingProcessArgs>();
        }

        private void Instance_BroadcastProhibitProcess(object sender, ObservableCollection<SettingProcessArgs> e)
        {
            ControlProcessStatus = "Các tiến trình bị cấm:";
            ListProcess = e;
        }

        private void Instance_BroadcastOpenOnlyProcess(object sender, ObservableCollection<SettingProcessArgs> e)
        {
            ControlProcessStatus = "Chỉ được phép mở";
            ListProcess = e;
        }
    }
}
