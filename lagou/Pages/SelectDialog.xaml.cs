using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “内容对话框”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace lagou
{
    public sealed partial class SelectDialog : ContentDialog
    {

        public class ITEM
        {
            public string Name { get; set; }
            public bool IsOn { get; set; }
        }
        public SelectDialog(ObservableCollection<ITEM> list)
        {
            this.InitializeComponent();
            var task = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low,async () => {
                await Task.Delay(100);
                LIST.ItemsSource = list;
            });
        }


        public string Result=""; 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (LIST.ItemsSource == null) return;

            foreach (ITEM item in LIST.Items) {
                if (item.IsOn) {
                    Result += "," + item.Name;
                }
            }
            if (Result != "") {
                Result = Result.Substring(1);
            } 

            Hide();
        }
    }
}
