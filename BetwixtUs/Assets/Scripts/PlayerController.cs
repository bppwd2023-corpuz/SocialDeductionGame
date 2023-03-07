using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{   
    public float speed;
    private Vector2 move, mouselook;
    private Vector3 rotationTarget;
    private bool aiming;
    public void OnMove(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context){
        mouselook = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        aiming = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1)){
            aiming = true;
        }
        if(Input.GetMouseButtonUp(1)){
            aiming = false;
        }

        if(aiming){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouselook);

            if(Physics.Raycast(ray, out hit)){
                rotationTarget = hit.point;
            }

            movePlayerWithAim();
        }
        else{
            movePlayer();
        }
        
    }

    public void movePlayer(){
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if(movement != Vector3.zero){
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
        
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    public void movePlayerWithAim(){
        var lookPos = rotationTarget - transform.position;
        lookPos.y=0;
        var rotation = Quaternion.LookRotation(lookPos);
        Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);
        if(aimDirection != Vector3.zero){
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        Vector3 movement = new Vector3(move.x, 0f, move.y);
        transform.Translate(movement * speed * Time.deltaTime, Space.World); 
    }
}
