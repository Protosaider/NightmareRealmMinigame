using System;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldInitializer : MonoBehaviour
{
	[SerializeField]
	private Transform _cellsHolderUI;
	[SerializeField]
	private GameObject _cellUIPrefab;
	[SerializeField]
	private GameField _gameField;

	[SerializeField]
	private GameFieldCellsData _cellsData;

	public void Activate() => gameObject.SetActive(true);

	private void Awake()
	{
		Initialize();
	}

	public void Refresh()
	{
		var markers = new BagOfMarkers<MarkerData>(new[]
		{
			_cellsData.MarkerTypeOne, _cellsData.MarkerTypeTwo, _cellsData.MarkerTypeThree
		}, _cellsData.MarkerEachTypeCount);

		_gameField.Clear();

		for (int y = 0; y < _gameField.GridSize; y++)
		{
			for (int x = 0; x < _gameField.GridSize; x++)
			{
				var cell = new Cell<CellData>();

				if (x % 2 == 1)
				{
					if (y % 2 == 0)
					{
						cell.CellStatus = ECellStatus.Blocked;
						cell.Item = _cellsData.Blockage;
					}
					else
						cell.CellStatus = ECellStatus.Empty;
				}
				else
				{
					cell.CellStatus = ECellStatus.Occupied;
					cell.Item = markers.NextMarker();
				}

				_gameField.Grid.SetCell(x, y, cell);
			}
		}

    }

	private void Initialize()
	{
		var markers = new BagOfMarkers<MarkerData>(new[]
		{
			_cellsData.MarkerTypeOne, _cellsData.MarkerTypeTwo, _cellsData.MarkerTypeThree
		}, _cellsData.MarkerEachTypeCount);


        for (int y = 0; y < _gameField.GridSize; y++)
		{
			for (int x = 0; x < _gameField.GridSize; x++)
			{
				var cell = new Cell<CellData>();

				if (x % 2 == 1)
				{
					if (y % 2 == 0)
					{
						cell.CellStatus = ECellStatus.Blocked;
						cell.Item = _cellsData.Blockage;
					}
					else
						cell.CellStatus = ECellStatus.Empty;
                }
				else
				{
					cell.CellStatus = ECellStatus.Occupied;
					cell.Item = markers.NextMarker();
				}

				_gameField.Grid.SetCell(x, y, cell);

				Instantiate(_cellUIPrefab, _cellsHolderUI, true);
            }
        }
	}

	private sealed class BagOfMarkers<T>
	{
		private Dictionary<T, Int32> _bag = new Dictionary<T, Int32>();
		private List<T> _items = new List<T>();

        public BagOfMarkers(IList<T> markers, Int32 eachMarkersAmount)
		{
			Refill(markers, eachMarkersAmount);
		}

		public void Refill(IList<T> markers, Int32 eachMarkersAmount)
		{
			_bag = new Dictionary<T, Int32>(markers.Count);
			_items = new List<T>(markers);

            for (int i = 0; i < markers.Count; i++)
			{
				_bag.Add(markers[i], eachMarkersAmount);
			}
        }

		public T NextMarker()
		{
			var item = _items[UnityEngine.Random.Range(0, _items.Count)];

			if (_bag[item] == 1)
			{
				_bag.Remove(item);
				_items.Remove(item);
			}
			else
				_bag[item]--;

			return item;
		}
	}
}