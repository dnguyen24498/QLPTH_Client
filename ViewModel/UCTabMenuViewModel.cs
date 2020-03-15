using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMHelper;
using GeneralClass;
using Client;
using System.Windows.Input;
using System.Windows;

namespace ViewModel
{
    public class UCTabMenuViewModel:BaseViewModel
    {
        private int _newMessage;
        private string _messageTabHeader;
        public string MessageTabHeader
        {
            get => _messageTabHeader;
            set
            {
                _messageTabHeader = value;
                OnPropertyChanged();
            }
        }

        public ICommand FocusMessageTabCommand { get; set; }
        public UCTabMenuViewModel()
        {
            MessageTabHeader = "Tin nhắn";
            Client.CustomClient.Instance.BroadcastMessage += Instance_BroadcastMessage;
            FocusMessageTabCommand = new RelayCommand<object>((p) => true, (p) =>
              {
                  _newMessage = 0;
                  MessageTabHeader = "Tin nhắn";
              });
        }

        private void Instance_BroadcastMessage(object sender, MessageArgs e)
        {
            MessageTabHeader = $"Tin nhắn({++_newMessage})";
        }
    }
}
