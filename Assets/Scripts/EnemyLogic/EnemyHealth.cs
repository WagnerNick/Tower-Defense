using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private BalloonTypeSO initialType;
    [SerializeField] private MeshRenderer balloonRenderer;

    private BalloonTypeSO currentType;
    private int currentHealth;
    private MaterialPropertyBlock propertyBlock;

    private Enemy enemy;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        ApplyType(initialType);
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= Mathf.RoundToInt(damageAmount);

        if (currentHealth <= 0)
        {
            Pop();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + Mathf.RoundToInt(healAmount), currentType.health);
    }

    void Pop()
    {
        if (currentType.popsInto != null)
        {
            ApplyType(currentType.popsInto);
        }
        else
        {
            EnemyPool.Instance.ReturnToPool(enemy);
        }
    }

    void ApplyType(BalloonTypeSO type)
    {
        currentType = type;
        currentHealth = type.health;

        enemy.speed = type.speed;
        enemy.damage = type.damage;

        if (balloonRenderer != null)
        {
            balloonRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_Color", type.color);
            balloonRenderer.SetPropertyBlock(propertyBlock);

            balloonRenderer.transform.localScale = Vector3.one * type.scale;
        }
    }
}
