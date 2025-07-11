// (c) 2023 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;

namespace Kalkatos
{
	/// <summary>
	/// Provides a simple dependency injection system.
	/// </summary>
	public class Injection
	{
		private static Dictionary<Type, object> instances = new();

		/// <summary>
		/// Binds an object of type T to the dependency injection system.
		/// </summary>
		/// <typeparam name="T">The type of the object to bind.</typeparam>
		/// <param name="obj">The object to bind.</param>
		public static void Bind<T> (T obj)
		{
			if (!instances.ContainsKey(typeof(T)))
			{
				instances.Add(typeof(T), obj);
				return;
			}
			instances[typeof(T)] = obj;
		}

		/// <summary>
		/// Resolves an instance of type T from the dependency injection system.
		/// </summary>
		/// <typeparam name="T">The type of the object to resolve.</typeparam>
		/// <param name="instance">The resolved instance of type T.</param>
		/// <param name="quiet">If true, suppresses error logging if no binding is found.</param>
		public static void Resolve<T> (out T instance, bool quiet = false)
		{
			if (instances.ContainsKey(typeof(T)))
			{
				instance = (T)instances[typeof(T)];
				return;
			}
			if (!quiet)
				Logger.LogError($"No binding found for type {typeof(T)}");
			instance = default;
		}

		public static T Resolve<T> (bool quiet = false)
		{
			T instance;
			Resolve(out instance, quiet);
			return instance;
		}
	}
}
