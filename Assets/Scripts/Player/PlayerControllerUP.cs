using UnityEngine;

public class PlayerControllerUP : MonoBehaviour {

    public float moveSpeed;
    public float jumpHeight;
    public float groundCheckRadius;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public Vector3 Movimiento;
    
    private bool grounded;
    private bool canJump;
    private bool canDoubleJump;
    private Animator _animator;

    private void Start()
    {
        // Guardamos la referencia al Animator para no tener que buscarla en cada colisión.
        _animator = GetComponentInParent<PlayerController>().GetComponent<Animator>();
    }

    // Salto solo cuando pisa el suelo
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Water") {
            _animator.SetBool("jumping", false);
            canJump = true;
            canDoubleJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Water") {
            if (_animator != null) {
                _animator.SetBool("jumping", true);
            }
        }
    }

    public bool getJump() {
        if(canJump) 
            return true;
        
        return false;
    }

    public bool getDoubleJump() {
        if (canDoubleJump)
            return true;

        return false;
    }

    public void setJump(bool jump) {
        this.canJump = jump;
    }
    public void setDoubleJump(bool jump) {
        this.canDoubleJump = jump;
    }

}