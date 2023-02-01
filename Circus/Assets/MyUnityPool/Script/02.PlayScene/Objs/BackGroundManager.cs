using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    private const int MAP_END_SIZE = 5;
    private int MapCount = 0;
    private int NowEndMapNum = 0;
    public GameObject BackGroundPrefabs;
    private const int BACKGROUND_COUNT = 3;
    private List<GameObject> BackGroundObjsPool;

    void Start()
    {
        MapCount = 0;
        MoveConnectManager.SetMapCount(MapCount);
        NowEndMapNum = BACKGROUND_COUNT - 1;
        BackGroundObjsPool = new List<GameObject>();

        //GameObject findtemp = GFunc.FindRootObjs("GameObjs");
        //GameObject preFabTemp = findtemp.FindChildObjs("BackGround_Sample");

        float Backs = BackGroundPrefabs.RectTrans().sizeDelta.x;
        float StartPos = 0.0f - Backs * 0.4f;
        for (int i = 0; i < BACKGROUND_COUNT; i++)
        {
            GameObject temp = Instantiate(BackGroundPrefabs);
            temp.transform.SetParent(gameObject.transform);
            temp.RectTrans().localPosition = new Vector3(StartPos, 0f, 0f);
            temp.RectTrans().localScale = new Vector3(1f, 1f, 1f);
            BackGroundObjsPool.Add(temp);

            StartPos += Backs;
        }
    }

    void Update()
    {
        MapMove();
        Reposition();
    }

    public void MapMove()
    {
        if (InputManager.isJump == false)
        {
            //플레이어가 일정 범위에 걸쳤을 때
            if (MoveConnectManager.isMapMove == true)
            {
                float movePoint = InputManager.LR_KeyInput() * (-1) * (InputManager.SPEED * Time.deltaTime);
                for (int i = 0; i < BackGroundObjsPool.Count; i++)
                {
                    BackGroundObjsPool[i].RectTranPosMove(movePoint, 0f, 0f);
                }
            }
        }
        else if (InputManager.isJump == true)
        {
            /* 점프 시 플레이어 움직임 따라가기
                만약 맵 끝이 아니라면
                움직임 제한 구역이라면
             */
        }
    }

    public void Reposition()
    {
        if (BackGroundObjsPool[0].RectTrans().localPosition.x < -1100.0f)
        {
            float RePosX = 0.0f;
            GameObject objtemp = BackGroundObjsPool[0];
            RePosX = BackGroundObjsPool[BackGroundObjsPool.Count - 1].RectTrans().localPosition.x
                        + BackGroundPrefabs.RectTrans().sizeDelta.x;
            objtemp.RectTrans().localPosition = new Vector3(RePosX, 0f, 0f);

            BackGroundObjsPool.RemoveAt(0);
            BackGroundObjsPool.Add(objtemp);
        }

        if (BackGroundObjsPool[BackGroundObjsPool.Count - 1].RectTrans().localPosition.x > 1100.0f)
        {
            float RePosX = 0.0f;
            GameObject objtemp = BackGroundObjsPool[BackGroundObjsPool.Count - 1];
            RePosX = BackGroundObjsPool[0].RectTrans().localPosition.x
                        - BackGroundPrefabs.RectTrans().sizeDelta.x;
            objtemp.RectTrans().localPosition = new Vector3(RePosX, 0f, 0f);

            BackGroundObjsPool.RemoveAt(BackGroundObjsPool.Count - 1);
            BackGroundObjsPool.Insert(0, objtemp);
        }
    }


    public void Reposition2()
    {
        for (int i = 0; i < BackGroundObjsPool.Count; i++)
        {
            if (BackGroundObjsPool[i].RectTrans().localPosition.x < -1100.0f && MapCount != MAP_END_SIZE)
            {
                MapCount += 1;
                MoveConnectManager.SetMapCount(MapCount);
                NowEndMapNum = i;

                float tempSetX = 0.0f;
                switch (i)
                {
                    case 0:
                        tempSetX = BackGroundObjsPool[2].RectTrans().localPosition.x + BackGroundPrefabs.RectTrans().sizeDelta.x;
                        break;
                    case 1:
                        tempSetX = BackGroundObjsPool[0].RectTrans().localPosition.x + BackGroundPrefabs.RectTrans().sizeDelta.x;
                        break;
                    case 2:
                        tempSetX = BackGroundObjsPool[1].RectTrans().localPosition.x + BackGroundPrefabs.RectTrans().sizeDelta.x;
                        break;
                }
                BackGroundObjsPool[i].RectTrans().localPosition =
                    new Vector3(tempSetX, 0f, 0f);
            }

            if (BackGroundObjsPool[i].RectTrans().localPosition.x > 1100.0f && MapCount != 0)
            {
                MapCount -= 1;
                MoveConnectManager.SetMapCount(MapCount);
                float tempSetX = 0.0f;
                switch (i)
                {
                    case 0:
                        tempSetX = BackGroundObjsPool[1].RectTrans().localPosition.x - BackGroundPrefabs.RectTrans().sizeDelta.x;
                        NowEndMapNum = 2;
                        break;
                    case 1:
                        tempSetX = BackGroundObjsPool[2].RectTrans().localPosition.x - BackGroundPrefabs.RectTrans().sizeDelta.x;
                        NowEndMapNum = 0;
                        break;
                    case 2:
                        tempSetX = BackGroundObjsPool[0].RectTrans().localPosition.x - BackGroundPrefabs.RectTrans().sizeDelta.x;
                        NowEndMapNum = 1;
                        break;
                }
                BackGroundObjsPool[i].RectTrans().localPosition =
                    new Vector3(tempSetX, 0f, 0f);
            }
            else { continue; }
        }
    }
}
