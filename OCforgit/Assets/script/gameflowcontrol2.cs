using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class gameflowcontrol2 : MonoBehaviour
{
    public List<GameObject> deck = new List<GameObject>();
    public List<GameObject> playedpile = new List<GameObject>();
    public GameObject cardprefab;
    public player p1, p2, p3, p4;
    public player[] playerarr = new player[4];
    public player currentplayer;
    public GameObject errormsg;
    public GameObject suitbutton1, suitbutton2, suitbutton3, suitbutton4; //for 7 change
    public GameObject drawbutton;

    public AudioSource err;
    public AudioSource click;

    //for ability
    public GameObject clockbutton, clckend; //abil1
    public GameObject exodiabutton, exodiaend; //abil2
    public GameObject oraclebuttonprefab, oraclebutton; public int oraclecnt;  //abil4
    public GameObject detectiveprefab, detectivebutton; public GameObject clubprefab, diaprefab, heartprefab, spadeprefab; GameObject dclub, ddia, dheart, dspade; 
    public GameObject[] detectivetextprefab = new GameObject[3]; List<int> detectivetargets = new List<int>(); int currentdetectivetarget; GameObject detectivetxt;
    // Start is called before the first frame update

    public void playerr()
    {
        err.Play();
    }

    public void playclick()
    {
        click.Play();
    }

    void deckgen()
    {
        int i;
        int[] temp = new int[54];
        for (i = 0; i < 54; i++)
        {
            temp[i] = i;
        }
        shuffle(ref temp, 54);
        for (i = 0; i < 54; i++)
        {
            GameObject newcard = Instantiate(cardprefab);
            newcard.name = temp[i].ToString();
            newcard.GetComponent<cards>().isfaceup = false;
            newcard.GetComponent<cards>().selectable = false;
            deck[i] = newcard;
        }

    }
    
    void shuffle (ref int[] list, int n)
    {
        System.Random random = new System.Random();
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            int temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }

    }

    void deckregen()
    {
        int i;
        int pilenum = playedpile.Count - 1;
        int[] temp = new int[pilenum];
        for (i = 0; i < pilenum; i++)
        {
            temp[i] = playedpile[0].GetComponent<cards>().cardnum;
            Destroy(playedpile[0]);
            playedpile.RemoveAt(0);
        }
        shuffle(ref temp, pilenum);
        for (i = 0; i < pilenum; i++)
        {
            GameObject newcard = Instantiate(cardprefab);
            newcard.name = temp[i].ToString();
            newcard.GetComponent<cards>().isfaceup = false;
            newcard.GetComponent<cards>().selectable = false;
            deck.Add(newcard);
        }
        playedpile[0].transform.position = new Vector3(1, 0, 0);
    }


    public TextMeshProUGUI endturntext;
    public GameObject turnendbutton;


    public void p1turnend()
    {
        endturntext.text = "enemy turn";
        turnendbutton.GetComponent<Button>().interactable = false;
        drawbutton.GetComponent<Button>().interactable = false;
        if (p1.ability == 1)
        {
            if (clckend.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text != "used")
            {
                clckend.GetComponent<Button>().interactable = false;
                clckend.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "enemy turn";
            }
        }
        else if (p1.ability == 2)
        {
            int cnter = 0;
            foreach (GameObject cardinhand in currentplayer.hand)
            {
                print(cardinhand);
                if (cardinhand.GetComponent<cards>().cardnum % 13 == 12)
                {
                    print("count");
                    cnter++;
                }
            }
            if (cnter == 4)
            {
                exodiaend.GetComponent<Button>().interactable = true;
            }
        }
        else if (p1.ability == 4)
        {
            drawbutton.GetComponent<Image>().sprite = cardprefab.GetComponent<cards>().cardback;
        }
    }
    public void startturn()
    {
        victory();
        
        if (currentplayer == p1) //user turn end
        {
            print("p2turn");
            currentplayer = p2;
        }
        else if(currentplayer==p2)
        {
            print("p3turn");
            currentplayer = p3;
        }
        else if (currentplayer == p3)
        {
            print("p2turn");
            currentplayer = p4;
        }
        else if (currentplayer == p4)//bot turn end
        {
            print("p1turn");
            currentplayer = p1;
        }
        if (currentplayer.isbot)  //bot turn start
        {
            bool playedcard = false;
            endturntext.text = "enemy turn";
            turnendbutton.GetComponent<Button>().interactable = false;
            drawbutton.GetComponent<Button>().interactable = false;

            //bot logic
            foreach (GameObject cardinhand in currentplayer.hand)
            {
                //print(cardinhand.GetComponent<cards>().cardnum);
                if (legalcheck(cardinhand.GetComponent<cards>().cardnum))
                {
                    if (cardinhand.GetComponent<cards>().cardnum % 13 == 6)
                    {
                        cardinhand.GetComponent<cards>().selected = true;
                        handtopile();
                        playedcard = true;
                        Invoke("startturn", 1.9f);
                    }
                    else
                    {
                        cardinhand.GetComponent<cards>().selected = true;
                        handtopile();
                        playedcard = true;
                        Invoke("startturn", 1f);
                    }
                    break;
                }
            }
            if (playedcard == false)
            {
                print("going to draw");
                drawfromdeck();
                Invoke("startturn", 1f);
            }
            if (p1.ability == 5)
            {
                print("curplayer is" + currentplayer);
                foreach (GameObject card in currentplayer.hand)
                {
                    card.GetComponent<cards>().isfaceup = false;
                }
                currentplayer.hand[0].GetComponent<cards>().isfaceup = true;
                currentplayer.hand[currentplayer.hand.Count - 1].GetComponent<cards>().isfaceup = true;
            }
        }
        else //user turn start
        {
            endturntext.text = "play&end";
            turnendbutton.GetComponent<Button>().interactable = true;
            drawbutton.GetComponent<Button>().interactable = true;
            if (p1.ability == 1)
            {
                if (clckend.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text != "used")
                {
                    clckend.GetComponent<Button>().interactable = true;
                    clckend.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "end turn\n(ability)";
                }
            }
            else if (p1.ability == 4 && oraclecnt > 0)
            {
                oraclebutton.GetComponent<Button>().interactable = true;
            }
            else if (p1.ability == 7)
            {
                currentdetectivetarget = 0;
                detectivetargets.Clear();
                for (int i = 1; i < 4; i++)
                {
                    if(playerarr[i].hand.Count==1)
                    {
                        detectivetargets.Add(i);
                    }
                }
                if (detectivetargets.Count != 0)
                {
                    currentdetectivetarget = 0;
                    detectivebutton.GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    public void drawfromdeckatstart()
    {
        if (currentplayer == p1)
        {
            GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x - 6 + (p1.hand.Count % 12), transform.position.y - 3.5f - (p1.hand.Count / 12) * 0.5f, transform.position.z - p1.hand.Count), Quaternion.identity);
            p1.hand.Add(newcard);
            newcard.name = deck[deck.Count - 1].name;
            newcard.GetComponent<cards>().isfaceup = true;
            newcard.GetComponent<cards>().selectable = true;
            newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
        }
        else if (currentplayer == p2)
        {
            GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x - 8.5f - (p2.hand.Count / 12), transform.position.y + 3 - (p2.hand.Count % 12) * 0.5f, transform.position.z - p2.hand.Count), Quaternion.Euler(0, 0, 90));
            p2.hand.Add(newcard);
            newcard.name = deck[deck.Count - 1].name;
            newcard.GetComponent<cards>().isfaceup = false;
            newcard.GetComponent<cards>().selectable = false;
            newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
        }
        else if (currentplayer == p3)
        {
            GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x - 6 + (p3.hand.Count % 12), transform.position.y + 3.5f + (p3.hand.Count / 12) * 0.5f, transform.position.z - p3.hand.Count), Quaternion.identity);
            p3.hand.Add(newcard);
            newcard.name = deck[deck.Count - 1].name;
            newcard.GetComponent<cards>().isfaceup = false;
            newcard.GetComponent<cards>().selectable = false;
            newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
        }
        else if (currentplayer == p4)
        {
            GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x + 8.5f + (p4.hand.Count / 12), transform.position.y + 3 - (p4.hand.Count % 12) * 0.5f, transform.position.z - p4.hand.Count), Quaternion.Euler(0, 0, 90));
            p4.hand.Add(newcard);
            newcard.name = deck[deck.Count - 1].name;
            newcard.GetComponent<cards>().isfaceup = false;
            newcard.GetComponent<cards>().selectable = false;
            newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
        }
        Destroy(deck[deck.Count - 1]);
        deck.RemoveAt(deck.Count - 1);
    }

    public void drawfromdeck()
    {
        playclick();
        GameObject topofpile = playedpile[playedpile.Count - 1];
        if (topofpile.GetComponent<cards>().isattacking == false)
        {

            if (deck.Count == 0)
            {
                deckregen();
            }

            //print("draw1");
            if (currentplayer == p1)
            {
                if (p1.ability == 6)
                {
                    GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x - 6 + (p1.hand.Count % 12), transform.position.y - 3.5f - (p1.hand.Count / 12) * 0.5f, transform.position.z - p1.hand.Count), Quaternion.identity);
                    p1.hand.Add(newcard);
                    newcard.name = deck[deck.Count - 1].name;
                    newcard.GetComponent<cards>().isfaceup = true;
                    newcard.GetComponent<cards>().selectable = true;
                    newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
                    Destroy(deck[deck.Count - 1]);
                    deck.RemoveAt(deck.Count - 1);
                    GameObject newcard2 = Instantiate(cardprefab, new Vector3(transform.position.x - 6 + (p1.hand.Count % 12), transform.position.y - 3.5f - (p1.hand.Count / 12) * 0.5f, transform.position.z - p1.hand.Count), Quaternion.identity);
                    p1.hand.Add(newcard2);
                    newcard2.name = deck[deck.Count - 1].name;
                    newcard2.GetComponent<cards>().isfaceup = true;
                    newcard2.GetComponent<cards>().selectable = true;
                    newcard2.GetComponent<cards>().cardnum = int.Parse(newcard2.name);
                }
                else
                {
                    GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x - 6 + (p1.hand.Count % 12), transform.position.y - 3.5f - (p1.hand.Count / 12) * 0.5f, transform.position.z - p1.hand.Count), Quaternion.identity);
                    p1.hand.Add(newcard);
                    newcard.name = deck[deck.Count - 1].name;
                    newcard.GetComponent<cards>().isfaceup = true;
                    newcard.GetComponent<cards>().selectable = true;
                    newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
                }
            }
            else if (currentplayer == p2)
            {
                GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x - 8.5f - (p2.hand.Count / 12) * 0.5f, transform.position.y + 3 - (p2.hand.Count % 12) * 0.5f, transform.position.z - p2.hand.Count), Quaternion.Euler(0, 0, 90));
                p2.hand.Add(newcard);
                newcard.name = deck[deck.Count - 1].name;
                newcard.GetComponent<cards>().isfaceup = false;
                newcard.GetComponent<cards>().selectable = false;
                newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
            }
            else if (currentplayer == p3)
            {
                GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x - 6 + (p3.hand.Count % 12), transform.position.y + 3.5f + (p3.hand.Count / 12) * 0.5f, transform.position.z - p3.hand.Count), Quaternion.identity);
                p3.hand.Add(newcard);
                newcard.name = deck[deck.Count - 1].name;
                newcard.GetComponent<cards>().isfaceup = false;
                newcard.GetComponent<cards>().selectable = false;
                newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
            }
            else if (currentplayer == p4)
            {
                GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x + 8.5f + (p4.hand.Count / 12) * 0.5f, transform.position.y + 3 - (p4.hand.Count % 12) * 0.5f, transform.position.z - p4.hand.Count), Quaternion.Euler(0, 0, 90));
                p4.hand.Add(newcard);
                newcard.name = deck[deck.Count - 1].name;
                newcard.GetComponent<cards>().isfaceup = false;
                newcard.GetComponent<cards>().selectable = false;
                newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
            }
            Destroy(deck[deck.Count - 1]);
            deck.RemoveAt(deck.Count - 1);
        }
        else
        {
            int cnt, i;
            i = playedpile.Count - 1;
            cnt = 0;
            while (i >= 0)
            {
                //print("going through attack cnt" + playedpile[i]);
                //print("cardnum of attack card is " + playedpile[i].GetComponent<cards>().cardnum);
                if (playedpile[i].GetComponent<cards>().isattacking == false)
                {
                    break;
                }
                else
                {
                    playedpile[i].GetComponent<cards>().isattacking = false;
                    if (playedpile[i].GetComponent<cards>().cardnum == 52) //black joker
                    {
                        cnt += 5;
                    }
                    else if (playedpile[i].GetComponent<cards>().cardnum == 53) //red joker
                    {
                        cnt += 5;
                    }
                    else if (playedpile[i].GetComponent<cards>().cardnum % 13 == 0)
                    {
                        cnt += 3;
                    }
                    else if (playedpile[i].GetComponent<cards>().cardnum % 13 == 1)
                    {
                        cnt += 2;
                    }
                }
                //print("count is currently" + cnt);
                i--;
            }

            if (cnt >= deck.Count - 1)
            {
                deckregen();
            }

            //print("draw" + cnt);
            if (currentplayer == p1)
            {
                for (i = 0; i < cnt; i++)
                {
                    GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x - 6 + (p1.hand.Count % 12), transform.position.y - 3.5f - (p1.hand.Count / 12) * 0.5f, transform.position.z - p1.hand.Count), Quaternion.identity);
                    p1.hand.Add(newcard);
                    newcard.name = deck[deck.Count - 1].name;
                    newcard.GetComponent<cards>().isfaceup = true;
                    newcard.GetComponent<cards>().selectable = true;
                    newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
                    Destroy(deck[deck.Count - 1]);
                    deck.RemoveAt(deck.Count - 1);
                }
            }
            else if (currentplayer == p2)
            {
                for (i = 0; i < cnt; i++)
                {
                    GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x - 8.5f - (p2.hand.Count / 12) * 0.5f, transform.position.y + 3 - (p2.hand.Count % 12) * 0.5f, transform.position.z - p2.hand.Count), Quaternion.Euler(0, 0, 90));
                    p2.hand.Add(newcard);
                    newcard.name = deck[deck.Count - 1].name;
                    newcard.GetComponent<cards>().isfaceup = false;
                    newcard.GetComponent<cards>().selectable = false;
                    newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
                    Destroy(deck[deck.Count - 1]);
                    deck.RemoveAt(deck.Count - 1);
                }
            }
            else if (currentplayer == p3)
            {
                for (i = 0; i < cnt; i++)
                {
                    GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x - 6 + (p3.hand.Count % 12), transform.position.y + 3.5f + (p3.hand.Count / 12) * 0.5f, transform.position.z - p3.hand.Count), Quaternion.identity);
                    p3.hand.Add(newcard);
                    newcard.name = deck[deck.Count - 1].name;
                    newcard.GetComponent<cards>().isfaceup = false;
                    newcard.GetComponent<cards>().selectable = false;
                    newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
                    Destroy(deck[deck.Count - 1]);
                    deck.RemoveAt(deck.Count - 1);
                }
            }
            else if (currentplayer == p4)
            {
                for (i = 0; i < cnt; i++)
                {
                    GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x + 8.5f + (p4.hand.Count / 12) * 0.5f, transform.position.y + 3 - (p4.hand.Count % 12) * 0.5f, transform.position.z - p4.hand.Count), Quaternion.Euler(0, 0, 90));
                    p4.hand.Add(newcard);
                    newcard.name = deck[deck.Count - 1].name;
                    newcard.GetComponent<cards>().isfaceup = false;
                    newcard.GetComponent<cards>().selectable = false;
                    newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
                    Destroy(deck[deck.Count - 1]);
                    deck.RemoveAt(deck.Count - 1);
                }
            }
        }
    }

    void initialdeal()
    {
        GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z - playedpile.Count), Quaternion.identity);
        playedpile.Add(newcard);
        newcard.name = deck[deck.Count - 1].name;
        newcard.GetComponent<cards>().isfaceup = true;
        newcard.GetComponent<cards>().selectable = false;
        Destroy(deck[deck.Count - 1]);
        deck.RemoveAt(deck.Count - 1);
        if (p1.ability == 6)
        {
            for (int i = 0; i < 3; i++)
            {
                drawfromdeckatstart();
            }
            currentplayer = p2;
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                drawfromdeckatstart();
            }
            currentplayer = p2;
        }
        for (int i = 0; i < 5; i++)
        {
            drawfromdeckatstart();
        }
        currentplayer = p3;
        for (int i = 0; i < 5; i++)
        {
            drawfromdeckatstart();
        }
        currentplayer = p4;
        for (int i = 0; i < 5; i++)
        {
            drawfromdeckatstart();
        }
        currentplayer = p1;
    }

    public bool legalcheck(int cardnum)
    {
        int shape = cardnum / 13;  //0: club , 1: dia , 2: heart , 3: spade
        int number = cardnum % 13;
        int temp;
        GameObject topofpile = playedpile[playedpile.Count - 1];
        temp = int.Parse(topofpile.name);
        /*
        print("topofpile in legalcheck" + topofpile);
        print("topofpile num in legalcheck as getcomp" + temp);
        print("topofpile name in legalcheck as getcomp" + topofpile.name);
        print("temp is " + temp);
        */
        for (int i = 0; i < 54; i++)
        {
           // print("searching in legalcheck" + i);
            if (GameObject.Find(topofpile.name).GetComponent<cards>().cardface == cardprefab.GetComponent<cards>().cardrefer[i])
            {
                //print("i in legalcheck:" + i);
                temp = i;
                break;
            }
        }
        if (cardnum == 52 || cardnum == 53)
        {
            if (currentplayer.hand.Count == 1)
            {
                if (currentplayer.ability != 3) //abil is not joker 
                {
                    return false;
                }
            }
        }
        if (topofpile.GetComponent<cards>().isattacking == false)
        {
            if (cardnum != 52 && cardnum != 53)
            {
                if (shape == temp / 13)
                {
                    return true;
                }
                if (number == temp % 13)
                {
                    return true;
                }
                if ((cardnum <= 12 || (cardnum >= 39 && cardnum <= 51)) && temp == 52)  // played card is black & top is blackjoker
                {
                    return true;
                }
                if ((cardnum >= 13 && cardnum <= 38) && temp == 53) // played card is red & top is redjoker
                {
                    return true;
                }
            }
            if (cardnum == 52) //played card is black joker
            {
                if (temp <= 12 || (temp >= 39 && temp <= 51))  // top is black
                {
                    return true;
                }
                if (temp == 53)
                {
                    return true;
                }
            }
            if (cardnum == 53) //played card is red joker
            {
                if (temp >= 13 && temp <= 38)
                {
                    return true;
                }
                if (temp == 52)
                {
                    return true;
                }
            }

        }
        else
        {
            if (temp == 52)
            {
                if (cardnum == 53)
                {
                    return true;
                }
            }
            else if (temp == 53)
            {
                if (cardnum == 52)
                {
                    return true;
                }
            }
            else if (temp % 13 == 1)  //top is atk and 2
            {
                if (cardnum % 13 == 1)
                {
                    return true;
                }
                else if ((temp / 13 == cardnum / 13) && cardnum % 13 == 0)
                {
                    return true;
                }
                else if (temp <= 12 || (temp >= 39 && temp <= 51))  // top is black
                {
                    if (cardnum == 52)
                    {
                        return true;
                    }
                }
                else if (temp >= 13 && temp <= 38)
                {
                    if (cardnum == 53)
                    {
                        return true;
                    }
                }
            }
            else if (temp % 13 == 0)  //top is atk and 1
            {
                if (cardnum % 13 == 0)
                {
                    return true;
                }
                else if (temp <= 12 || (temp >= 39 && temp <= 51))  // top is black
                {
                    if (cardnum == 52)
                    {
                        return true;
                    }
                }
                else if (temp >= 13 && temp <= 38)
                {
                    if (cardnum == 53)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void changetoclub()
    {
        playedpile[playedpile.Count - 1].GetComponent<cards>().toclub();
    }
    public void changetodia()
    {
        playedpile[playedpile.Count - 1].GetComponent<cards>().todia();
    }
    public void changetoheart()
    {
        playedpile[playedpile.Count - 1].GetComponent<cards>().toheart();
    }
    public void changetospade()
    {
        playedpile[playedpile.Count - 1].GetComponent<cards>().tospade();
    }

    public void handtopile()
    {
        //print("current player is : " + currentplayer);
        int i, cnt=0, index, n;
        n = currentplayer.hand.Count;
        index = n;
        for (i = 0; i < n; i++)
        {
            if (currentplayer.hand[i].GetComponent<cards>().selected == true)
            {
                cnt++;
                index = i;
            }
        }
        //print("there are selected card of : " + cnt);
        //print("it is at index : " + index);
        if (cnt == 1)
        {
           // print("top card in handtopile is : " + playedpile[playedpile.Count - 1]);
            if (legalcheck(currentplayer.hand[index].GetComponent<cards>().cardnum))
            {
                GameObject newcard = Instantiate(cardprefab, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z - playedpile.Count), Quaternion.identity);
                newcard.name = currentplayer.hand[index].name;
                newcard.GetComponent<cards>().isfaceup = true;
                newcard.GetComponent<cards>().selectable = false;
                newcard.GetComponent<cards>().cardnum = int.Parse(newcard.name);
                if (currentplayer.hand[index].GetComponent<cards>().cardnum % 13 == 0 || currentplayer.hand[index].GetComponent<cards>().cardnum % 13 == 1 || currentplayer.hand[index].GetComponent<cards>().cardnum == 52 || currentplayer.hand[index].GetComponent<cards>().cardnum == 53)
                {
                    newcard.GetComponent<cards>().isattacking = true;
                }
                playedpile.Add(newcard);
                Destroy(currentplayer.hand[index]);
                currentplayer.hand.RemoveAt(index);
                if (currentplayer == p1 || currentplayer == p3)
                {
                    for (i = index; i < currentplayer.hand.Count; i++)
                    {
                        currentplayer.hand[i].transform.position += Vector3.left;
                        currentplayer.hand[i].transform.position += Vector3.forward;                        
                    }
                    for (i = 0; i < currentplayer.hand.Count; i++)
                    {
                        if (currentplayer.hand[i].transform.position.x <= transform.position.x - 7)
                        {
                            if (currentplayer == p1)
                            {
                                currentplayer.hand[i].transform.position += new Vector3(12, 0.5f, 0);
                            }
                            else if (currentplayer == p3)
                            {
                                currentplayer.hand[i].transform.position += new Vector3(12, -0.5f, 0);
                            }
                        }
                    }
                }
                else if (currentplayer == p2 || currentplayer == p4)
                {
                    for (i = index; i < currentplayer.hand.Count; i++)
                    {
                        currentplayer.hand[i].transform.position += Vector3.up * 0.5f;
                        currentplayer.hand[i].transform.position += Vector3.forward;
                    }
                    for (i = 0; i < currentplayer.hand.Count; i++)
                    {
                        if (currentplayer.hand[i].transform.position.y >= transform.position.y + 3.5)
                        {
                            if (currentplayer == p2)
                            {
                                currentplayer.hand[i].transform.position += new Vector3(0.5f, -6f, 0);
                            }
                            else if (currentplayer == p4)
                            {
                                currentplayer.hand[i].transform.position += new Vector3(-0.5f, -6f, 0);
                            }
                        }
                    }
                }
                playclick();
                if (newcard.GetComponent<cards>().cardnum % 13 == 6)
                {
                    if (currentplayer.isbot == true)
                    {
                        int max = 0, maxindex = 0;
                        int[] tmp = new int[5];
                        foreach(GameObject cardinhand in currentplayer.hand)
                        {
                            tmp[cardinhand.GetComponent<cards>().cardnum / 13]++;
                        }
                        for(i=0;i<4;i++)
                        {
                            if(tmp[i]>max)
                            {
                                max = tmp[i];
                                maxindex = i;
                            }
                        }
                        if (maxindex == 0)
                        {
                            Invoke("changetoclub", 1f);
                        }
                        else if (maxindex == 1)
                        {
                            Invoke("changetodia", 1f);
                        }
                        else if (maxindex == 2)
                        {
                            Invoke("changetoheart", 1f);
                        }
                        else if (maxindex == 3)
                        {
                            Invoke("changetospade", 1f);
                        }                      
                    }
                    else
                    {
                        Canvas _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                        GameObject newbutton1 = Instantiate(suitbutton1, _canvas.transform);
                        GameObject newbutton2 = Instantiate(suitbutton2, _canvas.transform);
                        GameObject newbutton3 = Instantiate(suitbutton3, _canvas.transform);
                        GameObject newbutton4 = Instantiate(suitbutton4, _canvas.transform);
                        newbutton1.GetComponent<Button>().onClick.AddListener(delegate { Invoke("startturn", 1f); });
                        newbutton1.GetComponent<Button>().onClick.AddListener(p1turnend);
                        newbutton2.GetComponent<Button>().onClick.AddListener(delegate { Invoke("startturn", 1f); });
                        newbutton2.GetComponent<Button>().onClick.AddListener(p1turnend);
                        newbutton3.GetComponent<Button>().onClick.AddListener(delegate { Invoke("startturn", 1f); });
                        newbutton3.GetComponent<Button>().onClick.AddListener(p1turnend);
                        newbutton4.GetComponent<Button>().onClick.AddListener(delegate { Invoke("startturn", 1f); });
                        newbutton4.GetComponent<Button>().onClick.AddListener(p1turnend);
                    }
                }                
                else if(currentplayer.isbot==false)
                {
                    p1turnend();
                    Invoke("startturn", 1f);
                }
            }
            else
            {
                Canvas _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                playerr();
                GameObject msg = Instantiate(errormsg, _canvas.transform);
            }
        }
        else
        {
            Canvas _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
            playerr();
            GameObject msg = Instantiate(errormsg, _canvas.transform);
        }
    }


    public void victory()    //victory and lose
    {
        if(currentplayer.hand.Count==0)
        {
            if (currentplayer == p1)
            {
                SceneManager.LoadScene("victory");
            }
            else
            {
                SceneManager.LoadScene("lose");
            }
        }
    }


    public void ability1handler(GameObject button)
    {
        button.GetComponent<Button>().interactable = false;
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "used";
    }

    public void ability2handler()
    {
        SceneManager.LoadScene("victory");
    }

    public void ability4handler(GameObject button)
    {
        Sprite topofdrawpile = deck[deck.Count-1].GetComponent<cards>().cardface;
        oraclecnt--;
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = oraclecnt + " uses\nremain";
        drawbutton.GetComponent<Image>().sprite = topofdrawpile;
        oraclebutton.GetComponent<Button>().interactable = false;
    }

    public void ability7handler()
    {
        Canvas _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        detectivebutton.GetComponent<Button>().interactable = false;
        detectivetxt = Instantiate(detectivetextprefab[detectivetargets[currentdetectivetarget]-1], _canvas.transform);
        dclub = Instantiate(clubprefab, _canvas.transform);
        ddia = Instantiate(diaprefab, _canvas.transform);
        dheart = Instantiate(heartprefab, _canvas.transform);
        dspade = Instantiate(spadeprefab, _canvas.transform);
        dclub.GetComponent<Button>().onClick.AddListener(dclubcheck);
        ddia.GetComponent<Button>().onClick.AddListener(ddiacheck);
        dheart.GetComponent<Button>().onClick.AddListener(dheartcheck);
        dspade.GetComponent<Button>().onClick.AddListener(dspadecheck);
    }
    private void dclubcheck()
    {
        if (playerarr[detectivetargets[currentdetectivetarget]].hand[0].GetComponent<cards>().cardnum >= 0 && playerarr[detectivetargets[currentdetectivetarget]].hand[0].GetComponent<cards>().cardnum <= 12)
        {
            currentplayer = playerarr[detectivetargets[currentdetectivetarget]];
            bool atktmp= playedpile[playedpile.Count - 1].GetComponent<cards>().isattacking;
            playedpile[playedpile.Count - 1].GetComponent<cards>().isattacking = false;
            drawfromdeck();
            playedpile[playedpile.Count - 1].GetComponent<cards>().isattacking = atktmp;
            currentplayer = p1;
        }
        Destroy(dclub);
        Destroy(ddia);
        Destroy(dheart);
        Destroy(dspade);
        Destroy(detectivetxt);
        if(currentdetectivetarget+1 < detectivetargets.Count)
        {
            currentdetectivetarget++;
            detectivebutton.GetComponent<Button>().interactable = true;
        }
    }
    private void ddiacheck()
    {
        if (playerarr[detectivetargets[currentdetectivetarget]].hand[0].GetComponent<cards>().cardnum >= 13 && playerarr[detectivetargets[currentdetectivetarget]].hand[0].GetComponent<cards>().cardnum <= 25)
        {
            currentplayer = playerarr[detectivetargets[currentdetectivetarget]];
            bool atktmp = playedpile[playedpile.Count - 1].GetComponent<cards>().isattacking;
            playedpile[playedpile.Count - 1].GetComponent<cards>().isattacking = false;
            drawfromdeck();
            playedpile[playedpile.Count - 1].GetComponent<cards>().isattacking = atktmp;
            currentplayer = p1;
        }
        Destroy(dclub);
        Destroy(ddia);
        Destroy(dheart);
        Destroy(dspade);
        Destroy(detectivetxt);
        if (currentdetectivetarget + 1 < detectivetargets.Count)
        {
            currentdetectivetarget++;
            detectivebutton.GetComponent<Button>().interactable = true;
        }
    }
    private void dheartcheck()
    {
        if (playerarr[detectivetargets[currentdetectivetarget]].hand[0].GetComponent<cards>().cardnum >= 26 && playerarr[detectivetargets[currentdetectivetarget]].hand[0].GetComponent<cards>().cardnum <= 38)
        {
            currentplayer = playerarr[detectivetargets[currentdetectivetarget]];
            bool atktmp = playedpile[playedpile.Count - 1].GetComponent<cards>().isattacking;
            playedpile[playedpile.Count - 1].GetComponent<cards>().isattacking = false;
            drawfromdeck();
            playedpile[playedpile.Count - 1].GetComponent<cards>().isattacking = atktmp;
            currentplayer = p1;
        }
        Destroy(dclub);
        Destroy(ddia);
        Destroy(dheart);
        Destroy(dspade);
        Destroy(detectivetxt);
        if (currentdetectivetarget + 1 < detectivetargets.Count)
        {
            currentdetectivetarget++;
            detectivebutton.GetComponent<Button>().interactable = true;
        }
    }
    private void dspadecheck()
    {
        if (playerarr[detectivetargets[currentdetectivetarget]].hand[0].GetComponent<cards>().cardnum >= 39 && playerarr[detectivetargets[currentdetectivetarget]].hand[0].GetComponent<cards>().cardnum <= 51)
        {
            currentplayer = playerarr[detectivetargets[currentdetectivetarget]];
            bool atktmp = playedpile[playedpile.Count - 1].GetComponent<cards>().isattacking;
            playedpile[playedpile.Count - 1].GetComponent<cards>().isattacking = false;
            drawfromdeck();
            playedpile[playedpile.Count - 1].GetComponent<cards>().isattacking = atktmp;
            currentplayer = p1;
        }
        Destroy(dclub);
        Destroy(ddia);
        Destroy(dheart);
        Destroy(dspade);
        Destroy(detectivetxt);
        if (currentdetectivetarget + 1 < detectivetargets.Count)
        {
            currentdetectivetarget++;
            detectivebutton.GetComponent<Button>().interactable = true;
        }
    }

    void Start()
    {
        p1 = GameObject.Find("p1").GetComponent<player>();
        p2 = GameObject.Find("p2").GetComponent<player>();
        p3 = GameObject.Find("p3").GetComponent<player>();
        p4 = GameObject.Find("p4").GetComponent<player>();
        playerarr[0] = p1;
        playerarr[1] = p2;
        playerarr[2] = p3;
        playerarr[3] = p4;
        currentplayer = p1;
        deckgen();
        initialdeal();
        drawbutton.GetComponent<Button>().onClick.AddListener(delegate { Invoke("startturn", 1f); });      
        if (p1.ability == 1)
        {
            Canvas _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
            clckend = Instantiate(clockbutton, _canvas.transform);
            clckend.GetComponent<Button>().onClick.AddListener(startturn);
            clckend.GetComponent<Button>().onClick.AddListener(delegate { ability1handler(clckend); });
        }
        else if (p1.ability == 2)
        {
            Canvas _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
            exodiaend = Instantiate(exodiabutton, _canvas.transform);
            exodiaend.GetComponent<Button>().interactable = false;
            exodiaend.GetComponent<Button>().onClick.AddListener(ability2handler);
        }
        else if (p1.ability == 4)
        {
            Canvas _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
            oraclebutton = Instantiate(oraclebuttonprefab, _canvas.transform);
            oraclecnt = 5;
            oraclebutton.GetComponent<Button>().onClick.AddListener(delegate { ability4handler(oraclebutton); });
        }
        else if (p1.ability == 5)
        {
            p2.hand[0].GetComponent<cards>().isfaceup = true;
            p2.hand[currentplayer.hand.Count - 1].GetComponent<cards>().isfaceup = true;
            p3.hand[0].GetComponent<cards>().isfaceup = true;
            p3.hand[currentplayer.hand.Count - 1].GetComponent<cards>().isfaceup = true;
            p4.hand[0].GetComponent<cards>().isfaceup = true;
            p4.hand[currentplayer.hand.Count - 1].GetComponent<cards>().isfaceup = true;
        }
        else if (p1.ability == 7)
        {
            Canvas _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
            detectivebutton = Instantiate(detectiveprefab, _canvas.transform);
            detectivebutton.GetComponent<Button>().onClick.AddListener(ability7handler);
            detectivebutton.GetComponent<Button>().interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
