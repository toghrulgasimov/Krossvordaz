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
    public GameObject prefab;
    IEnumerator init()
    {


        //35.227.46.95
        //127.0.0.1
        string url = "http://127.0.0.1/countaz?score=" + PlayerPrefs.GetInt("score");
        using (WWW www = new WWW(url))
        {
            yield return www;

            //Debug.Log(www.text);
            if (www.text.Equals(""))
            {
                int rey = 0;
                if (PlayerPrefs.HasKey("reyting")) rey = PlayerPrefs.GetInt("reyting");
                reytinqt.text = "Azərbaycan üzrə Reyting\n" + rey;
            }else
            {
                string[] a = www.text.Split('^');

                int rey = int.Parse(a[0])+1;
                Text[] T = new Text[] { r1, r2, r3, r4, r5, r6, r7, r8 };
                int p = 25;
                for(int i = 1; i < a.Length-1; i += 2)
                {
                    GameObject o = Instantiate(prefab);
                    o.transform.SetParent(border, false);
                    Text t = o.GetComponent<Text>();
                    t.fontSize = p;
                    p -= 5;
                    p = Mathf.Max(15, p);
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
                        t.color = new Color(0, 0, 0, p);
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

    IEnumerator callmission()
    {

        X.gameObject.SetActive(false);
        //35.227.46.95
        if (!PlayerPrefs.HasKey("level" + PlayerPrefs.GetInt("level")))
        {
            string url = "http://127.0.0.1/newmissia?l=" + PlayerPrefs.GetInt("level");
            using (WWW www = new WWW(url))
            {
                yield return www;

                if(!www.text.Equals("soon"))
                {
                    if(www.text.Length > 100)
                    {
                        
                    //Debug.Log("--------------------------------------");
                    PlayerPrefs.SetInt("level" + PlayerPrefs.GetInt("level"), 1);
                    //write to file
                    //Debug.Log(www.text);
                    WriteToFile(Application.persistentDataPath + "/MYFILENAME.txt",www.text);
                    PlayerPrefs.SetString("game", "");
                    X.gameObject.SetActive(true);
                    X.GetComponentInChildren<Text>().text = "Başla";
                    X.gameObject.SetActive(true);
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

        //35.227.46.95
        //127.0.0.1
        string url = "http://127.0.0.1/yarish";
        using (WWW www = new WWW(url))
        {
            yield return www;

            if (www.text.Length > 2)
            {
                WriteToFile(Application.persistentDataPath + "/YARISH.txt", www.text);
            }

        }


    }

    IEnumerator checkversia()
    {

        PlayerPrefs.SetString("versia", "2.4");
        //35.227.46.95
        //127.0.0.1
        string url = "http://127.0.0.1/versia";
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
    {   //35.227.46.95
        //127.0.0.1
        string url = "http://127.0.0.1/online?name=" + PlayerPrefs.GetString("name");
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (www.text.StartsWith("com"))
            {
                Application.OpenURL("market://details?id=" + www.text);
            }
        }

    }
    void FixedUpdate()
    {
        StartCoroutine(online());
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
    void Start () {
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
        //X.gameObject.SetActive(false);
        admin.gameObject.SetActive(false);

        StartCoroutine(callmission());
        StartCoroutine(callYarish());
        StartCoroutine(checkversia());
        StartCoroutine(init());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
