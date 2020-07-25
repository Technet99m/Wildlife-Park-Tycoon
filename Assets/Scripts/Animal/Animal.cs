using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private AnimalAnimationController anim;
    [SerializeField] private Need selected;
    [SerializeField] private Transform matingPos;
    [SerializeField] private SpriteRenderer mood;
    [SerializeField] private Transform body;

    
    public AnimalData data;
    public AnimalStatus status;
    public List<Need> needs;
    public bool isBusy;

    [HideInInspector] 
    public Cage cage;

    [HideInInspector]
    public AnimalStats stats;

    public event System.Action<Animal> Free;
    public event System.Action ReachedMatePos;

    public int Followers { get { return Free==null? 0 : Free.GetInvocationList().Length; } }
    public int finalCost { get => Mathf.CeilToInt(data.happiness * stats.price); }

    private MovementController movement;
    private Item target;
    private Animal mate;

    private void OnEnable()
    {
        Technet99m.TickingMachine.EveryTick += OnTick;
    }
    private void OnDisable()
    {
        Technet99m.TickingMachine.EveryTick -= OnTick;
    }

    public void Initialize(AnimalStats stats)
    {
        cage = transform.parent.GetComponent<Cage>();
        cage.animals.Add(this);
        movement = GetComponent<MovementController>();
        movement.TargetReached += OnTargetReached;
        data = new AnimalData();
        this.stats = stats;
        status = new AnimalStatus(data, stats, needs,transform, mood, body);
        selected = null;
    }
    public void Mate()
    {
        if(mate!=null) mate.Free -= OnMateFree;

        anim.Mate();
        data.sexualActivity = 0;
        Technet99m.Utils.InvokeAfterDelay(() => FinishNeed(), 3f);
    }
    public void FinishNeed()
    {
        target = null;
        isBusy = false;
        status.Done(selected);
        if (StateMachine.state == State.Loading)
            return;
        Free?.Invoke(this);
        anim.Idle();
    }
    public void GoMate()
    {
        selected = needs.Find((x) => x.type == NeedType.Sex);
        if (selected == null)
            selected = new Need() { type = NeedType.Sex };
        if (StateMachine.state == State.Loading)
            return;
        movement.SetNewTarget(cage.GetPlaceToMate());
        Debug.Log("Going Mate");
        isBusy = true;
    }
    public void OnMateFree(Animal sender)
    {
        mate.GoMate();
        mate.ReachedMatePos += OnMateReached;
    }
    public void OnMateReached()
    {
        Debug.Log("Going Mate too");
        movement.SetNewTarget(mate.matingPos.position);
    }
    public void Sell()
    {
        DataManager.AddMoney(finalCost);
        cage.animals.Remove(this);
        target?.Empty(transform.position);
        Destroy(gameObject);
    }

    private void OnTick()
    {
        if (needs.Count > 0 && !isBusy)
        {
            for (int i = 0; i < needs.Count; i++)
            {
                bool dealed = false;
                switch (needs[i].type)
                {
                    case NeedType.Food:
                        target = cage.GetProperFeeder(needs[i].food);
                        if (target != null)
                        {
                            if (StateMachine.state == State.Loading)
                            {
                                selected = needs[i];
                                isBusy = true;
                                FinishNeed();
                                return;
                            }
                            var tmp = target.GetFree();
                            if (tmp != null)
                            {
                                selected = needs[i];
                                isBusy = true;
                                movement.SetNewTarget(tmp.position);
                                dealed = true;
                            }
                        }
                        break;
                    case NeedType.Special:
                        target = cage.GetProperSpecial(needs[i].special);
                        if (target != null)
                        {
                            if (StateMachine.state == State.Loading)
                            {
                                selected = needs[i];
                                isBusy = true;
                                FinishNeed();
                                return;
                            }
                            var tmp = target.GetFree();
                            if (tmp != null)
                            {
                                selected = needs[i];
                                isBusy = true;
                                movement.SetNewTarget(tmp.position);
                                dealed = true;
                            }
                        }
                        break;
                    case NeedType.Sex:
                        if (data.male)
                        {
                            mate = cage.GetProperMate();
                            if (mate != null)
                            {
                                selected = needs[i];
                                isBusy = true;
                                if (StateMachine.state == State.Loading)
                                {

                                    mate.GoMate();
                                    mate.FinishNeed();
                                    FinishNeed();
                                    
                                    return;
                                }
                                mate.Free += OnMateFree;
                                movement.Stop();
                                
                                dealed = true;
                                if (!mate.isBusy)
                                    OnMateFree(mate);
                            }
                        }
                        break;
                }
                if (dealed)
                    return;

            }
        }
        if (StateMachine.state == State.Loading)
            return;
        if (!isBusy && !movement.isWalking)
            Technet99m.Utils.InvokeAfterDelay(() =>
            {
                if (!movement.isWalking && !isBusy)
                    movement.SetNewTarget(cage.GetFreeTileInGrid());
            }, 3f);

    }
    private void OnTargetReached()
    {
        if (isBusy)
        {
            if (selected.type == NeedType.Food)
            {
                anim.Eat(selected.food, target.transform.position.x > transform.position.x);
                Technet99m.Utils.InvokeAfterDelay(() => { target.Empty(transform.position); FinishNeed(); }, 4.5f);
            }
            else if (selected.type == NeedType.Special)
            {
                float delay = anim.DoSpecial(target as SpecialItem);
                Technet99m.Utils.InvokeAfterDelay(() => { target.Empty(transform.position); FinishNeed(); }, delay);
            }
            else if (selected.type == NeedType.Sex)
            {
                if (data.male)
                {
                    mate.ReachedMatePos -= OnMateReached;
                    mate.Mate();
                    Mate();
                }
                else
                {
                    ReachedMatePos?.Invoke();
                }
            }
            return;
        }
    }
}
