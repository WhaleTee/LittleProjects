public interface PersistentData {
  void SaveData(SaveData data);
  void LoadData(ref SaveData data);
}