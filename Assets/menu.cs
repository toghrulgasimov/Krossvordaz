using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour {

    public Text scoret;
    public Text levelt;
    public Text admin;
    public Text reytinqt, r1,r2,r3,r4,r5,r6,r7,r8;
    public Button X;
    public RectTransform border;
    public GameObject prefab, prefab2;
    public Button statistika, Region, novbeti;
    public void openGames()
    {
        //PlayerPrefs.DeleteAll();
        Application.OpenURL("market://details?id=com.teg.azerioyunlar");
    }
    IEnumerator init()
    {


        //35.231.39.26
        //127.0.0.1
        border.sizeDelta = new Vector2(0, 6530);
        int childs = border.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(border.GetChild(i).gameObject);
        }
        string url = "http://35.231.39.26/countaz?score=" + PlayerPrefs.GetInt("score");
        using (WWW www = new WWW(url))
        {
            yield return www;

            //Debug.Log(www.text);
            if (www.text.Equals(""))
            {
                //int rey = 0;
                //if (PlayerPrefs.HasKey("reyting")) rey = PlayerPrefs.GetInt("reyting");
                //reytinqt.text = "Azərbaycan üzrə Reyting\n" + rey;
            }else
            {
                string[] a = www.text.Split('^');

                int rey = int.Parse(a[0])+1;
                Text[] T = new Text[] { r1, r2, r3, r4, r5, r6, r7, r8 };
                int p = 45;
                for(int i = 1; i < a.Length-1; i += 2)
                {
                    GameObject o = Instantiate(prefab2);
                    o.transform.SetParent(border, false);
                    Button b = o.GetComponent<Button>();
                    Text t = b.GetComponentInChildren<Text>();
                    //t.fontSize = p;
                    p -= 5;
                    p = Mathf.Max(23, p);
                    if(i == 1)
                    {
                        t.color = new Color(255, 0, 0, p);
                    }
                    else if(i == 3)
                    {
                        t.color = new Color(100, 0, 0, p);
                    }
                    else if(i == 5)
                    {
                        t.color = new Color(50, 0, 0, p);
                    }
                    else
                    {
                        //t.color = new Color(0, 0, 0, p);
                    }

                    //p -= 30;
                    string s = (i / 2 + 1) + ". " + a[i] + " " + a[i + 1] + " Xal";
                    string[] ars = s.Split('\n');
                    s = string.Join("AAAA",ars);
                    t.text = s;
                    //t.w
                }
                PlayerPrefs.SetInt("reyting", rey);
                reytinqt.text = "Azərbaycan üzrə Reyting\n" + rey;
            }

        }
        scoret.text = "Xal\n" + PlayerPrefs.GetInt("score");
        levelt.text = "Səviyyə\n" + PlayerPrefs.GetInt("level");

    }
    IEnumerator initreg()
    {

        border.sizeDelta = new Vector2(0, 3530);
        //35.231.39.26
        //127.0.0.1
        int childs = border.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(border.GetChild(i).gameObject);
        }
        string url = "http://35.231.39.26/countregaz?score=" + PlayerPrefs.GetInt("score");
        using (WWW www = new WWW(url))
        {
            yield return www;

            //Debug.Log(www.text);
            if (www.text.Equals(""))
            {
                int rey = 0;
                if (PlayerPrefs.HasKey("reyting")) rey = PlayerPrefs.GetInt("reyting");
                reytinqt.text = "Azərbaycan üzrə Reyting\n" + rey;
            }
            else
            {
                int rey = 0;
                if (PlayerPrefs.HasKey("reyting")) rey = PlayerPrefs.GetInt("reyting");
                reytinqt.text = "Azərbaycan üzrə Reyting\n" + rey;
                Debug.Log(www.text);
                string[] a = www.text.Split('^');

                //int rey = int.Parse(a[0]) + 1;
                int p = 45;
                for (int i = 1; i < a.Length - 1; i += 2)
                {
                    GameObject o = Instantiate(prefab2);
                    o.transform.SetParent(border, false);
                    Button b = o.GetComponent<Button>();
                    Text t = b.GetComponentInChildren<Text>();
                    //t.fontSize = p;
                    p -= 5;
                    p = Mathf.Max(23, p);
                    if (i == 1)
                    {
                        t.color = new Color(255, 0, 0, p);
                    }
                    else if (i == 3)
                    {
                        t.color = new Color(100, 0, 0, p);
                    }
                    else if (i == 5)
                    {
                        t.color = new Color(50, 0, 0, p);
                    }
                    else
                    {
                        //t.color = new Color(0, 0, 0, p);
                    }

                    //p -= 30;
                    string s = (i / 2 + 1) + ". " + a[i] + " " + a[i + 1] + " Xal";
                    string ai = a[i];
                    b.onClick.AddListener(() => clickreg(ai)); ;
                    string[] ars = s.Split('\n');
                    s = string.Join("AAAA", ars);
                    t.text = s;
                    //t.w
                }
                //PlayerPrefs.SetInt("reyting", rey);
                //reytinqt.text = "Azərbaycan üzrə Reyting\n" + rey;
            }

        }
        scoret.text = "Xal\n" + PlayerPrefs.GetInt("score");
        levelt.text = "Səviyyə\n" + PlayerPrefs.GetInt("level");

    }
    IEnumerator initreglist(string s)
    {

        border.sizeDelta = new Vector2(0, 3530);
        //35.231.39.26
        //127.0.0.1
        int childs = border.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(border.GetChild(i).gameObject);
        }
        string url = "http://35.231.39.26/countreglistaz?score=" + PlayerPrefs.GetInt("score") + "&reg="+ s;
        using (WWW www = new WWW(url))
        {
            yield return www;

            //Debug.Log(www.text);
            if (www.text.Equals(""))
            {
                int rey = 0;
                if (PlayerPrefs.HasKey("reyting")) rey = PlayerPrefs.GetInt("reyting");
                reytinqt.text = "Azərbaycan üzrə Reyting\n" + rey;
            }
            else
            {
                int rey = 0;
                if (PlayerPrefs.HasKey("reyting")) rey = PlayerPrefs.GetInt("reyting");
                reytinqt.text = "Azərbaycan üzrə Reyting\n" + rey;
                Debug.Log(www.text);
                string[] a = www.text.Split('^');

                //int rey = int.Parse(a[0]) + 1;
                int p = 45;
                for (int i = 1; i < a.Length - 1; i += 2)
                {
                    GameObject o = Instantiate(prefab2);
                    o.transform.SetParent(border, false);
                    Button b = o.GetComponent<Button>();
                    Text t = b.GetComponentInChildren<Text>();
                    //t.fontSize = p;
                    p -= 5;
                    p = Mathf.Max(23, p);
                    if (i == 1)
                    {
                        t.color = new Color(255, 0, 0, p);
                    }
                    else if (i == 3)
                    {
                        t.color = new Color(100, 0, 0, p);
                    }
                    else if (i == 5)
                    {
                        t.color = new Color(50, 0, 0, p);
                    }
                    else
                    {
                        //t.color = new Color(0, 0, 0, p);
                    }

                    //p -= 30;
                    string ss = (i / 2 + 1) + ". " + a[i] + " " + a[i + 1] + " Xal";
                    string[] ars = ss.Split('\n');
                    ss = string.Join("AAAA", ars);
                    t.text = ss;
                    //t.w
                }
                //PlayerPrefs.SetInt("reyting", rey);
                //reytinqt.text = "Azərbaycan üzrə Reyting\n" + rey;
            }

        }
        scoret.text = "Xal\n" + PlayerPrefs.GetInt("score");
        levelt.text = "Səviyyə\n" + PlayerPrefs.GetInt("level");

    }
    private void clickreg(string s)
    {
        StartCoroutine(initreglist(s));
        Debug.Log(s);
    }
    IEnumerator callmission()
    {

        //X.gameObject.SetActive(false);
        //35.231.39.26
        if (!PlayerPrefs.HasKey("level" + PlayerPrefs.GetInt("level")))
        {
            string url = "http://35.231.39.26/newmissia?l=" + PlayerPrefs.GetInt("level");
            using (WWW www = new WWW(url))
            {
                yield return www;

                if(!www.text.Equals("soon"))
                {
                    if(www.text.Length > 100)
                    {

                        //Debug.Log("--------------------------------------");
                        novbeti.transform.gameObject.SetActive(false);
                        PlayerPrefs.SetInt("novbeti", 0);
                        PlayerPrefs.SetInt("level" + PlayerPrefs.GetInt("level"), 1);
                    //write to file
                    //Debug.Log(www.text);
                    WriteToFile(Application.persistentDataPath + "/MYFILENAME.txt",www.text);
                    PlayerPrefs.SetString("game", "");
                    WriteToFile(Application.persistentDataPath + "/game.txt", "");
                        X.GetComponentInChildren<Text>().text = "Başla";
                    X.gameObject.SetActive(true);
                        levelt.text = "Səviyyə\n" + PlayerPrefs.GetInt("level");
                    }
                }
                else
                {
                    X.gameObject.SetActive(true);
                    X.GetComponentInChildren<Text>().text = "Hazırlanir";
                }

            }
            
        }
        else
        {
            X.gameObject.SetActive(true);
            X.GetComponentInChildren<Text>().text = "Başla";
        }
    
        
    }
    IEnumerator callYarish()
    {

        //35.231.39.26
        //127.0.0.1
        string url = "http://35.231.39.26/yarish";
        using (WWW www = new WWW(url))
        {
            yield return www;

            if (www.text.Length > 2)
            {
                WriteToFile(Application.persistentDataPath + "/YARISH.txt", www.text);
                Debug.Log("Yarisssssssss file yazildi");
            }

        }


    }

    IEnumerator checkversia()
    {

        PlayerPrefs.SetString("versia", "3.6");
        //35.231.39.26
        //127.0.0.1
        string url = "http://35.231.39.26/versia";
        using (WWW www = new WWW(url))
        {
            yield return www;
            if(PlayerPrefs.GetString("versia").Equals(www.text))
            {
                admin.gameObject.SetActive(false);
            }else
            {
                if(www.text.Length >=2)
                admin.gameObject.SetActive(true);
            }


        }



    }

    IEnumerator online()
    {   //35.231.39.26
        //127.0.0.1
        string url = "http://35.231.39.26/online?name=" + PlayerPrefs.GetString("name");
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (www.text.StartsWith("com"))
            {
                Application.OpenURL("market://details?id=" + www.text);
            }
        }

    }
    int sec = 0;
    void FixedUpdate()
    {
        if(sec % 10 == 0)
        {
            StartCoroutine(online());
        }
        sec++;
        
    }
    public void basla()
    {
        PlayerPrefs.SetInt("yarish", 0);
        SceneManager.LoadScene(2);
    }
    public void baslayarish()
    {
        
        PlayerPrefs.SetInt("yarish", 1);
        SceneManager.LoadScene(2);
    }

    public void WriteToFile(string FILE_PATH, string str)
    {
        
        StreamWriter sr = System.IO.File.CreateText(FILE_PATH);
        sr.Write(str);
        
        sr.Close();
    }
    private void statf()
    {
        StartCoroutine(init());
    }
    private void regf()
    {
        StartCoroutine(initreg());
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

        var shareSubject = "I challenge you to beat my high score in" +
                   "Fire Block";
        var shareMessage = "Krossvord yarışması\n https://play.google.com/store/apps/details?id=com.sadas.asdasd2";
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
    public void math()
    {
        SceneManager.LoadScene(4);
    }
    public void novb()
    {
        if(PlayerPrefs.GetInt("level" + PlayerPrefs.GetInt("level")) == 1)
        {
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        }
        StartCoroutine(callmission());
        
    }
    public void wow()
    {
        SceneManager.LoadScene(5);
    }
    void Start () {

        if(!PlayerPrefs.HasKey("novbeti") || PlayerPrefs.GetInt("novbeti") == 0)
        {
            novbeti.transform.gameObject.SetActive(false);
        }
        else
        {
            novbeti.transform.gameObject.SetActive(true);
        }
        statistika.onClick.AddListener(() => statf());
        Region.onClick.AddListener(() => regf());
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
        //PlayerPrefs.DeleteAll();
        //X.gameObject.SetActive(false);
        admin.gameObject.SetActive(false);

        StartCoroutine(callmission());
        StartCoroutine(callYarish());
        StartCoroutine(checkversia());
        if(!PlayerPrefs.HasKey("reyting"))
            StartCoroutine(init());
        else StartCoroutine(initreg());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
