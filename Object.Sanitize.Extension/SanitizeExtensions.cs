using Ganss.XSS;
using System;
using System.Linq;
using System.Reflection;

namespace Object.Sanitize.Extension
{
    public static class SanitizeExtensions
    {
        public static void Sanitize<T>(this T sanitizable, params string[] notSanitizable) where T : class
        {
            if (typeof(T) == typeof(string))
            {
                throw new NotImplementedException("Can't apply extension method for type string, use the HtmlSanitizer class to do it!");
            }
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            var sanitizableProps = Assembly
               .GetCallingAssembly()
               .GetTypes()
               .Where(t => t.IsSubclassOf(typeof(object)))
               .SelectMany(t => t.GetProperties())
               .Where(t => t.PropertyType == typeof(string) && !notSanitizable.Contains(t.Name))
               .ToList();

            sanitizableProps.ForEach(prop => {
                prop.SetValue(sanitizable, sanitizer.Sanitize(prop.GetValue(sanitizable).ToString()));
            });
        }
    }
}
