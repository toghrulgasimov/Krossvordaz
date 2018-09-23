using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;


public class GameController : MonoBehaviour {
    public class Tapmaca
    {
        public string soz;
        public string sual;
        public int x, y, index;
        public bool saga;
        public GameObject[][] objects;
        public int cursor = 0;
        public string written = "";
        public string komekci = "";


        public Tapmaca(int x, int y, bool saga, string soz, string sual, int index)
        {
            this.x = x;
            this.y = y;
            this.soz = soz;
            this.sual = sual;
            this.saga = saga;
            this.index = index;
            this.objects = new GameObject[soz.Length][];
            for (int i = 0; i < soz.Length; i++) this.objects[i] = new GameObject[2];
            komekci = enable(50);
        }
        public string enable(int f)
        {
            int len = this.soz.Length;
            int c = (int)(len * f / 100);
            HashSet<int> S = new HashSet<int>();

            while (S.Count != c)
            {
                int x = UnityEngine.Random.Range(0, len - 1);
                S.Add(x);
            }
            string ans = "";
            for (int i = 0; i < this.soz.Length; i++)
            {
                if (S.Contains(i)) ans += this.soz[i];
                else ans += '*';
            }
            return ans;
        }

    }
    public GameObject enemy;
    public GameObject cub;
    public GameObject[][][] Kvadratlar;
    public int N = 5, M = 5;
    public RectTransform panel1, panel2, panel3;
    public Tapmaca[] T;
    public Tapmaca selected;
    public TextAsset textFile;
    public Text textSual;

    public static Color32 coloric = new Color32(255, 255, 255, 255);
    public static Color32 coloryazildi = new Color32(0, 255, 255, 255);
    public static Color32 colorsec = new Color32(0, 255, 255, 255);
    public static Color32 colordolu = new Color32(0, 0, 0, 255);
    public static Color32 colorcur = new Color32(244, 181, 65, 255);
    public static Color32 colorcol = new Color32(186, 195, 211, 255);

    public HashSet<int> TAPILANLARL;
    public int COUNTLEVEL;
    public int QAZANDIGIMXAL = 0;

    public AudioClip Ac;
    public AudioSource As;

    public AudioClip Ackey;
    public AudioSource Askey;

    public AudioClip Actapdi;
    public AudioSource Astapdi;

    public AudioClip Ackecdi;
    public AudioSource Askecdi;

