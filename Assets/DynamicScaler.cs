using System.Linq;
using UnityEngine;

public class DynamicScaler : MonoBehaviour
{
    public GameObject targetA;
    public GameObject targetB;
    private Transform parentA;
    private Transform parentB;
    public SpriteRenderer tSprite;
    public Vector2 offsetA;
    public Vector2 offsetB;
    private RectTransform rect;
    public bool brk;
    // Start is called before the first frame update
    void Start()
    {

        rect = GetComponent<RectTransform>();
        RectTransform rectA = targetA.GetComponent<RectTransform>();
        RectTransform rectB = targetB.GetComponent<RectTransform>();
        parentA = targetA.transform.parent;
        parentB = targetB.transform.parent;
        targetA.transform.SetParent(transform.parent);
        targetB.transform.SetParent(transform.parent);

        if (brk) Debug.Break();
        Vector3[] cornersA = new Vector3[4];
        Vector3[] cornersB = new Vector3[4];
        //cornersA = GetScreenCorners(rectA, Camera.main);
        rectA.GetWorldCorners(cornersA);
        rectB.GetWorldCorners(cornersB);
        //Debug.Log("AScreenCorners" + DebugCollections(GetScreenCorners(rectA, Camera.main)));
        //Debug.Log("AWorldCorners" + DebugCollections(cornersA));
        //Debug.Log("BWorldCorners" + DebugCollections(cornersB));
        float widthA = cornersA[1].x - cornersA[2].x;
        float widthB = cornersB[1].x - cornersB[2].x;
        float heigthA = cornersA[1].y - cornersA[1].y;
        float heightB = cornersB[1].y - cornersB[1].y;
        //Debug.Log("A XMin = " + rectA.rect.xMin + " XMax = " + rectA.rect.xMax);
        //Debug.Log("A YMin = " + rectA.rect.yMin + " YMax = " + rectA.rect.yMax);
        //Debug.Log("B XMin = " + rectB.rect.xMin + " XMax = " + rectB.rect.xMax);
        //Debug.Log("B YMin = " + rectB.rect.yMin + " YMax = " + rectB.rect.yMax);
        float distX = Mathf.Abs((rectB.localPosition.x + Mathf.Abs(rectB.rect.xMin) - (offsetB.x * 0.01f * rectB.rect.width)) - (rectA.localPosition.x - Mathf.Abs(rectA.rect.xMin) + (offsetA.x * 0.01f * rectA.rect.width)));
        float distY = Mathf.Abs((rectB.localPosition.y + Mathf.Abs(rectB.rect.yMin) - (offsetB.y * 0.01f * rectB.rect.height)) - (rectA.localPosition.y - Mathf.Abs(rectA.rect.yMin) + (offsetA.y * 0.01f * rectA.rect.height)));
        
        rect.localPosition = new Vector3(rectA.localPosition.x + (rectA.rect.xMin) + (offsetA.x * 0.01f * rectA.rect.width), rectA.localPosition.y - (rectA.rect.yMax) + (offsetA.y * 0.01f * rectA.rect.height), 0);
        rect.sizeDelta = new Vector2(distX, distY);
        //Debug.Log("ASizeDelta" + rect.sizeDelta);


        Vector3[] cornersX = new Vector3[4];
        rect.GetWorldCorners(cornersX);
        //Debug.Log("WorldCorners" + DebugCollections(cornersX));
        float xDiff = rect.rect.width / tSprite.sprite.bounds.size.x;
        float yDiff = rect.rect.height / tSprite.sprite.bounds.size.y;

        //tSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(distX, distY);
        Stretch(tSprite, xDiff, yDiff);

        targetA.transform.SetParent(parentA);
        targetB.transform.SetParent(parentB);

    }

    public void Stretch(SpriteRenderer sprite, float xScale, float yScale)
    {
        Vector3 scale = new Vector3(xScale, yScale, 1);
        sprite.transform.localScale = Vector3.Scale(sprite.transform.localScale, scale);
        rect = GetComponent<RectTransform>();
    }

    public string DebugCollections(Vector3[] toDebug)
    {
        return new string("Array Contents: \n" + string.Join(", \n", toDebug.Select(i => i.ToString())));
    }

    public static Vector3[] GetScreenCorners(RectTransform transform, Camera cam)
    {
        var worldCorners = new Vector3[4];
        var screenCorners = new Vector3[4];

        transform.GetWorldCorners(worldCorners);

        for (int i = 0; i < 4; i++)
        {
            screenCorners[i] = cam.WorldToScreenPoint(worldCorners[i]);
        }

        return screenCorners;
    }
}
