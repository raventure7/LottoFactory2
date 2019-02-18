using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MapDecoder {

	public event Action LevelComplete;

	public int rows, columns;
	
	private int[,] map;
	private List<Vector2> tileBehaviours;

	public MapDecoder(List<Vector2> tileBehaviours)
	{
		this.tileBehaviours = tileBehaviours;
	}
	
	public void ChangeMap(string mapName)
	{
		// load new map asset from Maps folder
		// populate 3d array with its data
		TextAsset file = Resources.Load<TextAsset>("Maps/" + mapName);
		
		string[] fileArray = file.text.Split('\n');
		rows = fileArray.Length;
		columns = Mathf.CeilToInt(fileArray[0].Length / 2f);
		
		map = new int[columns, rows];

		for(int i = rows-1; i >= 0; i--) 
		{
			string[] data = fileArray[i].Split(',');
			
			for(int j = columns-1; j >= 0; j--)
			{
				// flip the rows as we're running through the array backwards
				map[j, rows-i-1] = int.Parse(data[j]);
			}
		}
	}
	
	public int GetTile(int x, int y)
	{
		return map[x, y];	
	}
	
	public void SetTile(int x, int y, int value)
	{
		map[x, y] = value;	
	}

	public int GetMoveLimit(Vector2 pos, int dir)
	{
		Vector2 dirv = GetOutput(dir);
		int spaces = 0;
		
		// check out the grid space next to the selected tile
		// to see if there's space to drag in to
		
		do
		{
			if(pos.x + dirv.x < 0)
				break;
			else if(pos.x + dirv.x >= columns)
				break;
			else if(pos.y + dirv.y < 0)
				break;
			else if(pos.y + dirv.y >= rows)
				break;
			else if(map[(int)(pos.x + dirv.x), (int)(pos.y + dirv.y)] != 0)
				break;

			pos += dirv;
			spaces++;
		} 
		while(true);
		
		return spaces;
	}

	public int GetInput(int dir)
	{
		switch(dir)
		{
			// returns required input for given output
			case 0: return 2;
			case 1: return 3;
			case 2: return 0;
			case 3: return 1;
		}
		
		return 0;
	}

	public Vector2 GetOutput(int dir)
	{
		switch(dir)
		{
			// returns Vector2 movement from given output
			case 0: return new Vector2(0, 1);
			case 1: return new Vector2(1, 0);
			case 2: return new Vector2(0, -1);
			case 3: return new Vector2(-1, 0);
		}
		
		return Vector2.zero;
	}

	public void CheckLevel()
	{
		// starting at the bottom left, follow the path of the 
		// pipes to see if the puzzle has been completed
		Vector2 curTile = new Vector2(0, 0);
		bool broken = false;
		int output = 0;
		 
		while(!broken)
		{
			Vector2 curTileType = tileBehaviours[map[(int)curTile.x, (int)curTile.y]];

			if(curTileType.x == GetInput(output))
			{
				curTile += GetOutput((int)curTileType.y);
				output = (int)curTileType.y;
			}
			else if(curTileType.y == GetInput(output))
			{
				curTile += GetOutput((int)curTileType.x);
				output = (int)curTileType.x;
			}
			else
			{
				broken = true;
			}

			if(curTile.x > columns-1 || curTile.y > rows-1 || curTile.x < 0 || curTile.y < 0) 
			{
				if(curTile.x == columns-1 && curTile.y == rows)
					LevelComplete();
				
				broken = true;
			}
		}
	}
}
