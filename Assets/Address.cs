using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Address : MonoBehaviour {
    public GameObject but;
    public RectTransform content;
    public Button selected;
    public Button davamButton;
    IEnumerator axtar()
    {

        PlayerPrefs.SetString("versia", "2.8");
        //35.231.39.26
        //127.0.0.1
        string url = "http://35.231.39.26/versia";
        using (WWW www = new WWW(url))
        {
            yield return www;



        }



    }
    void Start () {
        davamButton.onClick.AddListener(() => davam());
        string[] ar = new string[]{"Gəncə","Xankəndi","Lənkəran","Mingəçevir", "Naxçıvan", "Naftalan","Sumqayıt","Şəki","Şirvan","Yevlax","Abşeron","Ağcabədi","Ağdam","Ağdaş","Ağstafa","Ağsu","Astara","Babək-Naxçıvan","Balakən", "Bakı", "Beyləqan","Bərdə","Biləsuvar","Cəbrayıl","Cəlilabad","Culfa-Naxçıvan","Daşkəsən","Füzuli","Gədəbəy","Goranboy","Göyçay","Göygöl","Hacıqabul","Xaçmaz","Xızı","Xocalı","Xocavənd","İmişli","İsmayıllı","Kəlbəcər","Kəngərli-Naxçıvan","Kürdəmir","Qax","Qazax","Qəbələ","Qobustan","Quba","Qubadlı","Qusar","Laçın","Lerik","Lənkəran","Masallı","Neftçala","Oğuz","Ordubad-Naxçıvan","Saatlı","Sabirabad","Salyan","Samux","Sədərək-Naxçıvan","Şabran","Şahbuz-Naxçıvan","Şamaxı","Şəki","Şəmkir","Şərur-Naxçıvan","Şuşa","Tərtər","Tovuz","Ucar","Yardımlı","Yevlax","Zaqatala","Zəngilan","Zərdab",""};
        Button[] B = new Button[ar.Length];
        for (int i = 0; i <= 75; i++)
        {
            GameObject a = (GameObject)Instantiate(but);
            B[i] = a.GetComponent<Button>();
            a.transform.SetParent(content.transform, false);
            Text t = B[i].GetComponentInChildren<Text>();
            t.text = ar[i];
            String s = t.text + "";
            Button b = B[i];
            B[i].onClick.AddListener(() => ButtonClicked(b, s));
        }

    }

    private void ButtonClicked(Button b, String s)
    {
        PlayerPrefs.SetString("reg", s);
        if(selected != null)
        {
            selected.GetComponentInChildren<Image>().color = Color.black;
        }
        Debug.Log(s);
        selected = b;
        selected.GetComponentInChildren<Image>().color = Color.gray;
    }
    private void davam()
    {
        if(selected != null)
        {
            SceneManager.LoadScene(1);
        }else
        {

        }
    }
    // Update is called once per frame
    void Update () {

	}
}
