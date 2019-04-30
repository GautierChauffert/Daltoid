using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : LivingEntity
{
	protected override void Start()
	{
		base.Start();
		
		// Initialize AI behaviour (this will launch the AI)
		this.behaviour = ExampleAI.Initialize(this);
	}
}
