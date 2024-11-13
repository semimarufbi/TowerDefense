using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class AddManager : MonoBehaviour, IUnityAdsInitializationListener
{
    private string gameId = "5730170"; // Insira o seu ID do projeto
    private bool testMode = true; // Defina como `false` em produção

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
        Advertisement.Banner.Show("Banner_Ad"); // Certifique-se de que "Banner_Ad" é o ID correto do banner no painel do Unity Ads
    }

    private void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }
}