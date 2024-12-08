using UnityEngine;

public class UpTriggerScript : MonoBehaviour {

    public GameStarttoEnd gameStarttoEnd;

    void OnTriggerEnter2D(Collider2D trigger) {
        if (trigger.gameObject.layer == 3) {
            gameStarttoEnd.Up();
        }
    }
}