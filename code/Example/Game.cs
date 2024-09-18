using Sandbox.Network;

namespace Pawns.Example;

public sealed class Game : Component, Component.INetworkListener
{
	[Property]
	public PrefabFile ClientPrefab { get; set; }

	[Property]
	public PrefabFile SpectatePrefab { get; set; }

	protected override void OnStart()
	{
		if ( !GameNetworkSystem.IsActive )
			GameNetworkSystem.CreateLobby();
	}

	public void OnActive( Connection channel )
	{
		var clientObj = SceneUtility.GetPrefabScene( ClientPrefab ).Clone();
		clientObj.NetworkSpawn( channel );

		var client = clientObj.Components.Get<Client>();
		client.AssignConnection( channel );
		client.AssignPawn<SpectatePawn>();

		// or assign it via the prefab file!
		// The pawn component does NOT need the PawnAttribute in this case.
		// client.AssignPawn<SpectatePawn>(SpectatePrefab);
	}
}
