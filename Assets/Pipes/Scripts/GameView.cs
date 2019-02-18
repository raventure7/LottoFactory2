using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


public class GameView : MonoBehaviour {

	public MenuView menu;
	public Texture2D backBtnTexture;
	public GameObject tile;
	public Material spriteMaterial;
	public Texture2D startTexture;
	public Texture2D endTexture;
	public List<Texture2D> tileTextures;
	public List<Vector2> tileBehaviours;

	private int rows, columns;
	private float xMove, yMove, scale, spacer, gap, tileSize;
	private Vector2[,] gridPositions;
	private Vector2 min, max;
	private List<GameObject> tiles;
	private Tile selected;
	private MapDecoder mapDecoder;
	
	void Start () 
	{
		tileSize = tileTextures[0].width;

		// tileBehaviours contains the input + output directions for each pipe
		// 0 - up
		// 1 - right
		// 2 - down
		// 3 - left

		// insert a blank tileBehaviour as the first entry in the list
		tileBehaviours.Insert(0, Vector2.zero);
		
		// set up map decoder, create call backs
		mapDecoder = new MapDecoder(tileBehaviours);
		mapDecoder.LevelComplete += HandleLevelComplete;
	}

	void HandleLevelComplete ()
	{
		// if level is complete, unlock the next one
		StartCoroutine(ReturnToMenu(1));
	}

	IEnumerator ReturnToMenu(float delay)
	{
		// return to menu after completing level
		yield return new WaitForSeconds(delay);

		menu.NextLevel();
		DestroyLevel();
	}

	public void DestroyLevel()
	{
		foreach(GameObject t in tiles)
			Destroy(t);		
	}
	
	public void CreateLevel(int mapNum)
	{
		// pass map file to the map decoder class to
		// extract data
		mapDecoder.ChangeMap("map" + mapNum);

		rows = mapDecoder.rows;
		columns = mapDecoder.columns;

		if(columns > 4)
			scale = 4 / (float)columns;
		else
			scale = 1;

		gap = ((tileSize / 100) * 0.025f) * scale;
		spacer = ((tileSize / 100) + gap) * scale;

		// grid positions holds the world position of each tile
		gridPositions = new Vector2[columns, rows];
		
		tiles = new List<GameObject>();

		float posY = ((rows-2) * 0.5f * spacer) + (gap/2);
		
		// create the grid of tiles, row by row
		for (int i = rows-1; i >= 0; i--) 
		{
			float posX = ((columns-2) * 0.5f * spacer) + (gap/2);
			
			for (int j = columns-1; j >= 0; j--) 
			{
				int tileType = mapDecoder.GetTile(j, i);

				if(tileType != 0) 
				{
					GameObject tile = CreateTile(new Vector2(posX, posY), tileTextures[tileType-1], true);
					Tile properties = tile.GetComponent<Tile>();

					// assign properties and delegate actions
					properties.xGrid = j;
					properties.yGrid = i;
					properties.value = tileType;
					properties.TouchDelegate = OnTouchTile;
					properties.DragDelegate = OnDragTile;
					properties.ReleaseDelegate = OnReleaseTile;

					tiles.Add(tile);
				}
				
				gridPositions[j, i] = new Vector2(posX, posY);

				posX -= spacer;
			}

			posY -= spacer;
		}

		// graphics for the start and end pipes
		GameObject startTile = CreateTile (new Vector2(gridPositions[0, 0].x, gridPositions[0, 0].y - spacer), startTexture, false);
		GameObject endTile = CreateTile (new Vector2(gridPositions[columns-1, rows-1].x, gridPositions[columns-1, rows-1].y + spacer), endTexture, false);
	
		tiles.Add(startTile);
		tiles.Add(endTile);
	}

