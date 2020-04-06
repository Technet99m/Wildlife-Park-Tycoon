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
                        var feeder = cage.GetProperFeeder(needs[i].food);
                        if (feeder != null)
                        {
                            var tmp = feeder.GetFree();
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
                        var special = cage.GetProperSpecial(needs[i].special);
                        if (special != null)
                        {
                            var tmp = special.GetFree();
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
            Free?.Invoke(this);
            isBusy = false;
            anim.Idle();
            GetComponent<AnimalState>().Done(selected);
        }
    }
    public void OnMateFree(Animal sender)
    {
        sender.isBusy = true;
        sender.Free -= OnMateFree;
        movement.SetNewTarget(sender.matingPos.position);
    }
}
