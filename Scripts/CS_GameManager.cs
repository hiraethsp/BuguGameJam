using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CS_GameManager : MonoBehaviour
{
    private static CS_GameManager instance = null;
    public static CS_GameManager Instance { get { return instance; } }
    public List<Box> boxList = new List<Box>();
    public GameObject boxPrefeb;
    public float timer;
    public float refreshTime = 20f;
    public float refreshTimer = 0f;
    public float step = 1f;
    public float fieldX = 8f;
    public float fieldY = 4f;
    public float[,] vis = new float[50, 50];
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI otherText;
    public TextMeshProUGUI endText;
    public GameObject player1;
    public GameObject player2;
    public bool end;
    public GameObject button;
    private void Awake()
    {
        end = false;
        //if (!isServer) return;
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        step = 1f;
    }
    // Start is called before the first frame update
    void Start()
    {
        button.SetActive(false);
        endText.text = "";
        refreshTime = 20f;
        refreshTimer = 20f;
        timer = 120f;
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                vis[i, j] = 0;
            }
        }
        int num = 0;
        while (num <= 48)
        {
            int ranX = Random.Range(-8, 9);
            int ranY = Random.Range(-4, 5);
            if ((ranX == 5 || ranX == -5) && ranY == 0) continue;
            if (vis[ranX + (int)fieldX, ranY + (int)fieldY] == 0)
            {
                vis[ranX + (int)fieldX, ranY + (int)fieldY] = 1;
                num++;
                Vector2 itemPosition = new Vector2(ranX, ranY);
                GameObject myBox = GameObject.Instantiate(boxPrefeb, itemPosition, Quaternion.identity);
                CS_GameManager.Instance.boxList.Add(myBox.GetComponent<Box>());
                myBox.GetComponent<Box>().myBelong = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (end) return;
        timer -= Time.deltaTime;
        refreshTimer -= Time.deltaTime;
        timeText.text = ((int)timer).ToString();
        if (timer <= 0)
        {
            End();
        }
        if (refreshTimer <= 0)
        {
            refreshTimer = refreshTime;
            Refresh();
        }
        //if (refreshTimer <= 0)
        //{
        //    refreshTimer = refreshTime;
        //    int max = 22;
        //    int min = 1;
        //    if (boxList.Count <= 40)
        //    {
        //        min = 1;
        //        max = 14;
        //    }
        //    else if (boxList.Count <= 80)
        //    {
        //        min = 1;
        //        max = 22;
        //    }
        //    else
        //    {
        //        min = 13;
        //        max = 22;
        //    }
        //    int ran = Random.Range(min, max);
        //    while (ran == player1.GetComponent<Player>().state) ran = Random.Range(min, max);
        //    player1.GetComponent<Player>().state = ran;
        //    Debug.Log(ran);
        //    ran = Random.Range(min, max);
        //    while (ran == player1.GetComponent<Player>().state || ran == player2.GetComponent<Other>().state) ran = Random.Range(min, max);
        //    player2.GetComponent<Other>().state = ran;
        //}
        //int playerNum = 0;
        //for (int i = 0; i < boxList.Count; i++)
        //{
        //    if (boxList[i].GetComponent<Box>().myBelong == 1)
        //    {
        //        playerNum++;
        //    }
        //}
        //playerText.text = playerNum.ToString();
        //int otherNum = 0;
        //for (int i = 0; i < boxList.Count; i++)
        //{
        //    if (boxList[i].GetComponent<Box>().myBelong == 2)
        //    {
        //        otherNum++;
        //    }
        //}
        //otherText.text = otherNum.ToString();
    }
    void End()
    {
        end = true;
        button.SetActive(true);
        int playerNum = 0;
        for (int i = 0; i < boxList.Count; i++)
        {
            if (boxList[i].GetComponent<Box>().myBelong == 1)
            {
                playerNum++;
            }
        }
        int otherNum = 0;
        for (int i = 0; i < boxList.Count; i++)
        {
            if (boxList[i].GetComponent<Box>().myBelong == 2)
            {
                otherNum++;
            }
        }
        if (playerNum > otherNum)
        {
            endText.text = "red player win";
        }
        else if (playerNum < otherNum)
        {
            endText.text = "blue player win";
        }
        else
        {
            endText.text = "neck and neck";
        }
        //if (!isServer) return;
    }
    public void MyButtonEvent()
    {
        SceneManager.LoadScene("Main");
    }
    public void Close()
    {
        Application.Quit();//ÍË³öÓ¦ÓÃ
    }
    public void Refresh()
    {
        for (int i = 0; i < boxList.Count; i++)
        {
            if (boxList[i].myBelong == 0)
            {
                boxList[i].DestroyBox();
                i--;
            }
        }
        int toNum = 0;
        if (boxList.Count <= 16)
        {
            toNum = 48;
        }
        else if (boxList.Count <= 48)
        {
            toNum = 32;
        }
        else if (boxList.Count <= 64)
        {
            toNum = 24;
        }
        else if (boxList.Count <= 80)
        {
            toNum = 16;
        }
        else if (boxList.Count <= 96)
        {
            toNum = 8;
        }
        int num = 0;
        while (num < toNum)
        {
            int ranX = Random.Range(-8, 9);
            int ranY = Random.Range(-4, 5);
            if ((ranX == 5 || ranX == -5) && ranY == 0) continue;
            if (CheckWithPosition(ranX, ranY))
            {
                num++;
                Vector2 itemPosition = new Vector2(ranX, ranY);
                GameObject myBox = GameObject.Instantiate(boxPrefeb, itemPosition, Quaternion.identity);
                CS_GameManager.Instance.boxList.Add(myBox.GetComponent<Box>());
                myBox.GetComponent<Box>().myBelong = 0;
            }
        }
    }

    public bool CheckWithPosition(float x, float y)
    {
        Vector2 position = new Vector2(x, y);
        if (position.x > fieldX || position.x < -fieldX
                 || position.y > fieldY || position.y < -fieldY)
        {
            return false;
        }
        for (int i = 0; i < boxList.Count; i++)
        {
            Vector2 t_position = boxList[i].transform.position;
            if (
                t_position == position
            )
            {
                return false;
            }
        }
        return true;
    }

}
