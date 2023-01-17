using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Meshadieme.Math;
using UnityEngine.UI;

public class scope : MonoBehaviour
{
    public GameObject beam;
    public Vector3 mouse;
    public GameObject metalName;
    public GameObject freqName;
    public GameObject chargingB;
    public GameObject metalParent;
    public Sprite metalInactive;
    public Sprite metalActive;
    public Sprite batteryCharged;
    public GameObject battery;
    public GameObject flux;
    public TextMeshProUGUI text;
    private bool starSelected;
    private bool fluxTrigger = false;
    private float[] freq = new float[] { 650, 750, 850, 950, 1050 };
    private string[] metals = new string[] { "Iron", "Silver", "Gold", "Lead", "Platinum" };
    // Start is called before the first frame update
    void Start()
    {
        starSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!starSelected)
        {
            mouse = Input.mousePosition - Camera.main.ScreenToWorldPoint(transform.position);
            float rad = Mathf.Atan2(mouse.y, mouse.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(rad, Vector3.forward);
        }

        if (metalName.activeSelf && !fluxTrigger)
        {
            fluxTrigger = true;
            //animate flux
            flux.SetActive(true);
            //animate charge up
            battery.GetComponent<Image>().sprite = batteryCharged;
            //animate text going up
            text.text = "KE x Number of Electrons\n" + 100 + "\nMax KE Ratio\n" + 95 + "%";
        }
    }
    public void starClicked(GameObject star)
    {
        //start beam
        Debug.Log("Star Clciked!");
        if (!beam.activeSelf)
        {
            //beam.SetActive(true);
            starSelected = true;
            fluxTrigger = false;
            //trigger beam animation
            shuffleBag sMetal = new shuffleBag(10, new int[] { 4, 4, 4, 4, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 0, 0 });
            shuffleBag sFreq = new shuffleBag(10, new int[] { 4, 4, 4, 4, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 0, 0 });
            int _sMetal = sMetal.Next();
            int _sFreq = sFreq.Next();
            metalName.GetComponent<TextMeshProUGUI>().text = metals[_sMetal];
            GameObject metalGO = metalParent.GetComponent<spinWheel>().spin();
            metalGO.GetComponent<Image>().sprite = metalActive;
            freqName.GetComponent<TextMeshProUGUI>().text = "Light Frequency\n" + freq[_sFreq] + "mm";
            freqName.SetActive(true);
            chargingB.SetActive(true);
            //math here for selection

            //charged battery from metal
            //show math numbers going up
        }

    }
}
