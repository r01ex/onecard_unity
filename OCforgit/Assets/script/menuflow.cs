using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class menuflow : MonoBehaviour
{
    // Start is called before the first frame update

    public void to2player()
    {
        SceneManager.LoadScene("abilityselect2");
    }

    public void to4player()
    {
        SceneManager.LoadScene("abilityselect4");
    }

    public void tomenu()
    {
        Destroy(GameObject.Find("p1"));
        Destroy(GameObject.Find("p2"));
        Destroy(GameObject.Find("p3"));
        Destroy(GameObject.Find("p4"));
        SceneManager.LoadScene("menu");
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
