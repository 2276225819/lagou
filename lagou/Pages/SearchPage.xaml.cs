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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Data.Json;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace lagou
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            Util.AppButtonLogin(this);
            this.init();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SEARCH.Height = 80; 
        }

        private void 职位(object sender, RoutedEventArgs e)
        {
            SEARCH.Height = 680;
        }


        string url;
        int page = 0;
        private async void 搜索(object sender, RoutedEventArgs e)
        {
            SEARCH.Height = 80;
            if (Waiting) return;
            this.Waiting = true;
            var t = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () => {
                await Task.Delay(1000);
                LIST.Focus(FocusState.Programmatic);
            });
            try {
                var url = "http://www.lagou.com/jobs/positionAjax.json?";
                if (JOB.Text != "")
                    url += "&kd=" + JOB.Text;
                if (CITY.Text != "")
                    url += "&city=" + CITY.Text;
                if (PX.IsChecked == true)
                    url += "&px=new";


                if (JY.Text != "")
                    url += "&gj=" + JY.Text;
                if (XL.Text != "")
                    url += "&xl=" + XL.Text;
                if (RZ.Text != "")
                    url += "&jd=" + RZ.Text;
                if (HY.Text != "")
                    url += "&hy=" + HY.Text;

                if (LIST.ItemsSource == null)
                    LIST.ItemsSource = list;

                list.Clear();
                this.url = url;
                this.page = 1;
                Debug.WriteLine(url);
                await this.search();
                 //?gj=%E5%BA%94%E5%B1%8A%E6%AF%95%E4%B8%9A%E7%94%9F%2C3%E5%B9%B4%E5%8F%8A%E4%BB%A5%E4%B8%8B&xl=%E5%A4%A7%E4%B8%93&jd=%E6%9C%AA%E8%9E%8D%E8%B5%84&hy=%E7%A7%BB%E5%8A%A8%E4%BA%92%E8%81%94%E7%BD%91&px=default&city=%E4%B8%8A%E6%B5%B7&needAddtionalResult=false";
            } 
            catch (Exception err) {
                Debug.WriteLine(err);

                throw;
            }
            this.Waiting = false;

        }
        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                搜索(null, null);
            }
        }



        /// ///////////////////////////////////////////// 

        public bool Waiting  {
            get {
                return PROGRESS.IsActive;
            }
            set
            {
                PROGRESS.IsActive = value;
                PROGRESSBG.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        class ITEM
        {
            public string Id { get; set; }
            public string Job { get; set; }
            public string Image { get; set; }
            public string Company { get; set; }
            public string Date { get; set; } 
            public string Industry { get; set; }
            public string Salary { get; set; }
            public string Myinf { get; set; }
        }

        ObservableCollection<ITEM> list = new ObservableCollection<ITEM>();
        async Task search()
        {
            var imgp = "http://www.lgstatic.com/thumbnail_120x120/";
            HttpClient c = new HttpClient(); 
            var str = await c.GetStringAsync(new Uri(url + "&pn="+ page++));

            var json = JsonObject.Parse(str)?.Get("content")?.Get("positionResult")?.Get("result")?.GetArray();
            if (json != null) {
                foreach (IJsonValue item in json) {
                    list.Add(new ITEM() {
                        Id = item.Get("positionId")?.GetNumber().ToString(),
                        Industry = item.Get("industryField")?.GetString(),
                        Job = item.Get("positionName")?.GetString(),
                        Image = imgp + item.Get("companyLogo")?.GetString(),
                        Company = item.Get("companyShortName")?.GetString(),
                        Date = item.Get("formatCreateTime").GetString(),
                        Salary = item.Get("salary")?.GetString(),
                        Myinf = item.Get("workYear")?.GetString()+"/"+item.Get("education")?.GetString(),
                    });
                }
            }
        }


        bool lockdialog = false; 
        async void show(object sender, PointerRoutedEventArgs e)
        {
            var tbox = sender as TextBox;
            if (tbox.Tag == null) return; 
            if (lockdialog) return; 
            lockdialog = true;
            var dialog = new SelectDialog((ObservableCollection<SelectDialog.ITEM>)tbox.Tag);
            await dialog.ShowAsync();
            { 
                tbox.Text = dialog.Result;
                //tbox.Tag = dialog.Result;
            }
            lockdialog = false;  
        }

        ObservableCollection<SelectDialog.ITEM> jy = new ObservableCollection<SelectDialog.ITEM>();
        ObservableCollection<SelectDialog.ITEM> yq = new ObservableCollection<SelectDialog.ITEM>();
        ObservableCollection<SelectDialog.ITEM> rz = new ObservableCollection<SelectDialog.ITEM>();
        ObservableCollection<SelectDialog.ITEM> hy = new ObservableCollection<SelectDialog.ITEM>();
        async void init( )
        {
            var doc = await HtmlDocument.FormUrlAsync("http://www.lagou.com/jobs/list_php");


            //城市  
            CITY.PointerEntered += async (s, e) => {
                if (lockdialog) return;
                lockdialog = true;
                var dialog = new CityDialog();
                await dialog.ShowAsync();
                { 
                    CITY.Text = dialog.Result; 
                }
                lockdialog = false;
                搜索(null, null); 
            };


            //工作经验 
            JY.Tag = jy;
            JY.PointerEntered += this.show;
            var rs = await Task.Run(() => {
                return doc.querySelectorAll("a[data-lg-tj-id=\"8r00\"]");
            });
            foreach (HtmlElement item in rs) {
                if (item.innerText.IndexOf("不限") >= 0)
                    continue;
                this.jy.Add(new SelectDialog.ITEM() { Name = item.innerText.Trim(new[] { '\r', '\n', ' ' }), IsOn = false }); 
            }

            //学历要求
            XL.Tag = yq;
            XL.PointerEntered += this.show;
            var ss = await Task.Run(() => {
                return doc.querySelectorAll("a[data-lg-tj-id=\"8s00\"]");
            });
            foreach (HtmlElement item in ss) {
                if (item.innerText.IndexOf("不限") >= 0)
                    continue;
                this.yq.Add(new SelectDialog.ITEM() { Name = item.innerText.Trim(new[] { '\r', '\n', ' ' }), IsOn = false });
            }

            //融资阶段
            RZ.Tag = rz;
            RZ.PointerEntered += this.show;
            var ts = await Task.Run(() => {
                return doc.querySelectorAll("a[data-lg-tj-id=\"8t00\"]");
            });
            foreach (HtmlElement item in ts) {
                if (item.innerText.IndexOf("不限") >= 0)
                    continue;
                this.rz.Add(new SelectDialog.ITEM() { Name = item.innerText.Trim(new[] { '\r', '\n', ' ' }), IsOn = false });
            }

            //行业领域
            HY.Tag = hy;
            HY.PointerEntered += this.show;
            var us = await Task.Run(() => {
                return doc.querySelectorAll("a[data-lg-tj-id=\"8u00\"]");
            });
            foreach (HtmlElement item in us) {
                if (item.innerText.IndexOf("不限") >= 0)
                    continue;
                this.hy.Add(new SelectDialog.ITEM() { Name = item.innerText.Trim(new[] { '\r', '\n', ' ' }), IsOn = false });
            }

        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var s = sender as ScrollViewer;
             
            if (e.IsIntermediate)
                return;
             
            if ((int)s.VerticalOffset != (int)s.ScrollableHeight)
                return;

            this.Waiting = true;
            await search();
            this.Waiting = false;

        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        { 
            Frame.Navigate(typeof(JobPage), (sender as FrameworkElement).Tag);
        }
    }
}
