using UnityEngine;

public class LeftTriggerScript : MonoBehaviour {

    public GameStarttoEnd gameStarttoEnd;

    void OnTriggerEnter2D(Collider2D trigger) {
        if (trigger.gameObject.layer == 3) {
            gameStarttoEnd.Left();
        }
    }
}