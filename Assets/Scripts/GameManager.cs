using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  public int highScore;
  public int currentScore;
  public Text highScoreText;
  public Text currentScoreText; 
  public GameObject[] weapons;

  [SerializeField] private int currentWeaponIndex = 0;

  void Start()
  {
    instance = this;
    SwitchWeapon(currentWeaponIndex);
  }

  void Update()
  {
    if (currentScore > highScore)
    {
      highScore = currentScore;
    }

    highScoreText.text = highScore.ToString();
    currentScoreText.text = currentScore.ToString();

    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      SwitchWeapon(0);
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      SwitchWeapon(1);
    }
    else if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      SwitchWeapon(2);
    }
    else if (Input.GetKeyDown(KeyCode.Alpha4))
    {
      SwitchWeapon(3);
    }
  }

  void SwitchWeapon(int newIndex)
  {
    weapons[currentWeaponIndex].SetActive(false);
    weapons[newIndex].SetActive(true);
    currentWeaponIndex = newIndex;
  }
}
