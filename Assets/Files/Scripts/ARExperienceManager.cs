 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Events;

[RequireComponent(typeof(ARPlaneManager))]
public class ARExperienceManager : MonoBehaviour
{
    
    [SerializeField] private UnityEvent OnInitialized;
    [SerializeField] private UnityEvent OnRestarted;
    
    
    private ARPlaneManager arPlaneManager;

    private bool Initialized{get; set;}

    void Awake()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();    
        arPlaneManager.planesChanged += PlanesChanged; 

        #if UNITY_EDITOR
            OnInitialized?.Invoke();
            Initialized = true;
            arPlaneManager.enabled = false;
        #endif

    }
    void PlanesChanged(ARPlanesChangedEventArgs args)
    {
        if(!Initialized)
        {
            Activate();
        }
    }
    private void Activate(){
        ARDebugManager.Instance.LogInfo("Activate Experience");
        OnInitialized?.Invoke();
        Initialized =true;
        arPlaneManager.enabled = false;

    }
    public void Restart(){
                

        ARDebugManager.Instance.LogInfo("Restart Experience");
        OnRestarted?.Invoke();
        Initialized = false;
        arPlaneManager.enabled  = true;

    }

}
