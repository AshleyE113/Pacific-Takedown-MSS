using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

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
            script.canAttack = false;
            CameraController.Shake(10f, 10f, 0.1f, 0.1f);
            script.ChangeAnimationState("Movement");
            FXManager.flashEffect(enemy);
            if (script.canBounce)
            {
                script.state = EnemyAI.State.Bounce;
                script.hitPause.Stop(script.HiPaVal);
                script.rb.velocity = Vector2.zero;
                script.Knockback(script.recievedKnockback, direction, true, other.gameObject);
                script.BouncedOffWall(1);
            }
            else
            {
                script.rb.velocity = Vector2.zero;
                script.Knockback(script.recievedKnockback, direction, false, other.gameObject);
                script.ChangeState(EnemyAI.State.Hit);
                script.Health -= 1;
                script.recoveryTimer = 0;
                script.ChangeAnimationState("DroneIdle");
            }
        }
    }
    

}
