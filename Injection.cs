// (c) 2023 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
using System.Collections.Generic;

namespace Kalkatos
{
	public class Injection
	{
		private static Dictionary<Type, object> instances = new();

		public static void Bind<T> (T obj)
		{
			if (!instances.ContainsKey(typeof(T)))
			{
				instances.Add(typeof(T), obj);
				return;
			}
			instances[typeof(T)] = obj;
		}

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
	}
}
