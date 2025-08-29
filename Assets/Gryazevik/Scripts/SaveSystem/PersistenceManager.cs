using System;
using System.IO;
using System.Linq;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class PersistenceManager {
  public static SaveData saveData;
  private static readonly string filePath = Path.Combine(Application.persistentDataPath, "saveData.dat");

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
  public static void SceneManagement() {
    SceneManager.sceneLoaded += OnSceneLoaded;
    SceneManager.sceneUnloaded += OnSceneUnloaded;
  }

  private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) => Initialize();

  private static void OnSceneUnloaded(Scene scene) {
    foreach (var persistent in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<PersistentData>()) {
      persistent.SaveData(saveData);
    }

    Save(saveData);
  }

  private static void Initialize() {
    saveData = Load();

    if (saveData == null) {
      saveData = new SaveData();
      Save(saveData);
    }

    foreach (var persistent in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<PersistentData>()) {
      persistent.LoadData(ref saveData);
    }
  }

  public static void Save(SaveData data) {
    var bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary);
    File.WriteAllBytes(filePath, bytes);
  }

  public static SaveData Load() {
    if (!File.Exists(filePath)) return null;

    var bytes = File.ReadAllBytes(filePath);
    return SerializationUtility.DeserializeValue<SaveData>(bytes, DataFormat.Binary);
  }
}