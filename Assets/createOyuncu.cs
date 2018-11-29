using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;


public class createOyuncu : MonoBehaviour {

    // Use this for initialization
    public InputField input;
    public Text info;
    IEnumerator yoxla()
    {

        if (input.text.Equals("")) {
            info.text = "Adınızı daxil edin";
        }else
        {
            //
            string url = "35.227.46.95/?name=" + input.text;
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
        //PlayerPrefs.DeleteAll();
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
