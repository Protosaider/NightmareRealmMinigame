using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameFieldCell : GridCellUI
{
	[SerializeField]
	private GameField _gameField;

	public Cell<CellData> Cell => _gameField.Grid.GetCell(CellIndex);

	public override void OnDrop(PointerEventData eventData)
	{
		GridCellDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<GridCellDragHandler>();

		if (itemDragHandler == null)
			return;

		if (itemDragHandler.GridCellUI as GameFieldCell != null)
		{
			_gameField.Swap(itemDragHandler.GridCellUI.CellIndex, CellIndex);
		}
	}

	public override void UpdateCellUI()
	{
		if (Cell.Item == null)
		{
			EnableCellUI(false);
			return;
		}

		EnableCellUI(true);

		CellIconImage.sprite = Cell.Item.Icon;
	}

	public override Boolean IsOnPointerDownExecutionAllowed()
	{
		if (Cell.Item == null || Cell.CellStatus == ECellStatus.Blocked)
			return false;

		return true;
	}
}