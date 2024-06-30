using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    // ��ȣ�ۿ��� ����ϴ� Ŭ���� �Դϴ�.

    // Interact ��ġ ����
    public Transform interactPosition;
    public float interactRange;

    void Start()
    {
        
    }

    void Update()
    {
        // ���� [E]�� �����ٸ�
        if (Input.GetKeyDown(KeyCode.E))
        {
            // ������ �ʴ� ���� ������.
            Ray r = new Ray(interactPosition.position, interactPosition.forward);

            // ���� ���� ��� �Ÿ��� ������ �ִٸ�..
            if(Physics.Raycast(r,out RaycastHit hitInfo, interactRange))
            {
                // ���� �տ� ���� ���� ��ȣ�ۿ� ������ �����̶��...
                if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interObj))
                {
                    interObj.Interact();
                }
            }
        }
    }
}
