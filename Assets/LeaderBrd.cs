using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderBrd : MonoBehaviour
{
    private int rng;
    public GameObject scope;
    public string username;
    public string fetch;
    public string urlGet;
    public string urlPost;
    public TextMeshProUGUI lRank;
    public TextMeshProUGUI lName;
    public TextMeshProUGUI lScore;
    public TextMeshProUGUI pRank;
    public TMP_InputField pName;
    public TextMeshProUGUI pScore;
    // Start is called before the first frame update
    void Start()
    {
        rng = UnityEngine.Random.Range(0, 999999);
        username = "User" + rng.ToString("D6");
        StartCoroutine(WebGet(urlGet));
    }

    IEnumerator WebGet(string url)
    {
        lName.text = "Loading...";
        using UnityWebRequest req = UnityWebRequest.Get(url);
        {
            yield return req.SendWebRequest();

            switch (req.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + req.error);
                    lName.text = req.error;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + req.error);
                    lName.text = req.error;
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received: " + req.downloadHandler.text);
                    fetch = req.downloadHandler.text;
                    RefreshBoard();
                    break;
            }
            req.Dispose();
        }
    }
    IEnumerator WebPost(LBEntry lbe)
    {
        string nJson = JsonConvert.SerializeObject(lbe);
        Debug.Log(nJson);

        using UnityWebRequest req = UnityWebRequest.Post(urlPost, nJson, "application/json");
        {
            Debug.Log("Type = " + req.uploadHandler.contentType);
            yield return req.SendWebRequest();

            switch (req.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + req.error);
                    lName.text = req.error;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + req.error);
                    lName.text = req.error;
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Form upload complete! = " + req.downloadHandler.text);
                    StartCoroutine(WebGet(urlGet));
                    break;
            }
        }
        req.Dispose();
    }

    public void NewName()
    {
        username = pName.text;
    }
    public void PopKeyboard()
    {
        TouchScreenKeyboard.Open(pName.text, TouchScreenKeyboardType.Default, false, false, false, false);
    }

    public void RefreshBoard()
    {
        List<LBEntry> table = JsonConvert.DeserializeObject<List<LBEntry>>(fetch);
        string nRank = "";
        string nName = "";
        string nScore = "";

        pName.text = username;
        List<LBSorter> newSort = new();
        int i = 0;
        foreach (LBEntry sort in table)
        {
            LBSorter nSort = new()
            {
                id = i,
                name = sort.id,
                score = int.Parse(sort.score),
                dt = sort.dt
            };
            newSort.Add(nSort);
            i++;
        }
        newSort = newSort.OrderBy(x => -x.score).ToList();
        if (scope.GetComponent<Scope>().starSelected)
        {
            pRank.text = scope.GetComponent<Scope>().newRank + "";
            pScore.text = scope.GetComponent<Scope>().newRatio + "";
        } else
        {
            pRank.text = "N/A";
            pScore.text = "N/A";
        }

        for (int j = 0; j < newSort.Count; j++)
        {
            nRank += (j + 1) + "\n\n";
            nName += newSort[j].name + "\n\n";
            nScore += newSort[j].score + "\n\n";
        }

        lRank.text = nRank;
        lName.text = nName;
        lScore.text = nScore;
    }

    public void AddEntry(string nName, float nScore)
    {
        LBEntry newLB = new();
        DateTime newDt = DateTime.Today;
        newLB.id = nName;
        newLB.score = nScore.ToString();
        //newLB.dt = "";
        newLB.dt = newDt.ToString("dd-MM-yyyy");

        List<LBEntry> table = JsonConvert.DeserializeObject<List<LBEntry>>(fetch);
        table.Add(newLB);

        List<LBSorter> newSort = new();
        int i = 0;
        foreach (LBEntry sort in table)
        {
            LBSorter nSort = new()
            {
                id = i,
                name = sort.id,
                score = int.Parse(sort.score),
                dt = sort.dt
            };
            newSort.Add(nSort);
            i++;
        }
        newSort = newSort.OrderBy(x => -x.score).ToList();
        scope.GetComponent<Scope>().newRank = newSort.FindIndex(ns => ns.id == (newSort.Count - 1)) + 1;
        StartCoroutine(WebPost(newLB));
    }

    [Serializable]
    public class LBEntry
    {
        public string dt;
        public string id;
        public string score;
    }
    [Serializable]
    public class LBSorter
    {
        public int id;
        public string dt;
        public string name;
        public int score;
    }
}
