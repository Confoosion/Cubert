using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Minigame_BarQTE : MonoBehaviour
{
    private class Slot
    {
        public Transform tf;
        public SpriteRenderer sr;
        public Collider2D col;
        public Color baseColor;
        public bool hit;
    }

    private const int MaxBars = 3;

    [SerializeField] private GameObject minigameObj;
    [SerializeField] private GameObject stopMinigameButton;
    [SerializeField] private SpriteRenderer bar;
    [SerializeField] private Transform selectBarTemplate;
    [SerializeField] private Transform foodIcon;

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private bool sweepLeftToRight = true;

    [SerializeField] private int roundsBeforeSecondBar = 3;
    [SerializeField] private int roundsBeforeThirdBar = 6;

    [SerializeField] private float edgePadding = 0.2f;
    [SerializeField] private float extraGap = 0.1f;
    [SerializeField] private float minLeadDistance = 1f;
    [SerializeField] private Color hitColor = new Color(1f, 1f, 1f, 0.3f);

    public UnityEvent onBarHit;
    public UnityEvent onRoundComplete;
    
    private readonly List<Slot> slots = new List<Slot>();
    private Collider2D foodCollider;
    private int activeCount;
    private int roundsCompleted;
    private float currentPos;
    private float dir;
    private bool running;

    private void Awake()
    {
        foodCollider = foodIcon.GetComponent<Collider2D>();

        slots.Add(MakeSlot(selectBarTemplate));
        for(int i = 1; i < MaxBars; i++)
        {
            Transform clone = Instantiate(selectBarTemplate, selectBarTemplate.parent);
            clone.name = selectBarTemplate.name + " (" + (i + 1) + ")";
            slots.Add(MakeSlot(clone));
        }

        minigameObj.SetActive(false);
        stopMinigameButton.SetActive(false);
    }

    private Slot MakeSlot(Transform tf)
    {
        SpriteRenderer sr = tf.GetComponent<SpriteRenderer>();
        return new Slot { tf = tf, sr = sr, col = tf.GetComponent<Collider2D>(), baseColor = sr.color };
    }

    public void StartGame()
    {
        minigameObj.SetActive(true);
        stopMinigameButton.SetActive(true);
        roundsCompleted = 0;
        running = true;

        currentPos = sweepLeftToRight ? 0f : 1f;
        dir = sweepLeftToRight ? 1f : -1f;

        UpdateFoodPosition();
        BeginRound();
    }

    private int BarsForRound()
    {
        if(roundsCompleted >= roundsBeforeThirdBar) return 3;
        if(roundsCompleted >= roundsBeforeSecondBar) return 2;
        return 1;
    }

    private void BeginRound()
    {
        activeCount = Mathf.Min(BarsForRound(), slots.Count);
        for(int i = 0; i < slots.Count; i++)
        {
            slots[i].tf.gameObject.SetActive(i < activeCount);
        }

        PlaceBars();
        ResetBars();
    }

    private void ResetBars()
    {
        for(int i = 0; i < activeCount; i++)
        {
            slots[i].hit = false;
            slots[i].sr.color = slots[i].baseColor;
        }
    }

    void Update()
    {
        if(!running) return;

        GetTravelRange(out float minX, out float maxX);
        float length = Mathf.Max(0.0001f, maxX - minX);

        currentPos += dir * (moveSpeed / length) * Time.deltaTime;
        if(currentPos >= 1f || currentPos <= 0f)
        {
            currentPos = Mathf.Clamp01(currentPos);
            UpdateFoodPosition();
            ReachedEnd();
            return;
        }

        UpdateFoodPosition();
    }

    private void UpdateFoodPosition()
    {
        GetTravelRange(out float minX, out float maxX);
        Vector3 p = foodIcon.position;
        p.x = Mathf.Lerp(minX, maxX, currentPos);
        foodIcon.position = p;
    }

    private void GetTravelRange(out float minX, out float maxX)
    {
        Bounds b = bar.bounds;
        float half = foodCollider != null ? foodCollider.bounds.extents.x : 0f;
        minX = b.min.x + half;
        maxX = b.max.x - half;
    }

    public void TryHit()
    {
        if(!running) 
        {
            StartGame();
            return;
        }

        Physics2D.SyncTransforms();

        Bounds food = foodCollider.bounds;
        Slot target = null;
        float best = float.MaxValue;

        for(int i = 0; i < activeCount; i++)
        {
            Slot s = slots[i];
            if(s.hit || !OverlapsInXY(food, s.col.bounds)) continue;

            float dist = Mathf.Abs(food.center.x - s.col.bounds.center.x);
            if(dist < best)
            {
                best = dist;
                target = s;
            }
        }

        if(target == null)
        {
            MissedClick();
            return;
        }

        target.hit = true;
        target.sr.color = hitColor;
        onBarHit?.Invoke();

        if(AllBarsHit())
        {
            CompleteRound();
        }
    }

    private bool AllBarsHit()
    {
        for(int i = 0; i < activeCount; i++)
        {
            if(!slots[i].hit) return false;
        }
        return true;
    }

    private bool OverlapsInXY(Bounds a, Bounds b)
    {
        return(a.min.x <= b.max.x && a.max.x >= b.min.x && a.min.y <= b.max.y && a.max.y >= b.min.y);
    }

    private void GetRangeAhead(float direction, float foodX, float lead, float minX, float maxX, out float lo, out float hi)
    {
        if(direction > 0f)
        {
            lo = Mathf.Max(minX, foodX + lead);
            hi = maxX;
        }
        else
        {
            lo = minX;
            hi = Mathf.Min(maxX, foodX - lead);
        }
    }

    private void PlaceBars()
    {
        Bounds b = bar.bounds;
        float selHalf = slots[0].col.bounds.extents.x;

        float foodHalf = foodCollider.bounds.extents.x;

        float minX = b.min.x + selHalf + edgePadding;
        float maxX = b.max.x - selHalf - edgePadding;

        float minSeparation = selHalf * 2f + foodHalf * 2f + extraGap;
        float lead = foodHalf + selHalf + minLeadDistance;
        float needed = (activeCount - 1) * minSeparation;
        float foodX = foodIcon.position.x;

        GetRangeAhead(dir, foodX, lead, minX, maxX, out float lo, out float hi);
        if(hi - lo < needed)
        {
            lo = minX;
            hi = maxX;
        }

        List<float> xs = new List<float>();
        bool failed = false;

        for(int i = 0; i < activeCount && !failed; i++)
        {
            bool ok = false;
            float x = 0f;
            for(int attempt = 0; attempt < 50 && !ok; attempt++)
            {
                x = Random.Range(lo, hi);
                ok = true;
                foreach(float other in xs)
                {
                    if(Mathf.Abs(x - other) < minSeparation) { ok = false; break; }
                }
            }

            if(ok) xs.Add(x);
            else failed = true;
        }

        if(failed)
        {
            xs.Clear();
            for(int i = 0; i < activeCount; i++)
            {
                float t = activeCount == 1 ? 0.5f : i / (activeCount - 1f);
                xs.Add(Mathf.Lerp(lo, hi, t));
            }
        }
        
        for(int i = 0; i < activeCount; i++)
        {
            Vector3 p = slots[i].tf.position;
            p.x = xs[i];
            slots[i].tf.position = p;
        }
    }

    private void CompleteRound()
    {
        roundsCompleted++;
        onRoundComplete?.Invoke();

        BeginRound();
    }

    private void ReachedEnd()
    {
        dir = -dir;
        ResetBars();
    }

    private void MissedClick()
    {
        ResetBars();
    }

    private void Fail()
    {
        dir = -dir;
        ResetBars();
    }

    public void StopGame()
    {
        running = false;
        minigameObj.SetActive(false);
        stopMinigameButton.SetActive(false);
    }
}
