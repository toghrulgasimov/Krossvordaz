﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;


public class createOyuncu : MonoBehaviour {

    // Use this for initialization
    public InputField input;
    public Text info;
    public string SERVER_URL = "https://tmhgame.com";
    IEnumerator yoxla()
        {
            Debug.Log("-----------------");

            if (input.text.Equals("")) {
                info.text = "Adınızı daxil edin";
            }else
            {
                //
                string url = SERVER_URL + "/g41?name=" + input.text+"&device="+SystemInfo.deviceUniqueIdentifier;
                using (WWW www = new WWW(url))
                {
                    yield return www;

                    Debug.Log(www.text.ToString());
                    if (www.text.IndexOf("5601373") != -1)
                    {
                        string[] t = www.text.Split('|');
                        Debug.Log(www.text + "&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                        PlayerPrefs.SetString("name", t[1]);
                        PlayerPrefs.SetInt("score", int.Parse(t[2]));
                        PlayerPrefs.SetInt("level", int.Parse(t[3]));
                        //PlayerPrefs.SetString("reg", t[4]);
                        SceneManager.LoadScene(3);
                    }
                    else
                    {
                        if(www.text.Equals(""))
                        {
                            info.text = "Sistemdə İş gedir Qeydiyyat dayandırılıb";
                        }else
                        {
                            info.text = input.text + " Mövcuddur başqa ad seçin";
                        }

                    }

                }
            }

        }
    IEnumerator ScoreAndLevel()
        {

            if (input.text.Equals("")) {
                info.text = "Adınızı daxil edin";
            }else
            {
                //
                string url = SERVER_URL + "/?name=" + input.text+"&device="+SystemInfo.deviceUniqueIdentifier;
                using (WWW www = new WWW(url))
                {
                    yield return www;

                    Debug.Log(www.text);
                    if (www.text.Equals("1"))
                    {
                        PlayerPrefs.SetString("name", input.text);
                        PlayerPrefs.SetInt("score", 0);
                        PlayerPrefs.SetInt("level", 1);
                        SceneManager.LoadScene(3);
                    }
                    else
                    {
                        if(www.text.Equals(""))
                        {
                            info.text = "Sistemdə İş gedir Qeydiyyat dayandırılıb";
                        }else
                        {
                            info.text = input.text + " Mövcuddur başqa ad seçin";
                        }

                    }

                }
            }

        }
    public void FF()
    {
        input.shouldHideMobileInput = true;
        StartCoroutine(yoxla());
    }
    private void Awake()
    {
        // PlayerPrefs.DeleteAll();
        //Application.OpenURL("market://details?id=com.by.connaction.connectionname");



        //PlayerPrefs.DeleteKey("reg");
        input.shouldHideMobileInput = true;
        if (PlayerPrefs.HasKey("name"))
        {
            if(PlayerPrefs.HasKey("reg"))
            {
                SceneManager.LoadScene(1);
            }else
            {
                if (!PlayerPrefs.HasKey("score")) PlayerPrefs.SetInt("score", 0);
                PlayerPrefs.SetInt("muss", PlayerPrefs.GetInt("score"));
                SceneManager.LoadScene(3);
            }
            
        }
    }
    
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update () {
		
	}
}
