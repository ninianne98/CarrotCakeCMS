using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Carrotware.CMS.Interface;
using System.ComponentModel;
/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 2 licenses.
*
* Date: October 2011
*/


namespace Carrotware.CMS.Core {
	public static class ReflectionUtilities {

		public static Object GetPropertyValue(Object obj, string property) {
			PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
			return propertyInfo.GetValue(obj, null);
		}

		public static Object GetPropertyValueFlat(Object obj, string property) {
			PropertyInfo[] propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
			PropertyInfo propertyInfo = null;
			foreach (PropertyInfo info in propertyInfos) {
				if (info.Name == property) {
					propertyInfo = info;
					break;
				}
			}
			if (propertyInfo != null) {
				return propertyInfo.GetValue(obj, null);
			} else {
				return null;
			}
		}

		public static bool DoesPropertyExist(Object obj, string property) {
			PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
			return propertyInfo == null ? false : true;
		}

		public static bool DoesPropertyExist(Type type, string property) {
			PropertyInfo propertyInfo = type.GetProperty(property);
			return propertyInfo == null ? false : true;
		}

		public static List<string> GetPropertyStrings(Object obj) {

			List<string> props = (from i in GetProperties(obj)
								  orderby i.Name
								  select i.Name).ToList();
			return props;
		}

		public static List<PropertyInfo> GetProperties(Object obj) {

			PropertyInfo[] info = obj.GetType().GetProperties();

			List<PropertyInfo> props = (from i in info.AsEnumerable()
										orderby i.Name
										select i).ToList();
			return props;
		}

		public static List<string> GetPropertyStrings(Type type) {

			List<string> props = (from i in GetProperties(type)
								  orderby i.Name
								  select i.Name).ToList();
			return props;
		}

		public static List<PropertyInfo> GetProperties(Type type) {

			PropertyInfo[] info = type.GetProperties();

			List<PropertyInfo> props = (from i in info.AsEnumerable()
										orderby i.Name
										select i).ToList();
			return props;
		}

		public static string GetPropertyString(Type type, string PropertyName) {

			string prop = (from i in GetProperties(type)
						   where i.Name.ToLower().Trim() == PropertyName.ToLower().Trim()
						   orderby i.Name
						   select i.Name).FirstOrDefault();
			return prop;
		}

		public static PropertyInfo GetProperty(Type type, string PropertyName) {

			PropertyInfo prop = (from i in GetProperties(type)
								 where i.Name.ToLower().Trim() == PropertyName.ToLower().Trim()
								 orderby i.Name
								 select i).FirstOrDefault();
			return prop;
		}

		public static List<ObjectProperty> GetObjectProperties(Object obj) {

			List<ObjectProperty> props = (from i in GetProperties(obj)
										  select GetCustProps(obj, i)).ToList();
			return props;
		}

		public static List<ObjectProperty> GetTypeProperties(Type theType) {

			List<ObjectProperty> props = (from i in GetProperties(theType)
										  select new ObjectProperty {
											  Name = i.Name,
											  PropertyType = i.PropertyType,
											  CanRead = i.CanRead,
											  CanWrite = i.CanWrite
										  }).ToList();
			return props;
		}

		public static ObjectProperty GetCustProps(Object obj, PropertyInfo prop) {

			ObjectProperty objprop = new ObjectProperty {
				Name = prop.Name,
				DefValue = obj.GetType().GetProperty(prop.Name).GetValue(obj, null),
				PropertyType = prop.PropertyType,
				CanRead = prop.CanRead,
				CanWrite = prop.CanWrite,
				Props = prop,
				CompanionSourceFieldName = "",
				FieldMode = (prop.PropertyType.ToString().ToLower() == "system.boolean") ?
						WidgetAttribute.FieldMode.CheckBox : WidgetAttribute.FieldMode.TextBox
			};

			foreach (Attribute attr in objprop.Props.GetCustomAttributes(true)) {
				if (attr is WidgetAttribute) {
					var widgetAttrib = attr as WidgetAttribute;
					if (null != widgetAttrib) {
						try { objprop.CompanionSourceFieldName = widgetAttrib.SelectFieldSource; } catch { objprop.CompanionSourceFieldName = ""; }
						try { objprop.FieldMode = widgetAttrib.Mode; } catch { objprop.FieldMode = WidgetAttribute.FieldMode.Unknown; }
					}
				}
			}

			objprop.FieldDescription = GetDescriptionAttribute(obj.GetType(), objprop.Name);

			return objprop;
		}


		public static string GetDescriptionAttribute(Type type, string fieldName) {

			PropertyInfo property = GetProperty(type, fieldName);
			if (property != null) {
				foreach (Attribute attr in property.GetCustomAttributes(typeof(DescriptionAttribute), true)) {
					if (attr != null) {
						DescriptionAttribute description = (DescriptionAttribute)attr;
						return description.Description;
					}
				}
			}

			return String.Empty;
		}

		public static IQueryable<T> SortByParm<T>(IQueryable<T> source, string SortByFieldName, string SortDirection) {
			string SortDir = "OrderBy";

			if (SortDirection.ToUpper() == "DESC") {
				SortDir = "OrderByDescending";
			}

			Type type = typeof(T);
			PropertyInfo property = GetProperty(type, SortByFieldName);
			ParameterExpression parameter = Expression.Parameter(type, SortByFieldName);
			MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
			LambdaExpression orderByExp = Expression.Lambda(propertyAccess, parameter);

			MethodCallExpression resultExp = Expression.Call(typeof(Queryable), SortDir, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));

			return source.Provider.CreateQuery<T>(resultExp);
		}


	}
}
