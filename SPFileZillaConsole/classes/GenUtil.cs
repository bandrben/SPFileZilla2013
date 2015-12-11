using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

namespace SPFileZillaConsole.classes
{

        public static class ObjectExtensions
    {

        public static bool IsNull(this object o)
        {
            return o == null ? true : (o.ToString().Trim().Length <= 0 ? true : false);
        }

        public static string SafeTrim(this object o)
        {
            return o.IsNull() ? "" : o.ToString().Trim();
        }

        public static string SafeToUpper(this object o)
        {
            return o.IsNull() ? "" : o.ToString().Trim().ToUpper();
        }

        public static bool IsEqual(this object o, object o2)
        {
            if (o == null || o2 == null)
            {
                return false;
            }
            else
            {
                return o.SafeToUpper() == o2.SafeToUpper();
            }
        }

        public static string CombineFS(this string s1, string s2)
        {
            return CombineFS(s1, s2, "\\");
        }

        public static string CombineWeb(this string s1, string s2)
        {
            return CombineFS(s1, s2, "/");
        }

        private static string CombineFS(string s1, string s2, string separator)
        {
            if (s1.IsNull())
                return s2.SafeTrim();
            else if (s2.IsNull())
                return s1.SafeTrim();
            else
            {
                return s1.SafeTrim().TrimEnd(separator.ToCharArray()) + separator + s2.SafeTrim().TrimStart(separator.ToCharArray());
            }
        }

    }

    public static class GenUtil
    {

        /// <summary>
        /// </summary>
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// </summary>
        public static List<string> ConvertStringToList(string str)
        {
            var lst = new List<string>();

            // normalize delimiters, shoul be ";"
            str = GenUtil.SafeTrim(str).Replace(",", ";");

            if (str.Contains(";"))
            {
                foreach (string cur_str in str.Split(new char[] { ';' }))
                {
                    if (!lst.Any(x => string.Compare(x.Trim(), cur_str.Trim(), true) >= 0))
                    {
                        lst.Add(cur_str.Trim());
                    }
                }
            }
            else
            {
                lst.Add(str);
            }

            return lst;
        }

        /// <summary>
        /// </summary>
        public static string NVL(object a, object b)
        {
            if (!IsNull(a))
            {
                return SafeTrim(a);
            }
            else if (!IsNull(b))
            {
                return SafeTrim(b);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// </summary>
        public static string ToNull(object x)
        {
            if ((x == null)
                || (Convert.IsDBNull(x))
                || x.ToString().Trim().Length == 0)
                return null;
            else
                return x.ToString();
        }

        /// <summary>
        /// </summary>
        public static bool IsNull(object x)
        {
            if ((x == null)
                || (Convert.IsDBNull(x))
                || x.ToString().Trim().Length == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// </summary>
        public static string SafeTrim(object x)
        {
            if (IsNull(x))
                return "";
            else
                return x.ToString().Trim();
        }

        /// <summary>
        /// Case insensitive comparison.
        /// </summary>
        public static bool IsEqual(object o1, object o2)
        {
            return SafeToUpper(o1) == SafeToUpper(o2);
        }

        /// <summary>
        /// If not valid returns 0.
        /// </summary>
        public static int SafeToNum(object o)
        {
            if (IsNull(o))
                return 0;
            else
            {
                if (IsInt(o))
                    return int.Parse(o.ToString());
                else
                    return 0;
            }
        }

        /// <summary>
        /// If not valid returns 0.
        /// </summary>
        public static double SafeToDouble(object o)
        {
            if (IsNull(o))
                return -1;
            else
            {
                double test;
                if (!double.TryParse(o.ToString(), out test))
                    return -1;
                else
                    return test;
            }
        }

        /// <summary>
        /// If not valid returns false.
        /// </summary>
        public static bool SafeToBool(object o)
        {
            if (SafeToUpper(o) == "1" ||
                SafeToUpper(o) == "YES" ||
                SafeToUpper(o) == "Y" ||
                SafeToUpper(o) == "TRUE")
                return true;
            else
                return false;
        }

        /// <summary>
        /// </summary>
        public static bool IsBool(object o)
        {
            o = SafeToUpper(o);
            return
                (o.ToString() == "1" || o.ToString() == "0" ||
                o.ToString() == "YES" || o.ToString() == "NO" ||
                o.ToString() == "Y" || o.ToString() == "N" ||
                o.ToString() == "TRUE" || o.ToString() == "FALSE");
        }

        /// <summary>
        /// If not valid returns 01/01/1900 12:00:00 AM.
        /// </summary>
        public static DateTime SafeToDateTime(object o)
        {
            if (IsNull(o))
                return DateTime.Parse("01/01/1900 12:00:00 AM");
            else
            {
                DateTime dummy;

                if (IsInt(o))
                    return new DateTime(Convert.ToInt64(o)); // use ticks
                else
                {
                    if (DateTime.TryParse(o.ToString(), out dummy))
                        return dummy;
                    else
                        return DateTime.Parse("01/01/1900 12:00:00 AM");
                }
            }
        }

        /// <summary>
        /// </summary>
        public static bool IsInt(object o)
        {
            if (IsNull(o))
                return false;

            Int64 dummy = 0;
            return Int64.TryParse(o.ToString(), out dummy);
        }

        /// <summary>
        /// </summary>
        public static bool IsDouble(object o)
        {
            if (IsNull(o))
                return false;

            Double dummy = 0;
            return Double.TryParse(o.ToString(), out dummy);
        }

        /// <summary>
        /// Trims and converts to upper case.
        /// </summary>
        public static string SafeToUpper(object o)
        {
            if (IsNull(o))
                return "";
            else
                return SafeTrim(o).ToUpper();
        }

        /// <summary>
        /// </summary>
        public static string SafeToProperCase(object o)
        {
            if (IsNull(o))
                return "";
            else
            {
                if (o.ToString().Trim().Length == 1)
                    return o.ToString().Trim().ToUpper();
                else
                    return o.ToString().Trim().Substring(0, 1).ToUpper() + o.ToString().Trim().Substring(1).ToLower();
            }
        }

        /// <summary>
        /// </summary>
        public static string SafeGetArrayVal(string[] list, int index)
        {
            return index < list.Length ? SafeTrim(list[index]) : "";
        }

        /// <summary>
        /// </summary>
        public static string SafeGetArrayVal(List<string> list, int index)
        {
            return index < list.Count ? SafeTrim(list[index]) : "";
        }

        /// <summary>
        /// </summary>
        public static Guid? SafeToGuid(object o)
        {
            if (IsGuid(o))
                return new Guid(SafeTrim(o));
            else
                return null;
        }

        /// <summary>
        /// </summary>
        public static bool IsGuid(object o)
        {
            if (IsNull(o))
                return false;

            try
            {
                var tmp = new Guid(SafeTrim(o));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        public static string NormalizeEol(string s)
        {
            return Regex.Replace(SafeTrim(s), @"\r\n|\n\r|\n|\r", "\r\n");
        }

    }
}
