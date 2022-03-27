using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class drawtext : MonoBehaviour
{
    public TextMeshProUGUI buttontext;
    public gameflowcontrol gameflowcontrol;

    // Start is called before the first frame update

    void updatetext()
    {

        cards topofpile = gameflowcontrol.playedpile[gameflowcontrol.playedpile.Count - 1].GetComponent<cards>();
        if (topofpile.isattacking == false)
        {
            if (gameflowcontrol.p1.ability == 6)
            {
                buttontext.text = "click\nto\ndraw 2";
            }
            else
            {
                buttontext.text = "click\nto\ndraw 1";
            }
        }
        else
        {
            int cnt = 0, i;
            i = gameflowcontrol.playedpile.Count - 1;
            while (i >= 0)
            {
                if (gameflowcontrol.playedpile[i].GetComponent<cards>().isattacking == false)
                {
                    break;
                }
                else
                {
                    if (gameflowcontrol.playedpile[i].GetComponent<cards>().cardnum == 52) //black joker
                    {
                        cnt += 5;
                    }
                    else if (gameflowcontrol.playedpile[i].GetComponent<cards>().cardnum == 53) //red joker
                    {
                        cnt += 5;
                    }
                    else if (gameflowcontrol.playedpile[i].GetComponent<cards>().cardnum % 13 == 0)
                    {
                        cnt += 3;
                    }
                    else if (gameflowcontrol.playedpile[i].GetComponent<cards>().cardnum % 13 == 1)
                    {
                        cnt += 2;
                    }
                }
                i--;
            }
            buttontext.text = "click\nto\ndraw " + cnt;
        }

    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        updatetext();
    }
}
