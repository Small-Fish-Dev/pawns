namespace Pawns.Example;

[Pawn( "spectate.prefab" )]
public sealed class SpectatePawn : Pawn
{
	[RequireComponent]
	public CameraComponent CameraComponent { get; set; }

	private Angles EyeAngles { get; set; }

	protected override void OnAwake()
	{
		CameraComponent.Enabled = !IsProxy;
	}

	protected override void OnUpdate()
	{
		EyeAngles += Input.AnalogLook;
		EyeAngles = EyeAngles.WithPitch( EyeAngles.pitch.Clamp( -90, 90 ) );
		Transform.Rotation = EyeAngles.ToRotation();
		Transform.Position += Input.AnalogMove * Transform.Rotation;
	}
}
