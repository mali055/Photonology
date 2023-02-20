using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SpinWheel : MonoBehaviour
{
    public GameObject metal1;
    public GameObject metal2;
    public GameObject metal3;
    public GameObject metal4;
    public GameObject metal5;
    public GameObject metal6;
    public GameObject metal7;
    public GameObject metal8;
    public GameObject metal9;
    public GameObject metal10;
    public GameObject metal11;
    public GameObject metal12;
    public GameObject metal13;
    public GameObject metal14;
    public GameObject metal15;
    public GameObject metalName;
    public float spinSpeed = 0.3f;
    private bool spinStarted;
    //private float ntime;
    private int loopCount;
    public float resetDist;
    public int loops;
    public int metalGap;

    void Update()
    {
        if (spinStarted)
        {
            if (loopCount == loops)
            {
                metalName.SetActive(true);
                spinStarted = false;
            }
            else
            {
                RectTransform rect = GetComponent<RectTransform>();
                Vector2 tPos = rect.anchoredPosition;
                rect.anchoredPosition = new(tPos.x + spinSpeed, tPos.y);

                //Debug.Break();
                resetDist += spinSpeed;
                if (resetDist >= metalGap)
                {
                    rect.anchoredPosition = new(tPos.x - resetDist + spinSpeed, tPos.y);
                    resetDist = 0;
                    loopCount++;
                }
                //metal7.GetComponent<Image>().material.SetTextureOffset("_MainTex", new Vector2(((Time.time - ntime) * spinSpeed), 0f));
            }
        }
    }

    public GameObject spin()
    {
        Debug.Log("Spin Called");
        loopCount = 0;
        resetDist = 0;
        spinStarted = true;
        return metal8;
    }
}
