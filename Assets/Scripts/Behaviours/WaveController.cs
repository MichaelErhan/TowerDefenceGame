using System;
using Netologia;
using Netologia.Systems;
using Netologia.TowerDefence.Settings;
using UnityEngine;
using Zenject;

namespace Behaviours
{
    public class WaveController : MonoBehaviour
    {
        private UnitSystem _units;              // injected
        private WavePresetSettings _settings;   // injected

        private (int Wave, int Pack, int Unit) _data;
        [SerializeField]
        private Transform[] _paths;
        [SerializeField]
        private Transform _spawner;
        
        [SerializeField] private GameObject waveInfoText; 
        public event Action OnLastWaveEnded;
        
        public float Delay { get; private set; }
        public bool InWave { get; private set; }
        public (int Wave, int Pack, int Unit) Data => _data;
        public int WaveCount => _settings != null ? _settings.Count : 0;

        public Vector3[] GetPath()
        {
            var path = new Vector3[_paths.Length];
            for (int i = 0, iMax = path.Length; i < iMax; i++)
                path[i] = _paths[i].position;
            return path;
        }

        private void Update()
        {
            // Проверяем, инициализированы ли _units и _settings
            if (_units == null || _settings == null)
            {
                Debug.LogError("UnitSystem or WavePresetSettings not initialized.");
                return;
            }

            // delaying
            if (Delay > 0)
            {
                Delay -= TimeManager.DeltaTime;
                return;
            }

            if (InWave)
                RespawnUnit();
            else
                InWave = true;
        }

        private void RespawnUnit()
        {
            if (_settings == null || _data.Wave >= _settings.Count)
            {
                Debug.LogError("WavePresetSettings not properly set up.");
                return;
            }

            var wave = _settings[_data.Wave];
            var pack = wave.Packs[_data.Pack];

            if (_units == null || pack.Prefab == null)
            {
                Debug.LogError("Unit system or prefab is null.");
                return;
            }

            // Spawn unit
            var unit = _units[pack.Prefab].Get;
            if (unit == null)
            {
                Debug.LogError("Unable to spawn unit, unit is null.");
                return;
            }

            unit.Respawn(pack.Preset.Preset, _spawner.position);
            _data.Unit++;
            
            // Pack isn't over
            if (_data.Unit < pack.Count)
            {
                Delay = pack.SpawnDelay;
                return;
            }

            // Try get new pack
            _data.Pack++;
            _data.Unit = 0;

            // get new pack success
            if (_data.Pack < wave.Packs.Length)
            {
                Delay = wave.Packs[_data.Pack].SpawnDelay;
                return;
            }

            // try prepare new wave
            _data.Wave++;
            _data.Pack = 0;

            // prepare new wave
            if (_data.Wave < _settings.Count)
            {
                Delay = _settings[_data.Wave].StartDelay;
                InWave = false;
            }
            else
            {
                // Finish game
                enabled = false; // Disable the WaveController
                OnLastWaveEnded?.Invoke(); // Trigger the event indicating the last wave has ended
                
                if (waveInfoText != null)
                {
                    waveInfoText.SetActive(false);
                }
            }
        }

        private void Awake()
        {
            // Проверяем, инициализированы ли _settings, прежде чем использовать его
            if (_settings != null && _settings.Count > _data.Wave)
            {
                Delay = _settings[_data.Wave].StartDelay;
            }
            else
            {
                Debug.LogError("WavePresetSettings is null or has invalid data.");
            }
        }

        private void OnDrawGizmos()
        {
            if (_paths is not null && _paths.Length > 2)
            {
                Gizmos.color = Color.cyan;
                var prev = _paths[0].position;
                for (int i = 1, iMax = _paths.Length; i < iMax; i++)
                {
                    var curr = _paths[i].position;
                    Gizmos.DrawLine(prev, curr);
                    prev = curr;
                }
            }
        }

        [Inject]
        private void Construct(UnitSystem units, WavePresetSettings settings)
        {
            _units = units;
            _settings = settings;
        }
    }
}






// using System;
// using Netologia;
// using Netologia.Systems;
// using Netologia.TowerDefence.Settings;
// using UnityEngine;
// using Zenject;
//
// namespace Behaviours
// {
// 	public class WaveController : MonoBehaviour
// 	{
// 		private UnitSystem _units;				//injected
// 		private WavePresetSettings _settings;	//injected
//
// 		private (int Wave, int Pack, int Unit) _data;
// 		[SerializeField]
// 		private Transform[] _paths;
// 		[SerializeField]
// 		private Transform _spawner;
// 		
// 		[SerializeField] private GameObject waveInfoText; 
// 		public event Action OnLastWaveEnded;
// 		
// 		public float Delay { get; private set; }
// 		public bool InWave { get; private set; }
// 		public (int Wave, int Pack, int Unit) Data => _data;
// 		public int WaveCount => _settings.Count;
//
// 		public Vector3[] GetPath()
// 		{
// 			var path = new Vector3[_paths.Length];
// 			for (int i = 0, iMax = path.Length; i < iMax; i++)
// 				path[i] = _paths[i].position;
// 			return path;
// 		}
//
// 		private void Update()
// 		{
// 			//delaying
// 			if (Delay > 0)
// 			{//тут есть потеря кадра
// 				Delay -= TimeManager.DeltaTime;
// 				return;
// 			}
//
// 			if (InWave)
// 				RespawnUnit();
// 			//start new wave
// 			else
// 				InWave = true;
// 		}
// 		private void RespawnUnit()
// 		{
// 			var wave = _settings[_data.Wave];
// 			var pack = wave.Packs[_data.Pack];
// 			//Spawn unit
// 			var unit = _units[pack.Prefab].Get;
// 			unit.Respawn(pack.Preset.Preset, _spawner.position);
// 			_data.Unit++;
// 			//Pack isn't over
// 			if (_data.Unit < pack.Count)
// 			{
// 				Delay = pack.SpawnDelay;
// 				return;
// 			}
// 			//Try get new pack
// 			_data.Pack++;
// 			_data.Unit = 0;
// 			//get new pack success
// 			if (_data.Pack < _settings[_data.Wave].Packs.Length)
// 			{
// 				Delay = wave.Packs[_data.Pack].SpawnDelay;
// 				return;
// 			}
// 			//try prepare new wave
// 			_data.Wave++;
// 			_data.Pack = 0;
// 			//prepare new wave
// 			if (_data.Wave < _settings.Count)
// 			{
// 				Delay = _settings[_data.Wave].StartDelay;
// 				InWave = false;
// 			}
// 			else
// 			{
// 				// Finish game
// 				enabled = false; // Disable the WaveController
// 				OnLastWaveEnded?.Invoke(); // Trigger the event indicating the last wave has ended
// 				
// 				if (waveInfoText != null)
// 				{
// 					waveInfoText.SetActive(false);
// 				}
// 			}
// 		}
// 		private void Awake()
// 			=> Delay = _settings[_data.Wave].StartDelay;
//
// 		private void OnDrawGizmos()
// 		{
// 			if (_paths is not null && _paths.Length > 2)
// 			{
// 				Gizmos.color = Color.cyan;
// 				var prev = _paths[0].position;
// 				for (int i = 1, iMax = _paths.Length; i < iMax; i++)
// 				{
// 					var curr = _paths[i].position;
// 					Gizmos.DrawLine(prev, curr);
// 					prev = curr;
// 				}
// 			}
// 		}
// 		[Inject]
// 		private void Construct(UnitSystem units, WavePresetSettings settings)
// 			=> (_units, _settings) = (units, settings);
// 	}
// }