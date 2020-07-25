using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MoveItemController : MonoBehaviour
{
    [SerializeField]
    private Button move, remove, refill;

    public GameObject refillBtn;
    public UnityAction movePressed;
    public UnityAction removePressed;
    public UnityAction refillPressed;
    public Text refillCost;
    
    public void SetPosition(Vector3 pos)
    {
        transform.GetChild(0).position = pos;
    }
    private void OnEnable()
    {
        move.onClick.AddListener(movePressed);
        remove.onClick.AddListener(removePressed);
        refill.onClick.AddListener(refillPressed);
    }
    private void OnDisable()
    {
        move.onClick.RemoveAllListeners();
        remove.onClick.RemoveAllListeners();
        refill.onClick.RemoveAllListeners();
        movePressed = null;
        removePressed = null;
        refillPressed = null;
        refillBtn.SetActive(false);
    }
}