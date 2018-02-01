using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class TypeNameExtensions
    {
        static TypeNameExtensions()
        {
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            
            {
                foreach (var t in a.GetTypes())
                {
                    AllTypesLkp.AddOrUpdate(new Tuple<string, string>(t.FullName, t.Name), t);
                }
                ;
            }
           
        }
        public static ConcurrentDictionary<string, Type> EntityTypesLkp = new ConcurrentDictionary<string, Type>(AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName.Contains("GenSoft.Entities")).GetTypes().ToDictionary(x => x.FullName));
        private static ConcurrentDictionary<Tuple<string, string>, Type> AllTypesLkp = new ConcurrentDictionary<Tuple<string, string>, Type>();
        public static string GetFriendlyName(this Type type)
        {
            string friendlyName = type.Name;
            if (type.IsGenericType)
            {
                int iBacktick = friendlyName.IndexOf('`');
                if (iBacktick > 0)
                {
                    friendlyName = friendlyName.Remove(iBacktick);
                }
                friendlyName += "<";
                Type[] typeParameters = type.GetGenericArguments();
                for (int i = 0; i < typeParameters.Length; ++i)
                {
                    string typeParamName = "";
                    if (typeParameters[i].IsGenericType)
                    {
                        typeParamName = typeParameters[i].GetFriendlyName();
                    }
                    else
                    {
                        typeParamName = typeParameters[i].Name;
                    }
                    
                    friendlyName += (i == 0 ? typeParamName : "," + typeParamName);
                }
                friendlyName += ">";
            }

            return friendlyName;
        }

        public static T Type2Generic<T>(this T ttype)
        {
           
            if (typeof(T).IsGenericType)
            {
                var type = typeof(T).GetGenericTypeDefinition();
                Type[] typeParameters = type.GetGenericArguments();
                List<Type> paramTypes = new List<Type>();
                for (int i = 0; i < typeParameters.Length; ++i)
                {
                    
                    if (typeParameters[i].IsGenericType)
                    {
                        paramTypes.Add(typeParameters[i].Type2Generic());
                    }
                    else
                    {
                        paramTypes.Add(typeParameters[i]);
                    }

                    
                }
                return (T)(object)type.GetType().MakeGenericType(paramTypes.ToArray());
            }

            
            
            return (T)(object)ttype;
        }

        public static Type[] GetTypeByName(string className)
        {
            var returnVal = new List<Type>();
            if (className.Contains('`') && className.Contains("[["))
            {
                var genClassName = className.Substring(0, className.IndexOf("[["));
                var genType = GetTypeByName(genClassName)[0];
                var subClassName = className.Substring(className.IndexOf("[[") + 2,className.IndexOf(',')  - (className.IndexOf("[[") + 2));
                var subType = GetTypeByName(subClassName)[0];
                return new Type[]{genType.MakeGenericType(subType)};

            }
            else
            {
                returnVal.AddRange(AllTypesLkp.Where(t =>
                    t.Key.Item1.ToLower() == className.ToLower() || t.Key.Item2.ToLower() == className.ToLower()).Select(x => x.Value).ToList());

                //foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                //{
                //    var assemblyTypes = a.GetTypes();
                //    returnVal.AddRange(assemblyTypes.Where(t =>
                //        t.Name.ToLower() == className.ToLower() ||
                //        t.FullName.ToLower() ==
                //        className.ToLower())); //|| t.FullName.ToLower().Contains(className.ToLower()) 
                //}
            }



            return returnVal.ToArray();
        }
    }
}
