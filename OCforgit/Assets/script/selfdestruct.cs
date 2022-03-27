using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selfdestruct : MonoBehaviour
{

    public void destroyself()
    {
        if (this.gameObject.name == "Text(Clone)")
        {
            Destroy(this.gameObject);
        }
    }
}
