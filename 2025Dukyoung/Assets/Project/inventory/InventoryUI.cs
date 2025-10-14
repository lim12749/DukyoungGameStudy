using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Refs")]
    public Inventory inventory;
    public Transform gridParent;           // GridLayoutGroup가 붙은 Panel
    public InventorySlotUI slotPrefab;
    public CanvasGroup panelGroup;         // 토글 표시
    public Canvas rootCanvas;              // Drag 아이콘 부모
    public GraphicRaycaster raycaster;     // UI 레이캐스트

    [Header("Drop to World")]
    public PickupItem pickupPrefab;
    public Transform dropOrigin;           // 보통 Main Camera transform
    public float dropDistance = 2f;

    [Header("Input (optional)")]
    public InputActionProperty toggleInventoryAction; // I키

    // Drag state
    Image dragIcon;
    RectTransform dragRT;
    int dragFrom = -1;

    readonly List<InventorySlotUI> _slots = new();

    void OnEnable()
    {
        if (inventory) inventory.OnChanged += Refresh;
        if (toggleInventoryAction.reference != null)
        {
            toggleInventoryAction.action.Enable();
            toggleInventoryAction.action.performed += _ => Toggle();
        }
        BuildSlots();
        Refresh();
        Hide(); // 시작은 닫힘
    }
    void OnDisable()
    {
        if (inventory) inventory.OnChanged -= Refresh;
        if (toggleInventoryAction.reference != null)
            toggleInventoryAction.action.performed -= _ => Toggle();
    }

    void Update()
    {
        // 폴백: I 키
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
            Toggle();
    }

    // ----- Open/Close -----
    public void Toggle() { if (panelGroup.alpha > 0.5f) Hide(); else Show(); }
    void Show() { panelGroup.alpha = 1; panelGroup.interactable = true; panelGroup.blocksRaycasts = true; }
    void Hide() { panelGroup.alpha = 0; panelGroup.interactable = false; panelGroup.blocksRaycasts = false; }

    // ----- Build & Refresh -----
    void BuildSlots()
    {
        foreach (Transform c in gridParent) Destroy(c.gameObject);
        _slots.Clear();
    }

    void Refresh()
    {
        // 슬롯 재생성(획득 순서대로)
        foreach (Transform c in gridParent) Destroy(c.gameObject);
        _slots.Clear();
        for (int i = 0; i < inventory.items.Count; i++)
        {
            var ui = Instantiate(slotPrefab, gridParent);
            var def = inventory.items[i].def;
            ui.Bind(this, i, def ? def.icon : null, def ? def.displayName : "");
            _slots.Add(ui);
        }
    }

    // ----- Drag & Drop -----
    public void BeginDrag(int fromIndex, Vector2 screenPos, Sprite icon)
    {
        if (fromIndex < 0 || fromIndex >= inventory.items.Count) return;
        dragFrom = fromIndex;

        if (!dragIcon)
        {
            var go = new GameObject("DragIcon", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(rootCanvas.transform, false);
            dragRT = go.GetComponent<RectTransform>();
            dragIcon = go.GetComponent<Image>();
            dragIcon.raycastTarget = false;
            dragRT.sizeDelta = new Vector2(64, 64);
        }
        dragIcon.sprite = icon;
        dragIcon.enabled = icon != null;
        Drag(screenPos);
    }

    public void Drag(Vector2 screenPos)
    {
        if (!dragRT) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootCanvas.transform as RectTransform, screenPos, rootCanvas.worldCamera, out var local);
        dragRT.anchoredPosition = local;
    }

    public void EndDrag(Vector2 screenPos)
    {
        if (dragFrom < 0) return;

        int target = RaycastSlotIndex(screenPos);
        if (target >= 0) inventory.Swap(dragFrom, target);

        dragFrom = -1;
        if (dragIcon) dragIcon.enabled = false;
    }

    int RaycastSlotIndex(Vector2 screenPos)
    {
        if (!raycaster) return -1;
        var data = new PointerEventData(EventSystem.current) { position = screenPos };
        var results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        foreach (var r in results)
        {
            var slot = r.gameObject.GetComponentInParent<InventorySlotUI>();
            if (slot != null) return slot.index;
        }
        return -1;
    }

    // ----- Right-click Drop -----
    public void RequestDrop(int index)
    {
        if (index < 0 || index >= inventory.items.Count) return;
        var def = inventory.items[index].def;

        // 인벤토리에서 제거 먼저
        inventory.RemoveAt(index);

        // 월드에 드롭(선택)
        if (pickupPrefab && dropOrigin && def)
        {
            Vector3 pos = dropOrigin.position + dropOrigin.forward * dropDistance;
            var spawned = Instantiate(pickupPrefab, pos, Quaternion.identity);
            spawned.item = def;
        }
    }
}
