using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System.IO;

public class wow : MonoBehaviour {
    //
    public Text menuInfo, waitsecond, gamesecond, ResultInfo, SozSayiInfo, YerInfo;
    private string liveserver = "";
    public RectTransform Borderres, Bordermenu;
    public string urlall;
    public GameObject oyun, waiting, result, menu;
    public GameObject prefab;
    // Use this for initialization
    public GameObject c3, c4, c5, c6, c7, c8, c9;
    private static GameObject[] C;
    public Text borderText;
    public GameObject infoText, second;
    public AudioSource tapdi, d1, d2, d3, d4, d5, d6, d7;

    public static GameObject t;
    private static GameObject lasto;
    private static LineRenderer l1;
    public GameObject g;
    public static GameObject lineo;
    public static Material mat;

    public string SERVER_URL = "https://tmhgame.com";


    private static Game game;

    
    IEnumerator initborder(string url, RectTransform border)
    {


        
        
        Debug.Log("initBorderrr");
        border.sizeDelta = new Vector2(0, 6530);
        int childs = border.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(border.GetChild(i).gameObject);
        }
        
        
        //string url = SERVER_URL + "/Wowcount?score=" + PlayerPrefs.GetInt("score");
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
                int wowelo = int.Parse(a[0]);
                int p = 45;
                for (int i = 2; i < a.Length - 1; i++)
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
                    
