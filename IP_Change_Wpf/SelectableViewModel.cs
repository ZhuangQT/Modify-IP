using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IP_Change_Wpf
{
    public class SelectableViewModel : INotifyPropertyChanged
    {
        private string _ID;
        private string _IP;
        private string _子网掩码;
        private string _默认网关;
        private string _DNS服务器;
        public string ID
        {
            get => _ID; set
            {
                if (_ID == value) return;
                _ID = value;
                OnPropertyChanged();
            }
        }
        public string IP
        {
            get => _IP;
            set
            {
                if (_IP == value) return;
                _IP = value;
                OnPropertyChanged();
            }
        }

        public string 子网掩码
        {
            get => _子网掩码;
            set
            {
                if (_子网掩码 == value) return;
                _子网掩码 = value;
                OnPropertyChanged();
            }
        }
        public string 默认网关
        {
            get => _默认网关;
            set
            {
                if (_默认网关 == value) return;
                _默认网关 = value;
                OnPropertyChanged();
            }
        }
        public string DNS服务器
        {
            get => _DNS服务器;
            set
            {
                if (_DNS服务器 == value) return;
                _DNS服务器 = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
