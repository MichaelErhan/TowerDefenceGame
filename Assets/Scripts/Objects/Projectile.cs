﻿using Netologia.Behaviours;
using UnityEngine;

namespace Netologia.TowerDefence
{
	public class Projectile : MonoBehaviour, IPoolElement<Projectile>
	{
		private float _damage;
		private Vector3? _endPosition;
		private Unit _target;
		
		private ElementalType _elementalType;

		[field: SerializeField]
		public ParticleSystem HitEffect { get; private set; }
		[field: SerializeField]
		public AudioClip HitSound { get; private set; }
		[field: SerializeField]
		public float MoveSpeed { get; private set; }
		
		public bool HasEffect { get; private set; }
		public bool HasSound { get; private set; }
		
		public Projectile Ref { get; set; }
		public int ID { get; set; }
		public Vector3 TargetPosition => _target != null ? _target.transform.position : _endPosition ?? Vector3.zero;
		
		//public Vector3 TargetPosition => _endPosition ?? _target.transform.position;
		public int TargetID { get; private set; } = -1;

		public void DealDamage()
		{
			//if (_endPosition.HasValue) return;
			
			if (_endPosition.HasValue || _target == null) return;
			
			_target.CurrentHealth -= _damage;
			
			//_target.TryAddEffect(TimeManager.Time, _elementalType);
			
			// Проверка на наличие цели перед вызовом метода
			if (_target != null)
			{
				_target.TryAddEffect(TimeManager.Time, _elementalType);
			}
		}
		
		//public void ResetTarget() => (_endPosition, _target) = (_target.transform.position, null);
		public void ResetTarget()
		{
			if (_target != null)
			{
				_endPosition = _target.transform.position;
			}
			_target = null;
		}

		// public void PrepareData(Vector3 position, Unit target, float damage, ElementalType type)
		// {
		// 	transform.position = position;
		// 	(_target, _damage, _elementalType, _endPosition) = (target, damage, type, null);
		// 	TargetID = target.ID;
		// }
		
		public void PrepareData(Vector3 position, Unit target, float damage, ElementalType type)
		{
			transform.position = position;
			_target = target;
			_damage = damage;
			_elementalType = type;
			_endPosition = null;

			if (_target != null)
			{
				TargetID = target.ID;
			}
		}
		private void Awake()
		{
			HasEffect = HitEffect != null;
			HasSound = HitSound != null;
		}
	}
}