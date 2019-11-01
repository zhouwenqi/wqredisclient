using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace wqredisclient.entity
{
    public class RedisKey : BaseEntity
    {
        private string key;
        private string name;
        private ObservableCollection<RedisKey> keys;
        public RedisKey() {
            keys = new ObservableCollection<RedisKey>();
        }

        public string Name
        {
            set { UpdateProperty(ref name, value); }
            get { return name; }
        }
        public ObservableCollection<RedisKey> Keys
        {
            set { UpdateProperty(ref keys, value); }
            get { return keys; }
        }
        public string Key
        {
            set { UpdateProperty(ref key, value); }
            get { return key; }
        }

    }
}
