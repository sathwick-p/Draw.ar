using System.Collections.Generic;
using Draw.Singletons;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARAnchorManager))]
public class ARDrawManager : Singleton<ARDrawManager>
{
    [SerializeField]
    private LineSettings lineSettings = null;

    [SerializeField]
    private UnityEvent OnDraw = null;

    [SerializeField]
    private ARAnchorManager anchorManager = null;

    [SerializeField]
    private Camera arCamera = null;

    private List<ARAnchor> anchors = new List<ARAnchor>();

    private Dictionary<int, ARLine> Lines = new Dictionary<int, ARLine>();
    [SerializeField] private UnityEvent OnClick;
    private bool CanDraw { get; set; }

    public Slider slider;
    public Material LineColorMat;
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    private Color LineColor;
    void Update()
    {
#if !UNITY_EDITOR
        DrawOnTouch();
#else
        DrawOnMouse();
#endif


    }

    public void AllowDraw(bool isAllow)
    {
        CanDraw = isAllow;
    }


    void DrawOnTouch()
    {
        if (!CanDraw) return;

        int tapCount = Input.touchCount > 1 && lineSettings.allowMultiTouch ? Input.touchCount : 1;

        for (int i = 0; i < tapCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector3 touchPosition = arCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, lineSettings.distanceFromCamera));

            ARDebugManager.Instance.LogInfo($"{touch.fingerId}");

            if (touch.phase == TouchPhase.Began)
            {
                OnDraw?.Invoke();

                ARAnchor anchor = anchorManager.AddAnchor(new Pose(touchPosition, Quaternion.identity));
                if (anchor == null)
                    Debug.LogError("Error creating reference point");
                else
                {
                    anchors.Add(anchor);
                    ARDebugManager.Instance.LogInfo($"Anchor created & total of {anchors.Count} anchor(s)");
                }

                ARLine line = new ARLine(lineSettings);
                Lines.Add(touch.fingerId, line);
                line.AddNewLineRenderer(transform, anchor, touchPosition);
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Lines[touch.fingerId].AddPoint(touchPosition);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Lines.Remove(touch.fingerId);
            }
        }

        Debug.Log(Lines.Keys);
        ARDebugManager.Instance.LogInfo("" + Lines.Keys);
    }

    void DrawOnMouse()
    {
        if (!CanDraw) return;

        Vector3 mousePosition = arCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lineSettings.distanceFromCamera));

        if (Input.GetMouseButton(0))
        {
            OnDraw?.Invoke();

            if (Lines.Keys.Count == 0)
            {
                ARLine line = new ARLine(lineSettings);
                Lines.Add(0, line);
                line.AddNewLineRenderer(transform, null, mousePosition);
            }
            else
            {
                Lines[0].AddPoint(mousePosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Lines.Remove(0);
        }
    }

    GameObject[] GetAllLinesInScene()
    {
        return GameObject.FindGameObjectsWithTag("Line");
    }

    public void ClearLines()
    {
        GameObject[] lines = GetAllLinesInScene();
        foreach (GameObject currentLine in lines)
        {
            LineRenderer line = currentLine.GetComponent<LineRenderer>();
            Destroy(currentLine);
        }


    }




    public void sliderChange()
    {

        lineSettings.startWidth = slider.value;
        lineSettings.endWidth = slider.value;
    }


    void Start()
    {
        LineColorMat.SetColor("_Color", Color.white);
    }
    public void ColorValues()
    {
        LineColor.r = redSlider.value;
        LineColor.g = greenSlider.value;
        LineColor.b = blueSlider.value;
        LineColor.a = 255;

        lineSettings.defaultMaterial = LineColorMat;
        LineColorMat.color = new Color(redSlider.value / 255f, greenSlider.value / 255f, blueSlider.value / 255f, 255 / 255f);
        // LineColorMat.SetColor("color",LineColor);
        // Debug.Log(""+LineColorMat.name);
        // Debug.Log("LC "+LineColor);
        // Debug.Log("MC "+LineColorMat.color);
    }



}