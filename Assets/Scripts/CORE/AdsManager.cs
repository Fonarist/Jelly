
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Jelly
{
    public class AdsManager : MonoBehaviour
    {
        private static AdsManager _instance = null;

#if UNITY_IOS
        public static string gameId = "3271644";
#elif UNITY_ANDROID
        public static string gameId = "3271645";
#else
        public static string gameId = "";
#endif

        public static string bannerPlacementId = "Banner";
        public static string videoPlacementId = "video";
        public static string rewardedPlacementId = "rewardedVideo";
        public static bool isAdsTestMode = true;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            Advertisement.Initialize(gameId, isAdsTestMode);
            StartCoroutine(ShowBannerWhenReady());
        }

        IEnumerator ShowBannerWhenReady()
        {
            while (!Advertisement.IsReady(bannerPlacementId))
            {
                yield return new WaitForSeconds(0.5f);
            }
            Advertisement.Banner.Show(bannerPlacementId);
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        }
    }
}
