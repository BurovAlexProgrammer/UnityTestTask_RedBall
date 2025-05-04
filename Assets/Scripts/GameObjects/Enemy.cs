using Services;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [Inject] private PlayerController _playerController;
    [Inject] private GameAudioService _audioService;
    
    [SerializeField] private float topKillTolerance = 0.5f; // допуск для определения "удара сверху"
    
    private bool IsPlayer(GameObject obj) => obj.CompareTag("Player");
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsPlayer(collision.gameObject)) return;
        
        var contact = collision.GetContact(0);
        var contactNormal = contact.normal;
        var hitFromAbove = contactNormal.y < -topKillTolerance;

        if (hitFromAbove)
        {
            KillEnemy();
            BouncePlayer(collision.gameObject);
        }
        else
        {
            _playerController.TakeDamage(collision.GetContact(0).point, 1);
        }
    }


    private void KillEnemy()
    {
        Destroy(gameObject);
    }

    private void BouncePlayer(GameObject player)
    {
        _audioService.PlaySfx("click");
        var rb = player.GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, 10f);
        }
    }
}