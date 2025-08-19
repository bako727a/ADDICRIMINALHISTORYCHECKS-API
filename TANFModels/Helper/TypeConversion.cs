using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TANFModels.Helper
{
    public static class TypeConversion
    {
        public static string ToStr(this object val)
        {
            if (val == null || val == DBNull.Value)
                return string.Empty;
            else
                return val.ToString();
        }

        public static int ToInt(this object val)
        {
            int res = 0;
            int.TryParse(val.ToStr(), out res);
            return res;
        }

        public static int? ToIntNull(this object val)
        {
            int? res = val.ToInt();
            return res == 0 ? null : res;
        }

        public static DateTime ToDateTime(this object str)
        {
            DateTime retValue;
            DateTime.TryParse(str.ToStr(), out retValue);
            return retValue;
        }

        public static DateTime? ToDateOrNull(this DateTime date)
        {
            return date == DateTime.MinValue ? null : (DateTime?)date;
        }

        public static double ToDouble(this object obj)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return 0.0;
            }
            else
            {
                double retValue;
                double.TryParse(obj.ToString(), out retValue);
                return retValue;
            }
        }


        public static decimal ToDecimal(this object str)
        {
            if (str == DBNull.Value)
            {
                return 0;
            }
            else
            {
                decimal retValue;
                decimal.TryParse(str.ToString(), out retValue);
                return retValue;
            }
        }

        public static long ToLong(this object str)
        {
            if (str == DBNull.Value || str == null)
            {
                return 0;
            }
            else
            {
                long retValue;
                long.TryParse(str.ToString(), out retValue);
                return retValue;
            }
        }
        public static string ToStringOrNull(this object val)
        {
            if (val == DBNull.Value || val == null)
                return null;
            return val.ToString();
        }

        public static bool ToBoolean(this object val)
        {
            if (val == DBNull.Value || val == null)
                return false;
            else if (val.ToString().ToLower() == "false" || val.ToString() == "0")
                return false;
            else
                return true;
        }
    }
}
