using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_Lab16UI : MonoBehaviour
{
    public Image hitImage;
    public float time = 0.5f;
    

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public IEnumerator FadeOut() //이 코루틴으로 알파값 변경
    {
        print("페이드 아웃 실행 중!");

        Color initialColor = hitImage.color;
        initialColor.a = 230f / 255f; // Alpha 값을 230으로 설정 (0~255의 범위를 0~1의 범위로 변환)
        hitImage.color = initialColor;

        
        float startAlpha = hitImage.color.a; //스타트 알파값은 방금 집어넣은 230f

        float endTime = 0f;

        while (endTime < time) //0.5초에서 0초가 될때까지
        {
            endTime += Time.deltaTime; //프레임단위로 깎아주면서

            float newAlpha = Mathf.Lerp(startAlpha, 0f, endTime / time); //러프로 깎아서 실시간 알파값 생성

            Color newColor = hitImage.color; //새로운 칼라 생성
            newColor.a = newAlpha; //알파값을 집어넣고...
            hitImage.color = newColor; //적용

            yield return null; //GPT: 코루틴을 일시정지하고 한 프레임 대기. 부드러운 애니메이션을 위해 필요하다.
        }

        // 마지막으로 완전히 투명하게 설정
        Color finalColor = hitImage.color;
        finalColor.a = 0f;
        hitImage.color = finalColor; //알파값이 0인 마지막 컬러를 주며 끝내기
    }

}
