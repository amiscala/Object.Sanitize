using Ganss.XSS;
using System;
using System.Linq;
using System.Reflection;

namespace Object.Sanitize.Extension
{
    public static class SanitizeExtensions
    {
        public static void Sanitize(this object sanitizable, params string[] notSanitizable)
        {
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
