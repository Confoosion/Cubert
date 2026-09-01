using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Singleton;

    [SerializeField] private Transform cameraTransform;

    [Header("Screens")]
    [SerializeField] private List<Transform> screens = new List<Transform>();
    [SerializeField] private ScreenMap screenMap;

    [Header("Cuberts")]
    [SerializeField] private GameObject cubertScreenPrefab;
    [SerializeField] private float cubertHeightSpacing;

    [Header("Switch Screen Buttons")]
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;

    [Header("Sounds")]
    [SerializeField] private AudioClip switchRoomAudio;

    private Transform currentScreen;
    public Transform CurrentScreen => currentScreen;
    private int currentScreenIndex = 0;

    void Awake()
    {
        if(Singleton == null) Singleton = this;

        currentScreen = screens[currentScreenIndex];
        SetScreen(currentScreen);
    }

    private void SetScreen(Transform screen)
    {
        cameraTransform.position = new Vector3(screen.position.x, screen.position.y, -10f);
        currentScreen = screen;

        CheckArrowUI();
        screenMap.SetSelectedRoom(screens.IndexOf(screen));
    }

    public void SwitchScreenLeft()
    {
        int index = currentScreenIndex - 1;
        if(index < 0) index = 0;

        currentScreenIndex = index;
        SoundManager.Singleton?.PlaySwitchRoom(switchRoomAudio);
        SetScreen(screens[index]);
    }

    public void SwitchScreenRight()
    {
        int index = currentScreenIndex + 1;
        if(index >= screens.Count) index = screens.Count - 1;

        currentScreenIndex = index;
        SoundManager.Singleton?.PlaySwitchRoom(switchRoomAudio);
        SetScreen(screens[index]);
    }

    private void CheckArrowUI()
    {
        if(currentScreenIndex - 1 < 0)
        {
            leftButton.SetActive(false);
            rightButton.SetActive(true);
        }
        else if(currentScreenIndex + 1 == screens.Count)
        {
            rightButton.SetActive(false);
            leftButton.SetActive(true);
        }
        else
        {
            leftButton.SetActive(true);
            rightButton.SetActive(true);
        }
    }

    public void AddCubert(GameObject cubert)
    {
        GameObject newCubert = Instantiate(cubertScreenPrefab, new Vector3(-17.8f, -cubertHeightSpacing * (screens.Count - 2), 0f), Quaternion.identity);
        newCubert.name = cubert.name;
        screens.Insert(1, newCubert.transform);
        screenMap.AddScreenToMap();
    }

    public void RemoveCubert()
    {
        
    }

    public void SetNeedScreen(Transform screen)
    {
        int index = screens.IndexOf(screen);
        if(index != -1)
        {
            screenMap.SetNeedRoom(index);
        }
    }

    public void RemoveNeedScreen(Transform screen)
    {
        int index = screens.IndexOf(screen);
        if(index != -1)
        {
            screenMap.RemoveNeedRoom(index);
        }
    }

    public List<CubertScreen> GetCubertScreens()
    {
        List<CubertScreen> cubertScreens = new List<CubertScreen>();
        foreach(Transform scrn in screens)
        {
            if(scrn.TryGetComponent(out CubertScreen cbScrn))
            {
                cubertScreens.Add(cbScrn);
            }
        }

        return(cubertScreens);
    }
}
