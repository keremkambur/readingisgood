using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.DataLayer.Extensions
{
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, string> _typeToFriendlyName = new Dictionary<Type, string>
        {
            { typeof(string), "string" },
            { typeof(object), "object" },
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(short), "short" },
            { typeof(int), "int" },
            { typeof(long), "long" },
            { typeof(sbyte), "sbyte" },
            { typeof(float), "float" },
            { typeof(ushort), "ushort" },
            { typeof(uint), "uint" },
            { typeof(ulong), "ulong" },
            { typeof(void), "void" }
        };

        public static string GetFriendlyName(this Type type)
        {
            if (TypeExtensions._typeToFriendlyName.TryGetValue(type, out var friendlyName))
            {
                return friendlyName;
            }

            friendlyName = type.Name;
            if (type.IsGenericType)
            {
                var backtick = friendlyName.IndexOf('`');

                if (backtick > 0)
                {
                    friendlyName = friendlyName.Remove(backtick);
                }

                friendlyName += "<";
                var typeParameters = type.GetGenericArguments();

                for (var i = 0; i < typeParameters.Length; i++)
                {
                    var typeParamName = typeParameters[i].GetFriendlyName();
                    friendlyName += (i == 0 ? typeParamName : ", " + typeParamName);
                }

                friendlyName += ">";
            }

            if (type.IsArray)
            {
                return type.GetElementType().GetFriendlyName() + "[]";
            }

            return friendlyName;
        }
        
        public static bool HasInterface(this Type type, Type interfaceType)
            => type.GetInterfaces().Any(x => x == interfaceType);

        public static bool HasInterface<TInterface>(this Type type)
            => type.HasInterface(typeof(TInterface));


        public static string LastNamespacePart(this Type type)
            => type.Namespace?.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? string.Empty;


    }
}
