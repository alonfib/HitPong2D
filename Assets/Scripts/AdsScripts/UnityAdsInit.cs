using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsInit: MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode;
    private string _gameId = "";

    void Start()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        // Check the running platform
        _gameId = _iOSGameId;
        if (Application.platform == RuntimePlatform.Android)
        {
            _gameId = _androidGameId;
        }
        else
        {
            Debug.LogWarning("UnityAdsInit - Unsupported platform for Unity Ads");
            //return;
        }

        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(_iOSGameId, _testMode, this);
        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
