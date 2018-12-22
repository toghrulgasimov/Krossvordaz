using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System.IO;

public class GameControllerMath : MonoBehaviour {

    // Use this for initialization
    
    public Button a, b, c, d;
    public Text TimeText, WaitText, GameInfo, CurrInfo, MenuInfo, admin, question;
    public RectTransform border, borderres, QuestionPanel;
    
    
    private Test currTest;
    public GameObject game, waiting, menu, result, prefab;
    public Game currgame;

    private string urlall, urli;
    private string download = "";
    public bool startCount = false;


    InterstitialAd interstitial;

    //public RawImage img;

    public bool interfailed = true;
    public class Test
    {
        public string a, b, c, d, q, ans;
        public Test(string sual, string a, string b, string c, string d, string cavab)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.q = sual;
            this.ans = cavab;
        }
    }
    public class Game
    {
        public Test[] T;
        public int curr, sec, davam, point;
        public long lastTime = 0, begintime = 0;
        public bool sended;
        public Game(string download)
        {
            this.sended = false;
            this.curr = 0;
            this.point = 0;
            string[] gg = download.Split('!');
            this.sec = int.Parse(gg[2]);
            this.davam = int.Parse(gg[1]);
            string[] a = gg[0].Split('$');
            Debug.Log(a.Length);
            this.T = new Test[a.Length];
            begintime = now();
            for (int i = 0; i < a.Length; i++)
            {
                string[] b = a[i].Split('&');
                T[i] = new Test(b[0], b[1], b[2], b[3], b[4], b[5]);
                Debug.Log(T[i].q);
            }
        }
    }
    public void changeQuestion()
    {
        a.GetComponentInChildren<Text>().text = currgame.T[currgame.curr].a;
        b.GetComponentInChildren<Text>().text = currgame.T[currgame.curr].b;
        c.GetComponentInChildren<Text>().text = currgame.T[currgame.curr].c;
        d.GetComponentInChildren<Text>().text = currgame.T[currgame.curr].d;
        CurrInfo.text = (currgame.curr+1) + "/" + currgame.T.Length;
        question.text = currgame.T[currgame.curr].q;
        a.onClick.RemoveAllListeners();
        b.onClick.RemoveAllListeners();
        c.onClick.RemoveAllListeners();
        d.onClick.RemoveAllListeners();
        a.onClick.AddListener(() => bl("a"));
        b.onClick.AddListener(() => bl("b"));
        c.onClick.AddListener(() => bl("c"));
        d.onClick.AddListener(() => bl("d"));
        
    }
    IEnumerator initborder(string url, RectTransform border)
    {


        //35.227.46.95
        //127.0.0.1
        Debug.Log("initBorderrr");
        border.sizeDelta = new Vector2(0, 6530);
        int childs = border.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(border.GetChild(i).gameObject);
        }
        //127.0.0.1
        //35.227.46.95
        //string url = "http://35.227.46.95/countaz?score=" + PlayerPrefs.GetInt("score");
        using (WWW www = new WWW(url))
        {
            yield return www;

            //Debug.Log(www.text);
            if (www.text.Equals(""))
            {
                //int rey = 0;
                //if (PlayerPrefs.HasKey("reyting")) rey = PlayerPrefs.GetInt("reyting");
                //reytinqt.text = "Azərbaycan üzrə Reyting\n" + rey;
            }
            else
            {
                string[] a = www.text.Split('^');

                string rey = a[1];
                int el = int.Parse(a[0]);
                PlayerPrefs.SetInt("elo", PlayerPrefs.GetInt("elo") + el);
                int p = 45;
                for (int i = 2; i < a.Length-1; i++)
                {
                    GameObject o = Instantiate(prefab);
                    o.transform.SetParent(border, false);
                    Button b = o.GetComponent<Button>();
                    Text t = b.GetComponentInChildren<Text>();
                    //t.fontSize = p;
                    p -= 5;
                    p = Mathf.Max(23, p);
                    if (i == 0)
                    {
                        t.color = new Color(255, 0, 0, p);
                    }
                    else if (i == 1)
                    {
                        t.color = new Color(100, 0, 0, p);
                    }
                    else if (i == 2)
                    {
                        t.color = new Color(50, 0, 0, p);
                    }
                    else
                    {
                        //t.color = new Color(0, 0, 0, p);
                    }

                    //p -= 30;
                    string s = a[i];
                    string[] ars = s.Split('\n');
                    s = string.Join("AAAA", ars);
                    t.text = s;
                    if(result.active)
                    {
                        GameInfo.text = rey;
                    }
                    else if(menu.active)
                    {
                        MenuInfo.text = rey;
                    }
                    //t.w
                }
            }

        }

    }
    IEnumerator checkversia()
    {

        PlayerPrefs.SetString("versia", "3.6");
        //35.227.46.95
        //127.0.0.1
        string url = "http://35.227.46.95/versia";
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (PlayerPrefs.GetString("versia").Equals(www.text))
            {
                admin.gameObject.SetActive(false);
            }
            else
            {
                if (www.text.Length >= 2)
                    admin.gameObject.SetActive(true);
            }


        }



    }
    IEnumerator callgame()
    {
        //127.0.0.1
        //35.227.46.95
        Debug.Log("calll");
        string url = "http://35.227.46.95/Math";
        using (WWW www = new WWW(url))
        {
            yield return www;
            Debug.Log("callllllllllllllllllll" + www.text);
            download = www.text;
            if (download.Length < 10)
            {
                //No internet or server problem
            }
            else
            {
                currgame = new Game(download);
                menu.SetActive(false);
                result.SetActive(false);
                waiting.SetActive(true);
                TimeText.text = "";
                CurrInfo.text = "";
                WaitText.text = "";
                startCount = true;

            }

        }


    }
    IEnumerator sendResult()
    {
        //127.0.0.1
        //35.227.46.95
        string a = "name=" + PlayerPrefs.GetString("name") + "&score=" + currgame.point + "&lasttime=" + currgame.lastTime+"&elo=" + PlayerPrefs.GetInt("elo");
        string url = "http://35.227.46.95/Mathresult?" + a;
        using (WWW www = new WWW(url))
        {
            yield return www;

            //download = www.text;
            if (www.text.Equals("1"))
            {
                currgame.sended = true;
                showi();
            }

            

        }


    }
    IEnumerator Wait()
    {
        int childs = borderres.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(borderres.GetChild(i).gameObject);
        }
        yield return new WaitForSeconds(10);
        //127.0.0.1
        //35.227.46.95
        StartCoroutine(initborder("http://35.227.46.95/Mathresultb?name=" + PlayerPrefs.GetString("name"), borderres));
    }
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-9026840340673035/4960879307";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif
        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        interstitial.OnAdOpening += HandleOnAdOpened;

        interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);

    }
    IEnumerator reklamfail()
    {   //35.227.46.95
        //127.0.0.1
        string url = "http://35.227.46.95/reklamfail?name=" + PlayerPrefs.GetString("name");
        using (WWW www = new WWW(url))
        {
            yield return www;

        }

    }
    IEnumerator reklamsucces()
    {   //35.227.46.95
        //127.0.0.1
        string url = "http://35.227.46.95/reklamsucces?name=" + PlayerPrefs.GetString("name");
        using (WWW www = new WWW(url))
        {
            yield return www;

        }

    }
    IEnumerator reklamopen()
    {   //35.227.46.95
        //127.0.0.1
        string url = "http://35.227.46.95/reklamopen?name=" + PlayerPrefs.GetString("name");
        using (WWW www = new WWW(url))
        {
            yield return www;

        }

    }
    IEnumerator reklamload()
    {   //35.227.46.95
        //127.0.0.1
        string url = "http://35.227.46.95/reklamload?name=" + PlayerPrefs.GetString("name");
        using (WWW www = new WWW(url))
        {
            yield return www;

        }

    }
    private void showi()
    {
        Debug.Log("showiiiiiiiiiiiiiiiiiiiiiiiiiiiiii");
        
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            interstitial.Destroy();
            this.RequestInterstitial();
        }
    }
    IEnumerator showi2()
    {
        yield return new WaitForSeconds(3);
        showi();
    }
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log(args.Message + "fffffffffffffffffffffffffff");
        interfailed = true;
        StartCoroutine(reklamfail());
    }
    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        StartCoroutine(reklamsucces());
    }
    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
        StartCoroutine(reklamopen());
    }
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        interfailed = false;
        StartCoroutine(reklamload());
    }
    void Start () {
#if UNITY_ANDROID
        string appId = "ca-app-pub-9026840340673035~9445396711";
#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        RequestInterstitial();
        //127.0.0.1
        //35.227.46.95
        if (!PlayerPrefs.HasKey("elo"))
        {
            PlayerPrefs.SetInt("elo", 500);
            
        }
        if(PlayerPrefs.GetInt("elo") <= 0)
        {
            //SceneManager.LoadScene(1);
        }
        urlall = "http://35.227.46.95/Mathcount?elo=" + PlayerPrefs.GetInt("elo") + "&name="+ PlayerPrefs.GetInt("name");
        waiting.SetActive(false);
        game.SetActive(false);
        result.SetActive(false);
        menu.SetActive(true);
        int childs = border.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(border.GetChild(i).gameObject);
        }
        StartCoroutine(checkversia());
        StartCoroutine(initborder(urlall, border));
        
    }
    public void callMenu()
    {
        game.SetActive(false);
        waiting.SetActive(false);
        result.SetActive(false);

        menu.SetActive(true);
        StartCoroutine(initborder(urlall, border));
    }
    public void bl(string s)
    {    
        Debug.Log(s);
        if(s.Equals(currgame.T[currgame.curr].ans))
        {
            currgame.point++;
            currgame.lastTime = now() - currgame.begintime;
            Debug.Log(currgame.lastTime);
        }else
        {

        }
        currgame.curr++;
        if(currgame.curr == currgame.T.Length)
        {
            // ends game
            //game.SetActive(false);
            //result.SetActive(true);
            StartCoroutine(sendResult());

            question.transform.gameObject.SetActive(false);
            a.transform.gameObject.SetActive(false);
            b.transform.gameObject.SetActive(false);
            c.transform.gameObject.SetActive(false);
            d.transform.gameObject.SetActive(false);
            QuestionPanel.gameObject.SetActive(false);
            
            GameInfo.text = "Nəticələr hazırlanır..";
            //StartCoroutine(initborder(urlall, borderres));
        }
        else
        {
            changeQuestion();
        }
    }
    private int MissionSecond = 50;
    public static long now()
    {
        int h = System.DateTime.Now.Hour;
        int d = System.DateTime.Now.Minute;
        int s = System.DateTime.Now.Second;
        int ms = System.DateTime.Now.Millisecond;
        return h * 60 * 60 * 1000 + d * 60 * 1000 + s * 1000 + ms;
    }
    public void bashla()
    {
        
        StartCoroutine(callgame());
    }
    public void krossvord()
    {
        SceneManager.LoadScene(1);
    }
    public void paylash()
    {
        //C# code in unity
        //create intent for action send
        AndroidJavaClass intentClass = new
                         AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new
                         AndroidJavaObject("android.content.Intent");

        //set action to that intent object   
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

        var shareSubject = "Krossvord və Riyaziyyat Yarışması" +
                   "Fire Block";
        var shareMessage = "Krossvord və Riyaziyyat Yarışması\nhttps://play.google.com/store/apps/details?id=com.sadas.asdasd2";
        //set the type as text and put extra subject and text to share
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        //call createChooser method of activity class     
        AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your high score");
        currentActivity.Call("startActivity", chooser);
    }
    
    int secc = 0;
    private void FixedUpdate()
    {
        secc++;
        if(secc == 10)
        {
            secc = 0;
            if (interfailed)
            {
                Debug.Log("call int");
                interfailed = false;
                this.RequestInterstitial();
            }
        }
        Debug.Log(now());
        if(game.active)
        {
            if(currgame.davam == 0)
            {
                GameInfo.text = "";
                if(!currgame.sended)
                    StartCoroutine(sendResult());
                question.transform.gameObject.SetActive(true);
                a.transform.gameObject.SetActive(true);
                b.transform.gameObject.SetActive(true);
                c.transform.gameObject.SetActive(true);
                d.transform.gameObject.SetActive(true);
                QuestionPanel.gameObject.SetActive(true);
                // ends game
                game.SetActive(false);
                result.SetActive(true);
                StartCoroutine(Wait());



                GameInfo.text = "Nəticələr hazırlanır..";
            }
            TimeText.text = currgame.davam + "";
            currgame.davam--;
            
        }else if(waiting.active)
        {
            if(startCount)
            {
                WaitText.text = (currgame.sec + "");
                if(currgame.sec == 0)
                {
                    // begin game
                    waiting.SetActive(false);
                    game.SetActive(true);
                    changeQuestion();
                    return;
                }
                currgame.sec--;

            }
        }else if(menu.active)
        {

        }else if(result.active)
        {

        }
    }
}
