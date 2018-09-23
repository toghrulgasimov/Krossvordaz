using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour {

    public Text scoret;
    public Text reytinqt;
    IEnumerator init()
    {
        
        

        string url = "http://35.227.46.95/count?score=" + PlayerPrefs.GetInt("score");
        using (WWW www = new WWW(url))
        {
            yield return www;

            Debug.Log(www.text);
            if (www.text.Equals(""))
            {
                int rey = 0;
                if (PlayerPrefs.HasKey("reyting")) rey = PlayerPrefs.GetInt("reyting");
                reytinqt.text = "Azərbaycan üzrə Reyting\n" + rey;
            }else
            {
                int rey = int.Parse(www.text)+1;
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
    public void statistic()
    {
        SceneManager.LoadScene(3);
    }
	void Start () {
        StartCoroutine(init());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
