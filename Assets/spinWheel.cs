using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spinWheel : MonoBehaviour
{
    public GameObject metal1;
    public GameObject metal2;
    public GameObject metal3;
    public GameObject metal4;
    public GameObject metal5;
    public GameObject metal6;
    public GameObject metalName;
    public float spinSpeed = 0.3f;
    private bool spinStarted;
    //private float ntime;
    private int frameCount;
    private int resetCount;
    public int resetTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spinStarted)
        {
            //Debug.Log("Frame Called " + frameCount);
            frameCount++;
            if (frameCount == 661)
            {
                metalName.SetActive(true);
                spinStarted = false;

            } else
            {
                transform.Translate(spinSpeed, 0, 0);
                resetCount++;
                if (resetCount == resetTime)
                {
                    transform.Translate(- (spinSpeed * resetTime), 0, 0);
                    resetCount = 0;
                }
                //metal7.GetComponent<Image>().material.SetTextureOffset("_MainTex", new Vector2(((Time.time - ntime) * spinSpeed), 0f));
            }
        }
    }

    public GameObject spin()
    {
        Debug.Log("Spin Called");
        frameCount = 0;
        resetCount = 0;
        spinStarted = true;
        return metal4;
    }
}
