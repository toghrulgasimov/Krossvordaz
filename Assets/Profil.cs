using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class Profil : MonoBehaviour
{
    // Start is called before the first frame update

    public InputField ad, soyad, dogum, rayon, kend;
    public Button saxla;
    public Text info;
    void Start()
    {
        ad.text = PlayerPrefs.GetString("adp");
        soyad.text = PlayerPrefs.GetString("soyadp");
        dogum.text = PlayerPrefs.GetString("dogump");
        rayon.text = PlayerPrefs.GetString("rayonp");
        kend.text = PlayerPrefs.GetString("kendp");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator saveinServer()
     {
      string url = "tmhgame.tk/profil?ad=" + ad.text+"&device="+SystemInfo.deviceUniqueIdentifier+"&soyad="+soyad.text+"&dogum="+dogum.text+"&rayon2="+rayon.text+"&kend="+kend.text;
                      using (WWW www = new WWW(url))
                      {
                          yield return www;

                          Debug.Log(www.text);

                          if(www.text.Equals(""))
                                                        {
                                                            info.text = "Sistemdə İş gedir.";
                                                        }else
                                                        {

                                                        }

                          if (www.text.Equals("1"))
                          {
                              PlayerPrefs.SetString("adp", ad.text);
                              PlayerPrefs.SetString("soyadp", soyad.text);
                              PlayerPrefs.SetString("dogump", dogum.text);
                              PlayerPrefs.SetString("rayonp", rayon.text);
                              PlayerPrefs.SetString("kendp", kend.text);
                              info.text = "Yaddaşda saxlanıldı.";

                          }
                          else
                          {
                          info.text = "Sistemdə İş gedir.";

                          }
                          saxla.gameObject.SetActive(true);

                      }
     }
    public void save() {
        saxla.gameObject.SetActive(false);
        StartCoroutine(saveinServer());
    }
    public void menu() {
        SceneManager.LoadScene(1);
    }
}
