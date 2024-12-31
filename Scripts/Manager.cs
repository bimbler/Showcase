using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public int totalGold = 0;
    public int levelNumber = 1;
    public int goldEarnedLevel = 1;
    public int arrowPierce = 1;
    public int bowStrength = 1;
    public int offlineEarningLevel = 1;
    public int slowMoLevel = 1;
    public float barrierHealth;
    public float enemyHealth;
    public int enemiesAlive;
    /*public Slider vsMeter;*/
    public List<GameObject> bossList=new List<GameObject>();
    public SaveData saveData;
    public bool firstSave = false;
    public bool tutorial = false;

    /*private Slider barrierHealthSlider;
    private Slider enemyHealthSlider;
    private GameObject canvas;*/
    private bool levelStatus = false;
    //Load Save File
    private void Awake()
    {
        //Load Save Data
        saveData = LoadGameData();
        
        //Create new save file if no save data
        if (firstSave)
        {
            Debug.Log("Created new save file");
            saveData = CreateNewSaveFile();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tutorial = saveData.tutorial;
        levelNumber = saveData.levelNumber;
        /*canvas = GameObject.FindGameObjectWithTag("Canvas");
        barrierHealthSlider = GameObject.FindGameObjectWithTag("HealthBarLeft").GetComponent<Slider>();
        enemyHealthSlider = GameObject.FindGameObjectWithTag("HealthBarRight").GetComponent<Slider>();*/
        //enemiesAlive = GameObject.FindGameObjectsWithTag("EnemyParent").Length;
        enemyHealth = GameObject.FindGameObjectsWithTag("EnemyParent").Length;
        bossList.AddRange(GameObject.FindGameObjectsWithTag("BossParent"));
        /*enemiesAlive += GameObject.FindGameObjectsWithTag("BossParent").Length;*/
        foreach(GameObject g in bossList)
        {
            enemyHealth += g.GetComponent<BossManager>().health;
        }
        /*barrierHealth = 10f;
        barrierHealthSlider.maxValue = barrierHealth;
        barrierHealthSlider.value = barrierHealth;
        enemyHealthSlider.maxValue = enemyHealth;
        enemyHealthSlider.value = enemyHealth;*/
        /*vsMeter.value = enemyHealth / (barrierHealth + enemyHealth);*/

       /* Debug.Log(enemyHealth / (barrierHealth + enemyHealth) + "this is value of health");
        Debug.Log("value is " + vsMeter.value);*/

    }

    public void decreaseBarrierHealth()
    {
        barrierHealth--;
        /*vsMeter.value = enemyHealth / (barrierHealth + enemyHealth);*/
        /*barrierHealthSlider.value = barrierHealth;*/
        if (barrierHealth <= 0)
        {
            /*Invoke("LevelFailed", 0.5f);*/
            levelStatus = false;
        }
    }

    public void decreaseEnemyHealth(float health)
    {
        enemyHealth -= health;
        /*vsMeter.value = enemyHealth / (barrierHealth + enemyHealth);*/
        /*enemyHealthSlider.value = enemyHealth;*/
        if (enemyHealth <= 0)
        {
            /*Invoke("LevelPassed", 1f);*/
            levelStatus = true;
        }
    }

    public void UpgradeOfflineEarning()
    {
        offlineEarningLevel++;
        SaveGameData();
    }

    public void UpgradeGold()
    {
        goldEarnedLevel++;
        SaveGameData();
    }    

    public void UpgradeSlowMo()
    {
        slowMoLevel++;
        SaveGameData();
    }
    public void DecreaseEnemy()
    {
        enemiesAlive--;
    }

    private void LevelPassed()
    {
        GameObject.FindGameObjectWithTag("Bow").SetActive(false);
        Debug.Log("level ended");
        levelNumber++;
        if(levelNumber > 8)
        {
            levelNumber = 1;
        }
        SaveGameData();
        /*canvas.GetComponent<TempUIScript>().LevelPassed();*/
    }

    private void LevelFailed()
    {
        GameObject.FindGameObjectWithTag("Bow").SetActive(false);
        SaveGameData();
        /*canvas.GetComponent<TempUIScript>().LevelFailed();*/
    }

    private SaveData CreateNewSaveFile()
    {
        string path = Application.persistentDataPath + "/player.dont";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        SaveData data = new SaveData();

        formatter.Serialize(stream, data);
        stream.Close();

        return data;
    }

    public void LoadNextScene()
    {
        string levelName = "Level " + levelNumber;
        SceneManager.LoadScene(levelName);
    }

    private SaveData LoadGameData()
    {
        string path = Application.persistentDataPath + "/player.dont";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            firstSave = false;

            return data;
        }

        else
        {
            Debug.Log("You done Screwed up");
            firstSave = true;
            return null;
        }
    }

    private void SaveGameData()
    {
        string path = Application.persistentDataPath + "/player.dont";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        SaveData data = new SaveData(this);

        formatter.Serialize(stream, data);

        stream.Close();
    }
}
