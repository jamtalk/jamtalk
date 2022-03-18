using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJGameLibrary
{
    public class TypeChecker
    {
        public static bool IsSubclassOfGeneric(Type genericType, Type currentType)
        {
            while (currentType != null && currentType != typeof(object))
            {
                var cur = currentType.IsGenericType ? currentType.GetGenericTypeDefinition() : currentType;
                if (genericType == cur)
                {
                    return true;
                }
                currentType = currentType.BaseType;
            }
            return false;
        }
    }
}
