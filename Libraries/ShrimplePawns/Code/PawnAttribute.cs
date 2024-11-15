using System;

namespace ShrimplePawns;

/// <summary>
/// This attribute should be given to any class that derives Pawn.cs
/// </summary>
[AttributeUsage( AttributeTargets.Class )]
public class PawnAttribute : Attribute
{
	public string PrefabPath { get; private set; }

	/// <param name="prefabPath">The path to the prefab that contains the derived pawn component.</param>
	public PawnAttribute( string prefabPath )
	{
		PrefabPath = prefabPath;
	}
}
