using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private AnimalAnimationController anim;
    [SerializeField] private Need selected;
    [SerializeField] private Transform matingPos;

    private MovementController movement;
    private Item target;
    private Animal mate;

    public AnimalData data;
    public List<Need> needs;
    public bool isBusy;
    [HideInInspector] public Cage cage;
    public event System.Action<Animal> Free;
    public event System.Action ReachedMatePos;
    public int Followers { get { return Free==null? 0 : Free.GetInvocationList().Length; } }
    
    void Start()
    {
        cage = transform.parent.GetComponent<Cage>();
        cage.animals.Add(this);
        Technet99m.TickingMachine.EveryTick += OnTick;
        movement = GetComponent<MovementController>();
        movement.TargetReached += OnTargetReached;
        data = GetComponent<AnimalDataHolder>().data;
        selected = null;
    }
    
    public void OnTick()
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
                            var mate = cage.GetProperMate();
                            if (mate != null)
                            {
                                selected = needs[i];
                                if (!mate.isBusy)
                                    OnMateFree(mate);
                                mate.Free += OnMateFree;
                                isBusy = true;
                                dealed = true;
                            }
                        }
                        break;
                }
                if (dealed)
                    return;

            }
        }
        if (!isBusy && !movement.isWalking)
            Technet99m.Utils.InvokeAfterDelay(()=>
            {
                if (!movement.isWalking && !isBusy)
                    movement.SetNewTarget(cage.GetFreeTileInGrid());
            },3f);

    }
    public void OnTargetReached()
    {
        if (isBusy)
        {
            if (selected.type == NeedType.Food)
            {
                anim.Eat(selected.food, target.transform.position.x > transform.position.x);
                Technet99m.Utils.InvokeAfterDelay(() => { FinishNeed(); target.Empty(transform.position); }, 4.5f);
            }
            else if (selected.type == NeedType.Special)
            {
                anim.DoSpecial(selected.special);
                Technet99m.Utils.InvokeAfterDelay(() => { FinishNeed(); target.Empty(transform.position); }, 3f);
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
    public void Mate()
    {
        if (data.male)
            mate.Free -= OnMateFree;
        anim.Mate();
        data.sexualActivity = 0;
        Technet99m.Utils.InvokeAfterDelay(() => FinishNeed(), 3f);
    }
    public void FinishNeed()
    { 
        Free?.Invoke(this);
        isBusy = false;
        anim.Idle();
        GetComponent<AnimalStatus>().Done(selected);
    }
    public void GoMate()
    {
        selected = needs.Find((x) => x.type == NeedType.Sex);
        movement.SetNewTarget(cage.GetPlaceToMate());
        Debug.Log("Going Mate");
        isBusy = true;
    }
    public void OnMateFree(Animal sender)
    {
        mate = sender;
        mate.GoMate();
        mate.ReachedMatePos += OnMateReached;
    }
    public void OnMateReached()
    {
        Debug.Log("Going Mate too");
        movement.SetNewTarget(mate.matingPos.position);
    }
}
