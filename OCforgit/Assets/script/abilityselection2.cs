using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class abilityselection2 : MonoBehaviour
{

    public GameObject playerprefab;
    public Sprite[] buttonimage;
    public Button abil1, abil2, abil3;
    public TextMeshProUGUI abil1txt, abil2txt, abil3txt;
    public GameObject[] buttontxt;
    public GameObject newplayer1, newplayer2, newplayer3, newplayer4;
    // Start is called before the first frame update

    public int numberofabils;
    void choosethree()
    {
        System.Random random = new System.Random();
        int a1 = 0, a2 = 0, a3 = 0;
        while (true)
        {
            int k = random.Next(numberofabils) + 1;   //abil은 1부터 할당
            if (a1 == 0)
            {
                a1 = k;
            }
            if (a2 == 0 && a1 != k)
            {
                a2 = k;
            }
            if (a3 == 0 && a1 != k && a2 != k)
            {
                a3 = k;
            }
            if (a1 != 0 && a2 != 0 && a3 != 0)
            {
                break;
            }
        }
        abil1.image.sprite = buttonimage[a1];
        Instantiate(buttontxt[a1], abil1.transform);
        abil2.image.sprite = buttonimage[a2];
        Instantiate(buttontxt[a2], abil2.transform);
        abil3.image.sprite = buttonimage[a3];
        Instantiate(buttontxt[a3], abil3.transform);
    }
    void Start()
    {
        newplayer1 = Instantiate(playerprefab);
        newplayer1.name = "p1";
        newplayer1.GetComponent<player>().isbot = false;
        newplayer2 = Instantiate(playerprefab);
        newplayer2.name = "p2";
        newplayer2.GetComponent<player>().isbot = true;
        newplayer3 = Instantiate(playerprefab);
        newplayer3.name = "p3";
        newplayer3.GetComponent<player>().isbot = true;
        newplayer4 = Instantiate(playerprefab);
        newplayer4.name = "p4";
        newplayer4.GetComponent<player>().isbot = true;
        choosethree();
    }

    public void scenemove()
    {
        SceneManager.LoadScene("game2");
    }

    public void scenemoveto4()
    {
        SceneManager.LoadScene("game4");
    }

    public void selectabil1()
    {
        for (int i = 0; i <= numberofabils; i++)
        {
            if(abil1.image.sprite==buttonimage[i])
            {
                newplayer1.GetComponent<player>().ability = i;
                break;
            }
        }
    }
    public void selectabil2()
    {
        for (int i = 0; i <= numberofabils; i++)
        {
            if (abil2.image.sprite == buttonimage[i])
            {
                newplayer1.GetComponent<player>().ability = i;
                break;
            }
        }
    }
    public void selectabil3()
    {
        for (int i = 0; i <= numberofabils; i++)
        {
            if (abil3.image.sprite == buttonimage[i])
            {
                newplayer1.GetComponent<player>().ability = i;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
