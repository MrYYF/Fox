using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            PlayerManager.PlayerManagerInstance.TakeDamage(1);
        }
    }
}
