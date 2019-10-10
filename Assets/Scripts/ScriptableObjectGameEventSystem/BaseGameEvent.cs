using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseGameEvent<T> : ScriptableObject
{
	public event Action<T> EventListeners;
	public void Invoke(T item) => EventListeners?.Invoke(item);
}
