using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepeScript : MonoBehaviour {

    [SerializeField]
    private Animator m_Animator;

    private bool m_CanPlayAnimation = true;

    private void Update()
    {
        if (m_CanPlayAnimation)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_CanPlayAnimation = false;
                m_Animator.Play("PepeTheKing",0);
            }
        }
    }

    private void AnimationEnd()
    {
        m_CanPlayAnimation = true;
    }
}
