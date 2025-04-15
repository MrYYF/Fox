using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : Singleton<GameManager> {
    private CinemachineVirtualCamera virtualCamera;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void RigisterPlayer(GameObject player) {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCamera != null) {
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;
        }
        else {
            Debug.LogError("CinemachineVirtualCamera not found in the scene.");
        }
    }
}
