using System.Threading.Tasks;
using Sandbox.Network;

namespace Pawns.Example;

public sealed class Game : Component, Component.INetworkListener
{
	[Property]
	public PrefabFile ClientPrefab { get; set; }

	[Property]
	public PrefabFile SpectatePrefab { get; set; }

	protected override async Task OnLoad()
	{
		if ( !Sandbox.Game.IsPlaying )
			return;

		if ( !GameNetworkSystem.IsActive )
			GameNetworkSystem.CreateLobby();

		var clientObj = SceneUtility.GetPrefabScene( ClientPrefab ).Clone();
		clientObj.NetworkSpawn( Connection.Local );

		var client = clientObj.Components.Get<Client>();
		client.AssignConnection( Connection.Local );

		// Option #1
		// The pawn component MUST have PawnAttribute.
		client.AssignPawn<SpectatePawn>();

		// Option #2
		// The pawn component does NOT need PawnAttribute in this case.
		// client.AssignPawn<SpectatePawn>(SpectatePrefab);

		await GameTask.CompletedTask;
	}
}
