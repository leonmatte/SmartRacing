using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoad 
{
    public static void SaveData(PlayerData player, String path, String fileName)
    {
        String fullPath = Application.persistentDataPath + "/" + path + "/";
        bool checkFolderExists = Directory.Exists(fullPath);
        if (!checkFolderExists)
        {
            Directory.CreateDirectory(fullPath);
        }

        String json = JsonUtility.ToJson(player);
        File.WriteAllText(fullPath + fileName + ".json", json);
        Debug.Log("Save data ok." + fullPath);
    }

    public static PlayerData LoadData(String path, String fileName)
    {
        String fullPath = Application.persistentDataPath + "/" + path + "/";
        if (File.Exists(fullPath))
        {
            String textJson = File.ReadAllText(fullPath);
            PlayerData player = JsonUtility.FromJson<PlayerData>(textJson);
            return player;
        }
        else
        {
            Debug.Log("not file found on load data");
            return null;
        }
    }
}
