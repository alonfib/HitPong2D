using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAds : MonoBehaviour
{
    public Interstitial interstitial;
    public RewardAd rewardAd;
    public UnityAdsInit adsInitializer;
    public float adsMaxPoints;
    public readonly float winAdPoints = 3;
    public readonly float looseAdPoints = 2;

    private LogicManager logic;
    private bool isPrizeAd = false;

    private float adsPoints = 0;
    private const string AdsCounter = "AdsCounter";

    void Start()
    {
        adsPoints = PlayerPrefs.GetInt(AdsCounter, 0); // Default to 0 if not set
        logic = gameObject.GetComponent<LogicManager>();
        interstitial.OnAdComplete += AdCompleteHandler;
        rewardAd.OnAdComplete += AdCompleteHandler;
        LoadInterstitial();
    }

    public void WatchAdToNextLevel()
    {
        isPrizeAd = true;
        rewardAd.LoadAd();
    }
        
    private void AdCompleteHandler()
    {
        interstitial.LoadAd();
        adsPoints = 0;
        if (isPrizeAd)
        {
            logic.AdIncreaseProgression();
            isPrizeAd = false;
        }
    }

    public void HandleInGameAds(float points)
    {
        adsPoints = adsPoints + points;
        if(adsPoints > adsMaxPoints && Advertisement.isInitialized)
        {
            ShowInterstitial();
            adsPoints = 0;
        }
        PlayerPrefs.SetFloat(AdsCounter, adsPoints); // Save progression
        PlayerPrefs.Save(); // Ensure it's written to disk
    }

    public void LoadInterstitial()
    {
        interstitial.LoadAd();
    }

    public void ShowInterstitial()
    {
        interstitial.ShowAd();
    }
}