                    //t.w
                }
                if (result.active)
                {
                    ResultInfo.text = rey;
                    PlayerPrefs.SetInt("wowelo", PlayerPrefs.GetInt("wowelo") + wowelo);
                    Debug.Log("woweloeeeeeeeeeeeeeeeeeeeeee " + wowelo);
                }
                else if (menu.active)
                {
                    menuInfo.text = rey;
                }
            }

        }

    }
    IEnumerator Wait()//wait result
    {
        int childs = Borderres.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(Borderres.GetChild(i).gameObject);
        }
        ResultInfo.text = "Nəticələr Hazırlanır..";
        yield return new WaitForSeconds(10);
        
        
        StartCoroutine(initborder(SERVER_URL + "/wowresultb?name=" + PlayerPrefs.GetString("name"), Borderres));
    }
    IEnumerator callGame()
    {
        
        
        SozSayiInfo.text = "0 Söz";
        Debug.Log("call game");
        string url = SERVER_URL + "/wow";
        using (WWW www = new WWW(url))
        {
            yield return www;
            if(www.text.Length > 2)
            {
                Debug.Log("bashla");
                Debug.Log("calll");
                string download = www.text;
                game = new Game(download);
                menu.SetActive(false);
                result.SetActive(false);
                waiting.SetActive(true);
            }
            
            
        }


    }
    IEnumerator sendServer()
    {

        
        
        string url = SERVER_URL + "/Wowresult?score=" + game.Tapilanlar.Count+"&name="+PlayerPrefs.GetString("name")+"&lasttime="+game.lasttime + "&reg=" + PlayerPrefs.GetString("reg");
        using (WWW www = new WWW(url))
        {
            yield return www;

            if (www.text.Length > 2)
            {
                //set YerInfo
                string[] ar = www.text.Split('^');
                YerInfo.text = ar[0];
                borderText.text = ar[1];
            }

        }


    }
    IEnumerator sendServerLast()
    {

        Debug.Log("Send Server LASTTTT");
        
        
        string url = SERVER_URL + "/Wowresultlast?wowelo=" + PlayerPrefs.GetInt("wowelo") + "&name=" + PlayerPrefs.GetString("name") + "&reg=" + PlayerPrefs.GetString("reg");
        using (WWW www = new WWW(url))
        {
            yield return www;

            if (www.text.Length > 2)
            {

            }

        }


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
    public void callMenu()
    {
        oyun.SetActive(false);
        waiting.SetActive(false);
        result.SetActive(false);

        menu.SetActive(true);

        StartCoroutine(initborder(urlall, Bordermenu));
    }
    private void init()
    {
        C = new GameObject[10];
        for(int i = 0; i < 3; i++)
        {
            C[i] = null;
        }
        C[3] = c3;
        C[4] = c4;
        C[5] = c5;
        C[6] = c6;
        C[7] = c7;
        C[8] = c8;
        C[9] = c9;
        for(int i = 3; i <= 9; i++)
        {
            C[i].SetActive(false);
        }
        
    }
    public class  Game {

        public HashSet<string> S = new HashSet<string>();
        public List<string> words;
        public static GameObject gameo, lasto;
        public Select select;
        public int n, go;
        public long lasttime = 0;
        public TextMeshPro tm;
        public IDictionary<string, int> pos = new Dictionary<string, int>();
        public HashSet<string> SOZ = new HashSet<string>();
        public HashSet<string> Tapilanlar = new HashSet<string>();
        public int sec, davam;

        public Game(string downloaded)
        {
            Debug.Log(downloaded);
            
            string[] ar = downloaded.Split('|');
            this.davam = int.Parse(ar[1]);
            this.sec = int.Parse(ar[0]);
            go = ar[2].Length;
            //sec|dura|SOZ|sozlersozlersozelre
            for(int i = 3; i < ar.Length; i++)
            {
                this.SOZ.Add(ar[i]);
            }
            for(int i = 1; i <= go; i++)
            {
                this.S.Add(("h" + i + "s"));
                this.pos.Add(("h" + i + "s"), i);
            }
            
            tm = C[go].GetComponentInChildren<TextMeshPro>();
            lasto = null;
            select = new Select();


            Transform[] T = C[go].GetComponentsInChildren<Transform>();
            int ind = 0;
            for(int i = 0; i < T.Length; i++)
            {
                if(T[i].transform.gameObject.name.StartsWith("h") && !T[i].transform.gameObject.name.EndsWith("s"))
                {
                    Debug.Log(T[i].transform.gameObject.name);
                    T[i].transform.gameObject.GetComponent<TextMeshPro>().text = ""+ar[2][ind];
                    ind++;
                }
                if (!T[i].transform.gameObject.name.EndsWith("s")) continue;
                Renderer r = T[i].transform.gameObject.GetComponent<Renderer>();
                if(r != null)
                {
                    r.enabled = false;
                }
            }
            for(int i = 3; i <=9; i++ )
            {
                if (C[i] != null) C[i].SetActive(false);
            }
            C[go].SetActive(true);
            
        }
    }
    public class Select
    {
        public Stack<GameObject> S, SL;
        public bool []clicks;
        public Select()
        {
            clicks = new bool[] {false, false, false, false, false, false, false, false, false, false };
            
            S = new Stack<GameObject>();
            SL = new Stack<GameObject>();
        }
        public void add(GameObject ng)
        {
            this.S.Push(ng);
        }
        public GameObject peek()
        {
            return this.S.Peek();
        }
        public void removeAll()
        {
            while(S.Count > 0)
            {
                GameObject go = S.Pop();
                Destroy(go);
            }
            while (SL.Count > 0)
            {
                GameObject go = SL.Pop();

                go.GetComponent<Renderer>().enabled = false;
                //Destroy(go);
            }

        }
    }

    InterstitialAd interstitial;

    //public RawImage img;

    public bool interfailed = true;
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-2317424106273587/8919528582";
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
    {   
        
        string url = SERVER_URL + "/reklamfail?name=" + PlayerPrefs.GetString("name");
        using (WWW www = new WWW(url))
        {
            yield return www;

        }

    }
    IEnumerator reklamsucces()
    {   
        
        string url = SERVER_URL + "/reklamsucces?name=" + PlayerPrefs.GetString("name");
        using (WWW www = new WWW(url))
        {
            yield return www;

        }

    }
    IEnumerator reklamopen()
    {   
        
        string url = SERVER_URL + "/reklamopen?name=" + PlayerPrefs.GetString("name");
        using (WWW www = new WWW(url))
        {
            yield return www;

        }

    }
    IEnumerator reklamload()
    {   
        
        string url = SERVER_URL + "/reklamload?name=" + PlayerPrefs.GetString("name");
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

    private void Awake()
    {
        init();
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

        //PlayerPrefs.DeleteAll();
        Debug.Log(PlayerPrefs.GetInt("wowelo"));
        if(!PlayerPrefs.HasKey("wowelo"))
        {
            PlayerPrefs.SetInt("wowelo", 500);
        }
        urlall = SERVER_URL + "/Wowcount?wowelo=" + PlayerPrefs.GetInt("wowelo") + "&name=" + PlayerPrefs.GetInt("name");
        menuStart();
    }
    int SECOND = 0;
    private void FixedUpdate()
    {
        if (SECOND % 10 == 0)
        {
            if (interfailed)
            {
                Debug.Log("call int");
                interfailed = false;
                this.RequestInterstitial();
            }
        }
        if (SECOND % 4 == 0 && oyun.active)
        {
            StartCoroutine(sendServer());
        }
        if (waiting.active)
        {
            waitsecond.text = game.sec + "";
            if(game.sec == 0)
            {
                waitsecond.text = "";
                waiting.SetActive(false);
                oyun.SetActive(true);
            }
            game.sec--;
            
        }
        if(oyun.active)
        {
            gamesecond.text = game.davam + "";
            if(game.davam == 0)
            {
                borderText.text = "";
                StartCoroutine(sendServerLast());
                StartCoroutine(Wait());
                gamesecond.text = "";
                showi();
                game.select.removeAll();
                if(lineo != null)
                    lineo.SetActive(false);
                oyun.SetActive(false);
                result.SetActive(true);
            }
            game.davam--;
        }
        SECOND++;
    }
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
        
        StartCoroutine(callGame());
    }
    public void menuStart()
    {
        waiting.SetActive(false);
        result.SetActive(false);
        oyun.SetActive(false);
        menu.SetActive(true);
        
        StartCoroutine(initborder(urlall, Bordermenu));
    }
    void Update () {
        

        if(Input.touchCount >= 1)
        {
            if (oyun.active)
            {
                if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    if (game.SOZ.Contains(game.tm.text) && !game.Tapilanlar.Contains(game.tm.text))
                    {
                        tapdi.Play();
                        game.lasttime = now();
                        Debug.Log(game.tm.text);
                        game.Tapilanlar.Add(game.tm.text);
                        SozSayiInfo.text = game.Tapilanlar.Count + " Söz";
                    }
                    game.tm.text = "";
                    Game.lasto = null;
                    for (int i = 0; i < game.select.clicks.Length; i++)
                    {
                        game.select.clicks[i] = false;
                    }
                    game.select.removeAll();
                    Destroy(lineo);
                }
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    Touch touch = Input.GetTouch(0);
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (game.S.Contains(hit.transform.name) && game.select.clicks[game.pos[hit.transform.name]] == false)
                        {
                            //game.tm.text = hit.transform.gameObject.GetComponentInChildren<TextMeshPro>().getT;
                            d1.Play();
                            game.tm.text = (hit.transform.gameObject.GetComponentInChildren<TextMeshPro>().text);
                            Game.lasto = hit.transform.gameObject;
                            game.select.clicks[game.pos[hit.transform.name]] = true;
                            lineo = Instantiate(g);
                            game.select.SL.Push(Game.lasto);
                        }
                    }
                    return;
                }
                if (Game.lasto != null)
                {
                    Touch touch = Input.GetTouch(0);
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (game.S.Contains(hit.transform.name) && game.select.clicks[game.pos[hit.transform.name]] == false)
                        {

                            game.tm.text= (game.tm.text + hit.transform.gameObject.GetComponentInChildren<TextMeshPro>().text);
                            if(game.tm.text.Length == 1) {
                                d1.Play();
                            } else if(game.tm.text.Length == 2) {
                        d2.Play();
                            }else if(game.tm.text.Length == 3)
                            {
d3.Play();

                              }else if(game.tm.text.Length == 4)
                                {

d4.Play();
                                  }else if(game.tm.text.Length == 5) {

d5.Play();
                                   }else if(game.tm.text.Length == 6) {
d6.Play();

                                    }else if(game.tm.text.Length == 7) {
                                    d7.Play();
                                      }
                            game.select.clicks[game.pos[hit.transform.name]] = true;
                            LineRenderer lr = lineo.GetComponent<LineRenderer>();
                            lr.SetPosition(1, hit.transform.gameObject.transform.position);
                            lr.SetPosition(0, Game.lasto.transform.gameObject.transform.position);
                            game.select.add(lineo);
                            Game.lasto = hit.transform.gameObject;
                            game.select.SL.Push(Game.lasto);
                            lineo = Instantiate(g);

                        }
                    }

                    //create line
                    Vector3 mousePos = touch.position;
                    mousePos.z = Game.lasto.transform.position.z;
                    Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mousePos);
                    if (lineo.GetComponent<LineRenderer>() == null)
                    {
                        l1 = lineo.AddComponent<LineRenderer>();
                    }

                    l1.SetWidth(0.7F, 0.7F);
                    l1.SetVertexCount(2);
                    l1.material = mat;
                    Game.lasto.GetComponent<Renderer>().enabled = true;
                    l1.SetPosition(0, mouseWorld);
                    Vector3 p = Game.lasto.transform.position;
                    l1.SetPosition(1, p);
                    //create line end;


                }
            }
        }

    }
}
