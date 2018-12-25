using System;
using System.Linq;
using System.Reflection;

namespace nevladinaOrg.Web.Helpers
{
    public static class Utilities
    {
        public static void CopyTo<TSource, TDestination>(this TSource source, TDestination destination) where TSource : class where TDestination : class
        {
            var sourceProperties = source.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                foreach (var destinationProperty in destinationProperties
                                        .Where(dp => dp.Name == sourceProperty.Name &&
                                                     (Nullable.GetUnderlyingType(dp.PropertyType) ?? dp.PropertyType) == (Nullable.GetUnderlyingType(sourceProperty.PropertyType) ?? sourceProperty.PropertyType)))
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                    break;
                }
            }
        }

        public static string GetAssemblyName(Type type)
        {
            return new AssemblyName(type.GetTypeInfo().Assembly.FullName).Name;
        }
    }
}