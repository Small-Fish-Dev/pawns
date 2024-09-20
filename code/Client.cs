namespace Pawns;

public partial class Client : Component
{
	public static Client Local { get; private set; }

	[HostSync, Sync]
	public System.Guid ConnectionId { get; private set; }

	[HostSync, Sync]
	public Pawn Pawn { get; private set; }

	public Connection Connection => Connection.Find( ConnectionId );

	/// <summary>
	/// Assigns the given connection as the client's current connection.
	/// This connection will be used to give ownership to any future assigned pawns.
	/// </summary>
	/// <param name="connection">The connection to assign to the client.</param>
	public void AssignConnection( Connection connection )
	{
		ConnectionId = connection.Id;
		GameObject.Name = $"{Connection.DisplayName} - CLIENT";

		if ( connection == Connection.Local )
			Local = this;
	}

	/// <summary>
	/// Creates a <see cref="GameObject" /> from the given prefab file and assigns it as the client's current pawn.
	/// </summary>
	/// <param name="prefabFile">The prefab file that is used to create the pawn.</param>
	/// <returns>The pawn component that the client was assigned to.</returns>
	public T AssignPawn<T>( PrefabFile prefabFile ) where T : Pawn
	{
		var obj = SceneUtility.GetPrefabScene( prefabFile ).Clone();
		return InternalAssign<T>( obj );
	}

	/// <summary>
	/// Assigns the given pawn type as the client's current pawn.
	/// The pawn type must have a <see cref="PawnAttribute" /> defined in order to use this method.
	/// </summary>
	/// <returns>The pawn component that the client was assigned to.</returns>
	public T AssignPawn<T>() where T : Pawn
	{
		var pawnAttribute = TypeLibrary.GetType<T>().GetAttribute<PawnAttribute>();

		var path = pawnAttribute?.PrefabPath;
		if ( string.IsNullOrEmpty( path ) )
		{
			Log.Warning( $"{typeof( T )} had no PawnAttribute prefab assigned to it." );
			return null;
		}

		var obj = SceneUtility.GetPrefabScene( ResourceLibrary.Get<PrefabFile>( path ) ).Clone();
		return InternalAssign<T>( obj ); ;
	}

	private T InternalAssign<T>( GameObject obj ) where T : Pawn
	{
		var pawn = obj.Components.Get<Pawn>();
		if ( !pawn.IsValid() )
		{
			obj.Destroy();
			Log.Warning( $"Assigned GameObject ({obj.Name}) with no pawn component!" );
			return null;
		}

		if ( Pawn.IsValid() )
			Pawn.OnUnassign();

		if ( Connection is null )
			Log.Warning( $"Client does not have a connection! Assigning to host." );

		var assignedConnection = Connection ?? Connection.Host;
		obj.NetworkSpawn( assignedConnection );

		obj.Name = $"{assignedConnection.DisplayName} - {obj.Name.ToUpper()} Pawn";

		Log.Info( $"Setting pawn variable in realm {Client.Local}" );

		Pawn = pawn;
		Pawn.OnAssign( this );

		return (T)pawn;
	}
}
