using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // 아이템 종류 구분을 위한 enum형의 배열.
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,
    }

    // 아이템 타입 고르는 항목을 에디터에 추가함.
    public ItemType type;

    // 아이템에 기능을 할당함.
    private void OnItemPickup(GameObject player)
    {
        switch (type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;

            case ItemType.BlastRadius:
                player.GetComponent<BombController>().explosionRadius++;
                break;

            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().speed++;
                break;
        }

        // 아이템이 사용되면 아이템을 지움.
        Destroy(gameObject);
    }

    // Player 태그를 가진 다른 오브젝트 Trigger에 들어갔을 때, 할당된 기능을 사용함.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject);
        }
    }
}
