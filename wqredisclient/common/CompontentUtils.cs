using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using wqredisclient.components;

namespace wqredisclient.common
{
    public static class CompontentUtils
    {
        public static bool vaildInputBox(InputBox inputBox)
        {
            bool isVaild = false;         
            if(!string.IsNullOrEmpty(inputBox.DefaultValue) && string.IsNullOrEmpty(inputBox.Text))
            {
                inputBox.Text = inputBox.DefaultValue;
            }
            string text = inputBox.Text;
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace(" ", "").Replace("　", "");
                isVaild = !string.IsNullOrEmpty(text);
            }
            if(!isVaild)
            {
                inputBox.VaildStatus = InputVaildStatus.No;
            }else
            {
                inputBox.VaildStatus = InputVaildStatus.None;
            }
            return isVaild;
        }
    }
}
