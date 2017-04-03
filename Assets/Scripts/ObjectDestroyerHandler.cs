using UnityEngine;
using System.Collections;

public class ObjectDestroyerHandler : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        other.gameObject.SetActive(false);
    }

}
