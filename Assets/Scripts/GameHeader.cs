using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameHeader", menuName = "Custom/GameHeader")]
public class GameHeader : ScriptableObject
{
	public readonly Int32 MarkersRowSize = 5;

	private Cell<CellData>[] _row;

	public GameHeader() => _row = new Cell<CellData>[MarkersRowSize];

	[SerializeField]
	private VoidGameEvent OnGameHeaderUpdated;

	public void SetCell(Int32 index, Cell<CellData> cell)
	{
		_row[index] = cell;
		OnGameHeaderUpdated.Invoke();
	}
	public Cell<CellData> GetCell(Int32 cellIndex) => cellIndex >= 0 && cellIndex < _row.Length ? _row[cellIndex] : null;

	//public void Clear() => _row = new List<Cell<CellData>>(MarkersRowSize);
	public void Clear()
	{
		for (int i = 0; i < _row.Length; i++)
		{
			_row[i].Item = default;
		}
	}
}