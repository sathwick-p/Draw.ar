using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CameraBehv : MonoBehaviour
{
    private string pathoffile;

    public Canvas myCanvas;
    public void ClickShare()
    {
        StartCoroutine(TakeSSAndShare());
    }
    private void  GetAndroidExternalStoragePath()
    {
        if (Application.platform != RuntimePlatform.Android){
            pathoffile = Application.persistentDataPath;
            // return Application.persistentDataPath;
        }

        var jc = new AndroidJavaClass("android.os.Environment");
        var path = jc.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory",
            jc.GetStatic<string>("DIRECTORY_DCIM"))
            .Call<string>("getAbsolutePath");
        pathoffile = path;
        pathoffile = pathoffile + "/Camera";
        // return path;
    }
    void Start()
    {
        GetAndroidExternalStoragePath();
        ARDebugManager.Instance.LogInfo("path = "+pathoffile);
        // ARDebugManager.Instance.LogInfo("persistent path = "+Application.persistentDataPath);
    }
    private IEnumerator TakeSSAndShare()
    {
        myCanvas.enabled = false;
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        yield return new WaitForEndOfFrame();
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(pathoffile, "Capture" + timeStamp + ".png");
        // GetAndroidExternalStoragePath();
        File.WriteAllBytes(filePath, ss.EncodeToPNG());
        
        Destroy(ss);

        myCanvas.enabled = true;


    }
}
