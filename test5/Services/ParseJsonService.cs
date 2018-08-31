using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using test5.Models;

namespace test5.Services
{
    public class ParseJsonServices
    {
        public static object GetDataFromJson(Type type, JToken data)
        {
            // Get properties of type model
            var props = GetProps(type);

            // Check type is array or not
            if (data.Type == JTokenType.Array)
            {
                var result = new List<object>();
                
                // foreach the child item in data
                foreach (var item in data)
                {
                    // Reflection technical that get prob of type. New an instance of type
                    var obj = Activator.CreateInstance(type);

                    // Foreach the props and  get the value. Then set to instance
                    foreach (var prop in props)
                    {
                        var propValue = Convert.ChangeType(item[prop.Name].Value<string>(), prop.PropertyType);
                        prop.SetValue(obj, propValue);
                    }

                    // add instance to list instance
                    result.Add(obj);
                }

                // Return the result
                return result;
            }
            // If the type is the object
            else if (data.Type == JTokenType.Object)
            {
                // Reflection tech that get prob of type. New an instance of type
                var obj = Activator.CreateInstance(type);

                // foreach the child item in data
                foreach (var prop in props)
                {
                    // get the name of props
                    var jToken = data[prop.Name];

                    // create a type 
                    Type propType = null;

                    if (jToken.Type == JTokenType.Array)
                        // If the child of current object type is array. Then add the first type
                        propType = prop.PropertyType.GenericTypeArguments[0];
                    if (jToken.Type == JTokenType.Object)
                        // If the child of current object type is array. Add the props type
                        propType = prop.PropertyType;

                    // If the propType have value that is the prop have child. So continue extract
                    if (propType != null)
                    {
                        var propValue = GetDataFromJson(propType, jToken);
                        prop.SetValue(obj, propValue);
                    }
                }

                return obj;
            }
            return null;
        }
        private static List<PropertyInfo> GetProps(Type type)
        {
            var props = type.GetProperties();
            
            return props.ToList();
        }
 
    }

}
