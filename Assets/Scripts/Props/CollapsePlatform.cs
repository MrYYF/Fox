using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollapsePlatform : MonoBehaviour
{
    [Tooltip("��ʧʱ��")] public float fadeTime = 1f;
    [Tooltip("�ָ�ʱ��")] public float respawnTime = 1f;
    

    private Collider2D platformCollider;
    private SpriteRenderer spriteRenderer;
    private bool isCollapsing = false; //�Ƿ񴥷�
    [Tooltip("��˸����")] private int blinkCount = 5;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if the player is on top of the object
            if (collision.GetContact(0).normal.y < 0 && !isCollapsing)
            {
                // Start the collapse coroutine
                StartCoroutine(Collapse());
            }
        }
    }

    private IEnumerator Collapse()
    {
        Debug.Log("Collapse triggered");
        isCollapsing = true; // Set the flag to true

        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;

        // ʵ����˸Ч��
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            // ʣ��һ��ʱ��ʱ�ſ�ʼ��˸
            if (elapsedTime >= fadeTime/2)
            {
                float alpha = Mathf.Abs(Mathf.Sin(elapsedTime * Mathf.PI * blinkCount / fadeTime));
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }
            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        platformCollider.enabled = false; // Disable the collider

        yield return new WaitForSeconds(respawnTime);

        spriteRenderer.color = originalColor;
        platformCollider.enabled = true; // Re-enable the collider
        isCollapsing = false; // Reset the flag
    }
}
