using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class Interstitial : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    UnityAdsInit unityAdsInit;
    string _adUnitId = "Interstitial_iOS";

    void Start()
    {
        unityAdsInit = gameObject.GetComponent<UnityAdsInit>();
    }

    public event Action OnAdComplete;

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + _adUnitId);
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowAd()
    {
        if(Advertisement.isInitialized)
        {
            Debug.Log("Showing Ad: " + _adUnitId);
            try
            {
                Advertisement.Show(_adUnitId, this);
            } catch
            {
                LoadAd();
            }
        } else
        {
            unityAdsInit.InitializeAds();
        }
        // Note that if the ad content wasn't previously loaded, this method will fail
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()}  - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) {
        Debug.Log($"OnUnityAdsShowClick {_adUnitId}");
    }


    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        //if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        //{
            // Invoke the ad completion event
            OnAdComplete?.Invoke();
        //}
    }
}