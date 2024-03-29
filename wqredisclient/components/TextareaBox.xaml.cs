﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wqredisclient.common;

namespace wqredisclient.components
{
    /// <summary>
    /// TextareaBox.xaml 的交互逻辑
    /// </summary>
    public partial class TextareaBox : UserControl
    {
        private InputVaildStatus vaildStatus;
        private bool isNeedInput = false;
        public TextareaBox()
        {
            InitializeComponent();
        }

        public String Text
        {
            get { return input.Text; }
            set { input.Text = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public String Title
        {
            get { return label.Content.ToString(); }
            set { label.Content = value; }
        }
        public bool IsNeedInput
        {
            get
            {
                return isNeedInput;
            }
            set
            {
                isNeedInput = value;
                setInputVaildStatus();
            }
        }
        public InputVaildStatus VaildStatus
        {
            get
            {
                return vaildStatus;
            }
            set
            {
                vaildStatus = value;
                setInputVaildStatus();
            }
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(input.Text.Trim()))
            {
                label.Visibility = Visibility.Visible;
            }
            else
            {
                label.Visibility = Visibility.Hidden;
            }
        }

        private void setInputVaildStatus()
        {
            if (isNeedInput)
            {
                icoRequired.Visibility = Visibility.Visible;
            }
            else
            {
                icoRequired.Visibility = Visibility.Collapsed;
            }
            switch (vaildStatus)
            {
                case InputVaildStatus.No:
                    Color redColor = (Color)App.Current.FindResource("redColor");
                    icoRequired.Fill = new SolidColorBrush(redColor);
                    break;
                default:
                    icoRequired.Fill = (SolidColorBrush)App.Current.FindResource("icoNormalBrush");
                    break;
            }
        }
    }
}
