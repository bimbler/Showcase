using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int levelNumber;
    public int goldEarnedLevel;
    public int offlineEarningLevel;
    public int slowMoLevel;
    public int totalGold;

    public int totalEnemiesKilled;
    public int totalPowerupsUsed;
    public int totalArrowsFired;
    public int totalGoldCollected;

    public bool tutorial;

    public SaveData(Manager manager)
    {
        totalGold = manager.totalGold;
        levelNumber = manager.levelNumber;
        goldEarnedLevel = manager.goldEarnedLevel;
        slowMoLevel = manager.slowMoLevel;
        offlineEarningLevel = manager.offlineEarningLevel;
        tutorial = manager.tutorial;
    }

    public SaveData()
    {
        totalGold = 0;
        levelNumber = 1;
        goldEarnedLevel = 1;
        offlineEarningLevel = 1;
        tutorial = false;
    }
}
