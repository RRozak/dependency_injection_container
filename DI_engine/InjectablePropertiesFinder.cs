using System;
using System.Linq;
using System.Reflection;

namespace DI_engine
{
    public static class InjectablePropertiesFinder
    {
        public static PropertyInfo[] GetInjectableProperties<T>(T instance) where T : class
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            var dependencyProperties = Array.FindAll(properties,
                p => p.CustomAttributes.Select((attr, _) => attr.AttributeType).Contains(typeof(DependencyProperty)));

            var injectableProperties = Array.FindAll(dependencyProperties,
                p => p.GetSetMethod() != null);

            return injectableProperties;
        }
    }
}
