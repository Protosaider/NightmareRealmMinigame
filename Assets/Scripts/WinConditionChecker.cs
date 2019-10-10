using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionChecker : MonoBehaviour
{
	[SerializeField]
	private GameHeader _gameHeader;
	[SerializeField]
	private GameField _gameField;

	public void CheckWinCondition()
	{
		//if win => 
		for (int i = 0; i < _gameHeader.MarkersRowSize; i++)
		{
			if (i % 2 != 0)
				continue;

			var colorCell = _gameHeader.GetCell(i).Item as MarkerData;

            var column = _gameField.EnumerateColumn(i);

			foreach (var cell in column)
			{
				if (cell.CellStatus != ECellStatus.Occupied)
					return;

				var cellAsMarker = cell.Item as MarkerData;
				if (cellAsMarker.MarkerType != colorCell.MarkerType)
					return;
			}
		}

		GameLoop.Instance.SwitchToStateVictory();
	}
}
