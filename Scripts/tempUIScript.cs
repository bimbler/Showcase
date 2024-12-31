using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TempUIScript : MonoBehaviour
{
    public GameObject HealthBar;
    public GameObject SlowmotionBar;
    public GameObject LevelEndUI;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI bannerText;

    //Comment
    void Start()
    {
        HealthBar.SetActive(true);
        SlowmotionBar.SetActive(true);
        LevelEndUI.SetActive(false);
    }

    public void LevelPassed()
    {
        HealthBar.SetActive(false);
        SlowmotionBar.SetActive(false);
        LevelEndUI.SetActive(true);
        buttonText.text = "Next Level";
        bannerText.text = "STAGE CLEARED";
    }

    public void LevelFailed()
    {
        HealthBar.SetActive(false);
        SlowmotionBar.SetActive(false);
        LevelEndUI.SetActive(true);
        buttonText.text = "Replay Level";
        bannerText.text = "STAGE FAILED";
    }
}
