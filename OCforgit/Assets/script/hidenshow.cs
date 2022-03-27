using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hidenshow : MonoBehaviour
{
    public void hideitself()
    {
        if (this.gameObject.name != "club(Clone)" && this.gameObject.name != "dia(Clone)" && this.gameObject.name != "heart(Clone)" && this.gameObject.name != "spade(Clone)")
        {
            this.gameObject.GetComponent<Button>().enabled = false;
            this.gameObject.GetComponent<Image>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<Button>().enabled = true;
            this.gameObject.GetComponent<Image>().enabled = true;
        }
    }
    public void destroyall()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("suitbutton");
        for (int i=0;i<temp.Length;i++)
        {
            if (temp[i].name == "club(Clone)" || temp[i].name == "dia(Clone)" || temp[i].name == "heart(Clone)" || temp[i].name == "spade(Clone)")
            {
                Destroy(temp[i]);
            }
        }
    }
    private void Start()
    {
        hideitself();
    }
}
