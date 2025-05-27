using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerMovement mov;
    public Animator anim;
    private SpriteRenderer spriteRend;

    [Header("Movement Tilt")]
    //[SerializeField] [Range(0, 1)] private float tiltSpeed;

    [Header("Particle FX")]
    [SerializeField] private GameObject jumpFX;
    [SerializeField] private GameObject landFX;
    private ParticleSystem _jumpParticle;
    private ParticleSystem _landParticle;
    public bool startedJumping {  private get; set; }
    public bool justLanded { private get; set; }
    public bool StartDashing { private get; set; }
    public bool startedWallJump { private get; set; }
    public bool InteractE { private get; set; }


    public float currentVelY;

    private void Start()
    {
        mov = GetComponent<PlayerMovement>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        //anim = spriteRend.GetComponent<Animator>();

        

        _jumpParticle = jumpFX.GetComponent<ParticleSystem>();
        _landParticle = landFX.GetComponent<ParticleSystem>();
    }

    private void LateUpdate()
    {
        #region Tilt
        float tiltProgress;

        int mult = -1;

        if (mov.IsSliding)
        {
            tiltProgress = 0.25f;
        }
        else
        {
            tiltProgress = Mathf.InverseLerp(-mov.Data.runMaxSpeed, mov.Data.runMaxSpeed, mov.RB.velocity.x);
            mult = (mov.IsFacingRight) ? 1 : -1;
        }

        //float newRot = ((tiltProgress * maxTilt * 2) - maxTilt);
        //float rot = Mathf.LerpAngle(spriteRend.transform.localRotation.eulerAngles.z * mult, newRot, tiltSpeed);
        //spriteRend.transform.localRotation = Quaternion.Euler(0, 0, rot * mult);
        #endregion

        CheckAnimationState();
    }

    private void CheckAnimationState()
    {
        if (Mathf.Abs(mov.RB.velocity.x) > 0.05f)
        {
            //Debug.Log("Isrunning");
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }
        if (startedJumping)
        {
            anim.SetTrigger("Jump");
            GameObject obj = Instantiate(jumpFX, transform.position - (Vector3.up * transform.localScale.y / 2), Quaternion.Euler(-90, 0, 0));
            Destroy(obj, 1);
            startedJumping = false;
            return;
        }

        if (justLanded)
        {
            anim.SetTrigger("Land");
            GameObject obj = Instantiate(landFX, transform.position - (Vector3.up * transform.localScale.y / 1.5f), Quaternion.Euler(-90, 0, 0));
            Destroy(obj, 1);
            justLanded = false;
            return;
        }

        if (StartDashing)
        {
            //MovementOff();

            anim.SetBool("IsRunning", false);
            anim.SetTrigger("Dash");
            AudioManager.Instance.PlaySFX("Dash");
            GameObject obj = Instantiate(jumpFX, transform.position - (Vector3.up * transform.localScale.y / 2), Quaternion.Euler(-90, 0, 0));
            Destroy(obj, 1);
            StartDashing = false;
            return;
        }
        if (startedWallJump)
        {
            anim.SetTrigger("Jump");
            startedWallJump = false;
            return;
        }

        if (InteractE)
        {
           //Debug.Log("InteracPlayer");
            anim.SetTrigger("InteractPlayer");
            InteractE = false;
            return;

        }

        anim.SetBool("isSliding", mov.IsSliding);


        anim.SetFloat("Vel Y", mov.RB.velocity.y);
    }

    public IEnumerator MovementOff()
    {
        mov.enabled = false;
        yield return new WaitForSeconds(10f);
        MovementOn();

    }

    public void MovementOn()
    {
        mov.enabled = true;

    }

}
