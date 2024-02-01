using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] TMP_Text ammoTxt;
    [SerializeField] TMP_Text statsTxt;
    [SerializeField] GameObject optionPanel;
    [SerializeField] public GameObject failedPanel;
    [SerializeField] public GameObject completePanel;
    [SerializeField] GameObject[] otherOptions;
    [SerializeField] TMP_Text timeTxt;
    [SerializeField] SoundController soundController;
    [SerializeField] Boss boss;
    [SerializeField] Image bossHpBar;
    [SerializeField] GameObject bossPanel;

    public static UI Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(Instance); }
    }

    public enum State { Play,Stop };
    public State state;
    float timer;
    private void Update()
    {
        if(state == State.Play)
        {
            timer += Time.deltaTime;
            System.TimeSpan ts = System.TimeSpan.FromSeconds(timer);
            timeTxt.text = string.Format("you tooked "+" {0:00}:{1:00}:{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds+" to finish the game");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!optionPanel.activeSelf) 
            {
                optionPanel.SetActive(true);
                state = State.Stop;
            }
            else
            {
                foreach (GameObject go in otherOptions)
                {
                    if (go.activeSelf)
                    {
                        go.SetActive(false);
                        soundController.SaveVolumeBttn();
                        return;
                    }
                }
                optionPanel.SetActive(false);
                state = State.Play;
            }
        }
        if (bossHpBar.fillAmount != boss.HP / boss.maxHp)
        {
            bossHpBar.fillAmount = 
            Mathf.Lerp(bossHpBar.fillAmount, boss.HP / boss.maxHp, Time.deltaTime * 5);
        }
    }
    public void SetAmmo(int ammo, int maxAmmo)
    {
       ammoTxt.text = $"<color=green>{ammo}/{maxAmmo}</color>";
    }
    public void SetHP(float HP, float Shield)
    {
        statsTxt.text = $"<color=green>{HP}</color> / <color=blue>{Shield}</color>";
    }
    public void OnClickMain()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void OnclickRe()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
