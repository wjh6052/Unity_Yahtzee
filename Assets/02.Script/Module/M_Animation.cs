using UnityEngine;
using UnityEngine.Windows;

public class M_Animation : MonoBehaviour
{
    [HideInInspector]
    public Animator MeshAnimator;

    bool bIsSit; // æ…æ“¿ª∂ß


    private void Awake()
    {
        MeshAnimator = GetComponent<Animator>();
    }

    bool IsAnimParameter(string ParameterName)
    {
        foreach (AnimatorControllerParameter param in MeshAnimator.parameters)
        {
            if (param.name == ParameterName)
                return true;
        }
        return false;
    }

    public void MoveType(float input)
    {
        if (!IsAnimParameter("MoveType")) return;

        MeshAnimator.SetFloat("MoveType", input);
    }



    public bool IsSit
    {
        get { return bIsSit; }

        set 
        {
            if (!IsAnimParameter("MoveType")) return;

            bIsSit = value;
            MeshAnimator.SetBool("IsSit", bIsSit);
        }
    }
}
