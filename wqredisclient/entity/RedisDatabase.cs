using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wqredisclient.entity
{
    /// <summary>
    /// redis database
    /// </summary>
    public class RedisDatabase : INotifyPropertyChanged
    {
        private string name;
        private int keyCount = 0;
        private int id = 0;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                UpdateProperty(ref name, value);
            }
        }
        public int KeyCount
        {
            get
            {
                return keyCount;
            }
            set
            {
                UpdateProperty(ref keyCount, value);
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                UpdateProperty(ref id, value);
            }
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
        public RedisServer ParentServer { get; set; }
    }
}
