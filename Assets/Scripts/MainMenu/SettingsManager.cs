using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
  public Button normalDifficultyButton;
  public Button hardDifficultyButton;
  public Button savePrefsButton;
  public Slider smoothSpeedSlider;
  public Slider sensitivitySlider;

  [SerializeField] private float timeBetweenWaves;
  [SerializeField] private int zombiesPerWave;
  [SerializeField] private float smoothSpeed;
  [SerializeField] private float sensitivity;

  public Button map1Button;
  public Button map2Button;
  public Button continueButton;
  [SerializeField] private string selectedMap;
  void Start()
  {
    smoothSpeedSlider.minValue = 1f;
    smoothSpeedSlider.maxValue = 10f;
    sensitivitySlider.minValue = 1f;
    sensitivitySlider.maxValue = 5f;

    timeBetweenWaves = 10f;
    zombiesPerWave = 4;
    smoothSpeed = 10f;
    sensitivity = 2f;


    LoadSettings();

    normalDifficultyButton.onClick.AddListener(SetNormalDiff);
    hardDifficultyButton.onClick.AddListener(SetHardDiff);

    map1Button.onClick.AddListener(() => SelectMap("Map1"));
    map2Button.onClick.AddListener(() => SelectMap("Map2"));
    continueButton.onClick.AddListener(LoadSelectedMap);

    savePrefsButton.onClick.AddListener(SaveSettings);
  }

  public void LoadSettings()
  {
    if (PlayerPrefs.HasKey("TimeBetweenWaves")) 
    {
      timeBetweenWaves = PlayerPrefs.GetFloat("TimeBetweenWaves");
    }

    if (PlayerPrefs.HasKey("ZombiesPerWave")) 
    {
      zombiesPerWave = PlayerPrefs.GetInt("ZombiesPerWave");
    }

    if(PlayerPrefs.HasKey("SmoothSpeed"))
    {
      smoothSpeed = PlayerPrefs.GetFloat("SmoothSpeed");
      smoothSpeedSlider.value = smoothSpeed;
      }

    if(PlayerPrefs.HasKey("Sensitivity"))
    {
      sensitivity = PlayerPrefs.GetFloat("Sensitivity");
      sensitivitySlider.value = sensitivity;
    }

    if(PlayerPrefs.HasKey("SelectedMap"))
    {
      selectedMap = PlayerPrefs.GetString("SelectedMap");
    }
  }

  public void SaveSettings()
  {
    smoothSpeed = smoothSpeedSlider.value;
    sensitivity = sensitivitySlider.value;

    PlayerPrefs.SetFloat("TimeBetweenWaves", timeBetweenWaves);
    PlayerPrefs.SetInt("ZombiesPerWave", zombiesPerWave);
    PlayerPrefs.SetFloat("SmoothSpeed", smoothSpeed);
    PlayerPrefs.SetFloat("Sensitivity", sensitivity);

    PlayerPrefs.Save();
  }

  private void SetNormalDiff()
  {
    timeBetweenWaves = 10f;
    zombiesPerWave = 4;
  }

  private void SetHardDiff()
  {
    timeBetweenWaves = 8f;
    zombiesPerWave = 6;
  }

  private void SelectMap(string mapName)
  {
    selectedMap = mapName;

    PlayerPrefs.SetString("SelectedMap", selectedMap);
    LoadSelectedMap();
  }

  public void LoadSelectedMap()
  {
    SceneManager.LoadScene(selectedMap);
  }
}
