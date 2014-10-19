// Import libraries
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class FollowPath : MonoBehaviour
{
	// Create a type called FollowType
	public enum FollowType
	{
		MoveTowards,
		Lerp
	}

	// Create a field that uses the type FollowType and explicitly define this default as 0
	public FollowType Type = FollowType.MoveTowards;
	// Define the path that the object needs to follow
	public PathDefinition Path;
	// Define a default speed for the object to move at along the line
	public float Speed = 1;
	// The maximum distance in the path that is allowed before the object switches to move toward the next point on the path
	public float MaxDistanceToGoal = .1f;

	// Retrieve path in desired order from PathDefinition script
	private IEnumerator<Transform> _currentPoint;

	// The method that Unity calls first
	public void Start()
	{
		if (Path == null)
		{
			// Print error message and quit
			Debug.LogError("Path cannot be null", gameObject);
			return;
		}

		// Invoke the GetPathEnumerator method and store it in the private variable
		_currentPoint = Path.GetPathEnumerator();

		// Calls and uses the GetPathEnumerator method
		_currentPoint.MoveNext();

		// This statement allows you to break out of the start method in the case that there are no points in the path
		if (_currentPoint.Current == null)
			return;

		// Set position of the object to the first point in the path
		transform.position = _currentPoint.Current.position;
	}

	public void Update()
	{
		// This statement will get called if the object was created without a path or at least a path that doesn't have at least one point
		if (_currentPoint == null || _currentPoint.Current == null)
			return;

		if (Type == FollowType.MoveTowards)
			// Move toward the next point in the sequence
			transform.position = Vector3.MoveTowards (transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);
		else if (Type == FollowType.Lerp)
			// Find the linear interpellation of the path between the current point and the next point
			transform.position = Vector3.Lerp (transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);

		// Determine the distance between the object and the point it is moving toward
		var distanceSpared = (transform.position - _currentPoint.Current.position).sqrMagnitude;

		// Determine if the object is close enough to the point it is moving toward to start heading to the next point
		if (distanceSpared < MaxDistanceToGoal * MaxDistanceToGoal)
			// If it is, start heading to the next point
			_currentPoint.MoveNext ();

	}
}
