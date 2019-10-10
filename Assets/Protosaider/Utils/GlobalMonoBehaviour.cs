using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Protosaider.Utils
{
	
	public class GlobalMonoBehaviour : MonoBehaviour
	{
		[SerializeField, HideInInspector]
		private Boolean _dontDestroyOnLoad;

		public Boolean DontDestroyOnLoad
		{
			get => _dontDestroyOnLoad;
			set
			{
				if (value)
					DontDestroyOnLoad(gameObject);

				if (!value && _dontDestroyOnLoad != value)
					return;

				_dontDestroyOnLoad = value;
			}
		}

		static GlobalMonoBehaviour()
		{
			GlobalMonoBehaviourParent = new GameObject($"{nameof(GlobalMonoBehaviourParent)}");
			DontDestroyOnLoad(GlobalMonoBehaviourParent);
			//GlobalMonoBehaviourParentPersistent = new GameObject($"{nameof(GlobalMonoBehaviourParentPersistent)}");
			//DontDestroyOnLoad(GlobalMonoBehaviourParentPersistent);
		}

		private static readonly GameObject GlobalMonoBehaviourParent;
		//private static readonly GameObject GlobalMonoBehaviourParentPersistent;

		public event Action StartEvent = () => { };
		public event Action EnableEvent = () => { };
		public event Action DisableEvent = () => { };
		public event Action DestroyEvent = () => { };

		private void Start() => StartEvent();
		private void OnEnable() => EnableEvent();
		private void OnDisable() => DisableEvent();
		private void OnDestroy() => DestroyEvent();


		public event Action FixedUpdateEvent = () => { };
		public event Action UpdateEvent = () => { };
		public event Action LateUpdateEvent = () => { };

		private void FixedUpdate() => FixedUpdateEvent();
		private void Update() => UpdateEvent();
		private void LateUpdate() => LateUpdateEvent();


		public static TExecutor Create<TExecutor>(String name = "GlobalMonoBehaviour", Boolean dontDestroyOnLoad = false,
			HideFlags hideFlags = HideFlags.None) where TExecutor : GlobalMonoBehaviour
		{
			var gameObject = new GameObject(name)
			{
				hideFlags = hideFlags
			};

			var executor = gameObject.AddComponent<TExecutor>();
			executor.DontDestroyOnLoad = dontDestroyOnLoad;

			executor.transform.SetParent(GlobalMonoBehaviourParent.transform);

			//executor.transform.SetParent(dontDestroyOnLoad
			//	? GlobalMonoBehaviourParentPersistent.transform
			//	: GlobalMonoBehaviourParent.transform);

			return executor;
		}

	}
}