using System.Collections.Generic;
using Draw.Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARAnchorManager))]
public class ARDrawManager : Singleton<ARDrawManager>
{
    [SerializeField] private float distanceFromCamera = 0.3f;
    private string tagg = "Line";
    [SerializeField] private Material defaultColorMaterial;

    [SerializeField] private int cornerVertices = 5;
    [SerializeField] private int endCapVertices = 5;

    [Header("Tolerance Options")][SerializeField]
    private bool allowSimplification = false;

    [SerializeField] private float tolerance = 0.001f;

    [SerializeField] private float applySimplifyAfterPoints = 20.0f;

    [SerializeField, Range(0,1.0f)]
    private float minDistanceBeforeNewPoint = 0.01f;

    [SerializeField] private UnityEvent OnDraw;
    [SerializeField] private ARAnchorManager anchorManager;

    [SerializeField] private Camera arCamera;

    [SerializeField] private Color defaultColor = Color.white;

    [SerializeField] private float lineWidth = 0.05f;

    private LineRenderer prevLineRender;
    private LineRenderer currentLineRender;
    private List<ARAnchor> anchors = new List<ARAnchor>();
    private List<LineRenderer> lines = new List<LineRenderer>();

    private int positionCount = 0;

    private Vector3 prevPointdistance = Vector3.zero;

    private bool CanDraw {get; set;}

    void Update()
    {
        #if !UNITY_EDITOR
        if (Input.touchCount > 0)
            DrawOnTouch();
        #else
        if(Input.GetMouseButton(0))
            DrawOnMouse();
        else{
            prevLineRender = null;
        }
        #endif
    }

    public void AllowDraw(bool isAllow){
        CanDraw = isAllow;
    }

    private void SetLineSettings(LineRenderer currentLineRenderer){
        currentLineRender.startWidth = lineWidth;
        currentLineRender.endWidth = lineWidth;
        currentLineRender.numCornerVertices = cornerVertices;
        currentLineRender.numCapVertices = endCapVertices;

        if(allowSimplification) currentLineRender.Simplify(tolerance);

        currentLineRender.startColor = defaultColor;
        currentLineRender.endColor = defaultColor;
    }

    void DrawOnTouch(){
        if(!CanDraw) return;

        Touch touch = Input.GetTouch(0);

        Vector3 touchPosition = arCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, distanceFromCamera));
        if(touch.phase == TouchPhase.Began)
        {
            OnDraw?.Invoke();

            ARAnchor anchor = anchorManager.AddAnchor(new Pose(touchPosition, Quaternion.identity));

            if(anchor == null){
                Debug.Log("error creating refernce point");

            }
            else{
                anchors.Add(anchor);
                ARDebugManager.Instance.LogInfo($"Anchor created and total of {anchors.Count} anchor(s)");

            }
            AddNewLineRenderer(anchor,touchPosition);
        }
        else{
            UpdateLine(touchPosition);
        }
    }


    void DrawOnMouse(){
        if(!CanDraw) return;

        Vector3 mousePosition = arCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, distanceFromCamera));

        if(Input.GetMouseButton(0)){
            OnDraw?.Invoke();
            if(prevLineRender == null){
                AddNewLineRenderer(null,mousePosition);
            }
            else{
                UpdateLine(mousePosition);
            }
        }
    }


    void UpdateLine(Vector3 touchPosition){
        if(prevPointdistance == null){
            prevPointdistance = touchPosition;
        }
        if(prevPointdistance != null && Mathf.Abs(Vector3.Distance(prevPointdistance, touchPosition)) >= minDistanceBeforeNewPoint){
            prevPointdistance = touchPosition;
            Addpoint(prevPointdistance);
        }
    }
    void Addpoint(Vector3 position){

        positionCount++;

        currentLineRender.positionCount = positionCount;

        currentLineRender.SetPosition(positionCount-1,position);

        if(currentLineRender.positionCount % applySimplifyAfterPoints == 0 && allowSimplification){
            currentLineRender.Simplify(tolerance);
        }
    }
    void AddNewLineRenderer(ARAnchor arAnchor, Vector3 touchPostion){
        
        positionCount = 2;
        GameObject go = new GameObject($"LineRenderer_{lines.Count}");
        // go.tag = "Line";

        go.transform.parent = arAnchor?.transform ?? transform;
        go.transform.position = touchPostion;
        go.gameObject.tag = "Line";
        LineRenderer goLineRenderer = go.AddComponent<LineRenderer>();
        // goLineRenderer.tag = tagg;
        goLineRenderer.startWidth = lineWidth;
        goLineRenderer.endWidth = lineWidth;
        goLineRenderer.material =defaultColorMaterial;
        goLineRenderer.useWorldSpace = true;
        goLineRenderer.positionCount = positionCount;
        goLineRenderer.numCapVertices = 90;
        goLineRenderer.SetPosition(0, touchPostion);
        goLineRenderer.SetPosition(1, touchPostion);


        currentLineRender = goLineRenderer;
        SetLineSettings(goLineRenderer);

        prevLineRender = currentLineRender;

        lines.Add(goLineRenderer);

        ARDebugManager.Instance.LogInfo($"New line renderer created");


    }
    GameObject[] GetAllLinesInScene(){
        return GameObject.FindGameObjectsWithTag("Line");
    }

    public void ClearLines(){
        GameObject[] lines = GetAllLinesInScene();
        foreach (GameObject currentLine in lines)
        {
            LineRenderer line = currentLine.GetComponent<LineRenderer>();
            Destroy(currentLine);
        }
    }

    private Color GetRandomColor() => Random.ColorHSV(0f,1f,1f,1f,0.5f,1f);

}