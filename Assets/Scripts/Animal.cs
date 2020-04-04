using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [HideInInspector] public Cage cage;
    MovementController movement;
    [SerializeField] AnimalAnimationController anim;
    public List<Need> needs;
    public AnimalData data;
    Need selected;
    void Start()
    {
        cage = transform.parent.GetComponent<Cage>();
        cage.animals.Add(this);
        Technet99m.TickingMachine.EveryTick += OnTick;
        movement = GetComponent<MovementController>();
        movement.TargetReached += OnTargetReached;
        selected = null;
    }
    public void OnTick()
    {
        if(needs.Count>0 && selected == null)
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
                            selected = needs[i];
                            movement.SetNewTarget(feeder.GetFree().position);
                            dealed = true;
                        }
                        break;
                    case NeedType.Special:
                        var special = cage.GetProperSpecial(needs[i].special);
                        if (special != null)
                        {
                            selected = needs[i];
                            movement.SetNewTarget(special.GetFree().position);
                            dealed = true;
                        }
                        break;
                    case NeedType.Sex:
                        if (data.male)
                        {
                            var mate = cage.GetProperMate();
                            if (mate != null)
                            {
                                selected = needs[i];
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
        anim.Idle();
    }
}
