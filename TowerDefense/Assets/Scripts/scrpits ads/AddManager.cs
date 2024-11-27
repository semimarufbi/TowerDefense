using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    // Singleton para acesso global ao AdManager
    public static AdManager instance;

    // ID do projeto Unity Ads
    private string gameId = "5730170";

    // Define se o modo de teste est� ativado
    private bool testMode = true;

    // IDs para diferentes tipos de an�ncios no painel do Unity Ads
    private string interstitialAdId = "Interstitial_Android"; // An�ncio intersticial
    private string bannerAdId = "Banner_Android"; // An�ncio em formato de banner
    private string rewardedAdId = "Rewarded_Android"; // An�ncio recompensado
    private string naoPulavelId = "noskip"; // An�ncio n�o pul�vel

    // Alterna entre intersticial e an�ncio n�o pul�vel
    private bool showInterstitialNext = true;

    // Coroutine para controlar a exibi��o do loop de banners
    private Coroutine bannerLoopCoroutine;

    // Controla se um an�ncio intersticial est� sendo exibido
    private bool isShowingInterstitial = false;

    // Indica se o jogo est� pausado devido � exibi��o de um an�ncio
    public bool isGamePausedByAd = false;

    // Delegate para vincular a��es de recompensa
    public delegate void gifts();
    private gifts recompensa;

    // Inicializa o singleton
    private void Awake()
    {
        instance = this;
    }

    // M�todo chamado ao iniciar o script, inicializa os an�ncios
    void Start()
    {
        InitializeAds();
    }

    // Inicializa o Unity Ads
    private void InitializeAds()
    {
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    // Callback chamado quando a inicializa��o do Unity Ads � conclu�da
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");

        // Inicia o loop de banners ap�s a inicializa��o
        bannerLoopCoroutine = StartCoroutine(BannerLoop());
    }

    // Callback chamado quando a inicializa��o do Unity Ads falha
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }

    // Coroutine para exibir banners em loop
    private IEnumerator BannerLoop()
    {
        while (true)
        {
            if (!isShowingInterstitial) // Exibe o banner somente se o intersticial n�o estiver ativo
            {
                ShowBannerAd();
                yield return new WaitForSeconds(10); // Exibe o banner por 10 segundos
                HideBannerAd(); // Oculta o banner
            }

            yield return new WaitForSeconds(5); // Espera 5 segundos antes de exibir novamente
        }
    }

    // Carrega e exibe um an�ncio de banner
    private void ShowBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Load(bannerAdId, new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded, // Callback quando o banner � carregado com sucesso
            errorCallback = OnBannerError // Callback quando ocorre um erro ao carregar o banner
        });
    }

    // Callback chamado quando o banner � carregado
    private void OnBannerLoaded()
    {
        Advertisement.Banner.Show(bannerAdId);
        Debug.Log("Banner ad loaded and displayed.");
    }

    // Callback chamado quando h� um erro ao carregar o banner
    private void OnBannerError(string message)
    {
        Debug.LogError($"Failed to load banner ad: {message}");
    }

    // Oculta o banner atualmente exibido
    private void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    // Exibe um an�ncio intersticial
    public void ShowInterstitialAd()
    {
        if (!isGamePausedByAd)
        {
            Time.timeScale = 0; // Pausa o jogo
            isGamePausedByAd = true;
        }
        Advertisement.Show(interstitialAdId, this); // Exibe o an�ncio intersticial
        isShowingInterstitial = true;
    }

    // Exibe um an�ncio n�o pul�vel
    private void Naopulavel()
    {
        if (!isGamePausedByAd)
        {
            Time.timeScale = 0; // Pausa o jogo
            isGamePausedByAd = true;
        }
        Advertisement.Show(naoPulavelId, this); // Exibe o an�ncio n�o pul�vel
        isShowingInterstitial = true;
    }

    // Alterna entre an�ncios intersticiais e n�o pul�veis
    public void ShowNextAd()
    {
        if (showInterstitialNext)
        {
            ShowInterstitialAd();
        }
        else
        {
            Naopulavel();
        }

        showInterstitialNext = !showInterstitialNext; // Alterna para o pr�ximo tipo de an�ncio
    }

    // Exibe um an�ncio recompensado e define a a��o a ser executada ap�s ele
    public void ShowRewardedAdForAction(gifts action)
    {
        recompensa = action;
        ShowRewardedAd();
    }

    // Exibe um an�ncio recompensado
    public void ShowRewardedAd()
    {
        if (!isGamePausedByAd)
        {
            Time.timeScale = 0; // Pausa o jogo
            isGamePausedByAd = true;
        }
        Advertisement.Show(rewardedAdId, this);
    }

    // Callback chamado ao finalizar a exibi��o de um an�ncio
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            if (placementId == rewardedAdId)
            {
                recompensa?.Invoke(); // Executa a a��o de recompensa configurada
            }
            else if (placementId == interstitialAdId)
            {
                isShowingInterstitial = false; // Intersticial finalizado
            }
        }

        if (isGamePausedByAd)
        {
            Time.timeScale = 1; // Retoma o jogo
            isGamePausedByAd = false;
        }
    }

    // Callback chamado se a exibi��o do an�ncio falhar
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Unity Ads failed to show: {placementId}, Error: {error}, Message: {message}");
    }

    // Callback chamado quando um an�ncio come�a a ser exibido
    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Unity Ads started showing: {placementId}");
        Advertisement.Banner.Hide(); // Esconde o banner durante o an�ncio
    }

    // Callback chamado quando o an�ncio � clicado
    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Unity Ads clicked: {placementId}");
    }

    // A��o para reviver o jogador
    public void Reviver()
    {
        if (LevelManager.main != null)
        {
            LevelManager.main.Reiniciar();
        }
    }

    // A��o para recompensar o jogador
    public void Recompensa()
    {
        if (LevelManager.main != null)
        {
            LevelManager.main.RewardCurrency();
        }
    }

    // Vincula o bot�o para exibir um an�ncio recompensado com a a��o de recompensa
    public void BotaoRecompensa()
    {
        ShowRewardedAdForAction(Recompensa);
    }

    // Vincula o bot�o para exibir um an�ncio recompensado com a a��o de reviver
    public void BotaoReviver()
    {
        ShowRewardedAdForAction(Reviver);
    }
}
