using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.BLL
{
    public static class EnumHelper
    {
        /// <summary>
        /// Тип перечисления преобразовать в строку
        /// </summary>
        public static String ConvertEnumToString(this Enum eff)
        {
            return Enum.GetName(eff.GetType(), eff);
        }
    }
}
