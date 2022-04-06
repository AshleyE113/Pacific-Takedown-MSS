using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using Unity.Mathematics;

public class EnemyCollision : MonoBehaviour
{
    public static void SpecifiedCollision(Collision2D other, GameObject enemy)
    {
        EnemyAI script = enemy.GetComponent<EnemyAI>();
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            //script.Knockback(script.recievedKnockback, script.direction, false, other.gameObject);
            if (script.state == EnemyAI.State.Bounce)
            {
                //script.ChangeState(EnemyAI.State.Bounce);
                CameraController.Shake(2f, 2f, 0.1f, 0.1f);
                //Sound
                AkSoundEngine.PostEvent("Play_MetalBounce" , enemy);
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Computer"))
        {
            if (script.state == EnemyAI.State.Bounce)
            {
                //script.ChangeState(EnemyAI.State.Bounce);
                script.BouncedOffWall(6); //Extra Knockback
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Bumper") && enemy.GetComponent<EnemyBounce>().isBouncing == true)
        {
            if (script.state == EnemyAI.State.Bounce)
            {
                //script.ChangeState(EnemyAI.State.Bounce);
                script.BouncedOffWall(3); //Bumper damage 
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") /*&& gameObject.GetComponent<EnemyBounce>().isBouncing == true*/) //Not sure if this works yet. WILL MAKE THIS A FUNCION!!!!
        {
            if (script.state == EnemyAI.State.Bounce)
            {
                //script.ChangeState(EnemyAI.State.Bounce);
                script.BouncedOffWall(3); // subject to change 
            }
        }
    }

    public static void specifiedTrigger(Collider2D other, GameObject enemy)
    {
        EnemyAI script = enemy.GetComponent<EnemyAI>();
        int direction = (int) other.gameObject.transform.localEulerAngles.z;
        if (other.gameObject.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
        {
            //Sound
            AkSoundEngine.PostEvent("Play_MetalContact" , enemy);

            script.canAttack = false;
            CameraController.Shake(10f, 10f, 0.1f, 0.1f);
            GameObject thisRotationObject = PlayerController.rotationObject.transform.GetChild(1).gameObject;
            PlayerController.rotationObject.transform.GetChild(0).gameObject.transform.localScale =
                new Vector3(3f, 3f, 3f);
            FXManager.spawnEffect("oil",enemy,thisRotationObject.transform,PlayerController.rotationObject.transform.rotation, false,new Vector2(0f,0f));
            FXManager.spawnEffect("spark",thisRotationObject,thisRotationObject.transform,other.gameObject.transform.rotation, false,new Vector2(0f,0f));

            script.ChangeAnimationState("Movement");
            FXManager.flashEffect(enemy);
            if (script.canBounce)
            {
                script.state = EnemyAI.State.Bounce;
                script.hitPause.Stop(script.HiPaVal);
                script.rb.velocity = Vector2.zero;
                script.Knockback(script.recievedKnockback, direction, true, other.gameObject);
                script.BouncedOffWall(20);
            }
            else
            {
                script.hitPause.Stop(script.HiPaVal);
                script.rb.velocity = Vector2.zero;
                script.Knockback(script.recievedKnockback, direction, false, other.gameObject);
                script.ChangeState(EnemyAI.State.Hit);
                script.Health -= 20;
                script.recoveryTimer = 0;
                //script.ChangeAnimationState("DroneIdle");
                script.FreezeAnimation();
            }
        }
    }
    

}
