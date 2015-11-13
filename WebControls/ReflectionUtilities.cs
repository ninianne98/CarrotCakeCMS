using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.Web.UI.Controls {

	public static class ReflectionUtilities {

		public static BindingFlags PublicInstanceStatic {
			get {
				return BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
			}
		}

		public static Object GetPropertyValue(Object obj, string property) {
			PropertyInfo propertyInfo = obj.GetType().GetProperty(property, PublicInstanceStatic);
			return propertyInfo.GetValue(obj, null);
		}

		public static Object GetPropertyValueFlat(Object obj, string property) {
			PropertyInfo[] propertyInfos = obj.GetType().GetProperties(PublicInstanceStatic);
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
			PropertyInfo[] info = obj.GetType().GetProperties(PublicInstanceStatic);

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
			PropertyInfo[] info = type.GetProperties(PublicInstanceStatic);

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

		public static IQueryable<T> SortByParm<T>(IList<T> source, string sortByFieldName, string sortDirection) {
			return SortByParm<T>(source.AsQueryable(), sortByFieldName, sortDirection);
		}

		public static IQueryable<T> SortByParm<T>(IQueryable<T> source, string sortByFieldName, string sortDirection) {
			sortDirection = String.IsNullOrEmpty(sortDirection) ? "ASC" : sortDirection.Trim().ToUpper();

			string SortDir = sortDirection.Contains("DESC") ? "OrderByDescending" : "OrderBy";

			Type type = typeof(T);
			ParameterExpression parameter = Expression.Parameter(type, "source");

			PropertyInfo property = null;
			Expression propertyAccess = null;

			if (sortByFieldName.Contains('.')) {
				//handles complex child properties
				string[] childProps = sortByFieldName.Split('.');
				property = type.GetProperty(childProps[0]);
				propertyAccess = Expression.MakeMemberAccess(parameter, property);
				for (int i = 1; i < childProps.Length; i++) {
					property = property.PropertyType.GetProperty(childProps[i]);
					propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
				}
			} else {
				property = type.GetProperty(sortByFieldName);
				propertyAccess = Expression.MakeMemberAccess(parameter, property);
			}

			LambdaExpression orderByExp = Expression.Lambda(propertyAccess, parameter);

			MethodCallExpression resultExp = Expression.Call(typeof(Queryable), SortDir, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));

			return source.Provider.CreateQuery<T>(resultExp);
		}
	}
}