using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseGameEventListener<T, TEvent, TUnityEventResponse> : MonoBehaviour
	where TEvent : BaseGameEvent<T> 
	where TUnityEventResponse : UnityEvent<T>
{
	[SerializeField]
	private TEvent _gameEvent;

	[SerializeField]
	public TUnityEventResponse _unityEventResponse;

	public TEvent GameEvent
	{
		get => _gameEvent;
		set => _gameEvent = value;
	}

	private void OnEnable()
	{
		if (_gameEvent == null)
			return;

		GameEvent.EventListeners += OnEventInvoked;
	}

	private void OnDisable()
	{
		if (_gameEvent == null)
			return;

		GameEvent.EventListeners -= OnEventInvoked;
	}

    public void OnEventInvoked(T item) => _unityEventResponse.Invoke(item);
}