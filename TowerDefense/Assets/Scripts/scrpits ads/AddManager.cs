using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AddManager : MonoBehaviour,IUnityAdsInitializationListener
{
    private string gameid = "5730170";
    private bool testmode = true;


    void Start()
    {
        InitializeAds();
    }
    private void InitializeAds()
    {
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(gameid, testmode, this);
        }
    }
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        
    }
    
}
