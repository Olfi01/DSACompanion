using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DSACompanion
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PreferenceAttribute : Attribute
    {
        public string Key { get; set; }
        public object DefaultValue { get; set; }

        public PreferenceAttribute(string key)
        {
            Key = key;
        }
    }
}