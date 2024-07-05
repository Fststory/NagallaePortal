using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_GrabMe : MonoBehaviour
{
    public GameObject grabObj;
    public Transform PlayerGrabPoint;

    public bool detec;

    public bool imGrapping;

    #region ê����Ƽ �ܾ����: ���̵��� ���̵�ƿ�
    //public GameObject panel; // �г� ������Ʈ
    //public float fadeDuration = 1f; // ���̵� ���� �ð�
    //private Image panelImage;
    #endregion

    void Start()
    {
        #region ê����Ƽ �ܾ����: ���̵��� ���̵�ƿ�
        //if (panel != null)
        //{
        //    panelImage = panel.GetComponent<Image>();
        //}
        #endregion
    }

    void Update()
    {
        

        if (imGrapping)
        {
            if (Input.GetMouseButtonDown(1))
            {
                imGrapping = false;
                grabObj.transform.SetParent(null); //�θ� ������Ʈ ����
                Rigidbody rb = grabObj.GetComponent<Rigidbody>();
                rb.useGravity = true;

                //gameObject.AddComponent<Rigidbody>();

            }
        }
    }

    #region ê����Ƽ �ܾ����: ���̵��� ���̵�ƿ�
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Bullet") // �÷��̾�� �浹���� ���� ����
    //    {
    //        print("�浹!");
    //        StartCoroutine(FadeInAndOut());
    //    }
    //}

    //IEnumerator FadeInAndOut()
    //{
    //    // ���̵� ��
    //    yield return StartCoroutine(Fade(0f, 1f));

    //    // ��� ���
    //    yield return new WaitForSeconds(0.5f);

    //    // ���̵� �ƿ�
    //    yield return StartCoroutine(Fade(1f, 0f));
    //}
    //IEnumerator Fade(float startAlpha, float endAlpha)
    //{
    //    float elapsedTime = 0f;

    //    while (elapsedTime < fadeDuration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
    //        panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, alpha);
    //        yield return null;
    //    }

    //    panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, endAlpha);
    //}
    #endregion

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "LabObject")
        {
            detec = true;

            if (Input.GetMouseButtonDown(0))
            {
                grabObj = (GameObject)other.gameObject; //���ӿ�����Ʈ ������ �߰� (�־ȵǳ�?)

                grabObj.transform.position = PlayerGrabPoint.position;
                grabObj.transform.SetParent(gameObject.transform); //�θ� ������Ʈ ����

                Rigidbody rb = grabObj.GetComponent<Rigidbody>();
                rb.useGravity = false;

                //Destroy(rb);

                imGrapping = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LabObject")
        {
            detec=false;
        }
    }
}
