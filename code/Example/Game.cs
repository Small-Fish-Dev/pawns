using Sandbox.Network;

namespace Pawns.Example;

public sealed class Game : Component, Component.INetworkListener
{
	[Property]
	public GameObject ClientPrefab { get; set; }

	protected override void OnStart()
	{
		if ( !GameNetworkSystem.IsActive )
			GameNetworkSystem.CreateLobby();
	}

	public void OnActive( Connection channel )
	{
		var clientObj = ClientPrefab.Clone();
		clientObj.NetworkSpawn( channel );

		var client = clientObj.Components.Get<Client>();
		client.AssignConnection( channel );
		client.AssignPawn<SpectatePawn>();
	}
}
