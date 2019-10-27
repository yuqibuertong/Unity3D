using UnityEngine;

public class ScoreRecorder : MonoBehaviour {
    public SceneController sceneController;
    public int Score = 0;

	void Start () {
        sceneController = SSDirector.getInstance().currentScenceController as SceneController;
        sceneController.Recorder = this;
        Gate.addScore += GetScore;
	}

    void GetScore() {
        Score++;
    }
}