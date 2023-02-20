using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using Meshadieme.Math;
using UnityEngine.UI;
using System.Globalization;

public class Scope : MonoBehaviour
{
    public GameObject LBoard;
    public GameObject beam;
    public Vector3 mouse;
    public GameObject metalName;
    public GameObject freqName;
    public GameObject chargingB;
    public GameObject metalParent;
    public GameObject sky2;
    public GameObject sky3;
    public Sprite metalInactive;
    public Sprite metalActive;
    public GameObject battery;
    public GameObject flux;
    public GameObject restart;
    public TextMeshProUGUI text;
    public float offset;
    public float mouseX;
    public float paralax;
    public float newKE;
    public float newRatio;
    public int newRank;
    public bool starSelected;
    private bool interacted;
    private bool fluxTrigger = false;
    //private float[] freq = new float[] { 300f, 3E13f, 3E14f, 4.8E14f, 5.1E14f, 5.3E14f, 6E14f, 6.87E14f, 7.89E14f, 3E16f, 3E19F, 3E21f };
    private readonly float freqLow = 300f;
    private readonly float freqHigh = 3E21f;
    private readonly string[] metals = new string[] { "Aluminium", "Berylium", "Cadmium", "Calcium", "Carbon", "Cesium", "Cobalt", "Copper", "Gold", "Iron", "Lead", "Magnesium", "Mercury", "Nickel", "Niobium", "Potassium", "Platinum", "Selenium", "Silver", "Sodium", "Uranium", "Zinc" };
    private readonly float[] ev = new float[] { 4.08f, 5.0f, 4.07f, 2.9f, 4.81f, 2.1f, 5.0f, 4.7f, 5.1f, 4.5f, 4.14f, 3.68f, 4.5f, 5.01f, 4.3f, 2.29f, 6.35f, 5.11f, 4.73f, 2.36f, 3.6f, 4.3f };
    private readonly float maxkeR = 3E21f * 6.35f;
    // Start is called before the first frame update
    void Start()
    {
        starSelected = false;
        interacted = false;
        mouseX = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - GetComponent<RectTransform>().anchoredPosition3D).x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!starSelected)
        {
            RectTransform rTransform = GetComponent<RectTransform>();
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0)) interacted = true;
            if (interacted) mouseX = mouse.x - mouseX;
            if (mouse.y-1.6f < 0) mouse.y = Mathf.Abs(mouse.y-1.6f)+1.6f;
            if (interacted) ScopeAt(mouse);

            //Vector3 startPos2 = sky2.GetComponent<RectTransform>().anchoredPosition3D;
            //Vector3 startPos3 = sky3.GetComponent<RectTransform>().anchoredPosition3D;
            //if (interacted) sky2.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(startPos2.x + (paralax + mouseX), startPos2.y, startPos2.z);
            //if (interacted) sky3.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(startPos3.x + (paralax + (2 * mouseX)), startPos3.y, startPos3.z);

        }

        if (metalName.activeSelf && !fluxTrigger)
        {
            fluxTrigger = true;
            //animate flux
            flux.SetActive(true);
            //animate charge up
            restart.SetActive(true);
        }
    }
    public void StarClicked(GameObject star)
    {
        //start beam
        //Debug.Log("Star Clciked! = " + star);
        if (!beam.activeSelf)
        {
            //beam.SetActive(true);
            starSelected = true;
            fluxTrigger = false;
            //trigger beam animation
            shuffleBag sMetal = new shuffleBag(10, new int[] { 4, 4, 4, 4, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 0, 0 });
            //shuffleBag sFreq = new shuffleBag(10, new int[] { 4, 4, 4, 4, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 0, 0 });
            int _sMetal = sMetal.Next();
            //int _sFreq = sFreq.Next();
            float newFreq = Random.Range(freqLow, freqHigh);
            metalName.GetComponent<TextMeshProUGUI>().text = metals[_sMetal];
            GameObject metalGO = metalParent.GetComponent<SpinWheel>().spin();
            metalGO.GetComponent<Image>().sprite = metalActive;
            freqName.GetComponent<TextMeshProUGUI>().text = "Light Frequency\n" + newFreq.ToString("0.##E+0", CultureInfo.InvariantCulture).Replace("E-", "x10<sup>-").Replace("E+", "x10<sup>") + "</sup>Ghz";
            freqName.SetActive(true);
            chargingB.SetActive(true);
            beam.GetComponent<DynamicScaler>().targetA = star;
            beam.SetActive(true);

            //Scale light
            Vector3 starPos = star.GetComponent<RectTransform>().position;
            starPos.z = mouse.z;
            ScopeAt(starPos);

            //math here for selection
            //float maxke = freq[_sFreq] * ev[_sMetal];
            newKE = newFreq * ev[_sMetal];
            newRatio = Mathf.Ceil((newKE / maxkeR) * 100f);
            //text.text = "KE x Number of Electrons\n" + newKE.ToString("N10") + "\nMax KE Ratio\n" + newRatio + "%";
            text.text = "KE x Number of Electrons\n" + newKE.ToString("0.##E+0", CultureInfo.InvariantCulture).Replace("E-", "x10<sup>-").Replace("E+", "x10<sup>") + "</sup>\nMax KE Ratio\n" + newRatio + "%";
            LBoard.GetComponent<LeaderBrd>().AddEntry(LBoard.GetComponent<LeaderBrd>().username, newRatio);
            //charged battery from metal
            //show math numbers going up
        }

    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ScopeAt(Vector3 target)
    {
        RectTransform rTransform = GetComponent<RectTransform>();
        //float rad = Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg;
        //target.z = rTransform.position.z;
        transform.LookAt(target, Vector3.forward);
    }
}
