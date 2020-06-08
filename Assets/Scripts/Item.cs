using System.Collections;
using System.Collections.Generic;
using Technet99m;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Item : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] Transform[] actionPoints;
    [SerializeField]  protected bool[] areBusy;
    [SerializeField] Shader normal, tint;

    public int price;
    private Material mat;
    void Awake()
    {
        areBusy = new bool[actionPoints.Length];
        mat = new Material(normal);
        GetComponent<SpriteRenderer>().material = mat;
        for(int i = 0;i<transform.childCount;i++)
        {
            var sp = transform.GetChild(i).GetComponent<SpriteRenderer>();
            if (sp != null)
                sp.material = mat;
        }
        if (!placed)
            mat.shader = tint;
    }
    public Transform GetFree()
    {
        for (int i = 0; i < actionPoints.Length; i++)
        {
            if (!areBusy[i])
            {
                areBusy[i] = true;
                return actionPoints[i];
            }
        }
        return null;
    }
    public virtual void Empty(Vector2 pos)
    {
        int closest = 0;
        for(int i = 1;i<actionPoints.Length;i++)
        {
            if(Vector2.Distance(actionPoints[i].position, pos)< Vector2.Distance(actionPoints[closest].position, pos))
            {
                closest = i;
            }
        }
        areBusy[closest] = false;
    }

    #region Placing
    [SerializeField] Vector2Int placedSize, walkingSize;
    
    public bool placed;
    private bool holding;
    private float holdDelay = 1f;
    private float delay;
    private Vector3 lastPosition;
    private void Update()
    {
        if (holding)
            delay += Time.deltaTime;
        if (delay > holdDelay)
        {
            delay = 0;
            UIManager.Ins.movePanel.movePressed += EditPosition;
            UIManager.Ins.movePanel.removePressed += ()=> 
            {
                GameManager.Ins.activeCage.Leave(placedSize, walkingSize, transform.position);
                GameManager.Ins.activeCage.items.Remove(this);
                Destroy(gameObject);
                UIManager.Ins.movePanel.gameObject.SetActive(false);
            };
            if(this is Feeder)
            {
                UIManager.Ins.movePanel.refillBtn.SetActive(true);
                UIManager.Ins.movePanel.refillPressed += (this as Feeder).Refill;
                UIManager.Ins.movePanel.refillCost.text = price.ToString();
            }
            UIManager.Ins.movePanel.gameObject.SetActive(true);
            UIManager.Ins.movePanel.SetPosition(Utils.WorldToScreenPoint(transform.position));
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!placed)
            UIManager.Ins.setPanel.gameObject.SetActive(false);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!placed)
        {
            Vector3 delta = Vector3.zero;
            if (placedSize.x % 2 == 0)
                delta += Vector3.right * 0.5f;
            if (placedSize.y % 2 == 0)
                delta += Vector3.up * 0.5f;
            transform.position = GameManager.Ins.activeCage.RoundToGridCell(Utils.ScreenToWorldPoint(Input.mousePosition) - delta * GameManager.Ins.cellSize) + delta * GameManager.Ins.cellSize;
            if (GameManager.Ins.activeCage.CanPlace(placedSize, transform.position))
                mat.SetColor("_TintColor",Color.green);
            else
                mat.SetColor("_TintColor", Color.red);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!placed && GameManager.Ins.activeCage.CanPlace(placedSize, transform.position))
        {
            UIManager.Ins.setPanel.okPressed += Place;
            UIManager.Ins.setPanel.cancelPressed += Discard;
            UIManager.Ins.setPanel.gameObject.SetActive(true);
            UIManager.Ins.setPanel.transform.position = Utils.WorldToScreenPoint(transform.position);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (placed)
            holding = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (placed)
            holding = false;
        delay = 0;
    }
    public void Discard()
    {
        if (transform.parent!=GameManager.Ins.activeCage.transform)
        {
            DataManager.AddMoney(price);
            Destroy(gameObject);
        }
        else
        {
            transform.position = lastPosition;
            Place();
        }

    }
    public void EditPosition()
    {
        placed = false;
        mat.shader = tint;
        mat.SetColor("_TintColor",Color.white);
        lastPosition = transform.position;
        UIManager.Ins.movePanel.gameObject.SetActive(false);
        GameManager.Ins.activeCage.items.Remove(this);
        GameManager.Ins.activeCage.Leave(placedSize, walkingSize, transform.position);
    }
    public virtual void Place()
    {
        placed = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        float posZ = (transform.position.y + (placedSize.y - 1) * GameManager.Ins.cellSize * 0.5f);
        transform.position = new Vector3(transform.position.x, transform.position.y, posZ * 10);
        GameManager.Ins.activeCage.Place(placedSize, walkingSize, transform.position);
        if (UIManager.Ins != null)
        {
            UIManager.Ins.setPanel.okPressed -= Place;
            UIManager.Ins.setPanel.cancelPressed -= Discard;
        }
        GameManager.Ins.activeCage.items.Add(this);
        transform.parent = GameManager.Ins.activeCage.transform;
        mat.shader = normal;

    }
    private void OnDestroy()
    {
        UIManager.Ins.setPanel.okPressed -= Place;
        UIManager.Ins.setPanel.cancelPressed -= Discard;
    }
    #endregion
}
