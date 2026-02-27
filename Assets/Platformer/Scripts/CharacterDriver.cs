using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDriver : MonoBehaviour
{
public float walkSpeed =5f;
public float runSpeed =10f;
public float groundAcceleration =15f;

public float apexHeight = 4.5f;
public float apexTime = 0.5f; 
Quaternion facingRight;
Quaternion facingLeft;

Animator _animator;



Vector2 _velocity;
CharacterController _controller;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        facingLeft = Quaternion.Euler(0f,-90f,0f);
        facingRight = Quaternion.Euler(0f,90f,0f);

        _animator = GetComponent<Animator>();

        _controller = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        float gravityModifier = 1f;


        float direction = 0f;
        if(Keyboard.current.dKey.isPressed) direction += 1f;
        if(Keyboard.current.aKey.isPressed) direction -= 1f;
        bool jumpPressedThisFrame = Keyboard.current.spaceKey.wasPressedThisFrame;
        bool jumpHeld = Keyboard.current.spaceKey.isPressed;


if(_controller.isGrounded){

        if ( direction != 0f){

            if(Mathf.Sign(direction)!= Mathf.Sign(_velocity.x)){
                _velocity.x =0f;
            } 
            _velocity.x+= direction*groundAcceleration*Time.deltaTime;
            _velocity.x = Mathf.Clamp(_velocity.x,-walkSpeed,walkSpeed);


            transform.rotation = (direction>0f)? facingRight: facingLeft;
        }else{
            _velocity.x *= 1f - Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x,0f,groundAcceleration*Time.deltaTime);
        }
        if(jumpPressedThisFrame){
                _velocity.y = 2f*apexHeight/apexTime;
        }



}   
        else{
            if(!jumpHeld){
                gravityModifier = 2f;
            }
        }

        float gravity = 2f* apexHeight/(apexTime*apexTime); 
        _velocity.y-=gravity*gravityModifier*Time.deltaTime;

        float deltaX = _velocity.x *Time.deltaTime;
        float deltaY = _velocity.y *Time.deltaTime;

        Vector3 deltaPosition = new(deltaX,deltaY,0f);
        CollisionFlags collisions = _controller.Move(deltaPosition);
        if((collisions & CollisionFlags.CollidedAbove) != 0){
            _velocity.y =-1f;
        }
        if((collisions & CollisionFlags.CollidedSides) != 0){
            _velocity.x =0f;
        }

        // Debug.Log($"grounded: {_controller.isGrounded}");
        _animator.SetFloat("Speed",_velocity.x);
        
    }
}
