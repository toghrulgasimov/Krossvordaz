using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System.IO;
using UnityEngine.Networking;
using System.Text;


public class GameController : MonoBehaviour
{

    public class Info {
        public int score, missiascore, sozSecond;
        public String name, soz;
    }
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
        public int sozSecond = 0;

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
    public GameObject cub, gamecanvas, yarishcanvas;
    public GameObject[][][] Kvadratlar;
    private int N, M;
    public Button H1, H2, H3, H4, H5, H6, H7, H8, H9, H10, H11, H12;
    public Button[] B;
    public Tapmaca[] T;
    public Tapmaca selected;
    public Text textSual, info, infoyarish;

    public static Color32 coloric = new Color32(255, 255, 255, 255);
    public static Color32 coloryazildi = new Color32(0, 255, 255, 255);
    public static Color32 colorsec = new Color32(79, 117, 93, 255);
    public static Color32 colordolu = new Color32(0, 0, 0, 255);
    public static Color32 colorcur = new Color32(0, 0, 145 , 255);
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
    public Text Intelekt;

    public RectTransform p1,p2,p3,p4;
    public Text YarishText;

    public RectTransform rat;




    InterstitialAd interstitial;

    //public RawImage img;

    public bool interfailed = true;
    public string SERVER_URL = "https://tmhgame.com";


