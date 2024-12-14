using UnityEngine;
using System.Collections;

public class MazeSpawner : MonoBehaviour
{
	public enum MazeGenerationAlgorithm
	{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null;

	public bool UseGridPattern = false;  // trueなら縦方向に碁盤目状に区切り
	public int GridSpacing = 1;          // 縦方向に何マスおきに壁を入れるか

	private BasicMazeGenerator mMazeGenerator = null;

	void Start()
	{
		GenerateMazeInEditor();
	}

	public void GenerateMazeInEditor()
	{
		// 既存の子オブジェクトを削除
		while (transform.childCount > 0)
		{
			Transform child = transform.GetChild(0);
			DestroyImmediate(child.gameObject);
		}

		if (UseGridPattern)
		{
			GenerateZDirectionGridMaze();
		}
		else
		{
			if (!FullRandom)
			{
				Random.seed = RandomSeed;
			}
			switch (Algorithm)
			{
				case MazeGenerationAlgorithm.PureRecursive:
					mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
					break;
				case MazeGenerationAlgorithm.RecursiveTree:
					mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
					break;
				case MazeGenerationAlgorithm.RandomTree:
					mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
					break;
				case MazeGenerationAlgorithm.OldestTree:
					mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
					break;
				case MazeGenerationAlgorithm.RecursiveDivision:
					mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
					break;
			}
			mMazeGenerator.GenerateMaze();
			InstantiateMazeObjects();
		}
	}

	void InstantiateMazeObjects()
	{
		for (int row = 0; row < Rows; row++)
		{
			for (int column = 0; column < Columns; column++)
			{
				float x = column * (CellWidth + (AddGaps ? .2f : 0));
				float z = row * (CellHeight + (AddGaps ? .2f : 0));
				MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
				GameObject tmp;
				tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
				tmp.transform.parent = transform;
				if (cell.WallRight)
				{
					tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
					tmp.transform.parent = transform;
				}
				if (cell.WallFront)
				{
					tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
					tmp.transform.parent = transform;
				}
				if (cell.WallLeft)
				{
					tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
					tmp.transform.parent = transform;
				}
				if (cell.WallBack)
				{
					tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
					tmp.transform.parent = transform;
				}
				if (cell.IsGoal && GoalPrefab != null)
				{
					tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0)) as GameObject;
					tmp.transform.parent = transform;
				}
			}
		}
		if (Pillar != null)
		{
			for (int row = 0; row < Rows + 1; row++)
			{
				for (int column = 0; column < Columns + 1; column++)
				{
					float x = column * (CellWidth + (AddGaps ? .2f : 0));
					float z = row * (CellHeight + (AddGaps ? .2f : 0));
					GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity) as GameObject;
					tmp.transform.parent = transform;
				}
			}
		}
	}

	// 縦方向にのみ碁盤になるように壁を配置する処理
	void GenerateZDirectionGridMaze()
	{
		// フロアをすべて配置
		for (int row = 0; row < Rows; row++)
		{
			for (int column = 0; column < Columns; column++)
			{
				float x = column * (CellWidth + (AddGaps ? .2f : 0));
				float z = row * (CellHeight + (AddGaps ? .2f : 0));
				GameObject floorObj = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.identity);
				floorObj.transform.parent = transform;
			}
		}

		// Z軸方向に一定間隔ごとに横方向の壁（front/back）を置く
		// 例：GridSpacing=1なら全ての行間に壁を置く
		//    GridSpacing=2なら2マスごとに壁を置く
		for (int row = 0; row <= Rows; row += GridSpacing)
		{
			// row行目の前方に壁を敷く
			// 全列に渡って壁を設置することでZ方向へのマス目線を作る
			for (int column = 0; column < Columns; column++)
			{
				float x = column * (CellWidth + (AddGaps ? .2f : 0));
				float z = row * (CellHeight + (AddGaps ? .2f : 0)) - (CellHeight / 2);
				GameObject wallObj = Instantiate(Wall, new Vector3(x, 0, z) + Wall.transform.position, Quaternion.Euler(0, 0, 0));
				wallObj.transform.parent = transform;
			}
		}

		// ピラー配置（必要なら）
		if (Pillar != null)
		{
			for (int row = 0; row < Rows + 1; row++)
			{
				for (int column = 0; column < Columns + 1; column++)
				{
					float x = column * (CellWidth + (AddGaps ? .2f : 0));
					float z = row * (CellHeight + (AddGaps ? .2f : 0));
					GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity) as GameObject;
					tmp.transform.parent = transform;
				}
			}
		}

		// ゴールオブジェクト配置（任意）
		if (GoalPrefab != null)
		{
			// 最奥の行などに配置例
			float gx = (Columns - 1) * (CellWidth + (AddGaps ? .2f : 0));
			float gz = (Rows - 1) * (CellHeight + (AddGaps ? .2f : 0));
			GameObject goalObj = Instantiate(GoalPrefab, new Vector3(gx, 1, gz), Quaternion.identity) as GameObject;
			goalObj.transform.parent = transform;
		}
	}

	public void DeleteMaze()
	{
		// 既存の子オブジェクトを削除
		while (transform.childCount > 0)
		{
			Transform child = transform.GetChild(0);
			DestroyImmediate(child.gameObject);
		}
	}
}
