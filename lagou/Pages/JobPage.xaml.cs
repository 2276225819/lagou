using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.UI.Xaml.Documents;
using System.Diagnostics;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using System.Threading.Tasks;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace lagou
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class JobPage : Page
    {
        public JobPage()
        {
            this.InitializeComponent();
            Util.AppButtonLogin(this);
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try {
                this.Waiting = true;
                if (e.Parameter == null) return;
                var url = "http://www.lagou.com/center/job_" + e.Parameter + ".html";
                var c = new HttpClient();
                var doc = await HtmlDocument.FormUrlAsync(url);


                List<HtmlElement> jq = null;
                List<HtmlElement> jt = null;
                List<HtmlElement> cr = null;
                List<HtmlElement> tr = null;
                string img = "";
                string job = "";
                string com = "";
                string addr = "";
                string lat = "";
                string lng = "";
                Task.WaitAll(
                    Task.Run(() => { jq = doc.querySelectorAll("dd.job_request span"); }),
                    Task.Run(() => { jt = doc.querySelectorAll("dd.job_request p"); }),
                    Task.Run(() => { cr = doc.querySelectorAll("ul.c_feature li"); }),
                    Task.Run(() => { img = "http://" + doc.querySelector("img.b2").getAttribute("src"); }),
                    Task.Run(() => { job = doc.querySelector("[title]").getAttribute("title"); }),
                    Task.Run(() => { com = doc.querySelector("[title] div").innerText; })

                    );
                this.Waiting = false;


                var ct = "";
                Debug.WriteLine(cr.Count); 
                Debug.WriteLine(cr[0].innerText );

                ct += cr[0].innerText.Split(' ').Last() + "/";
                ct += cr[3].innerText.Split(' ').Last() + "/";
                ct += cr[1].innerText.Split(' ').Last();


                //PAGE1 
                this.DataContext = new ITEM() {
                    Image =  img,
                    Title = job+"("+com+")",
                    Context = ct,
                    Temptation = jt[1].innerText,
                    Salary = jq[0].innerText,
                    Address = jq[1].innerText,
                    Year = jq[2].innerText,
                    Edu = jq[3].innerText,
                    Natrue = jq[4].innerText,
                };
               await Task.Yield();
               Task.WaitAll(
                    Task.Run(() => { tr = doc.querySelectorAll(".job_bt p"); }),


                    Task.Run(() => { addr = doc.querySelector("[name=\"positionAddress\"]").getAttribute("value"); }),
                    Task.Run(() => { lng = doc.querySelector("[name=\"positionLng\"]").getAttribute("value"); }),
                    Task.Run(() => { lat = doc.querySelector("[name=\"positionLat\"]").getAttribute("value"); })
                     
                );

                //PAGE2  
                foreach (HtmlElement item in tr) {
                    var block = new Paragraph();
                    block.LineHeight = 30;
                    block.Inlines.Add(new Run() { Text = item.innerText });
                    DESC.Blocks.Add(block);
                }

                try {
                    //PAGE3  
                    MAP.MapServiceToken = "abcdef-abcdefghijklmno";
                    var pos = new Geopoint(new BasicGeoposition() {
                        Longitude = double.Parse(lng),
                        Latitude = double.Parse(lat),
                    });
                    await MAP.TrySetViewAsync(pos, 14);
                    MAP.MapElements.Add(new MapIcon() {
                        Title = addr,
                        Location = pos
                    });
                }
                catch (Exception) {
                    ;
                }
            }
            catch (Exception err) {
                Debug.WriteLine(err);
                throw;
            }
        }


        public bool Waiting
        {
            get
            {
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
            public string Title { get; set; }
            public string Context { get; set; }
            public string Image { get; set; }
            public string Temptation { get; set; }

            public string Salary { get; set; }
            public string Address { get; set; }
            public string Year { get; set; }
            public string Edu { get; set; }
            public string Natrue { get; set; }

        }
    }
}