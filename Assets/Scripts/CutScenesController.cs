using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScenesController : MonoBehaviour
{
    public Animator animator;

    public GameObject skyBackground;
    public GameObject cutscenesCamera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayAnimation();
        }

        skyBackground.transform.position = (Vector2)cutscenesCamera.transform.position;
    }

    private void PlayAnimation()
    {
        animator.Play("CutScenesAnimation1");
    }


}
