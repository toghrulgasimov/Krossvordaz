using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using GoogleMobileAds.Api;
using System.IO;

public class Banner : MonoBehaviour {

    //private BannerView bannerView;
    public Text bannerinfo;
public bool interfailed = true;
    public void Start()
    {
    #if UNITY_ANDROID
    //ca-app-pub-2317424106273587~1262699727
        string appId = "ca-app-pub-2317424106273587~1262699727";
    #elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
    #else
            string appId = "unexpected_platform";
    #endif

        // Initialize the Google Mobile Ads SDK.
        //MobileAds.Initialize(appId);
        

        this.RequestBanner();
    }
    private void RequestBanner()
    {
    #if UNITY_ANDROID
    //ca-app-pub-2317424106273587/2168587286
    //tt ca-app-pub-3940256099942544/6300978111
        string adUnitId = "ca-app-pub-2317424106273587/2168587286"; //ca-app-pub-2317424106273587/2168587286
    #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
    #else
            string adUnitId = "unexpected_platform";
    #endif

        // Create a 320x50 banner at the top of the screen.
        //bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);

        // Called when an ad request has successfully loaded.
        //        bannerView.OnAdLoaded += HandleOnAdLoaded;
                // Called when an ad request failed to load.
        //        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
                // Called when an ad is clicked.
         //       bannerView.OnAdOpening += HandleOnAdOpened;
                // Called when the user returned from the app after an ad click.
         //       bannerView.OnAdClosed += HandleOnAdClosed;
                // Called when the ad click caused the user to leave the application.
         //       bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;


        // Create an empty ad request.
        //AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        //bannerView.LoadAd(request);

    }
    public void HandleOnAdLoaded(object sender, EventArgs args)
        {
            //MonoBehaviour.print("HandleAdLoaded event received");
            interfailed = false;
            bannerinfo.text = "loaded";

        }

 //       public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
 //       {
//
//interfailed = true;
//bannerinfo.text = "fail load";
 //       }

        public void HandleOnAdOpened(object sender, EventArgs args)
        {
            //MonoBehaviour.print("HandleAdOpened event received");
            bannerinfo.text = "opened";
        }

        public void HandleOnAdClosed(object sender, EventArgs args)
        {
            //MonoBehaviour.print("HandleAdClosed event received");
            bannerinfo.text = "closed";
        }

        public void HandleOnAdLeavingApplication(object sender, EventArgs args)
        {
            //MonoBehaviour.print("HandleAdLeavingApplication event received");
            bannerinfo.text = "leaving app";
        }
    // Update is called once per frame
    void Update () {
		
	}
	int sec = 0;
//        void FixedUpdate()
//        {
//        Debug.Log("Bunnerdixed update");
//        int waitt = 10;
//        if (Application.internetReachability == NetworkReachability.NotReachable)
//                    {
//
//                    }else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
//                     {
//                            waitt = 10;
//                     }else
//                     {
//                            waitt = 3;
//                     }
//            if(sec % waitt == 0)
//            {
//                if (interfailed)
//                {
//                    //Debug.Log("call int");
//                    interfailed = false;
//
//                    bannerinfo.text = "call request";
//                    //this.RequestBanner();
//
//                }
//            }
//
//            sec++;
//
//        }
}
