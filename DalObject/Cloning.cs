using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DL
{
    static class Cloning
    {

        internal static T Clone<T>(this T original) where T : new()
        {
            object copyToObject = new T();
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                propertyInfo.SetValue(copyToObject, propertyInfo.GetValue(original, null), null);

            return (T)copyToObject;
        }
    }
}