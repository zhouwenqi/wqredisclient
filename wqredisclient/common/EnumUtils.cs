using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wqredisclient.common
{
    public enum InputVaildStatus
    {
        None = 0,
        Yes = 1,
        No = 2
    }
    public enum InputValueType
    {
        Text = 0,
        Letter = 1,
        Number = 2,
        Integer = 3,
        Email = 4,
        Ip = 5
    }
}
