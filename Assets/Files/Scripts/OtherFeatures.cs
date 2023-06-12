using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;
public class OtherFeatures : MonoBehaviour
{
    public TMPro.TextMeshProUGUI TextField;
    
    private LineSettings settings;

    public OtherFeatures(LineSettings settings)
    {
        this.settings = settings;
    }

    // public TextMesh textfield;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TextChange(){
        
        TextField.text = "Restart Experience";
    }
    
}