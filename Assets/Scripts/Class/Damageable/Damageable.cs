using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damageable
{
	public interface IDamageableEntity
	{
		void GetDamagedBy(Color color, float damage);
	}

	public struct DamageData
	{
		public Color color;
		public float damage;

		public DamageData(Color c, float d)
		{
			this.color = c;
			this.damage = d;
		}
	}
}


