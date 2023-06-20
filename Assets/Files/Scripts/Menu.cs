using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform sideMenuRectTransform;
    private float width,height;
    private float startPositionX;
    private float startingAnchoredPositionX;

    public enum Side {up}
    public Side side;

    public UnityEvent Active;
    public UnityEvent InActive;

    // Start is called before the first frame update
    void Start() {
        width = Screen.width;
        height = Screen.height;
    }

    public void OnDrag(PointerEventData eventData) {
        // sideMenuRectTransform.anchoredPosition = new Vector2(Mathf.Clamp(startingAnchoredPositionX - (startPositionX - eventData.position.x), GetMinPosition(), GetMaxPosition()), 0);
        sideMenuRectTransform.anchoredPosition = new Vector2(0,Mathf.Clamp(startingAnchoredPositionX - (startPositionX - eventData.position.y), GetMinPosition(), GetMaxPosition()));

    }

    public void OnPointerDown(PointerEventData eventData) {
        StopAllCoroutines();
        InActive?.Invoke();
        startPositionX = eventData.position.y;
        startingAnchoredPositionX = sideMenuRectTransform.anchoredPosition.y;
        // startPositionX = eventData.position.x;
        // startingAnchoredPositionX = sideMenuRectTransform.anchoredPosition.x;
    }

    public void OnPointerUp(PointerEventData eventData) {
        // StartCoroutine(HandleMenuSlide(.25f, sideMenuRectTransform.anchoredPosition.x, isAfterHalfPoint() ? GetMinPosition() : GetMaxPosition()));
        StartCoroutine(HandleMenuSlide(.25f, sideMenuRectTransform.anchoredPosition.y, isAfterHalfPoint() ? GetMinPosition() : GetMaxPosition()));
        Active?.Invoke();
    }

    private bool isAfterHalfPoint() {
        if (sideMenuRectTransform.anchoredPosition.y <= -height/2.3){
            return true;
        }
        else{
            return false;
        }
        
    }
    

    private float GetMinPosition() {
        // if(side == Side.right)
        //     return width / 2;
        // return -width * .4f;
        return -height/1.6818f;
    }

    private float GetMaxPosition() {
        // if(side == Side.right)
        //     return width * 1.4f;
        // return width / 2;
        return -height/2.44f;
    }

    private IEnumerator HandleMenuSlide(float slideTime, float startingX, float targetX) {
        for (float i = 0; i <= slideTime; i+= .025f) {
            sideMenuRectTransform.anchoredPosition = new Vector2(0,Mathf.Lerp(startingX, targetX, i / slideTime));
            yield return new WaitForSecondsRealtime(.025f);
        }

        
    }

}

// 240->600

// 2400
// -1453 -> -1000