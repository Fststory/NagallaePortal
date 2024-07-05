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

    #region 챗지피티 긁어오기: 페이드인 페이드아웃
    //public GameObject panel; // 패널 오브젝트
    //public float fadeDuration = 1f; // 페이드 지속 시간
    //private Image panelImage;
    #endregion

    void Start()
    {
        #region 챗지피티 긁어오기: 페이드인 페이드아웃
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
                grabObj.transform.SetParent(null); //부모 컴포넌트 해제
                Rigidbody rb = grabObj.GetComponent<Rigidbody>();
                rb.useGravity = true;

                //gameObject.AddComponent<Rigidbody>();

            }
        }
    }

    #region 챗지피티 긁어오기: 페이드인 페이드아웃
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Bullet") // 플레이어와 충돌했을 때만 실행
    //    {
    //        print("충돌!");
    //        StartCoroutine(FadeInAndOut());
    //    }
    //}

    //IEnumerator FadeInAndOut()
    //{
    //    // 페이드 인
    //    yield return StartCoroutine(Fade(0f, 1f));

    //    // 잠시 대기
    //    yield return new WaitForSeconds(0.5f);

    //    // 페이드 아웃
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
                grabObj = (GameObject)other.gameObject; //게임오브젝트 변수에 추가 (왜안되냐?)

                grabObj.transform.position = PlayerGrabPoint.position;
                grabObj.transform.SetParent(gameObject.transform); //부모 컴포넌트 지정

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
