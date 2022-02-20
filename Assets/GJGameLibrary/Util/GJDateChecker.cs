using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GJGameLibrary
{
    public class GJDateChecker
    {
        public static bool IsToday(DateTime date)
        {
            var now = DateTime.Now;
            return IsThisMonth(date) && date.Day == now.Day;
        }
        public static bool IsThisMonth(DateTime date)
        {
            var now = DateTime.Now;
            return isThisYear(date) && date.Month == now.Month;
        }

        public static bool isThisYear(DateTime date)
        {
            var now = DateTime.Now;
            return date.Year == now.Year;
        }
    }
}