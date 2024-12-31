using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class TempLevelLoader : MonoBehaviour
{
    private bool firstSave;
    void Awake()
    {
        string path = Application.persistentDataPath + "/player.dont";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            SceneManager.LoadScene("Level " + data.levelNumber);
        }

        else
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}
