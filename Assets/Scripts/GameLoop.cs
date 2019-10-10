using System;
using System.Collections;
using System.Collections.Generic;
using Protosaider;
using UnityEngine;
using UnityEngine.Events;

public class GameLoop : SingletonMB<GameLoop>
{
	[SerializeField]
	private Single _gameLoopTimeScale;

	[SerializeField]
	private VoidGameEvent _onSwitchFromStateTutorialToGame;
	[SerializeField]
	private VoidGameEvent _onSwitchToStateGame;
	[SerializeField]
	private VoidGameEvent _onSwitchToStateVictory;

    private enum EGameState
	{
		Tutorial,
		Game,
		Victory
	}

    private EGameState _currentState;

	public void SwitchToStateTutorial()
	{
		switch (_currentState)
		{
			case EGameState.Tutorial:
				return;
		}
		SetCurrentState(EGameState.Tutorial);
		// State Tutorial enter logic
	}

	public void SwitchToStateGame()
	{
		switch (_currentState)
		{
			case EGameState.Game:
				return;
            case EGameState.Tutorial:
				_onSwitchFromStateTutorialToGame.Invoke();
				SetCurrentState(EGameState.Game);
				return;
		}
		SetCurrentState(EGameState.Game);
        // State Game enter logic
		_onSwitchToStateGame.Invoke();
    }

	public void SwitchToStateVictory()
	{
		switch (_currentState)
		{
			case EGameState.Victory:
				return;
		}
		SetCurrentState(EGameState.Victory);
        // State Victory enter logic
		_onSwitchToStateVictory.Invoke();
    }

	private void SetCurrentState(EGameState value) => _currentState = value;

	private void eventHandler()
	{
		switch (_currentState)
		{
			case EGameState.Tutorial:
				break;
			case EGameState.Game:
				break;
			case EGameState.Victory:
				break;
		}
	}

}
