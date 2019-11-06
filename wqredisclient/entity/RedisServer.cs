using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace wqredisclient.entity
{
    /// <summary>
    /// Redis Server
    /// </summary>
    public class RedisServer : INotifyPropertyChanged
    {
        private bool isConnectioning = false;
        private bool isConnectioned = false;
        private ObservableCollection<RedisDatabase> databases = new ObservableCollection<RedisDatabase>();
        private CSRedis.RedisClient redisClient;
        public ObservableCollection<RedisDatabase> Databases {
            get { return this.databases; }
            set { UpdateProperty(ref databases,value); }
        }
        public RedisConnection Connection { get; set; }
        public CSRedis.RedisClient RedisClient {
            set { UpdateProperty(ref redisClient, value); }
            get { return redisClient; }
        }
        public bool IsConnectioning
        {
            set { UpdateProperty(ref isConnectioning,value); }
            get { return isConnectioning; }
        }
        public bool IsConnectioned
        {
            set { UpdateProperty(ref isConnectioned, value); }
            get { return isConnectioned; }
        }
        private void UpdateProperty<T>(ref T properValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(properValue, newValue))
            {
                return;
            }
            properValue = newValue;
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public string Uid { get; set; }
    }
}
