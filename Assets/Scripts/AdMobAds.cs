
using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine.UI;
using System;

public class AdMobAds : MonoBehaviour
{
    public static AdMobAds InstanceAds;
    //public string BannerID, interId;
    private void Awake()
    {
        if (InstanceAds != null)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceAds = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public string appId = "ca-app-pub-9964737277199132~7993647222";// real appId from AdMob

#if UNITY_ANDROID 
    string bannerId = "ca-app-pub-3940256099942544/6300978111";
    //string bannerId = "ca-app-pub-9964737277199132/9905510605";//real bannerId from AdMob
    string interId = "ca-app-pub-3940256099942544/1033173712";
    string rewardedId = "ca-app-pub-3940256099942544/5224354917";
    string nativeId = "ca-app-pub-3940256099942544/2247696110";
#elif UNITY_IPHONE
    string bannerId = "ca-app-pub-3940256099942544/2934735716";
    string interId = "ca-app-pub-3940256099942544/4411468910";
    string rewardedId = "ca-app-pub-3940256099942544/1712485313";
    string nativeId = "ca-app-pub-3940256099942544/3986624511";
#endif 
    BannerView bannerView;
    public InterstitialAd interAd;
    RewardedAd rewardAd;

    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus =>
        {
            print("Ads Initialize !!");

        });
        //LoadBannerAd();
        LoadIntersAd();
        LoadRewardedAd();
    }

    #region BannerAd
    public void LoadBannerAd()
    {
        //Create a banner
        CreateBannerView();

        //listen to banner events
        ListenToBannerEvents();

        //load the Banner
        if (bannerView == null)
        {
            CreateBannerView();
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("Loading Banner Ad");
        bannerView.LoadAd(adRequest);
    }
    void CreateBannerView()
    {
        if (bannerView != null)
        {
            DestroyBannerAd();
        }
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);
    }

    void ListenToBannerEvents()
    {
        // Raised when an ad is loaded into the banner view.
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            LoadBannerAd();
            Debug.LogError("Banner view failed to load an ad with error : " + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
         {
             Debug.Log("Banner view recorded an impression.");
         };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            print("Destroying Banner Ad");
            bannerView.Destroy();
            bannerView = null;
        }
    }
    #endregion

    #region InterstitialAd
    public void LoadIntersAd()
    {
        if (interAd != null)
        {
            interAd.Destroy();
            interAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("Loading inters Ad");
        InterstitialAd.Load(interId, adRequest, (InterstitialAd ad, LoadAdError error) =>
          {
              if (error != null || ad == null)
              {
                  print("Interstitial Ad Failed to Load" + error);
                  return;
              }
              print("Interstital Ad Loaded!!" + ad.GetResponseInfo());
              interAd = ad;
              InterstitialEvents(interAd);
          });
    }

    public void ShowInterstitialAd()
    {
        if (interAd != null && interAd.CanShowAd())
        {
            interAd.Show();
        }
        else
        {
            print("Interstitial Ad Not Ready!!");
        }
    }

    public void InterstitialEvents(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            LoadIntersAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
    #endregion

    #region RewardedAd
    public void LoadRewardedAd()
    {
        if (rewardAd != null)
        {
            rewardAd.Destroy();
            rewardAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("Loading rewarded Ad");
        RewardedAd.Load(interId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Rewarded Ad Failed to Load" + error);
                return;
            }
            print("Rewarded Ad Loaded!!" + ad.GetResponseInfo());
            rewardAd = ad;
            RewardedAdEvents(rewardAd);
        });
    }

    public void ShowRewardedAd()
    {
        if (rewardAd != null && rewardAd.CanShowAd())
        {
            rewardAd.Show((Reward reward) =>
            {
                print("Given Reward To Player!!");
                int temp = 100;
                temp = temp + PlayerPrefs.GetInt("score");
                PlayerPrefs.SetInt("score", temp);
                MainMenuSc.instanceMainM.ScoreText.text = PlayerPrefs.GetInt("score").ToString();
            });
        }
        else
        {
            print("Rewarded Ad Not Ready!!");
        }
    }

    public void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            LoadRewardedAd();
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion
}
