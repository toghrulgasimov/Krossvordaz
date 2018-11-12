using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System.IO;


public class GameController : MonoBehaviour
{
    
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
        public Stack<int> ButtonStack = new Stack<int>();
        public String StringRandomized;

        public String generateRandomList(String s)
        {
            HashSet<int> H = new HashSet<int>();
            while (H.Count != s.Length)
            {
                int d = UnityEngine.Random.Range(0, s.Length);

                H.Add(d);
            }
            String ans = "";
            int c = 0;
            foreach (int x in H)
            {
                ans += soz[x];
            }
            return ans;
        }

        public Tapmaca(int x, int y, bool saga, string soz, string sual, int index)
        {
            this.x = x;
            this.y = y;
            this.soz = soz;
            this.StringRandomized = generateRandomList(soz);
            this.sual = sual;
            this.saga = saga;
            this.index = index;
            this.objects = new GameObject[soz.Length][];
            for (int i = 0; i < soz.Length; i++) this.objects[i] = new GameObject[2];

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
    private int N, M;
    public Button H1, H2, H3, H4, H5, H6, H7, H8, H9, H10, H11, H12;
    public Button[] B;
    public Tapmaca[] T;
    public Tapmaca selected;
    public Text textSual, info;

    public static Color32 coloric = new Color32(255, 255, 255, 255);
    public static Color32 coloryazildi = new Color32(0, 255, 255, 255);
    public static Color32 colorsec = new Color32(0, 255, 255, 255);
    public static Color32 colordolu = new Color32(0, 0, 0, 255);
    public static Color32 colorcur = new Color32(244, 181, 65, 255);
    public static Color32 colorcol = new Color32(186, 195, 211, 255);

    public HashSet<int> TAPILANLARL;
    public int COUNTLEVEL;

    public AudioClip Ac;
    public AudioSource As;
    public AudioClip Ackey;
    public AudioSource Askey;
    public AudioClip Actapdi;
    public AudioSource Astapdi;
    public AudioClip Ackecdi;
    public AudioSource Askecdi;

    public Button adminnext, adminnextl, adminnextl2;

    public Text AdamKec;
    public RectTransform AdamKecpanel;

    public RectTransform p1,p2,p3,p4;
    public Text YarishText;


    InterstitialAd interstitial;

    //public RawImage img;

    
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

    public void sil()
    {

        if (selected.objects[selected.cursor][1].GetComponent<TextMesh>().text != "")
        {
            selected.objects[selected.cursor][1].GetComponent<TextMesh>().text = "";
        }
        else
        {
            selected.objects[selected.cursor][0].GetComponent<Renderer>().material.color = colorsec;
            for (int i = selected.cursor - 1; i >= 0; i--)
            {
                if (selected.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
                {
                    selected.cursor = i;
                    break;
                }
            }
            selected.objects[selected.cursor][0].GetComponent<Renderer>().material.color = colorcur;
            selected.objects[selected.cursor][1].GetComponent<TextMesh>().text = "";
        }
    }
    public bool avakebitib = false;
    public void nextMissia()
    {
        if (TAPILANLARL.Count >= T.Length * 84 / 100.0)
        {
            //next level;
            if (PlayerPrefs.HasKey("level" + PlayerPrefs.GetInt("level")))
            {
                PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
                //SceneManager.LoadScene(1);
                if(avakebitib)
                    Askecdi.PlayOneShot(Ackecdi);
            }else
            {
                if (avakebitib)
                    Astapdi.PlayOneShot(Actapdi);
            }
        }else
        {
            if (avakebitib)
                Astapdi.PlayOneShot(Actapdi);
        }
    }
    public void next(char a)
    {


        selected.objects[selected.cursor][1].GetComponent<TextMesh>().text = a + "";
        selected.objects[selected.cursor][0].GetComponent<Renderer>().material.color = colorsec;
        for (int i = selected.cursor + 1; i < selected.soz.Length; i++)
        {
            if (selected.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
            {
                selected.cursor = i;
                break;
            }
        }
        selected.objects[selected.cursor][0].GetComponent<Renderer>().material.color = colorcur;
        string w = "";
        for (int i = 0; i < selected.objects.Length; i++)
        {
            w += selected.objects[i][1].GetComponent<TextMesh>().text;
        }
        if (w.Equals(selected.soz) && !TAPILANLARL.Contains(selected.index))
        {
            TAPILANLARL.Add(selected.index);
            info.text = "Səviyyə " + PlayerPrefs.GetInt("level") + "    Cavab " + ((TAPILANLARL.Count * 100.0 / (T.Length - 1))).ToString("F0") + "%";
            if (TAPILANLARL.Count % 4 == 0)
            {
                StartCoroutine(showi2());
            }
            

            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1);
            StartCoroutine(UpdataServer());
            for (int i = 0; i < selected.objects.Length; i++)
            {
                selected.objects[i][0].GetComponent<Renderer>().material.color = Color.green;
            }

            if(PlayerPrefs.GetInt("yarish") == 0)
            {
                nextMissia();
                saveGame();
            }else
            {
                Astapdi.PlayOneShot(Actapdi);
                saveGameYarish();
                StartCoroutine(UpdataServerYarish());
            }
            for (int i = T.Length - 2; i >= 0; i--)
            {
                if (!TAPILANLARL.Contains(i))
                {
                    change(T[i], false);
                    break;
                }
            }
        }
        else
        {
            Askey.PlayOneShot(Ackey);

        }

    }
    public void saveGame()
    {

        string yaddas = "";
        foreach (int x in TAPILANLARL)
        {
            yaddas += x + "@";
        }
        PlayerPrefs.SetString("game", yaddas);

    }
    public void saveGameYarish()
    {

        string yaddas = "";
        foreach (int x in TAPILANLARL)
        {
            yaddas += x + "@";
        }
        PlayerPrefs.SetString("gameyarish", yaddas);

    }
    public void loadGame()
    {
        if (!PlayerPrefs.HasKey("game") || PlayerPrefs.GetString("game").Equals("")) return;
        string s = PlayerPrefs.GetString("game");
        string[] ar = s.Split('@');
        for (int i = 0; i < ar.Length - 1; i++)
        {
            int index = int.Parse(ar[i]);
            TAPILANLARL.Add(index);
            for (int j = 0; j < T[index].soz.Length; j++)
            {
                T[index].objects[j][0].GetComponent<Renderer>().material.color = Color.green;
                T[index].objects[j][1].GetComponent<TextMesh>().text = T[index].soz[j] + "";
            }
        }
    }
    public void loadGameYarish()
    {
        if (!PlayerPrefs.HasKey("gameyarish") || PlayerPrefs.GetString("gameyarish").Equals("")) return;
        string s = PlayerPrefs.GetString("gameyarish");
        string[] ar = s.Split('@');
        for (int i = 0; i < ar.Length - 1; i++)
        {
            int index = int.Parse(ar[i]);
            TAPILANLARL.Add(index);
            for (int j = 0; j < T[index].soz.Length; j++)
            {
                T[index].objects[j][0].GetComponent<Renderer>().material.color = Color.green;
                T[index].objects[j][1].GetComponent<TextMesh>().text = T[index].soz[j] + "";
            }
        }
    }
    IEnumerator UpdataServer()
    {   //35.227.46.95
        //127.0.0.1
        string url = "http://35.227.46.95/update?name=" + PlayerPrefs.GetString("name") + "&score=" + PlayerPrefs.GetInt("score");
        using (WWW www = new WWW(url))
        {
            yield return www;


            if (!www.text.Equals("0"))
            {
                AdamKec.gameObject.SetActive(true);
                AdamKecpanel.gameObject.SetActive(true);
                AdamKec.text = www.text + " Adamı Keçdiniz";
                yield return new WaitForSeconds(5);

            }
            AdamKec.gameObject.SetActive(false);
            AdamKecpanel.gameObject.SetActive(false);

            //Debug.Log("score from server" + www.text);
        }

        //

    }
    IEnumerator UpdataServerYarish()
    {   //35.227.46.95
        //127.0.0.1
        string url = "http://35.227.46.95/updateyarish?name=" + PlayerPrefs.GetString("name") + "&score=" + TAPILANLARL.Count;
        using (WWW www = new WWW(url))
        {
            yield return www;


            if (!www.text.Equals("0") && !www.text.Equals(""))
            {
                AdamKec.gameObject.SetActive(true);
                AdamKecpanel.gameObject.SetActive(true);
                AdamKec.text = www.text + " Adamı Keçdiniz";
                yield return new WaitForSeconds(5);

            }
            AdamKec.gameObject.SetActive(false);
            AdamKecpanel.gameObject.SetActive(false);

            //Debug.Log("score from server" + www.text);
        }

        //

    }
    IEnumerator online()
    {   //35.227.46.95
        //127.0.0.1
        string url = "http://35.227.46.95/online?name=" + PlayerPrefs.GetString("name");
        using (WWW www = new WWW(url))
        {
            yield return www;
            if(www.text.StartsWith("com"))
            {
                Application.OpenURL("market://details?id="+www.text);
            }
        }

    }
    string readFile(string FILE_PATH)
    {
        
        StreamReader inp_stm = new StreamReader(FILE_PATH);
        string ans = "";
        while (!inp_stm.EndOfStream)
        {
            ans += inp_stm.ReadLine() + "\n";
        }

        inp_stm.Close();
        return ans;
    }
    public void readMission()
    {
        string s = readFile(Application.persistentDataPath + "/MYFILENAME.txt");
        //Debug.Log(s);
        string[] kr = s.Split('\n');
        T = new Tapmaca[kr.Length - 2];
        string[] nm = kr[0].Split(' ');
        N = int.Parse(nm[0]);
        M = int.Parse(nm[1]);
        for (int i = 0; i < T.Length; i++)
        {
            string[] a = kr[i + 1].Split('{');
            int x = int.Parse(a[0]);
            int y = int.Parse(a[1]);
            bool saga = a[2].Equals("1") ? true : false;
            string soz = a[3];
            string sual = a[4];
            string SU = sual.ToLower(), SOZ = soz.ToLower();
            string ulduz = ""; for (int ii = 0; ii < soz.Length; ii++) ulduz += "*";
            sual = sual.Replace(SOZ, ulduz);
            string suans = "";
            for (int j = 0; j < SU.Length; j++)
            {
                if (SU[j] == '*') suans += "*";
                else suans += sual[j];
            }
            int iii = i;
            T[i] = new Tapmaca(x, y, saga, soz, sual, iii);

        }

        for (int i = 0; i < 100; i++)
        {
            //TAPILANLARL.Add(i);
        }
    }
    public string yarishinfo = "";
    public void readMissionYarish()
    {
        string s = readFile(Application.persistentDataPath + "/YARISH.txt");
        if(s.IndexOf('{') == -1)
        {
            yarishinfo = s;
            PlayerPrefs.SetString("gameyarish", "");
            return;
        }
        
        string ss = s.Substring(s.IndexOf('\n')+2);
        Debug.Log(ss);
        string[] kr = ss.Split('\n');
        T = new Tapmaca[kr.Length - 2];
        string[] nm = kr[0].Split(' ');
        Debug.Log(nm[0].Length + "----" + nm[1].Length);
        N = int.Parse(nm[0]);
        M = int.Parse(nm[1]);
        for (int i = 0; i < T.Length; i++)
        {
            string[] a = kr[i + 1].Split('{');
            int x = int.Parse(a[0]);
            int y = int.Parse(a[1]);
            bool saga = a[2].Equals("1") ? true : false;
            string soz = a[3];
            string sual = a[4];
            string SU = sual.ToLower(), SOZ = soz.ToLower();
            string ulduz = ""; for (int ii = 0; ii < soz.Length; ii++) ulduz += "*";
            sual = sual.Replace(SOZ, ulduz);
            string suans = "";
            for (int j = 0; j < SU.Length; j++)
            {
                if (SU[j] == '*') suans += "*";
                else suans += sual[j];
            }
            int iii = i;
            T[i] = new Tapmaca(x, y, saga, soz, sual, iii);

        }

        for (int i = 0; i < 100; i++)
        {
            //TAPILANLARL.Add(i);
        }
    }
    public void change(Tapmaca t, bool v)
    {

        //step.enabled = true;
        if (TAPILANLARL.Contains(t.index)) return;
        if (t.sual.StartsWith("/da"))
        {
            textSual.text = "";
            string url = "https://4bilder-1wort.net" + t.sual;
            //StartCoroutine(imgf(url));
        }
        else
        {
            textSual.text = t.sual;
        }

        for (int i = t.cursor; i < t.objects.Length; i++)
        {
            if (t.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
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
            Debug.Log(selected.objects);
            for (int i = 0; i < selected.objects.Length; i++)
            {
                if (selected.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
                {
                    selected.objects[i][0].GetComponent<Renderer>().material.color = coloric;
                }
            }
        }
        selected = t;
        for (int i = 0; i < t.objects.Length; i++)
        {
            if (t.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
            {
                t.objects[i][0].GetComponent<Renderer>().material.color = colorsec;
            }


        }
        if (t.objects[t.cursor][0].GetComponent<Renderer>().material.color != Color.green)
            t.objects[t.cursor][0].GetComponent<Renderer>().material.color = colorcur;
        else
        {
            for (int i = 0; i < selected.soz.Length; i++)
            {
                if (selected.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
                {
                    selected.cursor = i;
                    break;
                }
            }
        }



        for (int i = 0; i < selected.soz.Length; i++)
        {
            //B[i].GetComponent<Text>().text = selected.soz[i]+"";
            B[i].GetComponentInChildren<Text>().text = selected.StringRandomized[i] + "";
            if (selected.objects[i][0].GetComponent<Renderer>().material.color == Color.green && false)
            {
                String h = selected.objects[i][1].GetComponent<TextMesh>().text;
                for (int j = 0; j < 11; j++)
                {
                    if (!B[j].gameObject.activeSelf && B[j].GetComponentInChildren<Text>().text.Equals(h))
                    {
                        //B[j].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (i > selected.cursor)
                    B[i].gameObject.SetActive(true);

                if (selected.objects[i][1].GetComponent<TextMesh>().text == "") B[i].gameObject.SetActive(true);
                String h = selected.objects[i][1].GetComponent<TextMesh>().text;
                for (int j = 0; j < 11; j++)
                {
                    if (!B[j].gameObject.activeSelf && B[j].GetComponentInChildren<Text>().text.Equals(h))
                    {
                        B[j].gameObject.SetActive(true);
                    }
                }
                //cursora qeder yazilan butun herfleri gizle
            }
        }
        
        for (int k = selected.soz.Length; k < 11; k++)
        {
            B[k].gameObject.SetActive(false);
        }
        
    }
    

    public void menuload()
    {
        SceneManager.LoadScene(1);
    }
    void keyclick(int index)
    {
        if (B[index].GetComponentInChildren<Text>().text.Equals("x"))
        {
            sil();
            Askey.PlayOneShot(Ackey);
            if (selected.ButtonStack.Count > 0)
            {
                int d = -1;
                while (true)
                {
                    d = (int)selected.ButtonStack.Pop();
                    if (selected.objects[d][0].GetComponent<Renderer>().material.color != Color.green) break;
                    else B[d].gameObject.SetActive(true);
                }
                if (d != -1)
                {
                    Button p = B[d];
                    p.gameObject.SetActive(true);
                }

            }
        }
        else
        {
            B[index].gameObject.SetActive(false);
            selected.ButtonStack.Push(index);
            next(B[index].GetComponentInChildren<Text>().text[0]);
        }


    }
    public void adminn1()
    {
        for(int i = 1; i < T.Length; i++)
        {
            if(!TAPILANLARL.Contains(i))
            {
                change(T[i], false);
                break;
            }
        }
        for(int i = 0; i < selected.soz.Length; i++)
        {
            if(selected.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
                next(selected.soz[i]);
        }
    }
    public void adminn3()
    {
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") - 1);
        PlayerPrefs.DeleteKey("level" + PlayerPrefs.GetInt("level"));


    }
    public void adminn2()
    {
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level")+1);

    }
    public void notadmin()
    {
        adminnext.gameObject.SetActive(false);
        adminnextl.gameObject.SetActive(false);
        adminnextl2.gameObject.SetActive(false);
    }
    public void begin()
    {
        adminnext.onClick.AddListener(adminn1);
        adminnextl.onClick.AddListener(adminn2);
        adminnextl2.onClick.AddListener(adminn3);
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetString("game", "");
        //PlayerPrefs.SetInt("level", 2);
        //PlayerPrefs.SetInt("score", 87);

        AdamKec.gameObject.SetActive(false);
        AdamKecpanel.gameObject.SetActive(false);

        TAPILANLARL = new HashSet<int>();





        // call with level
        readMission();
        nextMissia();

        avakebitib = true;
        //Debug.Log("Tapmacalarin sayi------------------------------" + T.Length);
        B = new Button[] { H1, H2, H3, H4, H5, H6, H7, H8, H9, H10, H11, H12 };
        for (int i = 0; i < 12; i++)
        {
            int k = i;
            B[i].onClick.AddListener(() => keyclick(k));
        }


        Kvadratlar = new GameObject[N][][];
        string s = "QÜERTYUİOPÖĞASDFGHJKLIƏZXCVBNMÇŞ-";
        int ch = 0;
        for (float x = 0, i = 0; i < N; x += 2.2f, i++)
        {
            Kvadratlar[(int)i] = new GameObject[M][];

            for (float y = M * 2.2f, j = 0; j < M; y -= 2.4f, j++)
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
        for (int i = 0; i < T.Length - 1; i++)
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
                T[i].objects[j][1].GetComponent<TextMesh>().color = Color.black;
                T[i].objects[j][0].GetComponent<Renderer>().material.color = Color.white;
                T[i].objects[j][0].name = i + "";
                T[i].objects[j][1].name = i + "";
                //T[i].objects[j].onClick.AddListener(() => ButtonClicked(para));
                if (T[i].saga) y++;
                else x++;
            }

        }
        for (float x = 0, i = 0; i < N; x += 2.2f, i++)
        {


            for (float y = M * 2.2f, j = 0; j < M; y -= 2.4f, j++)
            {
                if (Kvadratlar[(int)i][(int)j][0].GetComponent<Renderer>().material.color != Color.white)
                {
                    Kvadratlar[(int)i][(int)j][0].SetActive(false);
                }
            }
        }

        loadGame();
        info.text = "Səviyyə " + PlayerPrefs.GetInt("level") + "    Cavab " + ((TAPILANLARL.Count * 100.0 / (T.Length - 1))).ToString("F0") + "%";
        change(T[1], false);
    }
    public void yarish()
    {
        adminnext.onClick.AddListener(adminn1);
        adminnextl.onClick.AddListener(adminn2);
        adminnextl2.onClick.AddListener(adminn3);
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetString("game", "");
        //PlayerPrefs.SetInt("level", 2);
        //PlayerPrefs.SetInt("score", 87);

        AdamKec.gameObject.SetActive(false);
        AdamKecpanel.gameObject.SetActive(false);

        TAPILANLARL = new HashSet<int>();





        // call with level
        readMissionYarish();
        //nextMissia();
        B = new Button[] { H1, H2, H3, H4, H5, H6, H7, H8, H9, H10, H11, H12 };
        for (int i = 0; i < 12; i++)
        {
            int k = i;
            B[i].onClick.AddListener(() => keyclick(k));
        }

        if (!yarishinfo.Equals(""))
        {
            p1.gameObject.SetActive(false);
            p2.gameObject.SetActive(false);
            p3.gameObject.SetActive(false);
            p4.gameObject.SetActive(false);

            YarishText.text = yarishinfo;
            return;
        }
        avakebitib = true;
        //Debug.Log("Tapmacalarin sayi------------------------------" + T.Length);
        


        Kvadratlar = new GameObject[N][][];
        string s = "QÜERTYUİOPÖĞASDFGHJKLIƏZXCVBNMÇŞ-";
        int ch = 0;
        for (float x = 0, i = 0; i < N; x += 2.2f, i++)
        {
            Kvadratlar[(int)i] = new GameObject[M][];

            for (float y = M * 2.2f, j = 0; j < M; y -= 2.4f, j++)
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
        for (int i = 0; i < T.Length - 1; i++)
        {
            int x = T[i].x, y = T[i].y;

            for (int j = 0; j < T[i].soz.Length; j++)
            {
                //Debug.Log(x + " " + y + " " + T[i].soz + " " + );
                T[i].objects[j] = Kvadratlar[x][y];
                Debug.Log(x + "," + y+","+i+","+j);
                //T[i].byttons[j].GetComponentInChildren<Text>().text = T[i].soz[j] + "";
                //T[i].objects[j].GetComponentInChildren<Image>().color = coloric;
                Tapmaca para = T[i];
                T[i].objects[j][1].GetComponent<TextMesh>().text = "";
                T[i].objects[j][1].GetComponent<TextMesh>().color = Color.black;
                T[i].objects[j][0].GetComponent<Renderer>().material.color = Color.white;
                T[i].objects[j][0].name = i + "";
                T[i].objects[j][1].name = i + "";
                //T[i].objects[j].onClick.AddListener(() => ButtonClicked(para));
                if (T[i].saga) y++;
                else x++;
            }

        }
        for (float x = 0, i = 0; i < N; x += 2.2f, i++)
        {


            for (float y = M * 2.2f, j = 0; j < M; y -= 2.4f, j++)
            {
                if (Kvadratlar[(int)i][(int)j][0].GetComponent<Renderer>().material.color != Color.white)
                {
                    Kvadratlar[(int)i][(int)j][0].SetActive(false);
                }
            }
        }

        loadGameYarish();
        info.text = "Səviyyə " + PlayerPrefs.GetInt("level") + "    Cavab " + ((TAPILANLARL.Count * 100.0 / (T.Length - 1))).ToString("F0") + "%";
        change(T[12], false);
    }


    void Awake()
    {
        
        notadmin();
        if(PlayerPrefs.GetInt("yarish") == 1)
        {
            yarish();
        }else
        {
            begin();
            //string aa = JsonUtility.ToJson(selected);
            //Debug.Log(aa);
        }
        
    }
    void FixedUpdate()
    {
        StartCoroutine(online());
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        //Input.GetMouseButtonDown(0)
        //Input.mousePosition
        {   //Input.touchCount >0
            //Input.GetTouch(0).position

            //0.6828691
            float f = Input.mousePosition[1] / Screen.height;
            if (f > 0.2414773f)
            {
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
                    //hit.transform.gameObject.GetComponent<Renderer> ().material.color = Color.green;

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
