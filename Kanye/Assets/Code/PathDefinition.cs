// Import libraries
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PathDefinition : MonoBehaviour
{
	// Create point objects to manipulate the points the user creates within this script
	public Transform[] Points;

	// Move an object along the line
	public IEnumerator<Transform> GetPathEnumerator()
	{
		// If there are not enough points, end the process
		if (Points == null || Points.Length < 1)
			yield break;

		// direction = 1 means going to the right/forward
		var direction = 1;
		var index = 0;

		while(true)
		{
			yield return Points[index];

			// If there is only one point, don't change the direction
			if (Points.Length == 1)
				continue;

			// If there are multiple points, go to the end of the line and then reverse direction
			if (index <= 0)
				direction = 1;
			else if (index >= Points.Length - 1)
				direction = -1;

			// This is where the direction of the object being manipulated gets changed
			index = index + direction;
		}

	}
	
	public void OnDrawGizmos()
	{
		// End the process if there are not enough points to make a line
		if (Points == null || Points.Length < 2)
			return;

		// Draw a visual line between the points given by the user
		for (var i = 1; i < Points.Length; i++)
		{
			// Draw a line between the previous point and the current point in that order
			Gizmos.DrawLine(Points[i - 1].position, Points[i].position);
		}
	}
}
