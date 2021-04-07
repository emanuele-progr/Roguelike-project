using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopUp : MonoBehaviour
{
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private TextMeshProUGUI textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private static int sortingOrder;

    public static TextPopUp Create(Vector3 position, int damageAmount, bool damage=true)
    {
        Vector3 position2 = position;
        position2.y += 0.8f;
        RectTransform CanvasRect = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        Transform TextPopUpTransform = Instantiate(GameManager.instance.pfTextPopUp, position2, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        Vector2 pos = TextPopUpTransform.position;
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint

        Vector2 WorldObject_ScreenPosition = new Vector2(
((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        // set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)


        TextPopUp textPopUp = TextPopUpTransform.GetComponent<TextPopUp>();
        textPopUp.gameObject.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;

        textPopUp.SetUp(damageAmount, damage);

        return textPopUp;
    }

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshProUGUI>();

    }

    public void SetUp(int damage, bool isDamage = true)
    {
        if (isDamage)
        {
            if (damage > 0)
            {
                textMesh.SetText("-" + damage.ToString());
                textColor = textMesh.color;
            }
            else
            {
                textMesh.color = Color.white;
                textColor = textMesh.color;
                textMesh.SetText("STUNNED");
                textMesh.fontSize = 5;
            }
        }
        else
        {
            textMesh.color = Color.green;
            if (damage == 99)
            {
                textMesh.SetText("SPEED UP");
                textMesh.fontSize = 5;
            }
            else
            {
                textMesh.SetText("+" + damage.ToString());
            }

            
        }
        
        disappearTimer = DISAPPEAR_TIMER_MAX;

        //sortingOrder++;
        //textMesh.sortingOrder = sortingOrder;
        moveVector = new Vector3(1, 1) * 50f;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }
        if(disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
        }

        if( textColor.a < 0)
        {
            Destroy(gameObject);
        }
    }
}
