using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDirection : MonoBehaviour
{

    public static void callDirection(string call, Vector2 playerFacing, PlayerController player)
    {
      Debug.Log("Calling callDirection");
      if(call == "HitDirection")
      {

        if (playerFacing.x == -1 && playerFacing.y == -1) //Facing Bottom Left
        {
          player.ChangeAnimationState("Lea_Hit_1");
        }
        else if (playerFacing.x == 0 && playerFacing.y == -1) //Facing Bottom Middle
        {
          player.ChangeAnimationState("Lea_Hit_2");
        }
        else if (playerFacing.x == 1 && playerFacing.y == -1) //Facing Bottom Right
        {
          player.ChangeAnimationState("Lea_Hit_3");
        }
        else if (playerFacing.x == -1 && playerFacing.y == 0) //Facing Middle Left
        {
          player.ChangeAnimationState("Lea_Hit_4");
        }
          else if (playerFacing.x == 1 && playerFacing.y == 0) //Facing Middle Right
        {
          player.ChangeAnimationState("Lea_Hit_6");
        }
        else if (playerFacing.x == -1 && playerFacing.y == 1) //Facing Top Left
        {
          player.ChangeAnimationState("Lea_Hit_7");
        }
        else if (playerFacing.x == 0 && playerFacing.y == 1) //Facing Top Middle
        {
          player.ChangeAnimationState("Lea_Hit_8");
        }
        else if (playerFacing.x == 1 && playerFacing.y == 1) //Facing Top Right
        {
          player.ChangeAnimationState("Lea_Hit_9");
        }
      }
      else if (call == "AttackDirection")
      {
        if (playerFacing.x == -1 && playerFacing.y == -1) //Facing Bottom Left
    {
      player.rb.AddForce((-player.transform.up-player.transform.right)*player.lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (player.attackIndex != 1)
      {
        player.ChangeAnimationState("Lea_Attack_0_1");
        Vector2 offset = new Vector2(-player.meleeRange*1.5f, -player.meleeRange*1.5f); //Offset our Swing Effect
        player.meleeEffect(false, 135f,offset);
      }
      else
      {
        player.ChangeAnimationState("Lea_Attack_0_1 Flip");
        Vector2 offset = new Vector2(-player.meleeRange, -player.meleeRange);
        player.meleeEffect(true, 135f,offset);
      }
    }
    else if (playerFacing.x == 0 && playerFacing.y == -1) //Facing Bottom Middle
    {
      player.rb.AddForce((-player.transform.up)*player.lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (player.attackIndex != 1)
      {
        player.ChangeAnimationState("Lea_Attack_0_2");
        Vector2 offset = new Vector2(0, -player.meleeRange); //Offset our Swing Effect
        player.meleeEffect(false, 180f,offset);
      }
      else
      {
        player.ChangeAnimationState("Lea_Attack_0_2 Flip");
        Vector2 offset = new Vector2(0, -player.meleeRange); //Offset our Swing Effect
        player.meleeEffect(true, 180f,offset);
      }
      
    }
    else if (playerFacing.x == 1 && playerFacing.y == -1) //Facing Bottom Right
    {
      player.rb.AddForce((-player.transform.up+player.transform.right)*player.lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (player.attackIndex != 1)
      {
        player.ChangeAnimationState("Lea_Attack_0_3");
        Vector2 offset = new Vector2(player.meleeRange*1.5f, -player.meleeRange*1.5f); //Offset our Swing Effect
        player.meleeEffect(false, 225f,offset);
      }
      else
      {
        player.ChangeAnimationState("Lea_Attack_0_3 Flip");
        Vector2 offset = new Vector2(player.meleeRange*1.5f, -player.meleeRange*1.5f); //Offset our Swing Effect
        player.meleeEffect(true, 225f,offset);
      }
    }
    else if (playerFacing.x == -1 && playerFacing.y == 0) //Facing Middle Left
    {
      player.rb.AddForce(-player.transform.right*player.lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (player.attackIndex != 1)
      {
        player.ChangeAnimationState("Lea_Attack_0_4");
        Vector2 offset = new Vector2(-player.meleeRange*2, 0); //Offset our Swing Effect
        player.meleeEffect(true, 90f,offset);
      }
      else
      {
        player.ChangeAnimationState("Lea_Attack_0_4 Flip");
        Vector2 offset = new Vector2(-player.meleeRange*2,0); //Offset our Swing Effect
        player.meleeEffect(false, 90f,offset);
      }
    }
    else if (playerFacing.x == 1 && playerFacing.y == 0) //Facing Middle Right
    {
      player.rb.AddForce(player.transform.right*player.lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (player.attackIndex != 1)
      {
        player.ChangeAnimationState("Lea_Attack_0_6");
        Vector2 offset = new Vector2(player.meleeRange*2, 0); //Offset our Swing Effect
        player.meleeEffect(false, 270f,offset);
      }
      else
      {
        player.ChangeAnimationState("Lea_Attack_0_6 Flip");
        Vector2 offset = new Vector2(player.meleeRange*2,0); //Offset our Swing Effect
        player.meleeEffect(true, 270f,offset);
      }
    }
    else if (playerFacing.x == -1 && playerFacing.y == 1) //Facing Top Left
    {
      player.rb.AddForce((player.transform.up-player.transform.right)*player.lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (player.attackIndex != 1)
      {
        player.ChangeAnimationState("Lea_Attack_0_7");
        Vector2 offset = new Vector2(-player.meleeRange*2, player.meleeRange*2); //Offset our Swing Effect
        player.meleeEffect(true, 45f,offset);
      }
      else
      {
        player.ChangeAnimationState("Lea_Attack_0_7 Flip");
        Vector2 offset = new Vector2(-player.meleeRange*2, player.meleeRange*2); //Offset our Swing Effect
        player.meleeEffect(false, 45f,offset);
      }
    }
    else if (playerFacing.x == 0 && playerFacing.y == 1) //Facing Top Middle
    {
      player.rb.AddForce(player.transform.up*player.lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (player.attackIndex != 1)
      {
        player.ChangeAnimationState("Lea_Attack_0_8");
        Vector2 offset = new Vector2(0, player.meleeRange*2); //Offset our Swing Effect
        player.meleeEffect(false, 0,offset);
      }
      else
      {
        player.ChangeAnimationState("Lea_Attack_0_8 Flip");
        Vector2 offset = new Vector2(0, player.meleeRange); //Offset our Swing Effect
        player.meleeEffect(true, 0f,offset);
      }
    }
    else if (playerFacing.x == 1 && playerFacing.y == 1) //Facing Top Right
    {
      player.rb.AddForce((player.transform.up+player.transform.right)*player.lungeSpeed,ForceMode2D.Impulse); //Lunge us in said direction
      if (player.attackIndex != 1)
      {
        player.ChangeAnimationState("Lea_Attack_0_9");
        Vector2 offset = new Vector2(player.meleeRange*2, player.meleeRange*2);
        player.meleeEffect(false, 315f,offset);
      }
      else
      {
        player.ChangeAnimationState("Lea_Attack_0_9 Flip");
        Vector2 offset = new Vector2(player.meleeRange*2, player.meleeRange*2);
        player.meleeEffect(true, 315f,offset);
      }
    } 
      }
    
  } 
}
    


