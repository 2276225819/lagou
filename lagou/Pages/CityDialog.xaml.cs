using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
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
using Windows.Web.Http;

// “内容对话框”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace lagou
{
    public sealed partial class CityDialog : ContentDialog
    {
        public CityDialog()
        {
            this.InitializeComponent();
            this.Result = "";
            this.init(); 
        }

        async void init()
        {
            var url = "http://www.lagou.com/lbs/getAllCitySearchLabels.json";
            var c = new HttpClient();
            var str = await c.GetStringAsync(new Uri(url));
            var obj = JsonObject.Parse(str).Get("content").Get("data").Get("allCitySearchLabels").GetObject();
            obj.Add("#", JsonArray.Parse("[{\"name\":\"全国\"}]"));

            var group = new List<string>() {
                  "#","A","B","C","D","E","F","G","H","I","J","K","L","N","M","O","P","Q","R","S","T","U","V","W","X","Y","Z"
            };

            var data = new CollectionViewSource() {
                ItemsPath = new PropertyPath("Item"),
                IsSourceGrouped = true,
                Source = group.Select(x => { 
                    var item = obj.Get(x)?.GetArray().Select(v => new { 
                        Title = v.Get("name").GetString()
                    }).ToList();
                    return new {
                        Key = x.ToUpper()[0],
                        Color = item==null ? "#ff666666" : "#ffffffff",
                        Item = item,
                    };
                })
            };

             
            outView.ItemsSource = data.View.CollectionGroups;
            inView.ItemsSource = data.View;

        }


    

        public string Result;
        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            this.Result = obj.Text == "全国" ? "" : obj.Text; 
            Hide();

        }
    }
}
