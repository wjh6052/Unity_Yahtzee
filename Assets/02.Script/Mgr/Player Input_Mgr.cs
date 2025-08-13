using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerInput_Mgr : MonoBehaviour
{
    public Vector2 InputMove;
    public Vector2 InputLook;

    public bool bWalk = false;

    public bool bEnter = false;




    public static PlayerInput_Mgr Inst;


    void Start()
    {
        Inst = this;
    }


    void OnMove(InputValue val)
    {
        if (bEnter) return;

        InputMove = val.Get<Vector2>();
    }


    void OnLook(InputValue val)
    {
        InputLook = val.Get<Vector2>();
    }

    void OnWalk(InputValue val)
    {
        bWalk = !bWalk;
    }

    void OnInteraction(InputValue val)
    {
        if (bEnter) return;

        if (Game_Mgr.Inst.LocalPlayer == null) return;

        switch(Game_Mgr.Inst.LocalPlayer.PlayerState)
        {
            case EPlayerState.NotReady:
                if(Game_Mgr.Inst.LocalPlayer)
                    Game_Mgr.Inst.LocalPlayer?.m_Interaction?.UseInteraction();
                break;
            case EPlayerState.Ready:
                Table_Mgr.Inst.TryStartGame();
                break;
        }
    }

    void OnMouse(InputValue val)
    {
        Table_Mgr.Inst.OnMouseClick();
    }

    void OnEsc(InputValue val)
    {
        if (bEnter) return;

        if (Game_Mgr.Inst.LocalPlayer == null) return;

        switch(Game_Mgr.Inst.LocalPlayer.PlayerState)
        {
            case EPlayerState.Ready:
                StartCoroutine(Game_Mgr.Inst.LocalPlayer.Co_SitUp());
                break;
        }

    }

    void OnEnter(InputValue val)
    {
        if (UICanvas_Mgr.Inst.UIType == EUIType.Loading) return;

        bEnter = !bEnter;

        UICanvas_Mgr.Inst.ChatPanel.SetChatPanel(bEnter);
    }
}
