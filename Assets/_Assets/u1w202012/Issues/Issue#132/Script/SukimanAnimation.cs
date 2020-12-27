using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SukimanAnimation : MonoBehaviour
{
    private Animator m_Animator;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Animator.SetBool("IsSleep", false);
        m_Animator.SetBool("IsJump", false);
        m_Animator.SetBool("IsBlink", false);
        float randomNum = Random.Range(0.0f, 10.0f);
        if (randomNum <= 1.0f)
        {
            m_Animator.SetBool("IsJump", false);
            m_Animator.SetBool("IsBlink", false);
            m_Animator.SetBool("IsSleep", true);
        } else if (1.0f < randomNum && randomNum <=  3.0f)
        {
            m_Animator.SetBool("IsBlink", false);
            m_Animator.SetBool("IsSleep", false);
            m_Animator.SetBool("IsJump", true);
        }
        else if (3.0f < randomNum && randomNum <= 6.0f)
        {
            m_Animator.SetBool("IsSleep", false);
            m_Animator.SetBool("IsJump", false);
            m_Animator.SetBool("IsBlink", true);
        }
    }
}