	GameObject CreateTile (Vector2 pos, Texture2D tex, bool collider)
	{
		float colliderScale = tileSize / 100f;

		// create tile game object
		GameObject newTile = Instantiate(tile, new Vector3(pos.x, pos.y, 0), Quaternion.identity) as GameObject;
		newTile.transform.parent = transform;
		newTile.transform.localScale = new Vector3(scale, scale, scale);

        // create sprite and give it the correct texture
        //Sprite sprite = new Sprite();
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tileSize, tileSize), new Vector2(0,0), 100f);	

		SpriteRenderer spriteRenderer = newTile.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = sprite;
		spriteRenderer.material = spriteMaterial;

		// if its a draggable tile setup collider, else delete it
		if(collider)
		{
			BoxCollider2D collider2D = newTile.GetComponent<BoxCollider2D>();
			collider2D.size = new Vector2(colliderScale, colliderScale);
			collider2D.offset = new Vector2(colliderScale/2, colliderScale/2);
		}
		else 
		{
			Destroy(newTile.GetComponent<BoxCollider2D>());
		}

		return newTile;
	}

	Vector2 startMousePos, newMousePos;
	bool moveHorizontal, moveVertical;
	int left, right, up, down;

	void OnTouchTile (GameObject tile)
	{
		// if the tile is selected, check to see if it can be
		// moved in any direction
		if(selected == null)
		{
			selected = tile.GetComponent<Tile>();
			startMousePos = GetMousePos();

			moveHorizontal = moveVertical = false;

			yMove = selected.transform.position.y;
			xMove = selected.transform.position.x;
		}
	}

	void OnDragTile(Vector2 touch)
	{
		// allow movement only in available directions
		// and limit range of movement
		if(selected != null)
		{
			// if tile is in starting position, detect if mouse movement
			// is predominantly horizontal or vertical and lock to that axis
			if(selected.transform.position == new Vector3(gridPositions[selected.xGrid, selected.yGrid].x,
			                                              gridPositions[selected.xGrid, selected.yGrid].y, 0))
			{
				newMousePos = GetMousePos();

				if(newMousePos != startMousePos)
				{
					if(Mathf.Abs(newMousePos.x - startMousePos.x) > Mathf.Abs(newMousePos.y - startMousePos.y))
					{
						moveHorizontal = true;
						moveVertical = false;
					} 
					else 
					{
						moveHorizontal = false;
						moveVertical = true;
					}
				}
			}

			Vector2 pos = new Vector2(selected.xGrid, selected.yGrid);
			
			// based on whether the user is trying to move the tile horizontally
			// or vertically, find out if there are any empty spaces in that
			// direction, and if so how many (to work out movement limits)
			if(moveHorizontal)
			{
				xMove = touch.x - selected.touchOffset.x;

				left = selected.xGrid - mapDecoder.GetMoveLimit(pos, 3);
				right = selected.xGrid + mapDecoder.GetMoveLimit(pos, 1);

				min = gridPositions[left, selected.yGrid];
				max = gridPositions[right, selected.yGrid];
				
				if(xMove > max.x) xMove = max.x;
				if(xMove < min.x) xMove = min.x;

				selected.transform.position = new Vector3(xMove, yMove, 0);
			}

			if(moveVertical) 
			{
				yMove = touch.y - selected.touchOffset.y;

				up = selected.yGrid + mapDecoder.GetMoveLimit(pos, 0);
				down = selected.yGrid - mapDecoder.GetMoveLimit(pos, 2);

				min = gridPositions[selected.xGrid, down];
				max = gridPositions[selected.xGrid, up];
				
				if(yMove > max.y) yMove = max.y;
				if(yMove < min.y) yMove = min.y;

				selected.transform.position = new Vector3(xMove, yMove, 0);
			}
		}
	}

	void OnReleaseTile(GameObject tile)
	{
		// on release, snap the tile into the closest
		// available grid space
		if(tile == selected.gameObject && selected != null)
		{
			int xSnap = selected.xGrid;
			int ySnap = selected.yGrid;

			if(moveHorizontal) 
			{
				for(int i = left; i <= right; i++)
				{
					if(Mathf.Abs(selected.transform.position.x - gridPositions[i, selected.yGrid].x) < (tileSize * scale / 200f))
					{
						xSnap = i;
						break;
					}
				}
			}

			if(moveVertical) 
			{
				for(int i = down; i <= up; i++)
				{
					if(Mathf.Abs(selected.transform.position.y - gridPositions[selected.xGrid, i].y) < (tileSize * scale / 200f))
					{
						ySnap = i;
						break;
					}
				}
			}

			xMove = gridPositions[xSnap, selected.yGrid].x;
			yMove = gridPositions[selected.xGrid, ySnap].y;

			selected.transform.position = new Vector3(xMove, yMove, 0);
			
			// update map decoder with the new layout
			if(selected.xGrid != xSnap || selected.yGrid != ySnap)
			{
				mapDecoder.SetTile(xSnap, ySnap, selected.value);
				mapDecoder.SetTile(selected.xGrid, selected.yGrid, 0);
				selected.xGrid = xSnap;
				selected.yGrid = ySnap;
			}
			
			selected = null;
		}

		// check to see if the level has been solved
		mapDecoder.CheckLevel();
	}

	Vector2 GetMousePos()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 10;

		return Camera.main.ScreenToWorldPoint(mousePos);
	}
}