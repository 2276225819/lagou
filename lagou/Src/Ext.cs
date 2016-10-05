using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Controls;

namespace lagou 
{
    /*
    class Json : IJsonValue
    {
        IJsonValue data;
        public Json(IJsonValue data)
        {
            this.data = data;
        }
        public static Json ParseObject(string s)
        {
            return new Json(JsonObject.Parse(s));
        }
        public static Json ParseArray(string s)
        {
            return new Json(JsonObject.Parse(s));
        }
        public Json this[string s]
        {
            get
            {
                return new Json(data.GetObject()[s]);
            }
        }
        public Json this[int s]
        {
            get
            {
                return new Json(data.GetArray()[s]);
            }
        }

        public JsonValueType ValueType
        {
            get
            {
                return data.ValueType;
            }
        }

        public JsonArray GetArray()
        {
            return data.GetArray();
        }

        public bool GetBoolean()
        {
            return data.GetBoolean();
        }

        public double GetNumber()
        {
            return data.GetNumber();
        }

        public JsonObject GetObject()
        {
            return data.GetObject();
        }

        public string GetString()
        {
            return data.GetString();
        }

        public string Stringify()
        {
            return data.Stringify();
        }
    }*/
    static class Ext
    { 
        public static IJsonValue Get(this IJsonValue d, string s)
        {
            if (d.ValueType != JsonValueType.Object) return null;
            var o = d.GetObject();
            if (!o.ContainsKey(s)) return null;
            return o[s];
        }

        public static IJsonValue Get(this IJsonValue d, int s)
        {
            if (d.ValueType != JsonValueType.Array) return null;
            var i = d.GetArray();
            if (i.Count > s || s < 0) return null;
            return i[s];
        }
    }



    class Util
    {
        public static void AppButtonLogin(Page page)
        {
            if (page.BottomAppBar == null)
                page.BottomAppBar = new CommandBar();
            var bar = page.BottomAppBar as CommandBar; 
            bar.PrimaryCommands.Clear();
            bar.SecondaryCommands.Clear();


            bar.Content = "help";


            var ft = bar.PrimaryCommands;
            ft.Add(new AppBarButton() { Label = "CC" });
            (ft.Last() as AppBarButton).Click += (s, e) => {
                Debug.WriteLine("ee");
            };
            var ct = bar.SecondaryCommands;
            ct.Add(new AppBarButton() { Label = "搜索" });
            (ct.Last() as AppBarButton).Click += (s, e) => {
                page.Frame.Navigate(typeof(SearchPage));
            }; 
            ct.Add(new AppBarButton() { Label = "登陆" });
            (ct.Last() as AppBarButton).Click += (s, e) => {
                page.Frame.Navigate(typeof(LoginPage));
            };
        }

    }
}
