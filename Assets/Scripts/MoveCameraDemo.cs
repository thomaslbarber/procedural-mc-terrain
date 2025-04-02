using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraDemo : MonoBehaviour
{
	public Vector3 moveAmount = new Vector3(0f, 0f, 5f);

	void Update()
	{
		transform.position += moveAmount * Time.deltaTime;
	}
}
