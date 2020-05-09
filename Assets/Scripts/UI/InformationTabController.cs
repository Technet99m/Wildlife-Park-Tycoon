using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Technet99m;

public class InformationTabController : MonoBehaviour
{
    [SerializeField] Animator anim;

    [SerializeField] Text Name;
    [SerializeField] Text Happiness;
    [SerializeField] Text Needs;
    [SerializeField] Image HappinessIcon;
    [SerializeField] Image SexIcon;
    [SerializeField] Slider progress;

    private Animal selected;
    public void Hide()
    {
        anim.Play("Hide");
        TickingMachine.EveryTick -= Refresh;
        selected = null;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Utils.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin,ray.direction);
            if(hit)
            {
                Animal tmp = hit.collider.GetComponent<Animal>();
                if (tmp != null)
                {
                    if (selected == null)
                    {
                        Show();
                    }
                    selected = tmp;
                    Refresh();
                }
            }
        }
    }
    private void Show()
    {
        anim.Play("Show");
        TickingMachine.EveryTick += Refresh;
    }
    private void Refresh()
    {
        Name.text = selected.data.name;
        Happiness.text = Mathf.RoundToInt(selected.data.happiness * 100).ToString()+"%";
        Happiness.color = Translator.HappinessColor(selected.data.happiness);
        HappinessIcon.sprite = Translator.Happiness(selected.data.happiness);
        SexIcon.sprite = Translator.Sex(selected.data.male);
        progress.value = selected.data.sexualActivity;
        Needs.text = Translator.Needs2Text(selected.needs);
    }
}
