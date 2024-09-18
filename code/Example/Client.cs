namespace Pawns;

public partial class Client
{
	// You can extend the client class and add whatever you need to it!

	protected override void OnUpdate()
	{
		if ( IsProxy )
		{
			Log.Info( Connection?.DisplayName );
		}
	}
}
