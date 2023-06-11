using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class OtherFeatures : MonoBehaviour
{
    public TMPro.TextMeshProUGUI TextField;

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
