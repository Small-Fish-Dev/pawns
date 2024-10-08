namespace Pawns;

public abstract class Pawn : Component
{
	[HostSync, Sync]
	public Client Owner { get; private set; }

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
