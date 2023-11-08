public abstract class MovementState
{
    public MovementState()
    {
        
    }

    public virtual void OnStart()
    {

    }
    public virtual void OnUpdate()
    {

    }
    public virtual void OnExit()
    {

    }

    protected abstract void GatherXInput();
    protected abstract void GatherYInput();
}
