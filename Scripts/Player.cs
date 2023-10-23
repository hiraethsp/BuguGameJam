using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int myId;
    float step;
    public int state;
    public float myState_walkTime = 0.2f;
    float walkTimer = 0;
    public float myState_skillTime = 0.5f;
    float skillTimer = 0;
    float fieldX = 8f;
    float fieldY = 4f;
    public GameObject myBox;
    public GameObject boxPrefeb;
    public float xDirect, yDirect;
    Rigidbody2D rdbody;

    void Start()
    {
        myId = 1;
        walkTimer = 0;
        xDirect = -1;
        yDirect = 0;
        skillTimer = 0;
        rdbody = GetComponent<Rigidbody2D>();
        step = CS_GameManager.Instance.step;
        fieldX = CS_GameManager.Instance.fieldX;
        fieldY = CS_GameManager.Instance.fieldY;
    }
    void FixedUpdate()
    {
        if (CS_GameManager.Instance.end) return;
        walkTimer -= Time.fixedDeltaTime;
        skillTimer -= Time.fixedDeltaTime;
        if (walkTimer <= 0)
        {
            walkTimer = 0;
            if (Input.GetKey(KeyCode.W))
            {
                Walk(90, 0, 1);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                Walk(180, -1, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Walk(270, 0, -1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Walk(0, 1, 0);
            }
        }
        if (skillTimer <= 0)
        {
            skillTimer = 0;
            Check();
            if (myBox == null)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    SetBox();
                }
            }
            if (myBox != null)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    myBox.GetComponent<Box>().Push(xDirect, yDirect, myId, state);
                    //Debug.Log(xDirect + " " + yDirect);
                    skillTimer = myState_skillTime;
                }
            }
        }
    }
    void Walk(float zAngle, float x, float y)
    {
        xDirect = x;
        yDirect = y;
        walkTimer = myState_walkTime;
        Check();
        if (myBox == null)
        {
            Vector2 targetPos = this.transform.position;
            targetPos.x += x * step;
            targetPos.y += y * step;
            if (targetPos.x > fieldX || targetPos.x < -fieldX
                || targetPos.y > fieldY || targetPos.y < -fieldY)
            {
                return;
            }
            this.transform.position = targetPos;
        }
        else
        {
            //Debug.Log("there is a box");
        }
    }
    void SetBox()
    {
        //Vector2 itemPosition = this.transform.position;
        //itemPosition.x += xDirect * step;
        //itemPosition.y += yDirect * step;
        //if (itemPosition.x > fieldX || itemPosition.x < -fieldX
        //        || itemPosition.y > fieldY || itemPosition.y < -fieldY)
        //{
        //    return;
        //}
        //myBox = GameObject.Instantiate(boxPrefeb, itemPosition, Quaternion.identity);
        //CS_GameManager.Instance.boxList.Add(myBox.GetComponent<Box>());
        //myBox.GetComponent<Box>().myBelong = myId;
        //myBox.GetComponent<Box>().Start();
        //myBox.GetComponent<Box>().SetSkill(state, xDirect, yDirect);
        //skillTimer = myState_skillTime;
    }
    void Check()
    {
        myBox = null;
        for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
        {
            Vector2 t_position = CS_GameManager.Instance.boxList[i].transform.position;
            if (
                t_position.x == this.transform.position.x + step * xDirect
                && t_position.y == this.transform.position.y + step * yDirect
            )
            {
                myBox = CS_GameManager.Instance.boxList[i].gameObject;
                break;
            }
        }
    }
    //void GetAuthority(GameObject go)
    //{
    //    var id = go.GetComponent<NetworkIdentity>();
    //    CmdAuthority(id, connectionToClient);

    //}
    //void CmdAuthority(NetworkIdentity id, NetworkConnectionToClient connClient)
    //{
    //    id.RemoveClientAuthority();
    //    id.AssignClientAuthority(connClient);
    //}
}
