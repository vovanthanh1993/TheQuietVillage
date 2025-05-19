public abstract class EnemyState
{
    protected EnemyController enemy;

    protected EnemyState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
