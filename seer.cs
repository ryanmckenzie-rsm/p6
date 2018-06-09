// AUTHOR: Ryan McKenzie
// FILENAME: seer.cs
// DATE: June 7, 2018
// REVISION HISTORY: v2.0.0
// PLATFORM: MSBuild 15.7.179.6572

// CLASS INVARIANT:
// * Failed requests will return empty strings.
// * A seer flips state every _q requests.
// * The seer begins active.
// * _q is determined by the user.

// INTERFACE INVARIANT:
// * The hidden message cannot be changed.
// * The name of the object can be retrieved using name().
// * The seer begins active.
// * The 1-arg constructor determines the max # of times the seer can be
// requested before changing state.
// * A failed request will return empty.

// IMPLEMENTATION INVARIANT:
// * The seer begins active.
// * The user determines _q.
// * The hidden message is predetermined in the constructor.
// * A seer flips state everytimes _stateChangeCount matches _q.
// * State change is evaluated AFTER the message is retrieved (so an active
// seer will return a message even if it flips to inactive during the call).
// * Invalid requests return empty strings.

// DESCRIPTION:
// * The seer returns its "secret" message via a public request method.
// * The returned message depends on the seer's active state.

// ASSUMPTIONS:
// * The message is predefined.
// * _q is defined by the client.
// * The user makes a request, passing a string.
// * If the seer is active, the message will be stored in the passed string.
// * If the seer is inactive, the passed string will be emptied.
// * The user must ping the seer object to determine if it is active.
// * The user can request the object's name (useful for templating).
// * A seer begins active.


using p6;


namespace p3
{
	public class seer : ISeer
	{
		// PUBLIC


		// Constructors

		public seer(int q)
		{
			_active = true;
			_q = q;
			_requestCount = 0;
			_message = "Be Sure To Drink Your Ovaltine.";
			_name = "seer";
		}
		// DESCRIPTION:
		// * Initializes its active state, _q using user input, request
		// count, _message, and _name.
		// * "q" is used to determine how many times the volatileSeer can be
		// requested before it flips state.
		// 
		// PRECONDITIONS:
		// * "q" must be greater than 0.
		// 
		// POSTCONDITIONS:
		// * The object begins active.
		// * _q is initialized using user input.
		// * _requestCount is initialized to 0.
		// * _message is initialized with a predetermined message.
		// * _name is initialized using the class's name.


		// Functionality

		public virtual void request(ref string fetchedMessage)
		{
			if (_active) {
				fetchedMessage = _message;
			} else {
				fetchedMessage = null;
			}

			_requestCount++;
			if (_requestCount == _q) {
				_active = !_active;
				_requestCount = 0;
			}
		}
		// DESCRIPTION:
		// * A request returns the message into the passed string, and then
		// evaulates the object's active state.
		// * Passes out the message if active.
		// * Passes out empty if inactive.
		// 
		// POSTCONDIITONS:
		// * If the request count matches _q, the seer flips state and the
		// request count resets to 0.


		// Accessors

		public virtual bool active()
		{
			return _active;
		}
		// DESCIPTION:
		// * Checks whether the seer is active.
		// * Returns true if the seer is active.
		// * Returns false if the seer is inactive.

		public virtual string name()
		{
			return _name;
		}
		// DESCRIPTION:
		// * Returns the name of the class, for templating purposes.


		// PROTECTED


		// Members

		protected bool _active;
		// Whether the seer is active or not

		protected int _q;
		// How many times the user can request the seer.

		protected uint _requestCount;
		// How many times the user has requested the seer.

		protected string _message;
		// The seer's hidden message.

		protected string _name;
		// The class's name.
	}
}
