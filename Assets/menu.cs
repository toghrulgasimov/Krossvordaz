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
    IEnumerator init()
    {


        //35.227.46.95
        string url = "http://35.227.46.95/countaz?score=" + PlayerPrefs.GetInt("score");
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
                for(int i = 1; i < a.Length-1; i += 2)
                {
                    T[i/2].text = (i/2+1) + " " + a[i] + " " + a[i + 1] + " Xal";
                    //Debug.Log(i + "1 " + a[i] + " " + a[i + 1] + " Xal");
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
            string url = "http://35.227.46.95/newmissia?l=" + PlayerPrefs.GetInt("level");
            using (WWW www = new WWW(url))
            {
                yield return www;

                if(!www.text.Equals("soon"))
                {
                    //Debug.Log("--------------------------------------");
                    PlayerPrefs.SetInt("level" + PlayerPrefs.GetInt("level"), 1);
                    //write to file
                    //Debug.Log(www.text);
                    WriteToFile(www.text);
                    PlayerPrefs.SetString("game", "");
                    X.gameObject.SetActive(true);
                    X.GetComponentInChildren<Text>().text = "Başla";
                    X.gameObject.SetActive(true);
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

    IEnumerator checkversia()
    {

        PlayerPrefs.SetString("versia", "1.9");
        //35.227.46.95
        string url = "http://35.227.46.95/versia";
        using (WWW www = new WWW(url))
        {
            yield return www;
            if(PlayerPrefs.GetString("versia").Equals(www.text))
            {
                admin.gameObject.SetActive(false);
            }else
            {
                admin.gameObject.SetActive(true);
            }


        }



    }
    public void basla()
    {
        SceneManager.LoadScene(2);
    }

    public void WriteToFile(string str)
    {
        string FILE_PATH = Application.persistentDataPath + "/MYFILENAME.txt";
        
        StreamWriter sr = System.IO.File.CreateText(FILE_PATH);
        sr.Write(str);
        
        sr.Close();
    }
    void Start () {
        //X.gameObject.SetActive(false);
        admin.gameObject.SetActive(false);

        StartCoroutine(callmission());
        StartCoroutine(checkversia());
        StartCoroutine(init());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
