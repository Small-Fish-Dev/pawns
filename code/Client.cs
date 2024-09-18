namespace Pawns;

public partial class Client : Component
{
	public static Client Local { get; private set; }

	[HostSync, Sync]
	public Connection Connection { get; private set; }

	[HostSync, Sync]
	public Pawn Pawn { get; private set; }

	/// <summary>
	/// Assigns the given connection as the client's current connection.
	/// </summary>
	/// <param name="connection">The connection to assign to the client.</param>
	public void AssignConnection( Connection connection )
	{
		Connection = connection;
		GameObject.Name = $"{Connection.Name} / {Connection.Id}";

		if ( connection == Connection.Local )
			Local = this;
	}

	/// <summary>
	/// Assigns the given pawn type as the client's current pawn.
	/// </summary>
	/// <param name="ownerTransferMode">The network owner transfer mode to apply to the new pawn.</param>
	/// <param name="networkOrphanMode">The network orphan mode to apply to the new pawn.</param>
	/// <returns>The pawn component that the client was assigned to.</returns>
	public T AssignPawn<T>( OwnerTransfer ownerTransferMode = OwnerTransfer.Fixed, NetworkOrphaned networkOrphanMode = NetworkOrphaned.Destroy ) where T : Pawn
	{
		var pawnAttribute = TypeLibrary.GetType<T>().GetAttribute<PawnAttribute>();

		var path = pawnAttribute?.PrefabPath;
		if ( string.IsNullOrEmpty( path ) )
		{
			Log.Warning( $"{typeof( T )} had no PawnAttribute prefab assigned to it." );
			return null;
		}

		var obj = SceneUtility.GetPrefabScene( ResourceLibrary.Get<PrefabFile>( path ) ).Clone();

		var pawn = obj.Components.Get<Pawn>();
		if ( pawn is null )
		{
			obj.Destroy();
			Log.Warning( $"Assigned GameObject ({obj.Name}) with no pawn component!" );
			return null;
		}

		if ( Pawn.IsValid() )
			Pawn.OnUnassign();

		obj.NetworkSpawn( Connection );
		obj.Network.SetOwnerTransfer( ownerTransferMode );
		obj.Network.SetOrphanedMode( networkOrphanMode );
		obj.Name = $"{Connection.DisplayName} - {obj.Name.ToUpper()} Pawn";

		Pawn = pawn;
		Pawn.OnAssign( this );

		return (T)pawn;
	}
}
