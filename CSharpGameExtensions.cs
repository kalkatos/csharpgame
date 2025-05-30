// (c) 2023 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

namespace Kalkatos
{
	/// <summary>
	/// Provides extension methods for logging messages with the object's type name prepended.
	/// </summary>
	public static class CSharpGameExtensions
	{
		/// <summary>
		/// Logs a message with the object's type name prepended.
		/// </summary>
		/// <param name="obj">The object from which the method is called.</param>
		/// <param name="message">The message to log.</param>
		public static void Log (this object obj, string message)
		{
			Logger.Log($"[{obj.GetType().Name}] {message}.");
		}

		/// <summary>
		/// Logs a warning message with the object's type name prepended.
		/// </summary>
		/// <param name="obj">The object from which the method is called.</param>
		/// <param name="message">The warning message to log.</param>
		public static void LogWarning (this object obj, string message)
		{
			Logger.LogWarning($"[{obj.GetType().Name}] {message}.");
		}

		/// <summary>
		/// Logs an error message with the object's type name prepended.
		/// </summary>
		/// <param name="obj">The object from which the method is called.</param>
		/// <param name="message">The error message to log.</param>
		public static void LogError (this object obj, string message)
		{
			Logger.LogError($"[{obj.GetType().Name}] {message}.");
		}
	}
}
