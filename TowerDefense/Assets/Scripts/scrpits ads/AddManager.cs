using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private string gameId = "5730170"; // Seu ID do projeto
    private bool testMode = true;
    private string interstitialAdId = "Interstitial_Android";
    private string bannerAdId = "Banner_Android"; // Verifique se este é o ID correto no painel do Unity Ads

    private Coroutine bannerLoopCoroutine; // Armazena a coroutine do BannerLoop
    private bool isShowingInterstitial = false; // Indica se o intersticial está sendo exibido

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
        LoadInterstitialAd(); // Carrega o anúncio intersticial quando a inicialização é concluída
        bannerLoopCoroutine = StartCoroutine(BannerLoop()); // Inicia o loop de exibição do banner após a inicialização
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }

    private IEnumerator BannerLoop()
    {
        while (true)
        {
            if (!isShowingInterstitial) // Só exibe o banner se o intersticial não estiver ativo
            {
                ShowBannerAd();
                yield return new WaitForSeconds(10); // Exibe o banner por 10 segundos
                HideBannerAd(); // Oculta o banner
            }

            yield return new WaitForSeconds(5); // Espera 5 segundos antes de exibir novamente
        }
    }

    private void ShowBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Load(bannerAdId, new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        });
    }

    private void OnBannerLoaded()
    {
        Advertisement.Banner.Show(bannerAdId);
        Debug.Log("Banner ad loaded and displayed.");
    }

    private void OnBannerError(string message)
    {
        Debug.LogError($"Failed to load banner ad: {message}");
    }

    private void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    // Método chamado ao clicar no botão de recompensa
    public void OnRewardButtonClick()
    {
        Debug.Log("Botão clicado! Tentando mostrar o anúncio...");
        ShowInterstitialAd();
    }

    // Método para mostrar o anúncio intersticial
    public void ShowInterstitialAd()
    {
        Debug.Log("Verificando se o anúncio está pronto...");

        isShowingInterstitial = true; // Marca que o intersticial está sendo exibido

        // Tente exibir o anúncio
        Advertisement.Show(interstitialAdId, this);

        // Para garantir que o banner seja ocultado enquanto o intersticial está ativo
        HideBannerAd();
    }

    public void OnUnityAdsShowStart(string adUnitId) { }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId == interstitialAdId)
        {
            Debug.Log("Anúncio intersticial completo.");
            isShowingInterstitial = false; // Marca que o intersticial foi fechado
            LoadInterstitialAd(); // Carrega o próximo anúncio intersticial

            // Reinicia o loop do banner quando o intersticial termina
            if (bannerLoopCoroutine == null)
            {
                bannerLoopCoroutine = StartCoroutine(BannerLoop());
            }

            // Recompensa o jogador com 100 moedas após o intersticial ser completado
            LevelManager.main.RewardCurrency();
            Debug.Log("Jogador recebeu 100 moedas de recompensa!");
        }
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

    public void OnUnityAdsShowClick(string adUnitId) { }

    // Carrega o anúncio intersticial
    private void LoadInterstitialAd()
    {
        Advertisement.Load(interstitialAdId, this);
    }
}
