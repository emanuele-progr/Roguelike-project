using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    public RectTransform canvasRectTran;
    public CanvasScaler canvasScaler;
    public Camera camera;
    
    public float speed = 5.0f;
    public bool overTolerance = false;
    public bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 localPositionOffsetForImages;
    private Vector3 offset2;

    public Image circleImg;
    public Image outerCircleImg;

    public Vector2 input;
    private Vector2 difference;
    private Vector3 startPos;

    private float heightOffset;
    private float widthOffset;
    private float quarterWidthOuterCircle;

    // Use this for initialization
    void Start()
    {
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTran, new Vector3(Screen.width, Screen.height, 1.0f), canvasRectTran.gameObject.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay ? null : camera, out anchoredPos);
        outerCircleImg.GetComponent<RectTransform>().anchoredPosition = anchoredPos;

        heightOffset = canvasRectTran.rect.height / 2;
        widthOffset = canvasRectTran.rect.width / 2;
        quarterWidthOuterCircle = outerCircleImg.rectTransform.rect.width / 4;
        
        pointA = new Vector2();
        pointB = new Vector2();
        localPositionOffsetForImages = new Vector2(widthOffset, heightOffset);
        offset2 = new Vector3(0f, 0f, 0f);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTran, Input.mousePosition, canvasRectTran.gameObject.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay ? null : camera, out anchoredPos);
            pointA.Set(Input.mousePosition.x, Input.mousePosition.y);

            //circleImg.transform.localPosition = (pointA - localPositionOffsetForImages - offset2);
            //outerCircleImg.transform.localPosition = (pointA - localPositionOffsetForImages - offset2);
            circleImg.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
            outerCircleImg.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
            circleImg.transform.localPosition -= offset2;
            outerCircleImg.transform.localPosition -= offset2;
            circleImg.enabled = true;
            outerCircleImg.enabled = true;
            startPos = outerCircleImg.transform.localPosition;
        }
        if (Input.GetMouseButton(0))
        {
            touchStart = true;
            pointB.Set(Input.mousePosition.x, Input.mousePosition.y);
        }
        else
        {
            touchStart = false;
        }
    }

    public Vector2 GetInput()
    {
        return input;
    }

    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointB - pointA;


            Vector2 direction = Vector2.ClampMagnitude(offset, quarterWidthOuterCircle);
            Vector3 dir = startPos + new Vector3(direction.x, direction.y, 0f);


            input = (direction / quarterWidthOuterCircle);
            
            
            if (Mathf.Abs(offset.x) > 8f || Mathf.Abs(offset.y) > 8f)
                overTolerance = true;
            else
                overTolerance = false;

            circleImg.transform.localPosition = dir;
            
        }
        else
        {
            overTolerance = false;
            circleImg.enabled = false;
            outerCircleImg.enabled = false;
        }
    }

}