using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    // 상호작용을 담당하는 클래스 입니다.

    // Interact 위치 설정
    public Transform interactPosition;
    public float interactRange;

    void Start()
    {
        
    }

    void Update()
    {
        // 만약 [E]를 누른다면
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 보이지 않는 손을 만들어낸다.
            Ray r = new Ray(interactPosition.position, interactPosition.forward);

            // 만약 손이 닿는 거리에 뭔가가 있다면..
            if(Physics.Raycast(r,out RaycastHit hitInfo, interactRange))
            {
                // 만약 손에 닿은 것이 상호작용 가능한 물건이라면...
                if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interObj))
                {
                    interObj.Interact();
                }
            }
        }
    }
}
