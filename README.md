# Pawns

> _"no we won't allow pawn games"_ -garry

s&box library that provides support for the classic entity Pawn system. This is useful in cases where the player can take control of different controllers. For example, an FPS game where you have the regular play pawn but also a spectating pawn.

## Example

```csharp
// Inherit the pawn component.
//
// Make sure to provide the PawnAttribute with a path to a prefab
// if you decide to use the template version of AssignPawn.
[Pawn( "spectate.prefab" )]
public sealed class SpectatePawn : Pawn
{

}
```

```csharp
// Setup and assign the client a connection.
var client = clientObj.Components.Get<Client>();
client.AssignConnection( channel );

// Give the client a pawn.

// Option #1
// The pawn component MUST have PawnAttribute.
client.AssignPawn<SpectatePawn>();

// Option #2
// The pawn component does NOT need PawnAttribute in this case.
client.AssignPawn(SpectatePrefab);
```

[A full example can be viewed here.](https://github.com/Small-Fish-Dev/pawns/tree/main/code/Example)
