using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    private string gameId = "5730170"; // Seu ID do projeto
    private bool testMode = true;
    private string interstitialAdId = "Interstitial_Android";
    private string bannerAdId = "Banner_Android"; // Verifique se este � o ID correto no painel do Unity Ads
     delegate void gifts();
    gifts recompensa;
    gifts reviver;
    private Coroutine bannerLoopCoroutine; // Armazena a coroutine do BannerLoop
    private bool isShowingInterstitial = false; // Indica se o intersticial est� sendo exibido
    private System.Action adCompletedAction; // Armazena a a��o a ser executada ap�s o an�ncio
    private string rewardedAdId = "Rewarded_Android"; // ID do an�ncio recompensado



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
      
        bannerLoopCoroutine = StartCoroutine(BannerLoop()); // Inicia o loop de exibi��o do banner ap�s a inicializa��o

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

    void Recompensa()
    {
        if (LevelManager.main != null)
        {
            LevelManager.main.RewardCurrency();
        }
        else
        {
            Debug.LogError("LevelManager.main is null. Cannot reward currency.");
        }
    }
    public void ShowRewardedAd(System.Action actionAfterAd)
    {
        adCompletedAction = actionAfterAd; // Armazena a a��o que ser� executada ap�s o an�ncio
        Advertisement.Show(rewardedAdId, this); // Exibe o an�ncio recompensado
    }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == rewardedAdId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            adCompletedAction?.Invoke(); // Executa a a��o armazenada (neste caso, Reviver ou Recompensa)
        }
    }


    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Unity Ads failed to show: {placementId}, Error: {error}, Message: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Unity Ads started showing: {placementId}");
        Advertisement.Banner.Hide();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Unity Ads clicked: {placementId}");
        Advertisement.Banner.Show(bannerAdId);
    }
   public void Reviver()
    {
        LevelManager.main.Reiniciar();
    }
    public void BotaoRecompensa()
    {
        ShowRewardedAd(Recompensa); // Mostra o an�ncio e depois d� a recompensa
    }

    public void BotaoReviver()
    {
        ShowRewardedAd(Reviver); // Passa o m�todo Reviver para ser executado ap�s o an�ncio
    }
}




