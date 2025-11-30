public interface IMovable
{
    void Move();
}

public interface IAttackable
{
    void Attack();
}

public interface IDamageable
{
    void TakeDamage(float amount);
}