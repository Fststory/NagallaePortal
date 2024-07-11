using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_TestQuaXVec3 : MonoBehaviour
{
    // ���ʹϾȰ� ����3�� ���� ������ �ľ��ϱ� ���� ��ũ��Ʈ

    public Vector3 initialVector = new Vector3(1, 0, 0); // �ʱ� ����
    public Vector3 rotationAxis = new Vector3(0, 1, 0); // ȸ�� ��
    public float rotationAngle = 45; // ȸ�� ����

    private Vector3 rotatedVector; // ȸ���� ����

    void Start()
    {
        // �ʱ� ���͸� ť���� ��ġ�� ����
        transform.position = initialVector;
    }

    void Update()
    {
        // ����ڰ� 'R' Ű�� ������ ȸ�� ���
        if (Input.GetKeyDown(KeyCode.R))
        {
            // ȸ�� ��� ������ ���ʹϾ� ����
            Quaternion rotation = Quaternion.AngleAxis(rotationAngle, rotationAxis);

            // ���ʹϾ�� ������ �������� ���� ȸ��
            rotatedVector = rotation * initialVector;

            // ȸ���� ���͸� ť���� ���ο� ��ġ�� ����
            transform.position = rotatedVector;

            Debug.Log("Rotated Vector: " + rotatedVector);
        }
    }

    void OnDrawGizmos()
    {
        // �ʱ� ���Ϳ� ȸ���� ���͸� �ð������� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, initialVector);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, rotatedVector);
    }
}