    IEnumerator update2(String soz, int sozSecond)
        {
        Info myObject = new Info();
            myObject.score = PlayerPrefs.GetInt("score");
            myObject.name = PlayerPrefs.GetString("name");
            myObject.sozSecond = sozSecond;
            myObject.soz = soz;
            myObject.missiascore = TAPILANLARL.Count;
            string bodyJsonString = JsonUtility.ToJson(myObject);
            string url = SERVER_URL + "/update2";

            var www  = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.Send();


            Debug.Log(www.downloadHandler.text);
            if (!www.downloadHandler.text.Equals("0") && !www.downloadHandler.text.Equals(""))
                        {
                            AdamKec.gameObject.SetActive(true);
                            AdamKecpanel.gameObject.SetActive(true);
                            AdamKec.text = www.downloadHandler.text + " Adamı Keçdiniz";
                            int rey = int.Parse(www.downloadHandler.text);
                            PlayerPrefs.SetInt("reyting", PlayerPrefs.GetInt("reyting") - rey);
                            yield return new WaitForSeconds(2);

                        }
                        AdamKec.gameObject.SetActive(false);
                        AdamKecpanel.gameObject.SetActive(false);
        }
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
    private void showi()
    {
        //Debug.Log("showiiiiiiiiiiiiiiiiiiiiiiiiiiiiii");
        int dif = 4;
        if(PlayerPrefs.HasKey("reklam"))
        {
            dif = PlayerPrefs.GetInt("reklam");
        }
        if (interstitial.IsLoaded() && Math.Abs(DateTime.Now.Minute - kohne) >= dif)
        {
            kohne = DateTime.Now.Minute;
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
       //Debug.Log(args.Message+"fffffffffffffffffffffffffff");
        interfailed = true;
        //StartCoroutine(reklamfail());
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
        //StartCoroutine(reklamload());
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
    public void sil2() {
        //String ch =
    }
    public bool avakebitib = false;
    public void nextMissia()
    {
        if (TAPILANLARL.Count >= T.Length - 3)
        {
            //next level;
            if (PlayerPrefs.HasKey("level" + PlayerPrefs.GetInt("level")) )
            {
                //PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
                if(PlayerPrefs.GetInt("novbeti") == 0)
                {
                    if (avakebitib)
                        Askecdi.PlayOneShot(Ackecdi);
                }
                else
                {
                    if (avakebitib)
                        Askecdi.PlayOneShot(Actapdi);
                }

                PlayerPrefs.SetInt("novbeti", 1);
                //SceneManager.LoadScene(1);

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

            for (int i = 0; i < selected.soz.Length; i++)
            {
                selected.objects[i][0].transform.Translate(new Vector3(0, 0, 1F));
                selected.objects[i][1].transform.Translate(new Vector3(0, 0, 1F));
            }
            TAPILANLARL.Add(selected.index);
            info.text = "Səviyyə " + PlayerPrefs.GetInt("level") + "    Cavab " + ((TAPILANLARL.Count * 100.0 / (T.Length - 1))).ToString("F0") + "%";
            if (true)
            {
                StartCoroutine(showi2());
            }


            if(PlayerPrefs.GetInt("yarish") == 0) {
                   PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1);
             }

            StartCoroutine(update2(selected.soz, selected.sozSecond));
            StartCoroutine(UpdataServerreg());
            StartCoroutine(CountCollection());
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

    IEnumerator UpdataServer()
    {
        string url = SERVER_URL + "/update?name=" + PlayerPrefs.GetString("name") + "&score=" + PlayerPrefs.GetInt("score")+"&reg="+PlayerPrefs.GetString("reg")+"&missiascore=" + TAPILANLARL.Count;
        using (WWW www = new WWW(url))
        {
            yield return www;


            if (!www.text.Equals("0") && !www.text.Equals(""))
            {
                AdamKec.gameObject.SetActive(true);
                AdamKecpanel.gameObject.SetActive(true);
                AdamKec.text = www.text + " Adamı Keçdiniz";
                int rey = int.Parse(www.text);
                PlayerPrefs.SetInt("reyting", PlayerPrefs.GetInt("reyting") - rey);
                yield return new WaitForSeconds(3);

            }
            AdamKec.gameObject.SetActive(false);
            AdamKecpanel.gameObject.SetActive(false);

            //Debug.Log("score from server" + www.text);
        }

        //

    }
    IEnumerator UpdataServerreg()
    {
        if (PlayerPrefs.HasKey("muss"))
        {
            PlayerPrefs.SetInt("muss", PlayerPrefs.GetInt("muss") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("muss", 1);
        }
        string url = SERVER_URL + "/updatereg?name=" + PlayerPrefs.GetString("name") + "&muss=" + PlayerPrefs.GetInt("muss")+ "&reg=" + PlayerPrefs.GetString("reg");
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (!www.Equals(""))
            {
                PlayerPrefs.SetInt("muss", 0);
            }


            //Debug.Log("score from server" + www.text);
        }

        //

    }
    IEnumerator UpdataServerYarish()
    {
        string url = SERVER_URL + "/updateyarish?name=" + PlayerPrefs.GetString("name") + "&score=" + TAPILANLARL.Count;
        using (WWW www = new WWW(url))
        {
            yield return www;


            if (!www.text.Equals("0") && !www.text.Equals(""))
            {
                AdamKec.gameObject.SetActive(true);
                AdamKecpanel.gameObject.SetActive(true);
                AdamKec.text = www.text + " Adamı Keçdiniz";
                yield return new WaitForSeconds(2);

            }
            AdamKec.gameObject.SetActive(false);
            AdamKecpanel.gameObject.SetActive(false);

            //Debug.Log("score from server" + www.text);
        }

        //

    }
    IEnumerator online()
    {
        string url = SERVER_URL + "/online?name=" + PlayerPrefs.GetString("name");
        using (WWW www = new WWW(url))
        {
            yield return www;
            if(www.text.StartsWith("com"))
            {
                Application.OpenURL("market://details?id="+www.text);
            }
        }

    }
    IEnumerator xeta(String s)
        {
            string url = SERVER_URL + "/xeta?xx=" + s+ "&name="+PlayerPrefs.GetString("name");
            using (WWW www = new WWW(url))
            {
                yield return www;
                if(www.text.StartsWith("com"))
                {
                    Application.OpenURL("market://details?id="+www.text);
                }
            }

        }
    IEnumerator getvariables()
    {
        string url = SERVER_URL + "/variable?name=" + PlayerPrefs.GetString("name");
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (!www.text.Equals(""))
            {
                try
                {
                    int mi = int.Parse(www.text);
                    PlayerPrefs.SetInt("reklam", mi);
                }catch(Exception e) { }
            }
        }

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
    IEnumerator CountCollection()
    {   
        string url = SERVER_URL + "/countcollection?name=" + PlayerPrefs.GetString("name") + "&score=" + PlayerPrefs.GetInt("score");
        using (WWW www = new WWW(url))
        {
            yield return www;


            if (!www.text.Equals(""))
            {
                double si = int.Parse(www.text + "") * 1.0;
                double rating = PlayerPrefs.GetInt("reyting") * 1.0;

                //0 -350
                double t = 1 - rating / si;
                t = 350 * t;
                //Debug.Log("RAAAAATING" + "----" + PlayerPrefs.GetInt("reyting"));
                //Debug.Log("size" + "----" + si);
                //Debug.Log("RAAAAATING" + "----" + t);
                int ans = 350 - (int)t;
                //Debug.Log("RAAAAATING" + "----" + t);
                rat.offsetMax = new Vector2(rat.offsetMax.x, -ans);
                Image img = rat.gameObject.GetComponent<Image>();
                img.color = Color.Lerp(Color.red, Color.green, ans * 1f / 350f);
                float ff = (100 - ans * 100f / 350f);
                string pr = ff.ToString("F2")+"";
                Intelekt.text = "İntellekt " + pr + "%";

            }


            //Debug.Log("score from server" + www.text);
        }

        //

    }

    public void saveGame()
    {

        string yaddas = "";
        foreach (int x in TAPILANLARL)
        {
            yaddas += x + "@";
        }
        //PlayerPrefs.SetString("game", yaddas);
        WriteToFile(Application.persistentDataPath + "/game.txt", yaddas);
    }
    public void saveGameYarish()
    {

        string yaddas = "";
        foreach (int x in TAPILANLARL)
        {
            yaddas += x + "@";
        }
        //PlayerPrefs.SetString("gameyarish", yaddas);
        WriteToFile(Application.persistentDataPath + "/gameyarish.txt", yaddas);

    }
    public void loadGame()
    {
        //if (!PlayerPrefs.HasKey("game") || PlayerPrefs.GetString("game").Equals("")) return;
        //string s = PlayerPrefs.GetString("game");
        //Application.persistentDataPath + "/YARISH.txt"
        string s = "";
        if(System.IO.File.Exists(Application.persistentDataPath + "/game.txt"))
        {
            //Debug.Log("-----------------------------Existiret");
            s = readFile(Application.persistentDataPath + "/game.txt");
            //Debug.Log("geleseeennnn");
        }
        else
        {
            if (!PlayerPrefs.HasKey("game") || PlayerPrefs.GetString("game").Equals("")) return;
            s = PlayerPrefs.GetString("game");
        }

        if (s.Length < 2) return;
        string[] ar = s.Split('@');
        //return;
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
        //if (!PlayerPrefs.HasKey("gameyarish") || PlayerPrefs.GetString("gameyarish").Equals("")) return;
        //string s = PlayerPrefs.GetString("gameyarish");
        string s = "";


        if (System.IO.File.Exists(Application.persistentDataPath + "/gameyarish.txt"))
        {
            //Debug.Log("-----------------------------Existiret");
            s = readFile(Application.persistentDataPath + "/gameyarish.txt");
            //Debug.Log("geleseeennnn");
        }
        else
        {
            if (!PlayerPrefs.HasKey("gameyarish") || PlayerPrefs.GetString("gameyarish").Equals("")) return;
            s = PlayerPrefs.GetString("gameyarish");
        }

        if (s.Length < 2) return;
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
    public void WriteToFile(string FILE_PATH, string str)
    {


        StreamWriter sr = System.IO.File.CreateText(FILE_PATH);
        sr.Write(str);

        sr.Close();
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
            infoyarish.text = s;

            //PlayerPrefs.SetString("gameyarish", "");
            WriteToFile(Application.persistentDataPath + "/gameyarish.txt", "");
            return;
        }

        infoyarish.text = "";
        gamecanvas.SetActive(true);
        yarishcanvas.SetActive(false);

        string ss = s.Substring(s.IndexOf('\n')+2);
        //Debug.Log(ss);
        string[] kr = ss.Split('\n');
        T = new Tapmaca[kr.Length - 2];
        string[] nm = kr[0].Split(' ');
        //Debug.Log(nm[0].Length + "----" + nm[1].Length);
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
    public void change2(Tapmaca t, bool v)
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
            //Debug.Log(selected.objects);
            for (int i = 0; i < selected.objects.Length; i++)
            {
                if (selected.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
                {
                    selected.objects[i][0].GetComponent<Renderer>().material.color = coloric;
                }
                selected.objects[i][0].transform.Translate(new Vector3(0, 0, 1F));
                selected.objects[i][1].transform.Translate(new Vector3(0, 0, 1F));

            }
        }
        selected = t;
        for (int i = 0; i < t.objects.Length; i++)
        {
            if (t.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
            {
                t.objects[i][0].GetComponent<Renderer>().material.color = colorsec;

            }
            selected.objects[i][0].transform.Translate(new Vector3(0, 0, -1F));
            selected.objects[i][1].transform.Translate(new Vector3(0, 0, -1F));

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
            if (selected.objects[i][0].GetComponent<Renderer>().material.color == Color.green && true)
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
    public void change(Tapmaca t, bool v) {
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
                for (int k = 0; k < 11; k++)
                        {
                            B[k].gameObject.SetActive(true);
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
                             //Debug.Log(selected.objects);
                             for (int i = 0; i < selected.objects.Length; i++)
                             {
                                 if (selected.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
                                 {
                                     selected.objects[i][0].GetComponent<Renderer>().material.color = coloric;
                                 }
                                 selected.objects[i][0].transform.Translate(new Vector3(0, 0, 1F));
                                 selected.objects[i][1].transform.Translate(new Vector3(0, 0, 1F));

                             }
                         }
                         selected = t;
                         selected.ButtonStack.Clear();
                         for (int i = 0; i < t.objects.Length; i++)
                         {
                             if (t.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
                             {
                                 t.objects[i][0].GetComponent<Renderer>().material.color = colorsec;

                             }
                             selected.objects[i][0].transform.Translate(new Vector3(0, 0, -1F));
                             selected.objects[i][1].transform.Translate(new Vector3(0, 0, -1F));

                         }
                         if (t.objects[t.cursor][0].GetComponent<Renderer>().material.color != Color.green)
                             t.objects[t.cursor][0].GetComponent<Renderer>().material.color = colorcur;
                         else
                         {
//                             for (int i = 0; i < selected.soz.Length; i++)
//                             {
//                                 if (selected.objects[i][0].GetComponent<Renderer>().material.color != Color.green)
//                                 {
//                                     selected.cursor = i;
//                                     break;
//                                 }
//                             }
                         }




                 for(int i = 0; i < selected.soz.Length; i++) {
                    B[i].GetComponentInChildren<Text>().text = selected.StringRandomized[i] + "";
                 }
                 for(int i = 0; i < selected.soz.Length; i++) {
                                    //B[i].GetComponentInChildren<Text>().text = selected.StringRandomized[i] + "";
                                    String h = selected.objects[i][1].GetComponent<TextMesh>().text;
                                    //Debug.Log(h);
                                    for (int j = 0; j < selected.soz.Length; j++)
                                    {
                                        //Debug.Log(B[j].GetComponentInChildren<Text>().text + " " + h);
                                        if (B[j].gameObject.activeSelf && B[j].GetComponentInChildren<Text>().text.Equals(h) && i <= selected.cursor)
                                        {
                                            //Debug.Log(B[j].GetComponentInChildren<Text>().text + " " + h);
                                            //Debug.Log("DONDDDDDDDDDDDDDDDDDD");
                                            B[j].gameObject.SetActive(false);
                                            break;
                                        }
                                    }
                 }
//                 String ss = "";
//                 for(int i = 0; i < selected.soz.Length; i++) {
//                    if(B[i].gameObject.activeSelf)
//                        ss += B[i].GetComponentInChildren<Text>().text;
//                 }
//                 Debug.Log(ss + "AAAAAAA");
                 for (int k = selected.soz.Length; k < 11; k++)
                                    {
                                         B[k].gameObject.SetActive(false);
                                    }
//                 for (int k = 0; k < ss.Length; k++)
//                     {
//                          B[k].gameObject.SetActive(true);
//                          B[k].GetComponentInChildren<Text>().text = ss[k] + "";
//                      }







//-------------------------------------------------
//                for (int i = 0; i < selected.soz.Length; i++)
//                        {
//                            //B[i].GetComponent<Text>().text = selected.soz[i]+"";
//                            B[i].GetComponentInChildren<Text>().text = selected.StringRandomized[i] + "";
//                            if (selected.objects[i][0].GetComponent<Renderer>().material.color == Color.green && true)
//                            {
//                                String h = selected.objects[i][1].GetComponent<TextMesh>().text;
//                                for (int j = 0; j < 11; j++)
//                                {
//                                    if (!B[j].gameObject.activeSelf && B[j].GetComponentInChildren<Text>().text.Equals(h))
//                                    {
//                                        //B[j].gameObject.SetActive(false);
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                if (i > selected.cursor)
//                                    B[i].gameObject.SetActive(true);
//
//                                if (selected.objects[i][1].GetComponent<TextMesh>().text == "") B[i].gameObject.SetActive(true);
//                                String h = selected.objects[i][1].GetComponent<TextMesh>().text;
//                                for (int j = 0; j < 11; j++)
//                                {
//                                    if (!B[j].gameObject.activeSelf && B[j].GetComponentInChildren<Text>().text.Equals(h))
//                                    {
//                                        B[j].gameObject.SetActive(true);
//                                    }
//                                }
//                                //cursora qeder yazilan butun herfleri gizle
//                            }
//                        }
//
//                        for (int k = selected.soz.Length; k < 11; k++)
//                        {
//                            B[k].gameObject.SetActive(false);
//                        }
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

            }else {
                for(int i = 0; i < selected.soz.Length; i++) {
                    if(!B[i].gameObject.activeSelf) {
                        B[i].gameObject.SetActive(true);
                        B[i].GetComponentInChildren<Text>().text = selected.objects[index][1].GetComponent<TextMesh>().text;
                        break;
                    }
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

        yarishcanvas.SetActive(false);
        gamecanvas.SetActive(true);
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
        //change(T[1], false);
        for (int i = T.Length - 2; i >= 0; i--)
                    {
                        if (!TAPILANLARL.Contains(i))
                        {
                            change(T[i], false);
                            break;
                        }
                    }
    }
    public void yarish()
    {
        yarishcanvas.SetActive(true);
        gamecanvas.SetActive(false);
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

        if (!infoyarish.text.Equals(""))
        {
            p1.gameObject.SetActive(false);
            p2.gameObject.SetActive(false);
            p3.gameObject.SetActive(false);
            p4.gameObject.SetActive(false);

            //YarishText.text = yarishinfo;
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
                //Debug.Log(x + "," + y+","+i+","+j);
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
        //change(T[12], false);
        for (int i = T.Length - 2; i >= 0; i--)
                    {
                        if (!TAPILANLARL.Contains(i))
                        {
                            change(T[i], false);
                            break;
                        }
                    }
    }


    void Awake()
    {

        try
        {
            if (PlayerPrefs.GetInt("yarish") == 1)
            {
                yarish();
            }
            else
            {
                begin();
                //string aa = JsonUtility.ToJson(selected);
                //Debug.Log(aa);
            }
//            int test = 0;
//            test = 1 / test;
        }
        catch(Exception e) {
        Debug.Log("------------------------------------");
        Debug.Log(e.ToString());
            StartCoroutine(xeta(e.ToString()));
            PlayerPrefs.SetString("game", "");
            PlayerPrefs.SetString("gameyarish", "");
            WriteToFile(Application.persistentDataPath + "/game.txt", "");
            WriteToFile(Application.persistentDataPath + "/gameyarish.txt", "");
            PlayerPrefs.SetInt("novbeti", 1);
        }
        StartCoroutine(CountCollection());
       notadmin();


    }
    int kohne = DateTime.Now.Minute;
    int sec = 0;
    void FixedUpdate()
    {

    if(selected != null) {
        selected.sozSecond = selected.sozSecond + 1;
    }




    if(sec%120 == 0) {
        StartCoroutine(getvariables());
    }
    int waitt = 10;
        if (Application.internetReachability == NetworkReachability.NotReachable)
            {

            }else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
             {
                    waitt = 10;
             }else
             {
                    waitt = 3;
             }
        if(sec % waitt == 0)
        {
            Debug.Log("asdads");
            //StartCoroutine(online());

            if (interfailed)
            {
                //Debug.Log("call int");
                interfailed = false;
                this.RequestInterstitial();

            }
        }

        sec++;

    }

    public AudioSource source;
    IEnumerator audio()
    {
        AudioListener.volume = 0.5f;
        source = GetComponent<AudioSource>();
        using (var www = new WWW(SERVER_URL + "/audio"+ "?name="+PlayerPrefs.GetString("name")))
        {
            yield return www;
            source.clip = www.GetAudioClip(true, true, AudioType.MPEG);
            source.loop = true;
            source.Play();
            //Debug.Log("asdasddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd");
        }
    }


    IEnumerator DownloadAndPlay()
    {
        WWW www = new WWW(SERVER_URL + "/audio"+ "?name="+PlayerPrefs.GetString("name"));
        yield return www;
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = www.GetAudioClip(true, false, AudioType.MPEG);
        audio.loop = true;
        audio.Play();
    }
    public void Start()
    {

        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                {
                    StartCoroutine(audio());
                    //StartCoroutine(DownloadAndPlay());
                }else {
                    Askecdi.PlayOneShot(Ackecdi);
                }

        //StartCoroutine(DownloadAndPlay());

#if UNITY_ANDROID
        string appId = "ca-app-pub-2317424106273587~1262699727";
#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        RequestInterstitial();
        StartCoroutine(getvariables());

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
          var dependencyStatus = task.Result;
          if (dependencyStatus == Firebase.DependencyStatus.Available) {
            // Create and hold a reference to your FirebaseApp,
            // where app is a Firebase.FirebaseApp property of your application class.
            //   app = Firebase.FirebaseApp.DefaultInstance;
            Debug.Log("Firebasse isledi");
            // Set a flag here to indicate whether Firebase is ready to use by your app.
          } else {
            UnityEngine.Debug.LogError(System.String.Format(
              "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            // Firebase Unity SDK is not safe to use here.
          }
        });

    }
    void Update() {

        //Debug.Log(source);
        if (source != null)
        {
           // source.Play();
        }

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
