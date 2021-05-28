using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadSystemData
{
    [Serializable]
    class Wrapper
    {
        public PlayerData[] playersData;
    }
    
    public static void SaveData(PlayerData [] players, String path, String fileName)
    {
        String fullPath = Application.persistentDataPath + "/" + path + "/";
        bool checkFolderExists = Directory.Exists(fullPath);
        if (!checkFolderExists)
        {
            Directory.CreateDirectory(fullPath);
        }

        Wrapper wrapper = new Wrapper {playersData = players};
        String json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(fullPath + fileName + ".json", json);
        Debug.Log("Save data ok." + fullPath);
    }

    public static PlayerData [] LoadData(String path, String fileName)
    {
        String fullPath = Application.persistentDataPath + "/" + path + "/" + fileName + ".json";
        if (File.Exists(fullPath))
        {
            String textJson = File.ReadAllText(fullPath);
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(textJson);
            return wrapper.playersData;
        }
        else
        {
            Debug.Log("not file found on load data");
            return null;
        }
    }
}
