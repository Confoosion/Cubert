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
    public List<Transform> Screens => screens;

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

        if(currentScreen.name == "Lubert")
        {
            currentScreen.GetComponent<CubertScreen>().PlayLubertSound();
        }
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

    public void SetCubert(GameObject cubert)
    {
        GameObject newCubert = Instantiate(cubertScreenPrefab, new Vector3(-17.8f, -cubertHeightSpacing * (screens.Count - 2), 0f), Quaternion.identity);
        newCubert.name = cubert.name;
        screens.Insert((screens.Count > 1) ? screens.Count - 1 : 1, newCubert.transform);
        screenMap.AddScreenToMap();

        GameObject spawned = Instantiate(cubert, Vector3.zero, Quaternion.identity);
        spawned.name = cubert.name;
        
        CubertScreen newCubertScreen = newCubert.GetComponent<CubertScreen>();
        newCubertScreen.SetCubert(spawned);
        newCubertScreen.SetBedSprite(spawned.GetComponent<Cubert>().PreferredBed);
    }

    public void AddCubert(GameObject cubert)
    {
        GameObject newCubert = Instantiate(cubertScreenPrefab, new Vector3(-17.8f, -cubertHeightSpacing * (screens.Count - 2), 0f), Quaternion.identity);
        newCubert.name = cubert.name;
        screens.Insert((screens.Count > 1) ? screens.Count - 1 : 1, newCubert.transform);
        screenMap.AddScreenToMap();
        newCubert.GetComponent<CubertScreen>().SetBedSprite(cubert.GetComponent<Cubert>().PreferredBed);
    }

    public void RemoveCubert()
    {
        GameObject cbrt = DaycareScreen.Singleton.GetCubertOnFrontDesk();
        foreach(Transform t in screens)
        {
            if(t.name == cbrt.name)
            {
                screenMap.RemoveScreenFromMap();
                GameObject toRemove = t.gameObject;
                screens.Remove(t);
                Destroy(toRemove);
                Destroy(cbrt);
                break;
            }
        }
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

    public void SetNPCScreen(bool show)
    {
        if(show)
        {
            screenMap.AddNPCInRoom();
        }
        else
        {
            screenMap.RemoveNPCInRoom();
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
