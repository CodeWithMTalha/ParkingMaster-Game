using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSc : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvas;
    public Text ScoreText;
    public Text MusicText;

    [Header("CarSelection")]
    public GameObject carSelection;
    public GameObject levelSelection;
    public GameObject[] carList;
    public CarModelCls[] cars;
    public Button playCarBtn;
    public Button BuyBtn;
    public Text BuyCoinsText;

    [Header("CarRotate")]
    public GameObject toRotate;
    public GameObject playerCar;
    public float RotateSpeed;

    SplashManager splashObj;
    public static MainMenuSc instanceMainM;
    // Start is called before the first frame update
    void Start()
    {
        instanceMainM = this;

        AdMobAds.InstanceAds.LoadBannerAd();

        foreach (CarModelCls car in cars)
        {
            if (car.price == 0)
            {
                car.isUnlocked = true;
            }
            else
            {
                car.isUnlocked = PlayerPrefs.GetInt(car.name, 0) == 0 ? false : true;
            }
        }
        //splashObj = GameObject.FindGameObjectWithTag("splashcontroll").GetComponent<SplashManager>();
        levelUnlockCheck();
        //splashObj = new SplashManager();
        splashObj = SplashManager.InstanceSplash;

        

    }

    private void Update()
    {
        UpdateUI();
    }

    private void FixedUpdate()
    {
        playerCar = GameObject.FindGameObjectWithTag("Player");
        toRotate.transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);
        playerCar.transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);

    }


    #region MainMenu
    //public bool ismusic = true;

    public void MusicPlay() //it attach with music btn
    {
        splashObj.musicPlay();
    }

    public void ShowIntersAds() ////it attach with different btn
    {
        AdMobAds.InstanceAds.ShowInterstitialAd();
    }
    public void ShowRewardedAds() ////it attach with different btn
    {
        AdMobAds.InstanceAds.ShowRewardedAd();
    }

    public void PlayMain() //it attach with play btn
    {
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
    }

    public void ExitFun() //it attach with Exit btn
    {
        Application.Quit();
    }

    public void RateUs(string rateLink) //it attach with Rate Us btn
    {
        Application.OpenURL(rateLink);
    }

    public void MoreGame(string moreGLink) //it attach with More Game btn
    {
        Application.OpenURL(moreGLink);
    }

    #endregion

    #region CarSelection

    int carCounterIndex;
    
    public void NextCar() //it attach with Next(>) btn
    {
        if(carCounterIndex != carList.Length - 1)
        {
            carCounterIndex++;
        }
        else
        {
            carCounterIndex = 0;
        }
        foreach(var item in carList)
        {
            item.SetActive(false);
        }
        carList[carCounterIndex].SetActive(true);

        CarModelCls c = cars[carCounterIndex];
        if (!c.isUnlocked)
            return;

    }

    public void PreviousCar() //it attach with pevious(<) btn
    {
        if (carCounterIndex != 0)
        {
            carCounterIndex--;
        }
        else
        {
            carCounterIndex = carList.Length - 1;
        }
        foreach (var item in carList)
        {
            item.SetActive(false);
        }
        carList[carCounterIndex].SetActive(true);

        CarModelCls c = cars[carCounterIndex];
        if (!c.isUnlocked)
            return;
    }

    public void CarUnlock() //it attach with BuyCar btn
    {
        CarModelCls c = cars[carCounterIndex];

        PlayerPrefs.SetInt(c.name, 1);
        PlayerPrefs.SetInt("selectcar", carCounterIndex);
        c.isUnlocked = true;
        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score", 0) - c.price);
    }

    private void UpdateUI() //it handle car perchasing update
    {
        ScoreText.text = "" + PlayerPrefs.GetInt("score");

        CarModelCls c = cars[carCounterIndex];
        if (c.isUnlocked)
        {
            BuyBtn.gameObject.SetActive(false);
            playCarBtn.gameObject.SetActive(true);
        }
        else
        {
            BuyBtn.gameObject.SetActive(true);
            playCarBtn.gameObject.SetActive(false);
            BuyCoinsText.text = "Buy: " + c.price;
            if (c.price < PlayerPrefs.GetInt("score"))
            {
                BuyBtn.interactable = true;
            }
            else
            {
                BuyBtn.interactable = false;
            }
        }
    }

    public void BackToMain() //when it move to Main from Car Selection
    {
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
    }

    public void PlayToLevelSelection() //when it move to LevelSelection From CarSelection 
    {
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        PlayerPrefs.SetInt("selectcar", carCounterIndex);
        //print(PlayerPrefs.GetInt("selectcar"));
    }
    #endregion

    #region LevelSelection
    [Header("Loading panel")]
    public GameObject LoadingPanel;
    public GameObject[] levelBtns;
    public void BackToCarSelection() //when it move to CarSelection from LevelSelection
    {
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        carList[carCounterIndex].SetActive(true);
    }

    public void LevelSelection(int levelNo)//it attach with levels btn
    {
        PlayerPrefs.SetInt("levelnum", levelNo);
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene("GamePlay");
    }

    public void levelUnlockCheck()
    {
        for(int i = 0; i < PlayerPrefs.GetInt("levelcomplete"); i++)
        {
            levelBtns[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    #endregion
}
