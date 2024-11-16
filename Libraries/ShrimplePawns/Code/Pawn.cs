namespace ShrimplePawns;

public abstract class Pawn : Component
{
	[HostSync]
	protected Client Owner { get; set; }

	public bool TryGetOwner<T>( out T owner ) where T : Client
	{
		owner = GetOwner<T>();
		return owner.IsValid();
	}

	public T GetOwner<T>() where T : Client
	{
		return Owner as T;
	}

	public virtual void OnAssign( Client client )
	{
		Owner = client;
	}

	public virtual void OnUnassign()
	{
		if ( GameObject.IsValid() )
			GameObject.Destroy();
	}
}
