using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.Events;

public class OtherFeatures : MonoBehaviour
{
    public TMPro.TextMeshProUGUI TextField;
    
    public Button startBtn;
    public Button nextBtn;
    public Button clickBtn;
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
        nextBtn.gameObject.SetActive(true);
        clickBtn.gameObject.SetActive(true);
    }
    public void DeActiveBtn(){
        startBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
        clickBtn.gameObject.SetActive(false);
    }

    
    
}