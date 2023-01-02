using System;
using System.Linq;
using System.Text;
using IL2CPU.API;

namespace Cosmos.IL2CPU.Extensions
{
    public static class TypeExtensions
    {
        public static string GetFullName(this Type aType, bool checkParam = true)
        {
            var xSB = new StringBuilder();
            if (aType.IsArray)
            {
                xSB.Append(aType.GetElementType().GetFullName(checkParam));
                xSB.Append("[");
                int xRank = aType.GetArrayRank();
                while (xRank > 1)
                {
                    xSB.Append(",");
                    xRank--;
                }
                xSB.Append("]");
                return xSB.ToString();
            }
            if (aType.IsByRef && aType.HasElementType)
            {
                return "&" + aType.GetElementType().GetFullName(checkParam);
            }
            if (aType.IsGenericType)
            {
                xSB.Append(aType.GetGenericTypeDefinition().FullName);
            }
            else
            {
                xSB.Append(aType.FullName);
            }


            if (aType.IsGenericParameter)
            {
                if (checkParam)
                {
                    xSB.Append(aType.DeclaringType.FullName);
                    xSB.Append(aType.DeclaringMethod?.Name);
                    var paramss = aType.DeclaringMethod?.GetParameters();
                    if (paramss != null)
                    {
                        foreach (var item in paramss)
                        {
                            xSB.Append(GetFullName(item.ParameterType, false));
                        }
                    }
                }
                xSB.Append(aType.Name);
            }

            if (aType.ContainsGenericParameters && !aType.IsGenericParameter)
            {
                xSB.Append("<");
                var xArgs = aType.GetGenericArguments();
                for (int i = 0; i < xArgs.Length - 1; i++)
                {
                    xSB.Append(GetFullName(xArgs[i],false));
                    xSB.Append(", ");
                }
                if (xArgs.Length == 0)
                {
                    Console.Write("");
                }
                xSB.Append(GetFullName(xArgs.Last(),false));
                xSB.Append(">");
            }
            return xSB.ToString();
        }
    }
}
