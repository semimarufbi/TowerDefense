using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    // Singleton para acesso global ao AdManager
    public static AdManager instance;

    // ID do projeto Unity Ads
    private string gameId = "5730170";

    // Define se o modo de teste está ativado
    private bool testMode = true;

    // IDs para diferentes tipos de anúncios no painel do Unity Ads
    private string interstitialAdId = "Interstitial_Android"; // Anúncio intersticial
    private string bannerAdId = "Banner_Android"; // Anúncio em formato de banner
    private string rewardedAdId = "Rewarded_Android"; // Anúncio recompensado
    private string naoPulavelId = "noskip"; // Anúncio não pulável

    // Alterna entre intersticial e anúncio não pulável
    private bool showInterstitialNext = true;

    // Coroutine para controlar a exibição do loop de banners
    private Coroutine bannerLoopCoroutine;

    // Controla se um anúncio intersticial está sendo exibido
    private bool isShowingInterstitial = false;

    // Indica se o jogo está pausado devido à exibição de um anúncio
    public bool isGamePausedByAd = false;

    // Delegate para vincular ações de recompensa
    public delegate void gifts();
    private gifts recompensa;

    // Inicializa o singleton
    private void Awake()
    {
        instance = this;
    }

    // Método chamado ao iniciar o script, inicializa os anúncios
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

    // Callback chamado quando a inicialização do Unity Ads é concluída
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");

        // Inicia o loop de banners após a inicialização
        bannerLoopCoroutine = StartCoroutine(BannerLoop());
    }

    // Callback chamado quando a inicialização do Unity Ads falha
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }

    // Coroutine para exibir banners em loop
    private IEnumerator BannerLoop()
    {
        while (true)
        {
            if (!isShowingInterstitial) // Exibe o banner somente se o intersticial não estiver ativo
            {
                ShowBannerAd();
                yield return new WaitForSeconds(10); // Exibe o banner por 10 segundos
                HideBannerAd(); // Oculta o banner
            }

            yield return new WaitForSeconds(5); // Espera 5 segundos antes de exibir novamente
        }
    }

    // Carrega e exibe um anúncio de banner
    private void ShowBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Load(bannerAdId, new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded, // Callback quando o banner é carregado com sucesso
            errorCallback = OnBannerError // Callback quando ocorre um erro ao carregar o banner
        });
    }

    // Callback chamado quando o banner é carregado
    private void OnBannerLoaded()
    {
        Advertisement.Banner.Show(bannerAdId);
        Debug.Log("Banner ad loaded and displayed.");
    }

    // Callback chamado quando há um erro ao carregar o banner
    private void OnBannerError(string message)
    {
        Debug.LogError($"Failed to load banner ad: {message}");
    }

    // Oculta o banner atualmente exibido
    private void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    // Exibe um anúncio intersticial
    public void ShowInterstitialAd()
    {
        if (!isGamePausedByAd)
        {
            Time.timeScale = 0; // Pausa o jogo
            isGamePausedByAd = true;
        }
        Advertisement.Show(interstitialAdId, this); // Exibe o anúncio intersticial
        isShowingInterstitial = true;
    }

    // Exibe um anúncio não pulável
    private void Naopulavel()
    {
        if (!isGamePausedByAd)
        {
            Time.timeScale = 0; // Pausa o jogo
            isGamePausedByAd = true;
        }
        Advertisement.Show(naoPulavelId, this); // Exibe o anúncio não pulável
        isShowingInterstitial = true;
    }

    // Alterna entre anúncios intersticiais e não puláveis
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

        showInterstitialNext = !showInterstitialNext; // Alterna para o próximo tipo de anúncio
    }

    // Exibe um anúncio recompensado e define a ação a ser executada após ele
    public void ShowRewardedAdForAction(gifts action)
    {
        recompensa = action;
        ShowRewardedAd();
    }

    // Exibe um anúncio recompensado
    public void ShowRewardedAd()
    {
        if (!isGamePausedByAd)
        {
            Time.timeScale = 0; // Pausa o jogo
            isGamePausedByAd = true;
        }
        Advertisement.Show(rewardedAdId, this);
    }

    // Callback chamado ao finalizar a exibição de um anúncio
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            if (placementId == rewardedAdId)
            {
                recompensa?.Invoke(); // Executa a ação de recompensa configurada
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

    // Callback chamado se a exibição do anúncio falhar
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Unity Ads failed to show: {placementId}, Error: {error}, Message: {message}");
    }

    // Callback chamado quando um anúncio começa a ser exibido
    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Unity Ads started showing: {placementId}");
        Advertisement.Banner.Hide(); // Esconde o banner durante o anúncio
    }

    // Callback chamado quando o anúncio é clicado
    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Unity Ads clicked: {placementId}");
    }

    // Ação para reviver o jogador
    public void Reviver()
    {
        if (LevelManager.main != null)
        {
            LevelManager.main.Reiniciar();
        }
    }

    // Ação para recompensar o jogador
    public void Recompensa()
    {
        if (LevelManager.main != null)
        {
            LevelManager.main.RewardCurrency();
        }
    }

    // Vincula o botão para exibir um anúncio recompensado com a ação de recompensa
    public void BotaoRecompensa()
    {
        ShowRewardedAdForAction(Recompensa);
    }

    // Vincula o botão para exibir um anúncio recompensado com a ação de reviver
    public void BotaoReviver()
    {
        ShowRewardedAdForAction(Reviver);
    }
}
