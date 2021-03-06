namespace IntoItIf.Core.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

   internal static class ObjectDictionaryHelpers
   {
      #region Methods

      internal static T GetObjectFromValidatedProperties<T>(this T arg, params string[] validPropertyNames)
         where T : new()
      {
         var dictProperties = arg.ToDictionary();
         var newDict = new Dictionary<string, object>();
         var type = typeof(T);
         foreach (var dictProperty in dictProperties)
         {
            var propInfo = type.GetProperty(dictProperty.Key);
            if (propInfo != null)
            {
               var propDefaultValue = propInfo.PropertyType.IsValueType
                  ? Activator.CreateInstance(propInfo.PropertyType)
                  : null;
               var propValue = dictProperty.Value;
               if (!validPropertyNames.Contains(dictProperty.Key) && !propValue.Equals(propDefaultValue))
                  throw new InvalidOperationException(
                     $"The valid property names allowed are: {string.Join(",", validPropertyNames)}");

               newDict.Add(dictProperty.Key, dictProperty.Value);
            }
         }

         var result = newDict.ToObject<T>();
         return result;
      }

      internal static IDictionary<string, object> ToDictionary(this object source)
      {
         var queryDictionary = new Dictionary<string, object>();
         if (source == null) return queryDictionary;
         foreach (var propertyHelper in PropertyHelper.GetProperties(source))
            queryDictionary.Add(propertyHelper.Name, propertyHelper.GetValue(source));

         return queryDictionary;
      }

      internal static T ToObject<T>(this IDictionary<string, object> source)
         where T : new()
      {
         if (source == null) throw new ArgumentNullException(nameof(source));
         var t = new T();
         var typeofT = t.GetType();
         var properties = typeofT.GetProperties();

         foreach (var property in properties)
         {
            if (!source.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
               continue;

            var item = source.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

            // Find which property type (int, string, double? etc) the CURRENT property is...
            var tPropertyType = typeofT.GetProperty(property.Name)
               ?.PropertyType;

            // Fix nullables...
            if (tPropertyType != null)
            {
               var newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

               // ...and change the type
               var newA = Convert.ChangeType(item.Value, newT);
               typeofT.GetProperty(property.Name)
                  ?.SetValue(t, newA, null);
            }
         }

         return t;
      }

      internal static T ToStatic<T>(this object source)
      {
         var entity = Activator.CreateInstance<T>();
         var properties = source.ToDictionary();
         if (properties == null) return entity;
         foreach (var property in properties)
         {
            var propInfo = entity.GetType()
               .GetProperty(property.Key);
            if (propInfo != null && propInfo.CanWrite) propInfo.SetValue(entity, property.Value, null);
         }

         return entity;
      }

      #endregion
   }
}