using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VMHelper;
using Client;
using GeneralClass;
using Model;
using System.Reflection;

namespace ViewModel
{
    public class MainWindowViewModel:BaseViewModel
    {
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand ClosedWindowCommand { get; set; }

        private string _areaName;
        public string AreaName
        {
            get => _areaName;
            set
            {
                _areaName = value;
                OnPropertyChanged();
            }
        }

        private string _floorName;
        public string FloorName
        {
            get => _floorName;
            set
            {
                _floorName = value;
                OnPropertyChanged();
            }
        }

        private string _classroomName;
        public string ClassroomName
        {
            get => _classroomName;
            set
            {
                _classroomName = value;
                OnPropertyChanged();
            }
        }

        private string _classroomStatus;
        public string ClassroomStatus
        {
            get => _classroomStatus;
            set
            {
                _classroomStatus = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            AreaName = "--";
            FloorName = "--";
            ClassroomName = "--";
            ClassroomStatus = "--";
            LoadedWindowCommand = new RelayCommand<Grid>((p) => true, (p) => {
                Client.CustomClient.Instance.BroadcastNotification += Instance_BroadcastNotification;
                getDataFromDatabaseAsync(p);
            });
            ClosedWindowCommand = new RelayCommand<object>((p) => true, (p) => {
                if (Client.ManageSettings.DeleteAllFileWhenClose)
                {
                    Client.CustomClient.Instance.DeleteAllFileReceived();
                }
            });

        }

        private void Instance_BroadcastNotification(object sender, MessageArgs e)
        {
            GenerateNotification.ShowNoti("Thông báo mới", e.Content, Notifications.Wpf.NotificationType.Information);
        }

        private async void getDataFromDatabaseAsync(Grid p)
        {
            p.Visibility = Visibility.Visible;
            GenerateNotification.ShowNoti("Thông báo mới", "Đang kết nối tới máy chủ dữ liệu...", Notifications.Wpf.NotificationType.Information);
            await Task.Run(() => getAndConnectToClassroomServer());
            p.Visibility = Visibility.Hidden;
        }

        private void getAndConnectToClassroomServer()
        {
            try
            {
                var classroom = DataProvider.Ins.DB.Classrooms.Where(x => x.ClassroomID.Equals(ManageSettings.RoomID)).SingleOrDefault();
                if (classroom != null)
                {
                    AreaName = classroom.FloorOfArea.Area.AreaName;
                    FloorName = classroom.FloorOfArea.FloorName;
                    ClassroomName = classroom.ClassroomName;
                    ClassroomStatus = classroom.ClassroomStatus;
                    Client.ManageSettings.IPServer = classroom.ClassroomServerIPV4Address;
                    Client.ManageSettings.Port = (int)classroom.ClassroomPortNumber;

                    Client.CustomClient.Instance.Connect();
                    GenerateNotification.ShowNoti("Thông báo mới", "Đã kết nối với máy chủ dữ liệu và máy chủ phòng.", Notifications.Wpf.NotificationType.Information);
                }
            }
            catch(Exception ex)
            {
                GenerateNotification.ShowNoti("Thông báo mới", $"Có lỗi khi lấy dữ liệu từ server, mã lỗi: {ex.Message}", Notifications.Wpf.NotificationType.Information);
            }
        }


    }
}
