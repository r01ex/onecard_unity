using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cards : MonoBehaviour
{

    public int cardnum;
    public bool isfaceup;
    public Sprite cardface;
    public Sprite cardback;
    public bool selectable;
    public bool selected;
    public bool isattacking;
    private SpriteRenderer SpriteRenderer;
    public Sprite[] cardrefer;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        cardface = cardrefer[int.Parse(this.name)];
        cardnum = int.Parse(this.name);
        selected = false;
    }

    public void OnSelected()
    {
        if (selectable)
        {
            if (selected == false)
            {
                transform.position = (Vector3)transform.position + Vector3.up;
                selected = true;
            }
            else if (selected == true)
            {
                transform.position = (Vector3)transform.position - Vector3.up;
                selected = false;
            }
        }
    }

    public void toclub()
    {
        print("toclub called");
        print("the card is : " + cardnum);
        cardface = cardrefer[cardnum % 13];
    }
    public void todia()
    {
        print("todia called");
        print("the card is : " + cardnum);
        cardface = cardrefer[(cardnum % 13) + 13];
    }
    public void toheart()
    {
        print("toheart called");
        print("the card is : " + cardnum);
        cardface = cardrefer[(cardnum % 13) + 26];
    }
    public void tospade()
    {
        print("tospade called");
        print("the card is : " + cardnum);
        cardface = cardrefer[(cardnum % 13) + 39];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(isfaceup==true)
        {
            SpriteRenderer.sprite = cardface;
        }
        else
        {
            SpriteRenderer.sprite = cardback;
        }
    }
}
