using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ITickable
{
    void Tick(float deltaTime);
}

public class TimeManager : MonoBehaviour
{
    private static TimeManager Singleton;
    private readonly List<ITickable> tickables = new List<ITickable>();
    private readonly List<ITickable> pendingAdds = new List<ITickable>();
    private readonly List<ITickable> pendingRemoves = new List<ITickable>();

    private float tickInterval = 5f;
    private float timer = 0f;

    void Awake() { if(Singleton == null) Singleton = this; }

    public static void Register(ITickable tickable) => Singleton.pendingAdds.Add(tickable);
    public static void Unregister(ITickable tickable) => Singleton.pendingRemoves.Add(tickable);

    void Update()
    {
        if(pendingAdds.Count > 0)
        {
            tickables.AddRange(pendingAdds);
            pendingAdds.Clear();
        }
        if(pendingRemoves.Count > 0)
        {
            foreach(var t in pendingRemoves) tickables.Remove(t);
            pendingRemoves.Clear();
        }

        if(timer >= tickInterval)
        {
            timer = 0f;
            float dt = Time.deltaTime;
            for(int i = 0; i < tickables.Count; i++)
            {
                tickables[i].Tick(dt);
            }
        }

        timer += Time.deltaTime;
    }
}
