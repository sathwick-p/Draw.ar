using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.Events;

public class OtherFeatures : MonoBehaviour
{
    public TMPro.TextMeshProUGUI TextField;
    
    public Button startBtn;
    private LineSettings settings;

    public OtherFeatures(LineSettings settings)
    {
        this.settings = settings;
    }

    public void TextChange(){
        
        TextField.text = "Restart Experience";
    }

    public void ActiveBtn(){
        startBtn.gameObject.SetActive(true);
    }
    public void DeActiveBtn(){
        startBtn.gameObject.SetActive(false);
    }

    
    
}