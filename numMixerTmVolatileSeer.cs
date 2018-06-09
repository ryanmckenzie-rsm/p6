// AUTHOR: Ryan McKenzie
// FILENAME: p6.cs
// DATE: June 7, 2018
// REVISION HISTORY: v1.0.0
// PLATFORM: MSBuild 15.7.179.6572

// DESCRIPTION:
// * This class is a composite of a numMixer, tmSeer, and volatileSeer.
// * This class is initialized with a user-determined maximum number of calls
// before the class becomes permanently inactive.
// * ping() implements a numMixer's ping().
// * request() alternates between a tmSeer and volatileSeer's request().

// ASSUMPTIONS:
// * The object becomes permanently inactive after a user-determined amount of
// pings/requests.
// * The user determines the max number of calls before deactivation.
// * ping() implements numMixer's ping() method.
// * request() implements tmSeer and volatileSeer's request() method,
// alternating between the two depending on "_state".

// CLASS INVARIANT:
// * Inherits from numMixer and implements ISeer's interface.
// * Becomes inactive after a user-determined amount of calls.
// * Cannot be reactivated.

// INTERFACE INVARIANT:
// * "maxCalls" determines the maximum number of times the object can be called
// before being permanently deactivated.
// * The object begins active.
// * The name of the object can be retrieved using name().
// * request() alternates between echoing its delegates.

// IMPLEMENTATION INVARIANT:
// * "_maxCalls" is set by the user in the constructor.
// * isActive() and active() are overridden in terms of "_maxCalls".
// * "_state" alternates between the two seer delegates after every request.
// * Calling ping() while the object is inactive returns false.
// * Calling request while the object is inactive returns null.


using p1;
using p3;


namespace p6
{
	class numMixerTmVolatileSeer : numMixer, ISeer
	{
		// PUBLIC


		// Constructors

		public numMixerTmVolatileSeer(int maxCalls) : base(maxCalls)
		{
			initialize(maxCalls);
		}
		// DESCRIPTION:
		// * Initializes a composition of a numMixer, tmSeer, and volatileSeer,
		// allowing a maximum of "maxCalls", before the object transitions to a
		// permanently inactive state.
		// * Seeds the numMixer with its default dataset.
		// 
		// PRECONDITIONS:
		// * "maxCalls" should be greater than 0.
		// 
		// POSTCONDITIONS:
		// * numMixer base is intiialized with "maxCalls"
		// * See initialize().

		public numMixerTmVolatileSeer(int maxCalls, int[] dataset) : base(maxCalls, dataset)
		{
			initialize(maxCalls);
		}
		// DESCRIPTION:
		// * Initializes a composition of a numMixer, tmSeer, and volatileSeer,
		// allowing a maximum of "maxCalls", before the object transitions to a
		// permanently inactive state.
		// * Seeds the numMixer with its "dataset".
		// 
		// PRECONDITIONS:
		// * "maxCalls" should be greater than 0.
		// * "dataset" must be of size > 0.
		// * "dataset" must contain integers of a parity the user wishes to
		// request.
		// 
		// POSTCONDITIONS:
		// * numMixer base is intiialized with "maxCalls"
		// * See initialize().


		// Functionality

		public override bool ping(ref int[] returnValues)
		{
			if (!isActive()) {
				return false;
			} else {
				_maxCalls--;
				return base.ping(ref returnValues);
			}
		}
		// DESCRIPTION:
		// * Echoes the functionality of a numMixer's ping, if the object is active.
		// 
		// PRECONDITIONS:
		// * "returnValues" must be of size > 0.
		// * The user must have provided integers of the requested parity at
		// object creation.
		// 
		// POSTCONDITIONS:
		// * If the call succeeds, "_maxCall" is decremented.

		public void request(ref string fetchedMessage)
		{
			if (!isActive()) {
				fetchedMessage = null;
			} else {
				switch (_state) {
					case 1:
						_tmSeerDel.request(ref fetchedMessage);
						break;
					case 2:
						_volatileSeerDel.request(ref fetchedMessage);
						break;
					default:
						fetchedMessage = null;
						break;
				}
				_state++;
				if (_state > 2) {
					_state = 1;
				}
				_maxCalls--;
			}
		}
		// DESCRIPTION:
		// * Alternates between echoing the functionality of a tmSeer and a
		// volatileSeer.
		// * If the object is inactive, the message is set to null.
		// 
		// POSTCONDITIONS:
		// * "_state" increments on a successful call.
		// * _"maxCalls" decrements on a successful call.


		// Accessors

		public bool active()
		{
			return isActive();
		}
		// DESCRIPTION:
		// * Returns whether the object is active or not.
		// * Returns true if the object is active.
		// * Returns false if the object is inactive.

		public override bool isActive()
		{
			return (_maxCalls > 0);
		}
		// DESCRIPTION:
		// * Returns whether the object is active or not.
		// * Returns true if the object is active.
		// * Returns false if the object is inactive.

		public string name()
		{
			return _NAME;
		}
		// DESCRIPTION:
		// * Returns the name of the class.


		// PRIVATE


		// Utility

		private void initialize(int maxCalls)
		{
			_maxCalls = maxCalls;
			_state = 1;
			_tmSeerDel = new tmSeer(maxCalls);
			_volatileSeerDel = new volatileSeer(maxCalls);
		}
		// DESCRIPTION:
		// * Initializes all members.
		// 
		// PRECONDITIONS:
		// * "maxCalls" should be greater than 0.
		// 
		// POSTCONDITIONS:
		// * "_maxCall" is set to "maxCalls".
		// * "_state" is set to 1.
		// * "_tmSeerDel" is initialized with "maxCalls".
		// * "_volatileSeerDel" is intiialized with "maxCalls".


		// Members

		const string _NAME = "numMixerTmVolatileSeer";
		// The name of the class.

		int _maxCalls;
		// Determines how many times the object can be pinged before becoming
		// permanently inactive.

		int _state;
		// Determines which seer delegate will be called.

		tmSeer _tmSeerDel;
		// The tmSeer delegate.

		volatileSeer _volatileSeerDel;
		// The volatileSeer delegate.
	}
}
