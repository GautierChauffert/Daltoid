using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorableEntity : MonoBehaviour
{
	private static float MINIMAL_COLOR_SCORE = 0.1f;

	public Color color = Color.white;

	protected virtual void Awake()
	{
		// empty
	}

	protected virtual void Start()
	{
		// empty
	}

	public static float ColorCompare(Color c1, Color c2)
	{
		float dist;
		float H1, S1, V1, H2, S2, V2;
		Vector2 dir1, dir2, pos1, pos2;

		if(c1 == Color.white || c2 == Color.white || c1 == Color.black || c2 == Color.black) {
			return MINIMAL_COLOR_SCORE;
		}

		// not finished yet, do not manage white and black
		// (need to use SATURATION for that...)

		Color.RGBToHSV(c1, out H1, out S1, out V1);
		Color.RGBToHSV(c2, out H2, out S2, out V2);

		dir1 = Quaternion.Euler(Vector3.forward * (H1 * 360f)) * Vector2.left;
		dir2 = Quaternion.Euler(Vector3.forward * (H2 * 360f)) * Vector2.left;

		pos1 = dir1 * V1;
		pos2 = dir2 * V2;

		dist = Vector2.Distance(pos1, pos2);

		return Mathf.Max(-0.5f * dist + 1f, MINIMAL_COLOR_SCORE);
	}
}
