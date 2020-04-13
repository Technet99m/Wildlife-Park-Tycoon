using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    MovementController movement;

    [SerializeField] AnimalAnimationController anim;
    [SerializeField] Need selected;
    [SerializeField] Transform matingPos;
    [HideInInspector] public Cage cage;

    Item target;
    Animal mate;
    public AnimalData data;
    public List<Need> needs;
    public bool isBusy;
    public event System.Action<Animal> Free;
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
        if(needs.Count>0 && !isBusy)
        {
            for (int i = 0; i < needs.Count; i++)
            {
                bool dealed = false;
                switch(needs[i].type)
                {
                    case NeedType.Food:
                        target= cage.GetProperFeeder(needs[i].food);
                        if (target != null)
                        {
                            var tmp = target.GetFree();
                            if (tmp!=null)
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
                    break;
                
            }
        }
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
            if (selected.type == NeedType.Special)
            {
                anim.DoSpecial(selected.special);
                Technet99m.Utils.InvokeAfterDelay(() => { FinishNeed(); target.Empty(transform.position); }, 3f);
            }
            if (selected.type == NeedType.Sex)
            {
                mate.Mate();
                Mate();
            }
        }
    }
    public void Mate()
    {
        if (!data.male)
            selected = needs.Find((x) => x.type == NeedType.Sex);
        else
            mate.Free -= this.OnMateFree;
        anim.Mate();
        Technet99m.Utils.InvokeAfterDelay(() => FinishNeed(), 3f);
    }
    public void FinishNeed()
    {

        Free?.Invoke(this);
        isBusy = false;
        anim.Idle();
        GetComponent<AnimalStatus>().Done(selected);
    }
    public void OnMateFree(Animal sender)
    {
        sender.isBusy = true;
        mate = sender;
        movement.SetNewTarget(sender.matingPos.position);
    }
}
