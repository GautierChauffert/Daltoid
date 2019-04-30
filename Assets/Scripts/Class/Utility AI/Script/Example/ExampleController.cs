using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleController : MovementController
{
	protected Example example;

	public ExampleController(Example e) : base(e)
	{
		this.transform = entity.transform;
	}
}
