using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public enum Task { Papers, Open, Close, Talk, CubertRoom, ReturnCubert, Care }

public class TaskManager : MonoBehaviour
{
    public static TaskManager Singleton;

    void Awake()
    {
        if(Singleton == null) Singleton = this;
    }

    [SerializeField] private TextMeshProUGUI taskText;

    private string PapersTask = "Read the papers on the desk";
    private string TalkTask = "Talk to ";
    private string OpenTask = "Open the Daycare";
    private string CloseTask = "Close the Daycare";
    private string CareTask = "Take care of the cuberts";

    private Task currentTask;
    public Task CurrentTask => currentTask;

    public void SetTask(Task task)
    {
        switch(task)
        {
            case Task.Papers:
                {
                    SetPaperTask();
                    break;
                }
            case Task.Open:
                {
                    SetOpenTask();
                    break;
                }
            case Task.Close:
                {
                    SetCloseTask();
                    break;
                }
            case Task.Talk:
                {
                    SetTalkTask();
                    break;
                }
            case Task.CubertRoom:
                {
                    SetCubertRoomTask();
                    break;
                }
            case Task.ReturnCubert:
                {
                    SetReturnCubertTask();
                    break;
                }
            case Task.Care:
                {
                    SetCareTask();
                    break;
                }
        }
    }

    private void SetPaperTask()
    {
        taskText.SetText(PapersTask);
        currentTask = Task.Papers;
    }

    private void SetOpenTask()
    {
        taskText.SetText(OpenTask);
        currentTask = Task.Open;
    }

    private void SetCloseTask()
    {
        taskText.SetText(CloseTask);
        currentTask = Task.Close;
    }

    private void SetTalkTask()
    {
        string task = TalkTask;
        task += DaycareScreen.Singleton.CurrentNPC.npcSO.npcName;
        taskText.SetText(task);
        currentTask = Task.Talk;
    }

    private void SetCubertRoomTask()
    {
        GameObject cubert = DaycareScreen.Singleton.CurrentNPC.npcSO.cubert;
        bool male = cubert.GetComponent<Cubert>().IsMale;
        string task = "Bring ";
        task += cubert.name + " to " + (male ? "his " : "her ") + "room";
        taskText.SetText(task);
        currentTask = Task.CubertRoom;
    }

    private void SetReturnCubertTask()
    {
        string task = "Return ";
        task += DaycareScreen.Singleton.CurrentNPC.npcSO.npcName + "'s cubert";
        taskText.SetText(task);
        currentTask = Task.ReturnCubert;
    }

    private void SetCareTask()
    {
        taskText.SetText(CareTask);
        currentTask = Task.Care;
    }
}
