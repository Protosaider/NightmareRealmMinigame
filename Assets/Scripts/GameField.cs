using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "GameField", menuName = "Custom/GameField")]
public class GameField : ScriptableObject
{
	public readonly Int32 GridSize = 5;

	public Grid<CellData> Grid { get; }

	public GameField() => Grid = new Grid<CellData>(GridSize, GridSize);

	[SerializeField]
	private VoidGameEvent OnGameFieldUpdated;

	public void OnEnable()
	{
		if (OnGameFieldUpdated != null)
			Grid.OnCellsUpdated += OnGameFieldUpdated.Invoke;
	}

	public void OnDisable()
	{
		if (OnGameFieldUpdated != null)
			Grid.OnCellsUpdated -= OnGameFieldUpdated.Invoke;
	}

	public void Clear() => Grid.ClearGrid();

	public IEnumerable<Cell<CellData>> EnumerateColumn(Int32 columnIndex) => Grid.EnumerateColumn(columnIndex);

	public void Swap(Int32 indexFrom, Int32 indexTo)
	{
		if (indexTo == indexFrom)
			return;

		if (Grid.GetCell(indexTo).CellStatus == ECellStatus.Occupied ||
			Grid.GetCell(indexTo).CellStatus == ECellStatus.Blocked)
			return;

		if (Grid.IsNeighbourIndex(indexFrom, indexTo))
			Grid.Swap(indexFrom, indexTo);
	}
}