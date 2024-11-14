using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private string gameId = "5730170"; // Insira o seu ID do projeto
    private bool testMode = true; // Defina como `false` em produção
    private string interstitialAdId = "Interstitial_Android"; // Certifique-se de que este é o ID correto do anúncio no painel do Unity Ads
    private int playerCoins = 0; // Variável para armazenar as moedas do jogador

    void Start()
    {
        InitializeAds();
    }

    private void InitializeAds()
    {
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        StartCoroutine(BannerLoop());
        LoadInterstitialAd(); // Carregar o intersticial no início
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }

    private IEnumerator BannerLoop()
    {
        while (true)
        {
            ShowBannerAd();
            yield return new WaitForSeconds(10); // Mostrar por 10 segundos
            HideBannerAd();
            yield return new WaitForSeconds(5); // Ocultar por 5 segundos
        }
    }

    private void ShowBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Show("Banner_Android"); // Certifique-se de que "Banner_Android" é o ID correto do banner no painel do Unity Ads
    }

    private void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    // Interstitial Ad Methods
    private void LoadInterstitialAd()
    {
        Advertisement.Load(interstitialAdId, this);
    }

    public void ShowInterstitialAd()
    {
        Advertisement.Show(interstitialAdId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Error loading ad {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Error showing ad {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        // Recompensa o jogador se o anúncio for exibido completamente
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            RewardPlayer();
            LoadInterstitialAd(); // Carrega o próximo anúncio intersticial
        }
    }

    private void RewardPlayer()
    {
        LevelManager.main.RewardCurrency(); // Adiciona 100 moedas
        Debug.Log("Player rewarded with 100 coins. Total coins: " + LevelManager.main.currency);
    }

    // Método para chamar pelo botão
    public void OnRewardButtonClick()
    {
        ShowInterstitialAd();
    }
}