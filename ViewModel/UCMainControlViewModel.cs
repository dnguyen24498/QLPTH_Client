using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMHelper;
using GeneralClass;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace ViewModel
{
    public class UCMainControlViewModel : BaseViewModel
    {
        private ObservableCollection<MessageArgs> _systemNotification = new ObservableCollection<MessageArgs>();
        private ObservableCollection<MessageArgs> _userNotification = new ObservableCollection<MessageArgs>();

        public ICommand LoadedUCMainControl { get; set; }

        public ICommand ChangeViewNotificationCommand { get; set; }

        private int _selectedView;
        public int SelectedView
        {
            get => _selectedView;
            set
            {
                _selectedView = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MessageArgs> _listNotification;
        public ObservableCollection<MessageArgs> ListNotification
        {
            get => _listNotification;
            set
            {
                _listNotification = value;
                OnPropertyChanged();
            }
        }

        public UCMainControlViewModel()
        {
            Client.CustomClient.Instance.BroadcastNotification += Instance_BroadcastNotification;
            ListNotification = new ObservableCollection<MessageArgs>();
            SelectedView = 0;
            ChangeViewNotificationCommand = new RelayCommand<ListBox>((p) => true, (p) =>
            {
                if (SelectedView == 0)
                {
                    p.ItemsSource = ListNotification;
                }
                else if (SelectedView == 1)
                {
                    p.ItemsSource = _systemNotification;
                }
                else if (SelectedView == 2)
                {
                    p.ItemsSource = _userNotification;
                }
            });
        }


        private void Instance_BroadcastNotification(object sender, MessageArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ListNotification.Add(e);
                if (e.Type == "InformationCircle")
                {
                    _systemNotification.Add(e);
                }
                else if (e.Type == "UserSupervisorCircle")
                {
                    _userNotification.Add(e);
                }
            });
           
        }
    }
}
