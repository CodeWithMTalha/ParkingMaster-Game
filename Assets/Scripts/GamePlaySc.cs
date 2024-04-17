using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlaySc : MonoBehaviour
{
    public GameObject[] Cars;
    public GameObject[] PakingLevels;
    GameObject currentLevel;
    GameObject currentCar;

    public static GamePlaySc InstanceGamePlay;

    private void Awake()
    {
        AdMobAds.InstanceAds.LoadBannerAd();
    }

    // Start is called before the first frame update
    void Start()
    {
        InstanceGamePlay = this;

        currentLevel = PakingLevels[PlayerPrefs.GetInt("levelnum") - 1];
        currentLevel.SetActive(true);

        currentCar = Instantiate(Cars[PlayerPrefs.GetInt("selectcar")]);
        currentCar.transform.position = currentLevel.transform.GetChild(0).transform.position;
        currentCar.transform.rotation = currentLevel.transform.GetChild(0).transform.localRotation;
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Pause Panel

    public void PauseGame()// Pause btn function
    {
        Time.timeScale = 0f;
    }
    public void ContinueGame()// Continue btn function
    {
        Time.timeScale = 1f;
    }
    public void HomeMenu() // Home btn function
    {
       
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
    public void RestartGame()// Restart btn function
    {
        
        SceneManager.LoadScene(2);
        Time.timeScale = 1f;
    }
    #endregion

    #region Complete Panel

    [Header("Complete Panel")]
    public GameObject completePanel;
    public GameObject bgShade;
    public bool is_Next = false;

    public void LevelComplete()
    {
        completePanel.SetActive(true);
        bgShade.SetActive(true);
        LevelUnlock();
        ScoreCalculate();
        Time.timeScale = 0f;
    }

    public void LevelUnlock()// it save next level value for unlock next level on complete
    {
        if(PlayerPrefs.GetInt("levelnum") >= PlayerPrefs.GetInt("levelcomplete"))
        {
            PlayerPrefs.SetInt("levelcomplete",PlayerPrefs.GetInt("levelnum"));
         
            //currentLevel = PakingLevels[PlayerPrefs.GetInt("levelcomplete")+1];
        }
    }

    public void ScoreCalculate()// it calculate coins 
    {
        int temp = 1000;
        temp = temp + PlayerPrefs.GetInt("score");
        PlayerPrefs.SetInt("score", temp);
    }

    public void ShowIntersAdsGP() ////it attach with different btn
    {
        
        AdMobAds.InstanceAds.ShowInterstitialAd();
        Invoke("NextLevel", 0.2f);
       
    }

    public void NextLevel()// it move to next level
    {

        // if (PlayerPrefs.GetInt("levelnum") >= PlayerPrefs.GetInt("levelcomplete"))
        //{
       
        currentLevel.SetActive(false);
        int i = PlayerPrefs.GetInt("levelnum");
        i += 1;
        //Debug.LogError(i);
        currentLevel = PakingLevels[i];
        currentLevel.SetActive(true);
        PlayerPrefs.SetInt("levelnum",i);
        PlayerPrefs.Save();
        SceneManager.LoadScene(2);
        //PlayerPrefs.SetInt("levelcomplete", currentLevel;

        Time.timeScale = 1f;
            currentCar.transform.position = currentLevel.transform.GetChild(0).transform.position;
            currentCar.transform.rotation = currentLevel.transform.GetChild(0).transform.localRotation;
        // }
    }

    #endregion

    #region Fail Panel
    [Header("Fail Panel")]
    public GameObject FailPanel;
    public void LevelFail()//it Handle the fail panel
    {
        FailPanel.SetActive(true);
        bgShade.SetActive(true);
        //LevelUnlock();
        Time.timeScale = 0f;
    }

    #endregion
}
