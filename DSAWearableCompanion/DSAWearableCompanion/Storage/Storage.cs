using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace DSAWearableCompanion.Storage
{
    public class Storage
    {
        private readonly ISharedPreferences preferences;
        private readonly ISharedPreferencesEditor edit;
        public Storage(Context context)
        {
            preferences = PreferenceManager.GetDefaultSharedPreferences(context);
            edit = preferences.Edit();
            Refresh();
        }

        public void SaveChanges()
        {
            var properties = GetType().GetProperties();
            foreach (var prop in properties)
            {
                var attribute = (PreferenceAttribute)prop.GetCustomAttributes(typeof(PreferenceAttribute), false).First();
                switch (prop.GetValue(this))
                {
                    case string str:
                        edit.PutString(attribute.Key, str);
                        break;
                    case ICollection<string> strs:
                        edit.PutStringSet(attribute.Key, strs);
                        break;
                    case bool b:
                        edit.PutBoolean(attribute.Key, b);
                        break;
                    case float f:
                        edit.PutFloat(attribute.Key, f);
                        break;
                    case int i:
                        edit.PutInt(attribute.Key, i);
                        break;
                    case long l:
                        edit.PutLong(attribute.Key, l);
                        break;
                    case object o:
                        edit.PutString(attribute.Key, JsonConvert.SerializeObject(o));
                        break;
                }
            }
            var fields = GetType().GetFields();
            foreach (var field in fields)
            {
                var attribute = (PreferenceAttribute)field.GetCustomAttributes(typeof(PreferenceAttribute), false).First();
                switch (field.GetValue(this))
                {
                    case string str:
                        edit.PutString(attribute.Key, str);
                        break;
                    case ICollection<string> strs:
                        edit.PutStringSet(attribute.Key, strs);
                        break;
                    case bool b:
                        edit.PutBoolean(attribute.Key, b);
                        break;
                    case float f:
                        edit.PutFloat(attribute.Key, f);
                        break;
                    case int i:
                        edit.PutInt(attribute.Key, i);
                        break;
                    case long l:
                        edit.PutLong(attribute.Key, l);
                        break;
                    case object o:
                        edit.PutString(attribute.Key, JsonConvert.SerializeObject(o));
                        break;
                }
            }
            edit.Apply();
        }

        public void Refresh()
        {
            var properties = GetType().GetProperties();
            foreach (var prop in properties)
            {
                var attribute = (PreferenceAttribute)prop.GetCustomAttributes(typeof(PreferenceAttribute), false).First();
                var ptype = prop.PropertyType;
                if (ptype == typeof(string))
                {
                    prop.SetValue(this, preferences.GetString(attribute.Key, (string)(attribute.DefaultValue ?? default(string))));
                }
                else if (typeof(ICollection<string>).IsAssignableFrom(ptype))
                {
                    prop.SetValue(this, preferences.GetStringSet(attribute.Key, (ICollection<string>)(attribute.DefaultValue ?? default(ICollection<string>))));
                }
                else if (ptype == typeof(bool))
                {
                    prop.SetValue(this, preferences.GetBoolean(attribute.Key, (bool)(attribute.DefaultValue ?? default(bool))));
                }
                else if (ptype == typeof(float))
                {
                    prop.SetValue(this, preferences.GetFloat(attribute.Key, (float)(attribute.DefaultValue ?? default(float))));
                }
                else if (ptype == typeof(int))
                {
                    prop.SetValue(this, preferences.GetInt(attribute.Key, (int)(attribute.DefaultValue ?? default(int))));
                }
                else if (ptype == typeof(long))
                {
                    prop.SetValue(this, preferences.GetLong(attribute.Key, (long)(attribute.DefaultValue ?? default(long))));
                }
                else
                {
                    prop.SetValue(this, JsonConvert.DeserializeObject(preferences.GetString(attribute.Key, string.Empty)));
                }
            }
            var fields = GetType().GetFields();
            foreach (var field in fields)
            {
                var attribute = (PreferenceAttribute)field.GetCustomAttributes(typeof(PreferenceAttribute), false).First();
                var ftype = field.FieldType;
                if (ftype == typeof(string))
                {
                    field.SetValue(this, preferences.GetString(attribute.Key, (string)(attribute.DefaultValue ?? default(string))));
                }
                else if (typeof(ICollection<string>).IsAssignableFrom(ftype))
                {
                    field.SetValue(this, preferences.GetStringSet(attribute.Key, (ICollection<string>)(attribute.DefaultValue ?? default(ICollection<string>))));
                }
                else if (ftype == typeof(bool))
                {
                    field.SetValue(this, preferences.GetBoolean(attribute.Key, (bool)(attribute.DefaultValue ?? default(bool))));
                }
                else if (ftype == typeof(float))
                {
                    field.SetValue(this, preferences.GetFloat(attribute.Key, (float)(attribute.DefaultValue ?? default(float))));
                }
                else if (ftype == typeof(int))
                {
                    field.SetValue(this, preferences.GetInt(attribute.Key, (int)(attribute.DefaultValue ?? default(int))));
                }
                else if (ftype == typeof(long))
                {
                    field.SetValue(this, preferences.GetLong(attribute.Key, (long)(attribute.DefaultValue ?? default(long))));
                }
                else
                {
                    field.SetValue(this, JsonConvert.DeserializeObject(preferences.GetString(attribute.Key, string.Empty)));
                }
            }
        }
    }
}