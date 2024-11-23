using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    public static AdManager instance;
    private string gameId = "5730170"; // Seu ID do projeto
    private bool testMode = true;
    private string interstitialAdId = "Interstitial_Android";
    private string bannerAdId = "Banner_Android"; // Verifique se este � o ID correto no painel do Unity Ads
    private string rewardedAdId = "Rewarded_Android"; // ID do an�ncio recompensado
    private string naoPulavelId = "noskip"; // ID do an�ncio n�o pul�vel
    private bool showInterstitialNext = true; // Controla qual tipo de an�ncio ser� mostrado: intersticial ou n�o pulavel

    private Coroutine bannerLoopCoroutine; // Armazena a coroutine do BannerLoop
    private bool isShowingInterstitial = false; // Indica se o intersticial est� sendo exibido
    private System.Action adCompletedAction; // A��o ap�s a exibi��o do an�ncio

    private delegate void gifts();
    private gifts recompensa;
    private gifts reviver;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitializeAds();
        recompensa = Recompensa;
        reviver = Reviver;
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

        // Inicia o loop de banners ap�s a inicializa��o
        bannerLoopCoroutine = StartCoroutine(BannerLoop());
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }

    private IEnumerator BannerLoop()
    {
        while (true)
        {
            if (!isShowingInterstitial) // S� exibe o banner se o intersticial n�o estiver ativo
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

    private void Naopulavel()
    {
        Advertisement.Show(naoPulavelId, this); // Exibe o an�ncio n�o pul�vel
    }

    void Recompensa()
    {
        if (LevelManager.main != null)
        {
            LevelManager.main.RewardCurrency(); // D� a recompensa ao jogador
        }
        else
        {
            Debug.LogError("LevelManager.main is null. Cannot reward currency.");
        }
    }

    public void ShowInterstitialAd()
    {
        Advertisement.Show(interstitialAdId, this); // Exibe o an�ncio intersticial
        isShowingInterstitial = true; // Define que o intersticial est� sendo exibido
        Time.timeScale = 0; // Pausa o tempo durante o an�ncio
    }

    public void ShowRewardedAd(System.Action actionAfterAd)
    {
        adCompletedAction = actionAfterAd; // Armazena a a��o que ser� executada ap�s o an�ncio
        Advertisement.Show(rewardedAdId, this); // Exibe o an�ncio recompensado
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        // Quando um an�ncio recompensado � conclu�do
        if (placementId == rewardedAdId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            adCompletedAction?.Invoke(); // Executa a a��o armazenada (Recompensa ou Reviver)
        }

        // Quando o an�ncio intersticial � conclu�do
        if (placementId == interstitialAdId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            isShowingInterstitial = false; // Define que o intersticial foi fechado
            Time.timeScale = 1; // Restaura o tempo normal
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Unity Ads failed to show: {placementId}, Error: {error}, Message: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Unity Ads started showing: {placementId}");
        Advertisement.Banner.Hide(); // Esconde o banner durante o an�ncio
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Unity Ads clicked: {placementId}");
        Advertisement.Banner.Show(bannerAdId); // Exibe o banner novamente ap�s o clique
    }

    public void Reviver()
    {
        LevelManager.main.Reiniciar(); // A��o de reviver o jogador
    }

    public void BotaoRecompensa()
    {
        ShowRewardedAd(Recompensa); // Mostra o an�ncio e d� a recompensa
    }

    public void BotaoReviver()
    {
        ShowRewardedAd(Reviver); // Passa o m�todo Reviver para ser executado ap�s o an�ncio
    }

    public void ShowNextAd()
    {
        if (showInterstitialNext)
        {
            ShowInterstitialAd();  // Mostra o an�ncio intersticial
        }
        else
        {
            Naopulavel();  // Mostra o an�ncio n�o pul�vel
        }

        // Alterna o tipo de an�ncio para o pr�ximo
        showInterstitialNext = !showInterstitialNext;
    }
}
