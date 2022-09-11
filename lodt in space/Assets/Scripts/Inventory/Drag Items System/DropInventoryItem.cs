using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropInventoryItem : MonoBehaviour, IDropHandler
{
    private List<ItemType> _allowedItems;

    private void Start()
    {
        _allowedItems = GetComponent<CellContainer>().AllowedItems;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.GetComponent<DragItems>() != null && eventData.pointerDrag.gameObject.GetComponent<DragItems>().GetDragging())
        {
            Debug.Log("Item: " + eventData.pointerDrag.name + " || Cell: " + transform.name);
            if (eventData.pointerDrag.gameObject == this.gameObject)
                return;

            if (eventData.pointerDrag.GetComponent<CellContainer>() != null && eventData.pointerDrag.GetComponent<CellContainer>().ItemPrefab != null)
            {
                if (_allowedItems.Count != 0)
                {
                    Debug.Log("Drop equip");
                    for (int i = 0; i < _allowedItems.Count; i++)
                    {
                        if (eventData.pointerDrag.GetComponent<CellContainer>().ItemPrefab.Type == _allowedItems[i])
                        {
                            Move(eventData);
                            break;
                        }
                    }
                }
                else
                {
                    Debug.Log("Drop simple");
                    Move(eventData, true);
                }
            }

            eventData.pointerDrag.gameObject.GetComponent<DragItems>().SetDragging(false);
        }
    }

    private void Move(PointerEventData Data, bool equip = false)
    {
        CellContainer movedCell = Data.pointerDrag.GetComponent<CellContainer>();
        CellContainer thisCell = GetComponent<CellContainer>();

        if (thisCell.Closed)
            return;

        if (movedCell.ItemPrefab.Type == ItemType.BackPackChip && thisCell.isBackpackSlot && movedCell.AllowedItems.Count != 0)
            return;

        if (thisCell.ItemPrefab == null)
        {
            SwapToNullCell(Data);
            return;
        }

        if (equip && movedCell.ItemPrefab != null)
        {
            if (movedCell.AllowedItems.Count != 0)
            {
                List<ItemType> allowedItems = movedCell.AllowedItems;

                if (allowedItems.Count != 0)
                {
                    for (int i = 0; i < allowedItems.Count; i++)
                    {
                        if (thisCell.ItemPrefab.Type == allowedItems[i])
                        {
                            if (!thisCell.ItemPrefab.IsCountable)
                            {
                                SwapToNotNullCell(Data);
                            }
                            else
                            {
                                if (thisCell.ItemPrefab == movedCell.ItemPrefab)
                                    SwapToCountableCell(Data);
                                else
                                    SwapToNotNullCell(Data);
                            }

                            break;
                        }
                    }

                    return;
                }
            }
        }

        if (!thisCell.ItemPrefab.IsCountable)
        {
            SwapToNotNullCell(Data);
        }
        else
        {
            if (thisCell.ItemPrefab == movedCell.ItemPrefab)
                SwapToCountableCell(Data);
            else
                SwapToNotNullCell(Data);
        }
    }

    private void SwapToCountableCell(PointerEventData Data)
    {
        int less = 0;
        CellContainer movingItem = Data.pointerDrag.GetComponent<CellContainer>();
        CellContainer cellForMoving = GetComponent<CellContainer>();
        cellForMoving.Amount += movingItem.Amount;
        less = cellForMoving.Amount - cellForMoving.ItemPrefab.MaxAmount;
        if (less > 0)
        {
            cellForMoving.Amount = cellForMoving.ItemPrefab.MaxAmount;
            movingItem.Amount = less;
        }
        else
        {
            movingItem.ClearCell();
        }

        StorageParser storage = FindObjectOfType<StorageParser>();
        if (storage != null)
            storage.ParseStorage();

        ProcStructParser procStruct = FindObjectOfType<ProcStructParser>();
        if (procStruct != null)
            procStruct.ParseStruct();

        FindObjectOfType<InventoryParser>().ParseInventory();
        FindObjectOfType<Inventory>().ParseInventory();
    }

    private void SwapToNullCell(PointerEventData Data)
    {
        CellContainer movingItem = Data.pointerDrag.GetComponent<CellContainer>();
        CellContainer cellForMoving = GetComponent<CellContainer>();
        cellForMoving.NewItemInCell(movingItem);
        movingItem.ClearCell();

        StorageParser storage = FindObjectOfType<StorageParser>();
        if (storage != null)
            storage.ParseStorage();

        ProcStructParser procStruct = FindObjectOfType<ProcStructParser>();
        if (procStruct != null)
            procStruct.ParseStruct();

        FindObjectOfType<InventoryParser>().ParseInventory();
        FindObjectOfType<Inventory>().ParseInventory();
    }

    private void SwapToNotNullCell(PointerEventData Data)
    {
        CellContainer supItem = GetComponent<CellContainer>();
        CellContainer item1 = Data.pointerDrag.GetComponent<CellContainer>();
        CellContainer item2 = new CellContainer(supItem.ItemPrefab, supItem.Amount, supItem.Durability, supItem.AllowedItems);
        GetComponent<CellContainer>().NewItemInCell(item1);
        item1.NewItemInCell(item2);

        StorageParser storage = FindObjectOfType<StorageParser>();
        if (storage != null)
            storage.ParseStorage();

        ProcStructParser procStruct = FindObjectOfType<ProcStructParser>();
        if (procStruct != null)
            procStruct.ParseStruct();

        FindObjectOfType<InventoryParser>().ParseInventory();
        FindObjectOfType<Inventory>().ParseInventory();
    }
}
