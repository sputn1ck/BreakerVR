using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{

	public GameObject Cube;
	public int level;
	public Transform target;
	public Text levelText;
	public Text scoreText;
	public Text highScoreText;
	public Text sumScoreText;
	public Transform cameraTransform;

	private Cube[,] solutionTiles;
	private Cube[,] interactiveTiles;

	private int solutionCount;
	private int activeCount = 0;
	private bool isWon = false;
	private bool isLost = false;
	private int score;
	private int sumScore;
	//private int highScore;

	void Start ()
	{
		Debug.Log ("start");
		score = 0;
		sumScore = 0;
		//highScore = 0;
		highScoreText.text = "High Score: " + GetScore();
		sumScoreText.text = "Sum Score: " + sumScore;
		buildLevel ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		activeCount = 0;
		for (int i = 0; i < level; i++) {
			for (int j = 0; j < level; j++) {
				if (solutionTiles [i, j].isSolution == false && interactiveTiles [i, j].isGreen == true ) {
					isLost = true;
				}
				if (solutionTiles [i, j].isSolution == true && interactiveTiles [i, j].isGreen == true) {
					activeCount++;
				}
			}
		}
		if (activeCount == solutionCount) {
			isWon = true;
		}
	}

	void FixedUpdate() {
		if (score > 0) {
			score--;
		}
		scoreText.text = "Score: " + score;
	}
	void LateUpdate ()
	{
		if (isLost) {
			sumScore += score;
			Debug.Log ("Game Lost");
			this.level = 1;
			if(this.sumScore > GetScore()) {
				SetScore(this.sumScore);


			}
			rebuildLevel ();
		}
		if (isWon) {
			Debug.Log ("Game Won");
			this.level++;
			sumScore+=score+(level*100);
			rebuildLevel ();
		}
	}

	void rebuildLevel ()
	{
		isLost = false;
		isWon = false;
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Cube");
		foreach (GameObject cube in gameObjects) {
			Destroy (cube);
		}
		buildLevel ();
	}

	void buildLevel ()
	{
		Debug.Log ("Building Level: " + this.level);
		levelText.text = "Level: " + this.level;
		sumScoreText.text = "Sum Score: " + this.sumScore;
		highScoreText.text = "High Score: " + GetScore();
		cameraTransform.transform.position = new Vector3 (0, 1.2f + ((float) level * 0.2f), 0);
		score = 500 * this.level;
		createInteractiveTiles ();
		createSolutionTiles ();
	}

	void createInteractiveTiles ()
	{
		interactiveTiles = new Cube[level, level];
		for (int i = 0; i < level; i++) {
			for (int j = 0; j < level; j++) {
				Vector3 pos = new Vector3 (0, 0.5f + j, 4);
				Vector3 rotatePos = rotateY (pos, -Mathf.PI / (12) * (i + 1) - Mathf.PI / 8);
				Vector3 lookAt = (target.position - rotatePos).normalized;
				GameObject unselectableCube = Object.Instantiate (Cube, rotatePos, Quaternion.LookRotation (lookAt)) as GameObject;
				Cube mono = unselectableCube.GetComponent (typeof(Cube)) as Cube;
				interactiveTiles [i, j] = mono;
			}
		}

	}

	void createSolutionTiles ()
	{
		solutionCount = 0;
		solutionTiles = new Cube[level, level];
		for (int i = 0; i < level; i++) {
			for (int j = 0; j < level; j++) {
				Vector3 pos = new Vector3 (0, 0.5f + j, 4);
				Vector3 rotatePos = rotateY (pos, Mathf.PI / (12) * (i + 1) + Mathf.PI / 8);
				Vector3 lookAt = (target.position - rotatePos).normalized;

				GameObject unselectableCube = Object.Instantiate (Cube, rotatePos, Quaternion.LookRotation (lookAt)) as GameObject;
				Cube mono = unselectableCube.GetComponent (typeof(Cube)) as Cube;
				solutionTiles [i, j] = mono;
				solutionTiles [i, j].isSelectable = false;
				if (solutionCount > 0) {	
					if (Random.Range (0, 100) < 60) {
						solutionTiles [i, j].setSolution ();
						solutionCount++;
					}
				} else {
					solutionTiles [i, j].setSolution ();
					solutionCount++;
				}
			}
		}
		
	}

	Vector3 rotate (float[][] m, Vector3 v)
	{
		return new Vector3 (m [0] [0] * v.x + m [0] [1] * v.y + m [0] [2] * v.z, 
		                   m [1] [0] * v.x + m [1] [1] * v.y + m [1] [2] * v.z,
		                   m [2] [0] * v.x + m [2] [1] * v.y + m [2] [2] * v.z);
	}

	Vector3 rotateZ (Vector3 v, float angle)
	{
		float[][] m = new float[][] { new float[] {
			Mathf.Cos (angle),
			-Mathf.Sin (angle),
			0
		},
										new float[] {-Mathf.Sin (angle), Mathf.Cos (angle), 0}, 
											new float[]{0, 0, 1 }};

		return rotate (m, v);
	}

	Vector3 rotateY (Vector3 v, float angle)
	{

		float[][] m = new float[][] { new float[] {
			Mathf.Cos (angle),
			0,
			-Mathf.Sin (angle)
		},
			new float[] {0, 1, 0}, 
			new float[]{Mathf.Sin (angle), 0, Mathf.Cos (angle) }};
		return rotate (m, v);
	}

	public int GetScore () {
		if(!PlayerPrefs.HasKey("Score")){
			PlayerPrefs.SetInt("Score",0);
		}
		return PlayerPrefs.GetInt("Score");
	}
	
	public void SetScore(int value){
		PlayerPrefs.SetInt("Score",value);
	}

}
