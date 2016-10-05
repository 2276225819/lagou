using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介绍

namespace lagou
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Util.AppButtonLogin(this);

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: 准备此处显示的页面。

            // TODO: 如果您的应用程序包含多个页面，请确保
            // 通过注册以下事件来处理硬件“后退”按钮:
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed 事件。
            // 如果使用由某些模板提供的 NavigationHelper，
            // 则系统会为您处理该事件。


            //await Task.Delay(1000);
            /*  
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                  Frame.Navigate(typeof(SearchPage));
              });

            
                        SEARCH.Navigate(typeof(SearchPage));
                        LOGIN.Navigate(typeof(LoginPage));
                          */
            /*  
         await new CityDialog().ShowAsync();
  */

            /*  
       var group = new List<string>() {
         "#","A","B","C","D","E","F","G","H","I","J","K","L","N","M","O","P","Q","R","S","T","U","V","W","X","Y","Z"
     };

     var items = new List<string>() { "Ass", "Ass", "Aswdwdwq", "Asfqwfewfwef", "sad", "asdas", "Bdsf", "gqwfw", "tttttww", "nnnww", "ew", "twww", "twwfefef", "www" };




     var data = new CollectionViewSource() {
         ItemsPath = new PropertyPath("Item"),
         IsSourceGrouped = true,
         Source = group.Select(x => {
             var item = items
                 .Where(w => w.ToLower()[0] == x.ToLower()[0])
                 .Select(i => new { Title = i })
                 .ToList();
             return new {
                 Key = x.ToUpper()[0],
                 Color = item.Count == 0 ? "#ff666666" : "#ffffffff",
                 Item = item,
             };
         })
     };

     outView.ItemsSource = data.View.CollectionGroups;
     inView.ItemsSource = data.View;


            this.DataContext = new {
                Data = data,
            };*/
        }
    }
}
