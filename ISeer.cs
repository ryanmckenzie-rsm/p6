// AUTHOR: Ryan McKenzie
// FILENAME: ISeer.cs
// DATE: June 7, 2018
// REVISION HISTORY: v1.0.0
// PLATFORM: MSBuild 15.7.179.6572


namespace p6
{
	interface ISeer
	{
		void request(ref string fetchedMessage);

		bool active();

		string name();
	}
}
