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

namespace DSACompanion.Storage
{
    public class AppStorage : Storage
    {
        public AppStorage(Context context) : base(context) { }

        [Preference("number_of_dies", DefaultValue = 3)]
        public int NumberOfDies { get; set; }
    }
}