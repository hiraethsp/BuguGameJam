using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Box : MonoBehaviour
{
    public GameObject boxPrefeb;
    float step;
    float state = 0f;
    public float xDirect, yDirect;
    public bool onWalk;
    public int myBelong;
    public float myState_walkTime = 0.3f;
    float walkTimer = 0;
    float fieldX = 8f;
    float fieldY = 4f;
    float boxNum = 4f;//��������1����
    public void Start()
    {
        state = 0;
        step = CS_GameManager.Instance.step;
        onWalk = false;
        fieldX = CS_GameManager.Instance.fieldX;
        fieldY = CS_GameManager.Instance.fieldY;
        boxNum = 3f;
    }
    public void Push(float x, float y, int id, int newState)
    {
        state = newState;
        myBelong = id;
        xDirect = x;
        yDirect = y;
        onWalk = true;
        walkTimer = 0;
    }
    public void FixedUpdate()
    {
        if (myBelong == 1)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }else if(myBelong == 2)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(72 / 255f, 72 / 255f, 214 / 255f, 1);
        }
        walkTimer -= Time.fixedDeltaTime;
        if (walkTimer <= 0 && onWalk == true)
        {
            walkTimer = 0;
            Walk();
        }
    }
    public void DestroyBox()
    {
        Debug.Log("destroy");
        onWalk = false;
        CS_GameManager.Instance.boxList.Remove(this);
        Destroy(gameObject);
    }
    public void Collide()
    {
        #region 5.��ǰ�����ӻ�ú��Լ���ͬ�����ƶ����������߽�
        if (state == 5)
        {
            for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
            {
                Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                if (
                    t_position.x == this.transform.position.x + step * xDirect
                    && t_position.y == this.transform.position.y + step * yDirect
                )
                {
                    CS_GameManager.Instance.boxList[i].GetComponent<Box>().Push(xDirect, yDirect, myBelong, 5);
                }
            }
        }
        #endregion
        #region 6.���ѳ��ĸ�����
        if (state == 6)
        {
            float[] arrX = new float[4] { 0, 0, 1, -1 };
            float[] arrY = new float[4] { 1, -1, 0, 0 };
            Vector2 itemPosition = transform.position;
            for (int i = 0; i < 4; i++)
            {
                itemPosition.x = transform.position.x + arrX[i];
                itemPosition.y = transform.position.y + arrY[i];
                if (CheckWithPosition(itemPosition) == true)
                {
                    GameObject myBox = GameObject.Instantiate(boxPrefeb, itemPosition, Quaternion.identity);
                    CS_GameManager.Instance.boxList.Add(myBox.GetComponent<Box>());
                    myBox.GetComponent<Box>().myBelong = myBelong;
                }

            }
        }
        #endregion
        #region 7.���������������ƿ�
        if (state == 7)
        {
            int[] arrX = new int[4] { 0, 0, 1, -1 };
            int[] arrY = new int[4] { 1, -1, 0, 0 };
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       (t_position.x == this.transform.position.x + step * arrX[j]
                        && t_position.y == this.transform.position.y + step * arrY[j])
                        || (t_position.x == this.transform.position.x + step * arrX[j] * 2
                        && t_position.y == this.transform.position.y + step * arrY[j] * 2)
                    )
                    {
                        CS_GameManager.Instance.boxList[i].GetComponent<Box>().Push(arrX[j], arrY[j], myBelong, 0);
                    }
                }
            }
        }
        #endregion
        #region 8.������Χ�ĸ�����������ڵ�����
        if (state == 8)
        {
            int[] arrX = new int[4] { 0, 0, 1, -1 };
            int[] arrY = new int[4] { 1, -1, 0, 0 };
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       (t_position.x == this.transform.position.x + step * arrX[j]
                        && t_position.y == this.transform.position.y + step * arrY[j])
                        || (t_position.x == this.transform.position.x + step * arrX[j] * 2
                        && t_position.y == this.transform.position.y + step * arrY[j] * 2)
                        || (t_position.x == this.transform.position.x + step * arrX[j] * 3
                        && t_position.y == this.transform.position.y + step * arrY[j] * 3)
                    )
                    {
                        CS_GameManager.Instance.boxList[i].GetComponent<Box>().Push(-arrX[j], -arrY[j], myBelong, 0);
                    }
                }
            }
        }
        #endregion
        #region 9.ת����Χ�˸�����Ϊ������
        if (state == 9)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                    {
                        Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                        if (
                           t_position.x == this.transform.position.x + step * j
                            && t_position.y == this.transform.position.y + step * k
                        )
                        {
                            CS_GameManager.Instance.boxList[i].GetComponent<Box>().myBelong = myBelong;
                        }
                    }
                }
            }
        }
        #endregion
        #region 10.������ĸ����ӱ䵽�Լ���Χ
        if (state == 10)
        {
            float[] arrX = new float[4] { 0, 0, 1, -1 };
            float[] arrY = new float[4] { 1, -1, 0, 0 };
            Vector2 itemPosition = transform.position;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j <= 2; j++)
                {
                    itemPosition.x = transform.position.x + arrX[i] * j;
                    itemPosition.y = transform.position.y + arrY[i] * j;
                    if (CheckWithPosition(itemPosition) == true)
                    {
                        int ran = Random.Range(0, CS_GameManager.Instance.boxList.Count);
                        CS_GameManager.Instance.boxList[ran].transform.position = itemPosition;
                        CS_GameManager.Instance.boxList[ran].GetComponent<Box>().myBelong = myBelong;
                    }
                }
            }
        }
        #endregion
        #region 18.ը��Χ�˸�
        if (state == 18)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (j == 0 && k == 0)
                    {
                        continue;
                    }
                    for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                    {
                        Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                        if (
                           t_position.x == this.transform.position.x + step * j
                            && t_position.y == this.transform.position.y + step * k
                        )
                        {
                            CS_GameManager.Instance.boxList[i].DestroyBox();
                        }
                    }
                }
            }
        }
        #endregion
        #region 19.ը����һ��
        if (state == 19)
        {
            if (xDirect == 0)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       t_position.y == this.transform.position.y && t_position.x != this.transform.position.x
                    )
                    {
                        CS_GameManager.Instance.boxList[i].DestroyBox();
                    }
                }
            }
            if (yDirect == 0)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       t_position.x == this.transform.position.x
                       && t_position.y != this.transform.position.y
                    )
                    {
                        CS_GameManager.Instance.boxList[i].DestroyBox();
                    }
                }
            }
        }
        #endregion
        #region 20.ըǰ����һ��˸�
        if (state == 20)
        {
            Vector2 nextPosition = this.transform.position;
            nextPosition.x += xDirect * step * 3;
            nextPosition.y += yDirect * step * 3;
            Debug.Log(nextPosition);
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                    {
                        Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                        if (
                           t_position.x == nextPosition.x + step * j
                            && t_position.y == nextPosition.y + step * k
                        )
                        {
                            CS_GameManager.Instance.boxList[i].DestroyBox();
                        }
                    }
                }
            }
        }
        #endregion
        #region 21.ʮ��ը
        if (state == 21)
        {
            int[] arrX = new int[4] { 0, 0, 1, -1 };
            int[] arrY = new int[4] { 1, -1, 0, 0 };
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       (t_position.x == this.transform.position.x + step * arrX[j]
                        && t_position.y == this.transform.position.y + step * arrY[j])
                        || (t_position.x == this.transform.position.x + step * arrX[j] * 2
                        && t_position.y == this.transform.position.y + step * arrY[j] * 2)
                    )
                    {
                        CS_GameManager.Instance.boxList[i].GetComponent<Box>().DestroyBox();
                    }
                }
            }
        }
        #endregion
        onWalk = false;
        state = 0;
    }
    public void Walk()
    {
        if (Check() == true)
        {
            Vector2 targetPos = this.transform.position;
            targetPos.x += xDirect * step;
            targetPos.y += yDirect * step;
            if (targetPos.x > fieldX || targetPos.x < -fieldX
                 || targetPos.y > fieldY || targetPos.y < -fieldY)
            {
                DestroyBox();
                return;
            }
            #region 1.�ƶ�·�������ĸ�����
            if (state == 1)
            {
                GameObject myBox = GameObject.Instantiate(boxPrefeb, transform.position, Quaternion.identity);
                CS_GameManager.Instance.boxList.Add(myBox.GetComponent<Box>());
                myBox.GetComponent<Box>().myBelong = myBelong;
                boxNum--;
                if (boxNum == 0)
                {
                    state = 0;
                    boxNum = 4f;
                }
            }
            #endregion
            #region 2.�����������ӻ����÷�����ƶ�
            if (state == 2)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       (t_position.x == this.transform.position.x + step * yDirect
                        && t_position.y == this.transform.position.y + step * xDirect)
                        || (t_position.x == this.transform.position.x - step * yDirect
                        && t_position.y == this.transform.position.y - step * xDirect)
                    )
                    {
                        CS_GameManager.Instance.boxList[i].GetComponent<Box>().Push(xDirect, yDirect, myBelong, 0);
                    }
                }
            }
            #endregion
            #region 3.�����������ӻ���򷴷�����ƶ�
            if (state == 3)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       (t_position.x == this.transform.position.x + step * yDirect
                        && t_position.y == this.transform.position.y + step * xDirect)
                        || (t_position.x == this.transform.position.x - step * yDirect
                        && t_position.y == this.transform.position.y - step * xDirect)
                    )
                    {
                        CS_GameManager.Instance.boxList[i].GetComponent<Box>().Push(-xDirect, -yDirect, myBelong, 0);
                    }
                }
            }
            #endregion
            #region 4.�����������ӻ�����ⷽ����ƶ�
            if (state == 4)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       (t_position.x == this.transform.position.x + step * yDirect
                        && t_position.y == this.transform.position.y + step * xDirect)
                    )
                    {
                        CS_GameManager.Instance.boxList[i].GetComponent<Box>().Push(yDirect, xDirect, myBelong, 0);
                    }
                    else if (
                        t_position.x == this.transform.position.x - step * yDirect
                        && t_position.y == this.transform.position.y - step * xDirect)
                    {
                        CS_GameManager.Instance.boxList[i].GetComponent<Box>().Push(-yDirect, -xDirect, myBelong, 0);
                    }
                }
            }
            #endregion
            this.transform.position = targetPos;
            walkTimer = myState_walkTime;
        }
        else
        {
            Collide();
        }
    }
    public bool Check()
    {
        for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
        {
            Vector2 t_position = CS_GameManager.Instance.boxList[i].transform.position;
            if (
                t_position.x == this.transform.position.x + step * xDirect
                && t_position.y == this.transform.position.y + step * yDirect
            )
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckWithPosition(Vector2 position)
    {
        if (position.x > fieldX || position.x < -fieldX
                 || position.y > fieldY || position.y < -fieldY)
        {
            return false;
        }
        for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
        {
            Vector2 t_position = CS_GameManager.Instance.boxList[i].transform.position;
            if (
                t_position == position
            )
            {
                return false;
            }
        }
        return true;
    }
    public void SetSkill(int thisState, float x, float y)
    {
        #region 11.����ĸ�����
        if (thisState == 11)
        {
            float[] arrX = new float[4] { 0, 0, 1, -1 };
            float[] arrY = new float[4] { 1, -1, 0, 0 };
            Vector2 itemPosition = transform.position;
            for (int i = 0; i < 4; i++)
            {
                if (!(arrX[i] == -x && arrY[i] == -y))
                {
                    itemPosition.x = transform.position.x + arrX[i];
                    itemPosition.y = transform.position.y + arrY[i];
                    if (CheckWithPosition(itemPosition) == true)
                    {
                        GameObject myBox = GameObject.Instantiate(boxPrefeb, itemPosition, Quaternion.identity);
                        CS_GameManager.Instance.boxList.Add(myBox.GetComponent<Box>());
                        myBox.GetComponent<Box>().myBelong = myBelong;
                    }
                }
            }
        }
        #endregion
        #region 12.�����������������
        if (thisState == 12)
        {
            Debug.Log("yes");
            float[] arrX = new float[4] { 0, 0, 1, -1 };
            float[] arrY = new float[4] { 1, -1, 0, 0 };
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector2 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       (t_position.x == this.transform.position.x + step * arrX[j]
                        && t_position.y == this.transform.position.y + step * arrY[j])
                        || (t_position.x == this.transform.position.x + step * arrX[j] * 2
                        && t_position.y == this.transform.position.y + step * arrY[j] * 2)
                    )
                    {
                        CS_GameManager.Instance.boxList[i].GetComponent<Box>().Push(-arrX[j], -arrY[j], myBelong, 0);
                    }
                }
            }
        }
        #endregion
        #region 13.�ƿ��������������
        if (thisState == 13)
        {
            int[] arrX = new int[4] { 0, 0, 1, -1 };
            int[] arrY = new int[4] { 1, -1, 0, 0 };
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector2 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       (t_position.x == this.transform.position.x + step * arrX[j]
                        && t_position.y == this.transform.position.y + step * arrY[j])
                        || (t_position.x == this.transform.position.x + step * arrX[j] * 2
                        && t_position.y == this.transform.position.y + step * arrY[j] * 2)
                    )
                    {
                        CS_GameManager.Instance.boxList[i].GetComponent<Box>().Push(arrX[j], arrY[j], myBelong, 0);
                    }
                }
            }
        }
        #endregion
        #region 14.ը��Χ�˸�
        if (thisState == 14)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (j == 0 && k == 0)
                    {
                        continue;
                    }
                    for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                    {
                        Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                        if (
                           t_position.x == this.transform.position.x + step * j
                            && t_position.y == this.transform.position.y + step * k
                        )
                        {
                            CS_GameManager.Instance.boxList[i].DestroyBox();
                        }
                    }
                }
            }
        }
        #endregion
        #region 15.ը����һ��
        if (thisState == 15)
        {
            if (x == 0)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       t_position.y == this.transform.position.y
                       && t_position.x != this.transform.position.x
                    )
                    {
                        CS_GameManager.Instance.boxList[i].DestroyBox();
                    }
                }
            }
            if (y == 0)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       t_position.x == this.transform.position.x
                       & t_position.y != this.transform.position.y
                    )
                    {
                        CS_GameManager.Instance.boxList[i].DestroyBox();
                    }
                }
            }
        }
        #endregion
        //16ż������е�����ʧЧ���ݲ����ԭ��
        #region 16.ըǰ��һ��
        if (thisState == 16)
        {
            if (x == 0)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector2 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       (t_position.y - this.transform.position.y) * y > 0
                        && t_position.x == this.transform.position.x
                    )
                    {
                        CS_GameManager.Instance.boxList[i].DestroyBox();
                    }
                }
            }
            if (y == 0)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector2 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       (t_position.x - this.transform.position.x) * x > 0
                        && t_position.y == this.transform.position.y
                    )
                    {
                        CS_GameManager.Instance.boxList[i].DestroyBox();
                    }
                }
            }
        }
        #endregion
        #region 17.ʮ��ը
        if (thisState == 17)
        {
            int[] arrX = new int[4] { 0, 0, 1, -1 };
            int[] arrY = new int[4] { 1, -1, 0, 0 };
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < CS_GameManager.Instance.boxList.Count; i++)
                {
                    Vector3 t_position = CS_GameManager.Instance.boxList[i].transform.position;
                    if (
                       (t_position.x == this.transform.position.x + step * arrX[j]
                        && t_position.y == this.transform.position.y + step * arrY[j])
                        || (t_position.x == this.transform.position.x + step * arrX[j] * 2
                        && t_position.y == this.transform.position.y + step * arrY[j] * 2)
                    )
                    {
                        CS_GameManager.Instance.boxList[i].GetComponent<Box>().DestroyBox();
                    }
                }
            }
        }
        #endregion
    }
}
