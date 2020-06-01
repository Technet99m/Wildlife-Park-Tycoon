using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Technet99m;

public class InformationTabController : MonoBehaviour
{
    [SerializeField] Animator anim;

    [SerializeField] InputField Name;
    [SerializeField] Text Happiness;
    [SerializeField] Text Needs;
    [SerializeField] Image HappinessIcon;
    [SerializeField] Image SexIcon;
    [SerializeField] Slider progress;
    [SerializeField] Image progressFill;
    [SerializeField] Image progressIcon;

    [SerializeField] Sprite sexFill, ageFill, pregnancyFill;
    [SerializeField] Sprite sexIcon, sexIconActive, ageIcon, pregnancyIcon;

    private Animal selected;
    private bool ignoreName;
    public void Hide()
    {
        ignoreName = false;
        anim.Play("Hide");
        TickingMachine.EveryTick -= Refresh;
        selected.GetComponent<AnimalAnimationController>().SetBase();
        selected = null;
    }
    public void StartChangingName()
    {
        ignoreName = true;
    }
    public void EndChangingName()
    {
        ignoreName = false;
        selected.data.name = Name.text;
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
                    else
                        selected.GetComponent<AnimalAnimationController>().SetBase();
                    selected = tmp;
                    selected.GetComponent<AnimalAnimationController>().SetHighlight();
                    ignoreName = false;
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
        if(!ignoreName)
            Name.text = selected.data.name;

        Happiness.text = Mathf.RoundToInt(selected.data.happiness * 100).ToString()+"%";
        Happiness.color = Translator.HappinessColor(selected.data.happiness);
        HappinessIcon.sprite = Translator.Happiness(selected.data.happiness);
        SexIcon.sprite = Translator.Sex(selected.data.male);
        if (selected.data.age > 1 && !selected.data.pregnant)
        {
            progress.value = selected.data.sexualActivity;
            progressFill.sprite = sexFill;
            progressIcon.sprite = selected.data.sexualActivity>1? sexIconActive : sexIcon;
        }
        else if(selected.data.age<1)
        {
            progress.value = selected.data.age;
            progressFill.sprite = ageFill;
            progressIcon.sprite = ageIcon;
        }
        else
        {
            progress.value = selected.data.pregnancy;
            progressFill.sprite = pregnancyFill;
            progressIcon.sprite = pregnancyIcon;
        }
        Needs.text = Translator.Needs2Text(selected.needs);
    }
}
