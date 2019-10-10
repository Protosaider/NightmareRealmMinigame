using System;
using System.Collections.Generic;

public class Grid<T>
{
	private readonly Cell<T>[] _grid;
	private readonly Int32 _width;
	private readonly Int32 _height;

	public Grid(Int32 width, Int32 height)
	{
		_width = width;
		_height = height;
		_grid = new Cell<T>[_width * _height];
	}

	public void ClearGrid()
	{
		for (int i = 0; i < _width * _height; i++)
			_grid[i].Item = default;
	}

	private Int32 CellIndex(Int32 x, Int32 y) => x + _width * y;

	public event Action OnCellsUpdated;

	public IEnumerable<Cell<T>> EnumerateRow(Int32 rowIndex)
	{
		if (rowIndex >= 0 && rowIndex <= _height)
			yield return null;
		else
		{
			for (int x = 0; x < _width; x++)
				yield return _grid[rowIndex * _width + x];
		}
	}

	public IEnumerable<Cell<T>> EnumerateColumn(Int32 columnIndex)
	{
		if (columnIndex < 0 && columnIndex >= _width)
			yield return null;
		else
		{
			for (int y = 0; y < _height; y++)
			{
				yield return _grid[columnIndex + y * _width];
			}
		}
	}

    #region Use xy coordinates

    public Boolean IsInside(Int32 x, Int32 y) => x < _width && x >= 0 && y < _height && y >= 0;
	public Cell<T> GetCell(Int32 x, Int32 y) => IsInside(x, y) ? _grid[CellIndex(x, y)] : null;
	public void SetCell(Int32 x, Int32 y, Cell<T> cell)
	{
		if (!IsInside(x, y))
			return;

		_grid[CellIndex(x, y)] = cell;

		OnCellsUpdated?.Invoke();
    }
    public void ChangeCellStatus(Int32 x, Int32 y, ECellStatus status)
	{
		if (IsInside(x, y))
			_grid[CellIndex(x, y)].CellStatus = status;
	}
	public void Swap(Int32 xFrom, Int32 yFrom, Int32 xTo, Int32 yTo)
	{
		var cell = _grid[CellIndex(xTo, yTo)];
		_grid[CellIndex(xTo, yTo)] = _grid[CellIndex(xFrom, yFrom)];
		_grid[CellIndex(xFrom, yFrom)] = cell;

		OnCellsUpdated?.Invoke();
	}

	#endregion

	#region Use index

	public Boolean IsInside(Int32 index) => index >= 0 && index < _height * _width;
	public Cell<T> GetCell(Int32 index) => IsInside(index) ? _grid[index] : null;
	public void SetCell(Int32 index, Cell<T> cell)
	{
		if (!IsInside(index))
			return;

		_grid[index] = cell;

		OnCellsUpdated?.Invoke();
	}
	public void ChangeCellStatus(Int32 index, ECellStatus status)
	{
		if (IsInside(index))
			_grid[index].CellStatus = status;
	}
	public void Swap(Int32 indexFrom, Int32 indexTo)
	{
		var cell = _grid[indexTo];
		_grid[indexTo] = _grid[indexFrom];
		_grid[indexFrom] = cell;

		OnCellsUpdated?.Invoke();
	}

	public Boolean IsNeighbourIndex(Int32 index, Int32 neighbourIndex, Boolean checkDiagonal = false)
	{
		for (int y = -1; y <= 1; y++)
		{
			for (int x = -1; x <= 1; x++)
			{
				if (x == y && x == 0)
					continue;

				if (x == 0 || y == 0)
				{
					var currentIndex = index + y * _width + x;

					if (neighbourIndex == currentIndex)
						return true;
				}
				else if (checkDiagonal)
				{
					var currentIndex = index + y * _width + x;

					if (neighbourIndex == currentIndex)
						return true;
				}
			}
		}
		return false;
	}

	#endregion
}