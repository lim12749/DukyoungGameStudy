using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour,
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image icon;
    [HideInInspector] public int index;
    InventoryUI owner;

    public void Bind(InventoryUI owner, int index, Sprite sprite, string label = "")
    {
        this.owner = owner; this.index = index;
        if (icon) { icon.enabled = sprite; icon.sprite = sprite; }
    }

    public void OnPointerClick(PointerEventData e)
    {
        if (e.button == PointerEventData.InputButton.Right)
            owner?.RequestDrop(index);
    }

    public void OnBeginDrag(PointerEventData e) { owner?.BeginDrag(index, e.position, icon ? icon.sprite : null); }
    public void OnDrag(PointerEventData e)      { owner?.Drag(e.position); }
    public void OnEndDrag(PointerEventData e)   { owner?.EndDrag(e.position); }
}
