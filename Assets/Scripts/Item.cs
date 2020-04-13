using System.Collections;
using System.Collections.Generic;
using Technet99m;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Item : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField] Transform[] actionPoints;
    [SerializeField] bool[] areBusy;
    
    void Awake()
    {
        areBusy = new bool[actionPoints.Length];
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
    public void Empty(Vector2 pos)
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
                GetComponent<SpriteRenderer>().color = Color.green;
            else
                GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!placed && GameManager.Ins.activeCage.CanPlace(placedSize, transform.position))
        {
            UIManager.Ins.setPanel.okPressed += Place;
            UIManager.Ins.setPanel.gameObject.SetActive(true);
            UIManager.Ins.setPanel.transform.position = Utils.WorldToScreenPoint(transform.position);
        }
    }
    public void Place()
    {
        placed = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * 10);
        GameManager.Ins.activeCage.Place(placedSize, walkingSize, transform.position);
        UIManager.Ins.setPanel.okPressed -= Place;
        GameManager.Ins.activeCage.feeders.Add(this);
        GameManager.Ins.activeCage.ShowGrid();
    }
    #endregion
}
