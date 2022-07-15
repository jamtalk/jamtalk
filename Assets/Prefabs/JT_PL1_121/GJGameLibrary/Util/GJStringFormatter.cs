using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace GJGameLibrary
{
    public class GJStringFormatter
    {
        public static string ParseDate(DateTime time) => string.Format("{0:s}", time);
        public static string GoodsParse_KM(int value)
        {
            float devidedValue;
            string unit;
            if (value >= 10000000f)
            {
                devidedValue = 10000000f;
                unit = "M";
            }
            else if (value >= 10000f)
            {
                devidedValue = 1000f;
                unit = "K";
            }
            else
            {
                devidedValue = 1f;
                unit = string.Empty;
            }

            var value_str_array = ((float)value / devidedValue).ToString("N2").Replace(",", string.Empty).Split('.');
            int intValue = int.Parse(value_str_array[0]);
            int floatValue = int.Parse(value_str_array[1]);
            string returnValue = GoodsParse(intValue);
            if (floatValue > 0)
                returnValue += "." + floatValue;
            return returnValue + unit;
        }
        public static string GoodsParse(int value) => string.Format("{0:#,0}", value);
        public static string GoodsParse(double value) => string.Format("{0:#,0}", value);
        public static string GoodsParse(decimal value) => string.Format("{0:#,0}", value);
        public static string DateTimeFormat(DateTime dateTime) => dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");
        public static string DateTimeToString(DateTime dateTime)
        {
            string year = dateTime.Year.ToString();
            string month = dateTime.Month < 10 ? "0" + dateTime.Month : dateTime.Month.ToString();
            string day = dateTime.Day < 10 ? "0" + dateTime.Day : dateTime.Day.ToString();
            return string.Format("{0}-{1}-{2}", year, month, day);
        }
        public static string DateTimeToString(string dateTime)
        {
            var time = DateTime.Parse(dateTime);
            string year = time.Year.ToString();
            string month = time.Month < 10 ? "0" + time.Month : time.Month.ToString();
            string day = time.Day < 10 ? "0" + time.Day : time.Day.ToString();
            return string.Format("{0}-{1}-{2}", year, month, day);
        }
        public static string DateTimeToParamString(DateTime dateTime)
        {
            var text = DateTimeToString(dateTime);
            string hour = (dateTime.Hour < 10 ? "0" : "") + dateTime.Hour;
            string min = (dateTime.Minute < 10 ? "0" : "") + dateTime.Minute;
            string sec = (dateTime.Second < 10 ? "0" : "") + dateTime.Second;
            text += string.Format(" {0}:{1}:{2}", hour, min, sec);
            return text;
        }
        public static string SHA256Hash(string data)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
        public static bool CheckEmailType(string text)
        {
            return Regex.IsMatch(text, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
        }
        public static string OnlyEnglish(string value) => Regex.Replace(value, @"[^a-zA-Z]", "");
    }
}
