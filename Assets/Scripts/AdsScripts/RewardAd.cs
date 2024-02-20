using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms
    UnityAdsInit unityAdsInit;

    public event Action OnAdComplete;


    void Start()
    {
        _adUnitId = _iOSAdUnitId;
        unityAdsInit = gameObject.GetComponent<UnityAdsInit>();
        //_showAdButton.interactable = false;
    }

    // Call this public method when you want to get an ad ready to show.
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        if (Advertisement.isInitialized)
        {
            Debug.Log("Loading Ad: " + _adUnitId);
            try
            {
                _showAdButton.interactable = false;
                Advertisement.Load(_adUnitId, this);
            }
            catch
            {
                Debug.Log("Failed Loading Ad");
                _showAdButton.interactable = true;
            }
        }
        else
        {
            unityAdsInit.InitializeAds();
        }
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            Advertisement.Show(_adUnitId, this);
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        try
        {
            Advertisement.Show(_adUnitId, this);
        } catch
        {
            _showAdButton.interactable = true;
        }
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            _showAdButton.interactable = true;
            OnAdComplete?.Invoke();

        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        _showAdButton.interactable = true;

        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        _showAdButton.interactable = true;
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }
}