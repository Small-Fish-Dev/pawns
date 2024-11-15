namespace Pawns.Example;

public class Client : Pawns.Client
{
	public static Client Local { get; private set; }

	protected override void OnStart()
	{
		if ( !IsProxy )
			Local = this;
	}
}
