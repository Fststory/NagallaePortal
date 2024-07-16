using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Y_CanUsePortal : MonoBehaviour
{
    // �� ��ũ��Ʈ�� ���� ���ӿ�����Ʈ�� ��Ż �� �̵��� �����մϴ�.

    #region ���� ���� (����)
    //public GameObject rPortal;
    //public GameObject bPortal;


    //void Start()
    //{

    //}

    //void Update()
    //{
    //    bPortal = GameObject.FindGameObjectWithTag("BluePortal");
    //    rPortal = GameObject.FindGameObjectWithTag("RedPortal");
    //}

    //private void OnTriggerEnter(Collider Portal)
    //{
    //    // ���� "RedPortal" �±׸� ���� GameObject�� ��Ҵٸ�
    //    if (Portal.gameObject.CompareTag("RedPortal"))
    //    {
    //        // ���� �������ִ� BluePortal�� �̵��ȴ�.
    //        transform.position = bPortal.transform.position;
    //    }
    //    //else if (Portal.gameObject.CompareTag("BluePortal"))
    //    //{
    //    //    transform.position = rPortal.transform.position;
    //    //}
    //}
    #endregion

    private GameObject cloneObject;

    private int inPortalCount = 0;

    private Y_Portal inPortal;
    private Y_Portal outPortal;

    private new Rigidbody rigidbody;
    protected new Collider collider;

    private static readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);     // y�� ���� 180�� ȸ���� ���� ���ʹϾ� ����(halfTurn)

    protected virtual void Awake()
    {
        cloneObject = new GameObject();                                     // Ŭ�� ������Ʈ ����
        cloneObject.SetActive(false);                                       // Ŭ�� ������Ʈ ��Ȱ��ȭ
        var meshFilter = cloneObject.AddComponent<MeshFilter>();            // Ŭ�� ������Ʈ�� MeshFilter ������Ʈ �߰�
        var meshRenderer = cloneObject.AddComponent<MeshRenderer>();        // Ŭ�� ������Ʈ�� MeshRenderer ������Ʈ �߰�

        meshFilter.mesh = GetComponent<MeshFilter>().mesh;                  // �׿������� MeshFilter�� mesh ĳ��
        meshRenderer.materials = GetComponent<MeshRenderer>().materials;    // �׿������� MeshRenderer�� materials ĳ��
        cloneObject.transform.localScale = transform.localScale;            // Ŭ�� ������Ʈ�� ũ��(localScale)�� �׿����� ��ü�� ũ��� ����.

        rigidbody = GetComponent<Rigidbody>();                              // �׿������� ������ٵ� ������Ʈ ĳ��
        collider = GetComponent<Collider>();                                // �׿������� �ݶ��̴� ������Ʈ ĳ��
    }

    private void LateUpdate()
    {
        if (inPortal == null || outPortal == null)                           // ���� inPortal�� outPortal �� �ϳ��� ����ִٸ�..
        {
            return;     // �ƹ� ���� ��ȯ���� �ʴ´�.
        }

        // ���� Ŭ�� ������Ʈ, inPortal, outPortal�� ��� Ȱ��ȭ ���ִٸ�..
        if (cloneObject.activeSelf && inPortal.IsPlaced && outPortal.IsPlaced)
        {
            var inTransform = inPortal.transform;           // inPortal�� ��ġ, ȸ�� ����
            var outTransform = outPortal.transform;         // outPortal�� ��ġ, ȸ�� ����

            // Update position of clone.(����)
            // ��ü�� ���� ��ǥ�� inPortal������ ���� ��ǥ�� ����� relativePos�� ��´�.
            Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
            // inPortal �������� y������ 180�� ȸ����Ų��.
            relativePos = halfTurn * relativePos;
            // Ŭ���� ��ġ�� outPortal ���ؿ��� relativePos ��ŭ�� ��ġ�� �д�.
            cloneObject.transform.position = outTransform.TransformPoint(relativePos);

            // Update rotation of clone.(����)
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
            relativeRot = halfTurn * relativeRot;
            cloneObject.transform.rotation = outTransform.rotation * relativeRot;
        }
        else
        {
            cloneObject.transform.position = new Vector3(-1000.0f, 1000.0f, -1000.0f);
        }
    }

    public void SetIsInPortal(Y_Portal inPortal, Y_Portal outPortal, Collider wallCollider)
    {
        this.inPortal = inPortal;           // �� ��Ż ����
        this.outPortal = outPortal;         // �ƿ� ��ſ ����

        Physics.IgnoreCollision(collider, wallCollider);    // �׿������� ��Ż�� ��ġ�� �� ������ �浹�� �����Ѵ�.(�� �ձ�)

        cloneObject.SetActive(false);

        ++inPortalCount;
    }

    public void ExitPortal(Collider wallCollider)
    {
        Physics.IgnoreCollision(collider, wallCollider, false);     // �׿������� ��Ż�� ��ġ�� �� ������ �浹 ���ø� �����Ѵ�.(�� �ձ� �Ұ�)
        --inPortalCount;

        if (inPortalCount == 0)
        {
            cloneObject.SetActive(false);
        }
    }
    public virtual void Warp()      // ��Ż �� �̵� �� �׿� ���� ����, �ӵ� ó���� ����ϴ� �Լ�, (�� �״�� ����)
    {
        var inTransform = inPortal.transform;                       // ����Ż�� Ʈ�������� ��� ����(inTransform)
        var outTransform = outPortal.transform;                     // �ƿ���Ż�� Ʈ�������� ��� ����(outTransform)

        // Quaternion halfTurn�� ������ ��Ż�� ���� ������ �� �� ���̴�.

        // Update position of object.(����)
        // �ݴ��� ��Ż�� ��ġ �̵�
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = halfTurn * relativePos;
        transform.position = outTransform.TransformPoint(relativePos);

        // Update rotation of object.(����)
        // �� ���� ����� ���� �������� ���´�
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;     // ��Ż�� ���� ������ ����
        relativeRot = halfTurn * relativeRot;           // ���� ����� �ݴ� ���� ��ȯ
        transform.rotation = outTransform.rotation * relativeRot;       // ��Ż���� ���� ���� ������ �����Ѵ�.

        // Update velocity of rigidbody.(����)
        // �� ���� �ӷ��� ���� ���� �״�� �����ȴ�
        Vector3 relativeVel = inTransform.InverseTransformDirection(rigidbody.velocity);
        relativeVel = halfTurn * relativeVel;
        rigidbody.velocity = outTransform.TransformDirection(relativeVel);

        // Swap portal references.(����)
        var tmp = inPortal;
        inPortal = outPortal;
        outPortal = tmp;
    }
}