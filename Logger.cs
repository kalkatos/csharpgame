// (c) 2023 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

using System;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif
#if GODOT
using Godot;
#endif

namespace Kalkatos
{
	/// <summary>
	/// Provides logging solutions for any platform.
	/// </summary>
	public class Logger
	{
#if UNITY_5_3_OR_NEWER
		private static ILogger log = new UnityLogger();
#elif GODOT
		private static ILogger log = new GodotLogger();
#else
		private static ILogger log = new BaseLogger();
#endif

        public static void Log (string msg) 
		{
			log.Log(msg);
		}

		public static void LogWarning (string msg)
		{
			log.LogWarning(msg);
		}

		public static void LogError (string msg)
		{
			log.LogError(msg);
		}
	}

	public interface ILogger
	{
		void Log (string msg);
		void LogWarning (string msg);
		void LogError (string msg);
	}

	public class BaseLogger : ILogger
	{
		public void Log (string msg)
		{
			Console.WriteLine(msg);
		}

		public void LogWarning (string msg)
		{
			Console.WriteLine($"[Warning] {msg}");
		}

		public void LogError (string msg)
		{
			Console.WriteLine($"[Error] {msg}");
		}
	}

#if UNITY_5_3_OR_NEWER
	public class UnityLogger : ILogger
	{
		public void Log (string msg)
		{
			Debug.Log(msg);
		}

		public void LogWarning (string msg)
		{
			Debug.LogWarning(msg);
		}

		public void LogError (string msg)
		{
			Debug.LogError(msg);
		}
	}
#endif

#if GODOT
    public class GodotLogger : ILogger
    {
        public void Log (string msg)
        {
			GD.Print(msg);
        }

        public void LogError (string msg)
        {
			GD.PrintErr(msg);
        }

        public void LogWarning (string msg)
        {
			GD.PushWarning(msg);
        }
    }
#endif
}
