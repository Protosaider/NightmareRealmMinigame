using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameHeaderCell : GridCellUI
{
	[SerializeField]
	private GameHeader _gameHeader;

	public Cell<CellData> Cell => _gameHeader.GetCell(CellIndex);

	public override void OnDrop(PointerEventData eventData) => throw new NotSupportedException();
	public override Boolean IsOnPointerDownExecutionAllowed() => throw new NotSupportedException();

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
}