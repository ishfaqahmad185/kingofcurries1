using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

 
namespace ExtensionMethods
{
    internal static class MyExtensions
    {
        internal static int ToInt32(this object obj)
        {
            return Convert.ToInt32(obj);
        }
        internal static decimal ToDecimal(this object obj)
        {
            return Convert.ToDecimal(obj);
        }
        internal static Double ToDouble(this object obj)
        {
            return Convert.ToDouble(obj);
        }
        internal static DateTime ToDate(this object obj)
        {
            return Convert.ToDateTime(obj);
        }
        internal static DateTime ToFromDate(this object obj)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return Convert.ToDateTime("1900-01-01");
            }
        }
        internal static DateTime ToToDate(this object obj)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return Convert.ToDateTime("2900-01-01");
            }
        }
        internal static bool ToBool(this object obj)
        {
            try
            {
                return Convert.ToBoolean(obj);
            }
            catch
            {
                return false;
            }
        }


    }
}
