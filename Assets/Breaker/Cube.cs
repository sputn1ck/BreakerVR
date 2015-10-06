using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Cube : MonoBehaviour {

	private Vector3 startingPosition;
	private bool clicked;
	public bool isSelectable;
	public bool isSolution;
	public bool isGreen;

	void Start() {
		startingPosition = transform.localPosition;
		SetClicked(false);
	}

	void Update() {
		if (this.isGreen) {
			GetComponent<Renderer> ().material.color = Color.Lerp(Color.red, Color.green, Time.time/8);
		}
	}
	
	public void Reset() {
		transform.localPosition = startingPosition;
		SetClicked (false);
	}
	
	public void SetClicked(bool clicked) {
		if (this.isSelectable) {
			//GetComponent<Renderer> ().material.color = clicked ? Color.green : Color.red;
			GetComponent<Renderer> ().material.color = Color.red;
			if(clicked) {
				this.isGreen = true;
			}
		} else {
			GetComponent<Renderer> ().material.color = isSolution ? Color.green: Color.red;
		}
	}

	public void setSolution() {
		isSolution = true;
	}
}
