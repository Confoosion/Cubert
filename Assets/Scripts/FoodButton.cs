using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FoodButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Reference")]
    [SerializeField] private Fridge fridge;

    [Header("Button")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite clickedSprite;
    [SerializeField] private Transform textTransform;
    private Image buttonImage;
    private bool isClicked = false;

    [Header("Food Timing")]
    [SerializeField] private float foodInterval; 
    private float timer = 0f;

    [Header("Sounds")]
    [SerializeField] private AudioClip vendingSound;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    void Update()
    {
        if(timer < foodInterval)
        {
            timer += Time.deltaTime;
        }
        else if(isClicked)
        {
            timer = 0f;
            fridge.SpawnFood();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            ButtonHeld();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(isClicked)
                ButtonLetGo();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isClicked)
            ButtonLetGo();
    }

    private void ButtonHeld()
    {
        buttonImage.sprite = clickedSprite;
        textTransform.localPosition -= new Vector3(0f, 2f, 0f);

        isClicked = true;

        SoundManager.Singleton.PlayFridgeAmbience(true, vendingSound);
    }

    private void ButtonLetGo()
    {
        buttonImage.sprite = idleSprite;
        textTransform.localPosition += new Vector3(0f, 2f, 0f);

        isClicked = false;

        SoundManager.Singleton.PlayFridgeAmbience(false);
    }
}
