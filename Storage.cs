// (c) 2023 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

using System.Collections.Generic;
using System.IO;
#if UNITY_5_3_OR_NEWER
using Newtonsoft.Json;
using UnityEngine;
#endif
#if GODOT
using System.Text.Json;
using Godot;
#endif

namespace Kalkatos
{
	/// <summary>
	/// Provides a simple storage system for saving and loading key-value pairs.
	/// </summary>
	public class Storage
	{
#if UNITY_5_3_OR_NEWER
		private static IStorage storage = new UnityStorage();
#elif GODOT
		private static IStorage storage = new GodotStorage();
#else
		private static IStorage storage = new FileStorage();
#endif

		public static void SetFilePath (string path)
		{
			storage.FilePath = path;
		}

		public static void SetFileName (string fileName)
		{
			storage.FileName = fileName;
		}

		public static void Save (string key, string value)
		{
			storage.Save(key, value);
		}

		public static void Save (string key, int value)
		{
			storage.Save(key, value);
		}

		public static void Save (string key, float value)
		{
			storage.Save(key, value);
		}

		public static bool TryLoad (string key, string defaultValue, out string value)
		{
			value = Load(key, defaultValue);
			return value != defaultValue;
		}

		public static string Load (string key, string defaultValue)
		{
			return storage.Load(key, defaultValue);
		}

		public static int Load (string key, int defaultValue)
		{
			return storage.Load(key, defaultValue);
		}

		public static float Load (string key, float defaultValue)
		{
			return storage.Load(key, defaultValue);
		}

		public static void Delete (string key)
		{
			storage.Delete(key);
		}
	}

	public interface IStorage
	{
		string FileName { get; set; }
		string FilePath { get; set; }
		void Save (string key, string value);
		void Save (string key, int value);
		void Save (string key, float value);
		string Load (string key, string defaultValue);
		int Load (string key, int defaultValue);
		float Load (string key, float defaultValue);
		void Delete (string key);
	}

	public class FileStorage : IStorage
	{
		private string filePath = "";
		private string fileName = "player-prefs.json";

		public virtual string FilePath { get => filePath; set => filePath = value; }
		public virtual string FileName { get => fileName; set => fileName = value; }

		public string Load (string key, string defaultValue)
		{
			return OpenLoad(key, defaultValue);
		}

		public int Load (string key, int defaultValue)
		{
			string valueStr = OpenLoad(key, defaultValue.ToString());
			return int.Parse(valueStr);
		}

		public float Load (string key, float defaultValue)
		{
			string valueStr = OpenLoad(key, defaultValue.ToString());
			return float.Parse(valueStr);
		}

		public void Save (string key, string value)
		{
			OpenSave(key, value);
		}

		public void Save (string key, int value)
		{
			OpenSave(key, value.ToString());
		}

		public void Save (string key, float value)
		{
			OpenSave(key, value.ToString());
		}

		public void Delete (string key)
		{
			if (File.Exists($"{FilePath}/{FileName}"))
			{
				var prefs = Deserialize<Dictionary<string, string>>(File.ReadAllText($"{FilePath}/{FileName}"));
				if (prefs.ContainsKey(key))
					prefs.Remove(key);
				SaveDictionary(prefs);
			}
		}

		private string OpenLoad (string key, string defaultValue)
		{
			if (File.Exists($"{FilePath}/{FileName}"))
			{
				var prefs = Deserialize<Dictionary<string, string>>(File.ReadAllText($"{FilePath}/{FileName}"));
				if (prefs.ContainsKey(key))
					return prefs[key];
				return defaultValue;
			}
			else
				return defaultValue;
		}

		private void OpenSave (string key, string value)
		{
			Dictionary<string, string> prefs = null;
			if (File.Exists($"{FilePath}/{FileName}"))
			{
				prefs = Deserialize<Dictionary<string, string>>(File.ReadAllText($"{FilePath}/{FileName}"));
				if (prefs.ContainsKey(key))
					prefs[key] = value;
				else
					prefs.Add(key, value);
			}
			else
				prefs = new Dictionary<string, string> { { key, value } };
			SaveDictionary(prefs);
		}

		private void SaveDictionary (Dictionary<string, string> dict)
		{
			File.WriteAllText($"{FilePath}/{FileName}", Serialize(dict));
		}

		private string Serialize<T> (T obj)
		{
#if UNITY_5_3_OR_NEWER
			return JsonConvert.SerializeObject(obj);
#elif GODOT
			return JsonSerializer.Serialize(obj);
#endif
		}

		private T Deserialize<T> (string json)
		{
#if UNITY_5_3_OR_NEWER
			return JsonConvert.DeserializeObject<T>(json);
#elif GODOT
			return JsonSerializer.Deserialize<T>(json);
#endif
		}
	}

#if UNITY_5_3_OR_NEWER
	public class UnityStorage : IStorage
	{
		public string FilePath { get => saveMethod.FilePath; set => saveMethod.FilePath = value; }
		public string FileName { get => saveMethod.FileName; set => saveMethod.FileName = value; }

		private static UnityStorageOption option;
		public static UnityStorageOption Option
		{
			get => option;
			set
			{
				option = value;
				switch (option)
				{
					case UnityStorageOption.PlayerPrefs:
						saveMethod = new PlayerPrefsMethod();
						break;
					case UnityStorageOption.PersistentDataPath:
						saveMethod = new PersistentDataPathMethod();
						break;
				}
			}
		}

		private static IStorage saveMethod = new PersistentDataPathMethod();

		public void Save (string key, string value) => saveMethod.Save(key, value);
		public void Save (string key, int value) => saveMethod.Save(key, value);
		public void Save (string key, float value) => saveMethod.Save(key, value);
		public string Load (string key, string defaultValue) => saveMethod.Load(key, defaultValue);
		public int Load (string key, int defaultValue) => saveMethod.Load(key, defaultValue);
		public float Load (string key, float defaultValue) => saveMethod.Load(key, defaultValue);
		public void Delete (string key) => saveMethod.Delete(key);

		private class PlayerPrefsMethod : IStorage
		{
			public string FilePath { get; set; }
			public string FileName { get; set; }
			public void Save (string key, string value) => PlayerPrefs.SetString(key, value);
			public void Save (string key, int value) => PlayerPrefs.SetInt(key, value);
			public void Save (string key, float value) => PlayerPrefs.SetFloat(key, value);
			public string Load (string key, string defaultValue) => PlayerPrefs.GetString(key, defaultValue);
			public int Load (string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);
			public float Load (string key, float defaultValue) => PlayerPrefs.GetFloat(key, defaultValue);
			public void Delete (string key) => PlayerPrefs.DeleteKey(key);
		}

		private class PersistentDataPathMethod : FileStorage
		{
			public override string FilePath { get => Application.persistentDataPath; set { } }
		}
	}

	public enum UnityStorageOption
	{
		PlayerPrefs,
		PersistentDataPath
	}
#endif

#if GODOT
	public class GodotStorage : FileStorage
	{
		public override string FilePath { get => OS.GetUserDataDir(); set { } }
	}
#endif
}
