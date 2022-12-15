using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;


public class Admin : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField soz, cavab;
    public Button teklifb;
    public Text info, xal;
    public string SERVER_URL = "https://tmhgame.com";
    void Start()
    {
        StartCoroutine(getXal());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void menu() {
         SceneManager.LoadScene(1);
    }
     public void profil() {
         SceneManager.LoadScene(7);
     }
     IEnumerator turnofinfo() {
        yield return new WaitForSeconds(3);
        info.text = "";
     }
     IEnumerator teklifserver()
          {
           string url = SERVER_URL + "/teklif?device=" + SystemInfo.deviceUniqueIdentifier+"&soz="+soz.text+"&cavab="+cavab.text;
                           using (WWW www = new WWW(url))
                           {
                               yield return www;

                               Debug.Log(www.text);



                               if (www.text.Equals("1"))
                               {
                                    soz.text = "";
                                    cavab.text = "";
                                   info.text = "Təklif qəbul edildi.";

                               }
                               else
                               {
                               info.text = "Internet yoxdur.";
                               }
                               teklifb.gameObject.SetActive(true);
                               StartCoroutine(turnofinfo());
                           }
          }
          IEnumerator getXal()
                    {
                     string url = SERVER_URL + "/xal?device=" + SystemInfo.deviceUniqueIdentifier;
                                     using (WWW www = new WWW(url))
                                     {
                                         yield return www;

                                         Debug.Log(www.text);



                                         if (!www.text.Equals("") && www.text.Split('@').Length > 1)
                                         {
                                              string[] a = www.text.Split('@');
                                            string soz = a[0];
                                            string xall = a[1];
                                            xal.text = "Söz " + soz + "    Xal +" + xall;
                                         }
                                         else
                                         {
                                         xal.text = "";
                                         }
                                     }
                    }
     public void teklif() {
        teklifb.gameObject.SetActive(false);
        info.text = "...";
        StartCoroutine(teklifserver());
     }
}
