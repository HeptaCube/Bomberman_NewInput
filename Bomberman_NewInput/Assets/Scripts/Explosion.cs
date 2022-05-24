using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimatedSpriteRenderer start;
    public AnimatedSpriteRenderer middle;
    public AnimatedSpriteRenderer end;

    public void SetActiveRenderer(AnimatedSpriteRenderer renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    public void SetDirection(Vector2 direction)
    {
        // 각도를 "Mathf.Atan2(float.y, float.x)"로 계산할 수 있다.
        // "Mathf.Atan2"의 반환값이 라디안이기 때문에 도수법을 사용하기 위해 "Mathf.Rad2Deg"을 이용한다.
        // 참고 자료 : https://blog.naver.com/sang9151/220821255191

        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }
}
