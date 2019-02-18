using UnityEngine;
using System;
using System.Collections;

public class Tile : MonoBehaviour {
	
	public Vector2 moveDirection;
	public Vector2 touchOffset;
	public int xGrid;
	public int yGrid;
	public int value;

	public delegate void OnTouchDelegate( GameObject touchedObject );
	public delegate void OnDragDelegate( Vector2 touchPos );
	public delegate void OnReleaseDelegate( GameObject touchedObject );
	
	OnTouchDelegate touchDelegate;
	OnDragDelegate dragDelegate;
	OnReleaseDelegate releaseDelegate;

	public OnTouchDelegate TouchDelegate
	{
		set { touchDelegate = value; }
	}

	public OnDragDelegate DragDelegate
	{
		set { dragDelegate = value; }
	}

	public OnReleaseDelegate ReleaseDelegate
	{
		set { releaseDelegate = value; }
	}

	void OnMouseDown()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 10;
		Vector2 touchPos = Camera.main.ScreenToWorldPoint(mousePos);

		touchOffset = new Vector2(touchPos.x - transform.position.x, 
		                          touchPos.y - transform.position.y);

		touchDelegate( gameObject );
	}

	void OnMouseDrag()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 10;
		Vector2 touchPos = Camera.main.ScreenToWorldPoint(mousePos);

		dragDelegate( touchPos );
	}

	void OnMouseUp()
	{
		touchOffset = new Vector2();

		releaseDelegate( gameObject );
	}
}
