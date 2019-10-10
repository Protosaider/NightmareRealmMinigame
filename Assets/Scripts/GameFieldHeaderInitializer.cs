using System.Collections.Generic;
using UnityEngine;

public class GameFieldHeaderInitializer : MonoBehaviour
{
	[SerializeField]
	private Transform _cellsHolderUI;
	[SerializeField]
	private GameObject _cellUIPrefab;
	[SerializeField]
	private GameHeader _gameHeader;

	[SerializeField]
	private GameFieldCellsData _cellsData;

	public void Activate() => gameObject.SetActive(true);

    private void Awake()
	{
		Initialize();
	}

	public void Refresh()
	{
        _gameHeader.Clear();

        var markers = new List<MarkerData>()
        {
            _cellsData.MarkerTypeOne, _cellsData.MarkerTypeTwo, _cellsData.MarkerTypeThree
        };

        for (int x = 0; x < _gameHeader.MarkersRowSize; x++)
        {
            var cell = new Cell<CellData>();

            if (x % 2 == 0)
            {
                var item = markers[UnityEngine.Random.Range(0, markers.Count)];
                cell.Item = item;
                markers.Remove(item);
            }

            _gameHeader.SetCell(x, cell);
        }
    }

	private void Initialize()
	{
		var markers = new List<MarkerData>()
		{
			_cellsData.MarkerTypeOne, _cellsData.MarkerTypeTwo, _cellsData.MarkerTypeThree
		};

		for (int x = 0; x < _gameHeader.MarkersRowSize; x++)
		{
			var cell = new Cell<CellData>();

			if (x % 2 == 0)
			{
				var item = markers[UnityEngine.Random.Range(0, markers.Count)];
				cell.Item = item;
				markers.Remove(item);
			}

			_gameHeader.SetCell(x, cell);

			Instantiate(_cellUIPrefab, _cellsHolderUI, true);
		}
	}
}