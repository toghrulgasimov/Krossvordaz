using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour {

    public Text scoret;
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
        
    }
    public void basla()
    {
        SceneManager.LoadScene(2);
    }
    
	void Start () {
        Debug.Log(PlayerPrefs.GetInt("score"));
        if (PlayerPrefs.GetInt("score") >= 200 && PlayerPrefs.GetInt("score") < 320 && !PlayerPrefs.HasKey("level2xeta"))
        {
            PlayerPrefs.SetInt("level2xeta", 1);
            string name = PlayerPrefs.GetString("name");
            int level = PlayerPrefs.GetInt("level");
            int xal = PlayerPrefs.GetInt("xal");
            //PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString("game", "");
            PlayerPrefs.SetString("name", name);
            PlayerPrefs.SetInt("xal", xal);
            PlayerPrefs.SetInt("level", 2);

        }
        if (PlayerPrefs.GetInt("score") >= 320 && !PlayerPrefs.HasKey("level3sekilisil"))
        {
            PlayerPrefs.SetInt("level3sekilisil", 1);
            string name = PlayerPrefs.GetString("name");
            int level = PlayerPrefs.GetInt("level");
            int xal = PlayerPrefs.GetInt("xal");
            //PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString("game", "");
            PlayerPrefs.SetString("name", name);
            PlayerPrefs.SetInt("xal", xal);
            PlayerPrefs.SetInt("level", 3);

        }
        StartCoroutine(init());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
