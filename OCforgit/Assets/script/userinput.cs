using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userinput : MonoBehaviour
{
    void getmouseclick()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit)
            {
                if(hit.collider.CompareTag("card"))
                {
                    //print(Time.frameCount);
                    GameObject card = hit.transform.gameObject;
                    card.GetComponent<cards>().OnSelected();
                    //card.GetComponent<cards>().selected = true;
                }
            }
        }
    }

   
     // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        getmouseclick();
    }
}