    InterstitialAd interstitial;
    public void Start()
    {

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
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }
    private void showi()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            interstitial.Destroy();
            this.RequestInterstitial();
        }
    }
    
    public void sil()
    {
        
        if(selected.objects[selected.cursor][1].GetComponent<TextMesh>().text != "")
        {
            selected.objects[selected.cursor][1].GetComponent<TextMesh>().text = "";
        }
        else
        {
            selected.objects[selected.cursor][0].GetComponent<Renderer>().material.color = colorsec;
            for (int i = selected.cursor-1; i >= 0; i--)
            {
                if(selected.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
                {
                    selected.cursor = i;
                    break;
                }
            }
            selected.objects[selected.cursor][0].GetComponent<Renderer>().material.color = colorcur;
            selected.objects[selected.cursor][1].GetComponent<TextMesh>().text = "";
        }
    }
    public void next(char a)
    {
       

        selected.objects[selected.cursor][1].GetComponent<TextMesh>().text = a + "";
        selected.objects[selected.cursor][0].GetComponent<Renderer>().material.color = colorsec;
        for (int i = selected.cursor + 1; i < selected.soz.Length; i++)
        {
            if(selected.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
            {
                selected.cursor = i;
                break;
            }
        }
        selected.objects[selected.cursor][0].GetComponent<Renderer>().material.color = colorcur;
        string w = "";
        for(int i = 0; i < selected.objects.Length; i++)
        {
            w += selected.objects[i][1].GetComponent<TextMesh>().text;
        }
        if(w.Equals(selected.soz) && !TAPILANLARL.Contains(selected.index))
        {
            TAPILANLARL.Add(selected.index);
            if(TAPILANLARL.Count%5 == 0)
            {
                //showi();
                Invoke("showi", 3);
            }
            saveGame();
            Astapdi.PlayOneShot(Actapdi);
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1);
            StartCoroutine(UpdataServer());
            for (int i = 0; i < selected.objects.Length; i++)
            {
                selected.objects[i][0].GetComponent<Renderer>().material.color = Color.green;
            }
            for(int i = 0; i < T.Length; i++)
            {
                if(!TAPILANLARL.Contains(i))
                {
                    change(T[i], false);
                    break;
                }
            }
        }else
        {
            Askey.PlayOneShot(Ackey);
        }

    }
    public void saveGame()
    {
        
        string yaddas = "";
        foreach(int x in TAPILANLARL)
        {
            yaddas += x + "@";
        }
        PlayerPrefs.SetString("game", yaddas);

    }
    public void loadGame()
    {
        if (!PlayerPrefs.HasKey("game")) return;
        string s = PlayerPrefs.GetString("game");
        string[] ar = s.Split('@');
        for(int i = 0; i < ar.Length-1; i++)
        {
            int index = int.Parse(ar[i]);
            TAPILANLARL.Add(index);
            for(int j = 0; j < T[index].soz.Length; j++)
            {
                T[index].objects[j][0].GetComponent<Renderer>().material.color = Color.green;
                T[index].objects[j][1].GetComponent<TextMesh>().text = T[index].soz[j] +"";
            }
        }
    }
    IEnumerator UpdataServer()
    {
        string url = "http://35.227.46.95/update?name=" + PlayerPrefs.GetString("name") + "&score="+PlayerPrefs.GetInt("score");
        using (WWW www = new WWW(url))
        {
            yield return www;
            Debug.Log("score from server" + www.text);
        }
    }

    public void menuload()
    {
        SceneManager.LoadScene(1);
    }
    void keyclick(Button b)
    {
        if (b.GetComponentInChildren<Text>().text.Equals("←"))
        {
            sil();
            Askey.PlayOneShot(Ackey);
        }
        else
        {
            next(b.GetComponentInChildren<Text>().text[0]);
        }

        
    }
    void Awake()
    {
        
        TAPILANLARL = new HashSet<int>();
        if (!PlayerPrefs.HasKey("xal"))
        {
            PlayerPrefs.SetInt("xal", 0);
        }
        if (!PlayerPrefs.HasKey("qizil"))
        {
            PlayerPrefs.SetInt("qizil", 20);
        }
        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
        }
        //Xaltext.text = "Xal " + PlayerPrefs.GetInt("xal");
        //Qiziltext.text = "Qizil " + PlayerPrefs.GetInt("qizil");

        // call with level
        readMission();
        
        //Debug.Log("Tapmacalarin sayi" + T.Length);
        Button[] buttons1 = panel1.GetComponentsInChildren<Button>();
        Button[] buttons2 = panel2.GetComponentsInChildren<Button>();
        Button[] buttons3 = panel3.GetComponentsInChildren<Button>();
        for(int i = 0; i < buttons1.Length; i++)
        {
            Button K = buttons1[i];
            buttons1[i].onClick.AddListener(() => keyclick(K));
        }
        for (int i = 0; i < buttons2.Length; i++)
        {
            Button K = buttons2[i];
            buttons2[i].onClick.AddListener(() => keyclick(K));
        }
        for (int i = 0; i < buttons3.Length; i++)
        {
            Button K = buttons3[i];
            buttons3[i].onClick.AddListener(() => keyclick(K));
        }

        Kvadratlar = new GameObject[N][][];
        string s = "QÜERTYUİOPÖĞASDFGHJKLIƏZXCVBNMÇŞ-";
        int ch = 0;
        for (float x = 0, i=0; i < N; x+=2.2f, i++)
        {
            Kvadratlar[(int)i] = new GameObject[M][];

            for (float y = M*2.2f, j=0; j < M; y-=2.4f, j++)
            {
                Kvadratlar[(int)i][(int)j] = new GameObject[2];
                GameObject t = Instantiate(enemy);
                Kvadratlar[(int)i][(int)j][1] = t;
                //cube.AddComponent<Rigidbody>();
                t.transform.position = new Vector3(x, y, 12);
                t.GetComponent<TextMesh>().text = "";
                ch++;
                ch %= s.Length;
                GameObject c = Instantiate(cub);
                Kvadratlar[(int)i][(int)j][0] = c;
                //cube.AddComponent<Rigidbody>();
                c.transform.position = new Vector3(x, y, 12);
            }
        }
        for (int i = 0; i < T.Length; i++)
        {
            int x = T[i].x, y = T[i].y;

            for (int j = 0; j < T[i].soz.Length; j++)
            {
                //Debug.Log(x + " " + y + " " + T[i].soz + " " + );
                T[i].objects[j] = Kvadratlar[x][y];
                //T[i].byttons[j].GetComponentInChildren<Text>().text = T[i].soz[j] + "";
                //T[i].objects[j].GetComponentInChildren<Image>().color = coloric;
                Tapmaca para = T[i];
                T[i].objects[j][1].GetComponent<TextMesh>().text = "";
                T[i].objects[j][0].GetComponent<Renderer>().material.color = Color.white;
                T[i].objects[j][0].name = i + "";
                T[i].objects[j][1].name = i + "";
                //T[i].objects[j].onClick.AddListener(() => ButtonClicked(para));
                if (T[i].saga) y++;
                else x++;
            }

        }
        loadGame();
        change(T[0],false);
    }
    public void readMission()
    {
        int levelcurrent = PlayerPrefs.GetInt("level");
        string text = textFile.text;

        string[] mm = text.Split('@');
        //Debug.Log("Umumumi oyunlarin sayi " + (mm.Length - 1));

        for (int i = 1; i < mm.Length; i++)
        {
            // butunt tapmacalar // 1den bashlayir
            //Debug.Log("Uzunluqq " + mm[i].Split('\n').Length);
            string[] ar = mm[i].Split('\n');
            for (int j = 1; j < ar.Length - 1; j++)
            {
                //Debug.Log(j + " " + ar[j]);
            }

        }

        COUNTLEVEL = mm.Length - 1;
        levelcurrent = Mathf.Min(levelcurrent, COUNTLEVEL);

        //Debug.Log("Currentttt Level" + levelcurrent);
        string[] kr = mm[levelcurrent].Split('\n');
        T = new Tapmaca[kr.Length - 2];

        for (int i = 0; i < T.Length; i++)
        {
            string[] a = kr[i + 1].Split('{');
            //if (a[0][0] == ' ') a[0] = a[0].Substring(1);
            //Debug.Log(i + "" + a[0] + " " + a[1] + " " + a[2] + " " + a[3] + " " + a[4]);
            int x = int.Parse(a[0]);
            int y = int.Parse(a[1]);
            bool saga = a[2].Equals("1") ? true : false;
            string soz = a[3];
            string sual = a[4];
            string SU = sual.ToLower(), SOZ = soz.ToLower();
            string ulduz = ""; for (int ii = 0; ii < soz.Length; ii++) ulduz += "*";
            sual = sual.Replace(SOZ, ulduz);
            string suans = "";
            for(int j = 0; j < SU.Length; j++)
            {
                if (SU[j] == '*') suans += "*";
                else suans += sual[j];
            }
            int iii = i;
            T[i] = new Tapmaca(x, y, saga, soz, sual, iii);

        }
    }
    public void change(Tapmaca t, bool v)
    {
        if (TAPILANLARL.Contains(t.index)) return;
        textSual.text = t.sual;
        for(int i = t.cursor; i < t.objects.Length; i++)
        {
            if(t.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
            {
                t.cursor = i;
                break;
            }
        }
        
        if (v)
        {
            As.PlayOneShot(Ac);
        }
        
        if (selected != null && !TAPILANLARL.Contains(selected.index))
        {
            for (int i = 0; i < selected.objects.Length; i++)
            {
                if(selected.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
                {
                    selected.objects[i][0].GetComponent<Renderer>().material.color = coloric;
                }
            }
        }
        selected = t;
        for (int i = 0; i < t.objects.Length; i++)
        {
            if(t.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
            {
                t.objects[i][0].GetComponent<Renderer>().material.color = colorsec;
                //Debug.Log("Color Changeddd" + i);
                //Destroy(t.objects[i]);
            }


        }
        if (t.objects[t.cursor][0].GetComponent<Renderer>().material.color != Color.green)
            t.objects[t.cursor][0].GetComponent<Renderer>().material.color = colorcur;
        else
        {
            for(int i = 0; i < selected.soz.Length; i++)
            {
                if(selected.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
                {
                    selected.cursor = i;
                    break;
                }
            }
        }
        //text.text = selected.sual + "-> " + selected.disable(10);
        //text.text = selected.sual;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        //Input.GetMouseButtonDown(0)
        //Input.mousePosition
        {   //Input.touchCount >0
            //Input.GetTouch(0).position

            //0.6828691
            float f = Input.mousePosition[1] / Screen.height ;
            if(f>0.2414773f)
            {
                //Debug.Log(f);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == "Player")
                    {
                        //Debug.Log("This is a Player");
                    }
                    else
                    {
                        //Debug.Log("This isn't a Player");
                    }
                    //hit.transform.gameObject.GetComponent<Renderer> ().material.color = Color.blue;

                    try
                    {
                        // Do something that can throw an exception
                        int tt = int.Parse(hit.transform.gameObject.name);
                        change(T[tt], true);
                        //Debug.Log(T[tt].sual);
                    }
                    catch (Exception e)
                    {
                        //Debug.LogException(e, this);
                    }
                }
            } 
            
        }
    }
    }
