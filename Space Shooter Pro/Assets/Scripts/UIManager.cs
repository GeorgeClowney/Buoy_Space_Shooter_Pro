using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _score, _gameOver, _restart, _ammo, _shield, _home;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Slider _energySlider;
    private GameManager _gm;
    // Start is called before the first frame update
    void Start()
    {
        _gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gm == null)
        {
            Debug.LogError("Game Manager is Null");
        }
        _shield.gameObject.SetActive(false);
        _gameOver.gameObject.SetActive(false);
        _restart.gameObject.SetActive(false);
        _home.gameObject.SetActive(false);
        _score.text = "Score: 0";
        _ammo.text = "Ammo: 15/15";
    }
    public void SetEnergy(float Energy)
    {
        _energySlider.value = Energy;
    }
  public void SetAmmo(int Ammo)
    {
        _ammo.text = "Ammo: " + Ammo + "/15";
    }
   public void UpdateScore(int playerScore)
    {
        _score.text = "Score: " + playerScore;
    }
    public void UpdateShield(int shields)
    {
        _shield.gameObject.SetActive(true);
        _shield.text = "Shield Strength: " + shields;
        if(shields <= 0)
        {
            _shield.gameObject.SetActive(false);
        }
    }
    public void UpdateHomingShots(int homingShots)
    {
        _home.gameObject.SetActive(true);
        _home.text = "Homing Shots: " + homingShots;
        if (homingShots <= 0)
        {
            _home.gameObject.SetActive(false);
        }
    }
    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];
        if(currentLives <= 0)
        {
            currentLives = 0;
        }
        if(currentLives <= 0)
        {
            _gm.GameOver();
            _gameOver.gameObject.SetActive(true);
            _restart.gameObject.SetActive(true);
            StartCoroutine(GameOverFlickerRoutine());
        }
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOver.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOver.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
