using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Jelly
{
    [System.Serializable]
    public class Save
    {
        [System.Serializable]
        public class SaveInfo
        {
            public int m_level;
            public int m_money;
            public bool m_isMusic;
            public bool m_isVibro;
        }

        public SaveInfo m_saveInfo = new Save.SaveInfo();

        public static Save m_current = new Save();

    }

    public static class SaveLoad
    {
        public static void SaveGame(Save.SaveInfo info)
        {
            Save.m_current.m_saveInfo = info;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(GetSavePath());
            bf.Serialize(file, Save.m_current);
            file.Close();
        }

        public static Save.SaveInfo LoadGame()
        {
            if (File.Exists(GetSavePath()))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/Arkanoid.save", FileMode.Open);
                Save.m_current = (Save)bf.Deserialize(file);
                file.Close();

                return Save.m_current.m_saveInfo;
            }

            return GetDefault();
        }

        private static string GetSavePath()
        {
            return Application.persistentDataPath + "/Arkanoid.save";
        }

        public static void Clean()
        {
            File.Delete(GetSavePath());
        }

        private static Save.SaveInfo GetDefault()
        {
            Save.SaveInfo save = new Save.SaveInfo();

            save.m_level = 1;
            save.m_money = 0;
            save.m_isMusic = true;
            save.m_isVibro = true;

            return save;
        }
    }
}
