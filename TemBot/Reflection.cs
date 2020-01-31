using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TemBot
{
    public static class Reflection
    {
        public static BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                           BindingFlags.Static | BindingFlags.FlattenHierarchy;
        public static Dictionary<String, FieldInfo> fields = new Dictionary<String, FieldInfo>();
        public static FieldInfo GetFieldFast(this Type type, String fieldName, String fieldType, String baseType)
        {
            var key = type.FullName + ":" + fieldName + "(" + fieldType + ")" + " " + baseType;
            if (fields.ContainsKey(key)) return fields[key];
            //UnityEngine.Debug.Log(key);
            var field = type.GetField(fieldName, flags);
            //UnityEngine.Debug.Log(field);
            if (field == null || !field.FieldType.Name.Contains(fieldType) || !field.DeclaringType.Name.Contains(baseType))
                field = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).FirstOrDefault(f =>
                                            (String.IsNullOrEmpty(fieldName) || f.Name == fieldName)
                                            && f.FieldType.ToString().Contains(fieldType)
                                            && f.DeclaringType.ToString().Contains(baseType));
            if (field == null || !field.FieldType.Name.Contains(fieldType) || !field.DeclaringType.Name.Contains(baseType))
                field = type.GetFields(flags).FirstOrDefault(f =>
                                            (String.IsNullOrEmpty(fieldName) || f.Name == fieldName)
                                            && f.FieldType.ToString().Contains(fieldType)
                                            && f.DeclaringType.ToString().Contains(baseType));
            if (field == null) field = type.GetField(fieldName, flags);
            if (field != null) fields[key] = field;
            return field;
        }
        public static Dictionary<String, PropertyInfo> properties = new Dictionary<String, PropertyInfo>();
        public static PropertyInfo GetPropertyFast(this Type type, String propertyName, String propretyType, String baseType)
        {
            var key = type.FullName + "." + propertyName + "(" + propretyType + ")" + " " + baseType;
            if (properties.ContainsKey(key)) return properties[key];
            var property = type.GetProperty(propertyName, flags);
            if (!property.PropertyType.Name.Contains(propretyType) || !property.DeclaringType.Name.Contains(baseType))
                property = type.GetProperties(flags).FirstOrDefault(p => p.Name == propertyName
                                            && p.PropertyType.ToString().Contains(propretyType)
                                            && p.DeclaringType.ToString().Contains(baseType));
            if (property != null) properties[key] = property;
            return property;
        }
        public static Object GetField(this Object obj, String fieldName)
        {
            return obj.GetField(fieldName, "", "");
        }
        public static Object GetField(this Object obj, String fieldName, String fieldType, String baseType)
        {
            if (obj == null) return null;
            var objType = obj is Type ? (Type)obj : obj.GetType();
            var field = objType.GetFieldFast(fieldName, fieldType, baseType);
            if (field != null) return obj is Type ? field.GetValue(null) : field.GetValue(obj);
            var property = objType.GetPropertyFast(fieldName, fieldType, baseType);
            if (property != null) return obj is Type ? property.GetValue(null, null) : property.GetValue(obj, null);
            return null;
        }
        public static T GetField<T>(this Type obj)
        {
            return (T)obj.GetField("", typeof(T).Name, obj.Name);
        }
        public static T GetField<T>(this Object obj, String fieldName)
        {
            return (T)obj.GetField<T>(fieldName, "", "");
        }
        public static Dictionary<String, FieldInfo> staticFields = new Dictionary<String, FieldInfo>();
        public static T GetField<T>(this Object obj, String fieldName, String fieldType, String baseType)
        {
            if (String.IsNullOrEmpty(fieldType)) fieldType = typeof(T).Name;
            if (String.IsNullOrEmpty(baseType)) baseType = obj is Type ? ((Type)obj).Name : obj.GetType().Name;
            return (T)GetField(obj, fieldName, fieldType, baseType);
        }
        public static void SetField<T>(this Object obj, String fieldName, String fieldType, String baseType, T val)
        {
            if (obj == null) return;
            var objType = obj.GetType();
            if (obj is Type)
                objType = (Type)obj;
            var field = objType.GetFieldFast(fieldName, fieldType, baseType);
            if (field != null)
            {
                field.SetValue(obj, val);
                return;
            }
            var property = objType.GetPropertyFast(fieldName, fieldType, baseType);
            if (property != null) property.SetValue(obj, val, null);
        }
        public static void SetField<T>(this Object obj, String fieldName, T val)
        {
            obj.SetField(fieldName, val, "", "");
        }
        public static void SetField<T>(this Object obj, String fieldName, T val, String fieldType, String baseType)
        {
            if (String.IsNullOrEmpty(fieldType))
                fieldType = typeof(T).Name;
            if (String.IsNullOrEmpty(baseType))
                baseType = obj is Type ? ((Type)obj).Name : obj.GetType().Name;
            SetField(obj, fieldName, fieldType, baseType, val);
        }
        public static List<Object> GetList(this Object obj)
        {
            var methods = obj.GetType().GetMethods(flags);
            var obj_get_Item = methods.First(m => m.Name == "get_Item");
            var obj_Count = obj.GetType().GetProperty("Count");
            var count = (Int32)obj_Count.GetValue(obj, new Object[0]);
            var elements = new List<Object>();
            for (Int32 i = 0; i < count; i++)
                elements.Add(obj_get_Item.Invoke(obj, new Object[] { i }));
            return elements;
        }
        public static Dictionary<String, MethodInfo> methods = new Dictionary<String, MethodInfo>();
        public static MethodInfo GetMethodFast(this Type type, String methodName)
        {
            var key = type.FullName + "." + methodName;
            if (methods.ContainsKey(key)) return methods[key];
            var method = type.GetMethod(methodName, flags);
            if (method != null) methods[key] = method;
            return method;
        }
        public static Object Invoke(this Object obj, String methodName, params Object[] paramArray)
        {
            var type = obj is Type ? (Type)obj : obj.GetType();
            var method = GetMethodFast(type, methodName);
            return obj is Type ? method.Invoke(null, paramArray) : method.Invoke(obj, paramArray);
        }
        /*public static T CreateInstance<T>(params Object[] paramArray)
        {
            return (T)Activator.CreateInstance(typeof(T), args: paramArray);
        }*/
    }
}