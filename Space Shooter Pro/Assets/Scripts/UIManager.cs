using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//UIManager is the first UI script built for the main game
//it stores info like the score and lives
//The script is attached to the Canvas called "UI"
public class UIManager : MonoBehaviour
{
    //All UI elements are set in the inspector
    [SerializeField]
    private Text _score, _gameOver, _restart, _ammo, _shield, _home;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Slider _energySlider;
    //GameManager is needed to quit the game once the player runs out of lives
    private GameManager _gm;
    // Start is called before the first frame update
    void Start()
    {
        _gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gm == null)
        {
            Debug.LogError("Game Manager is Null (UIManager)");
        }

        //Set Text elements to false at the start of the game
        _shield.gameObject.SetActive(false);
        _gameOver.gameObject.SetActive(false);
        _restart.gameObject.SetActive(false);
        _home.gameObject.SetActive(false);
        _score.text = "Score: 0";
        _ammo.text = "Ammo: 15/15";
    }
    //EnergySlider is the bar in the bottom left of the screen
    //It shows the player how much thurster energy they currently have
    public void SetEnergy(float Energy)
    {
        _energySlider.value = Energy;
    }
    public void UpdateScore(int playerScore)
    {
        _score.text = "Score: " + playerScore;
    }
    public void SetAmmo(int Ammo)
    {
        _ammo.text = "Ammo: " + Ammo + "/15";
    }
    //Shields are turned active once the player picks up the shield powerup
    //The text is set back to false once the player runs out of shields
    public void UpdateShield(int shields)
    {
        _shield.gameObject.SetActive(true);
        _shield.text = "Shield Strength: " + shields;
        if(shields <= 0)
        {
            _shield.gameObject.SetActive(false);
        }
    }
    //HomingShots text is turned active once the player picks up the homingshot powerup
    //The text is set back to false once the player runs out of homingshots
    public void UpdateHomingShots(int homingShots)
    {
        _home.gameObject.SetActive(true);
        _home.text = "Homing Shots: " + homingShots;
        if (homingShots <= 0)
        {
            _home.gameObject.SetActive(false);
        }
    }
    //Updates the Lives image at the top left of the screen
    //This is also where the game over function in the Game Manager script is called once the players lives is <= 0
    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];
        if(currentLives <= 0)
        {
            currentLives = 0;
        }
        if(currentLives <= 0)
        {
            _gm.isGameOver = true;
            _gameOver.gameObject.SetActive(true);
            _restart.gameObject.SetActive(true);
            StartCoroutine(GameOverFlickerRoutine());
        }
    }
    //GameOverFlickerRoutine makes the _gameOver text flicker on and off
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
