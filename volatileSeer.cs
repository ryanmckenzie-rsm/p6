// AUTHOR: Ryan McKenzie
// FILENAME: p6.cs
// DATE: June 7, 2018
// REVISION HISTORY: v1.0.0
// PLATFORM: MSBuild 15.7.179.6572

// CLASS INVARIANT:
// * Rejected requests return empty.
// * Requests are randomly rejected.
// * The user can change the hidden message.

// INTERFACE INVARIANT:
// * The hidden message can be changed with request's 1-arg.
// * The volatile seer will randomly reject requests independant of state.
// * Otherwise acts just like a seer.

// IMPLEMENTATION INVARIANT:
// * A request has a 50% chance to be rejected.
// * Rejected requests return empty.
// * The hidden message is set to the string passed in request().

// DESCRIPTION:
// * The volatileSeer acts like a seer, but randomly rejects requests.

// ASSUMPTIONS:
// * The user can replace the message using the reference parameter of
// request().
// * The volatileSeer randomly rejects requests using a random number
// generator.
// * there is a 50% chance to reject a request.
// * If a request is rejected, the string passed in request() is set to empty.


using System;


namespace p3
{
	public class volatileSeer : seer
	{
		// PUBLIC


		public volatileSeer(int q) : base(q)
		{
			_name = "volatileSeer";
		}
		// DESCRIPTION:
		// * Initializes the name and the random number generator.
		// * "q" is used to determine how many times the volatileSeer can be
		// requested before it flips state.
		// 
		// POSTCONDITIONS:
		// * The random number generator is initialized.
		// * _name is initialized with the name of the class.
		// * All other inherited values are determined by their parent
		// constructor.


		// Functionality

		public override void request(ref string fetchedMessage)
		{
			_message = fetchedMessage;
			base.request(ref fetchedMessage);
			if (genRandNum() < _REJECT_CHANCE) {
				fetchedMessage = null;
			}
		}
		// DESCRIPTION:
		// * Fetches a message using the parent method, and then determines
		// whether to reject the request or not.
		// * Passes out the message if active.
		// * Passes out empty if inactive.
		// * Has a 50% chance of passing out empty regardless of state.
		// 
		// PRECONDITIONS:
		// * fetchedMessage must not be empty.
		// 
		// POSTCONDITIONS:
		// * _message is set to fetchedMessage.


		// PRIVATE


		// Members

		private const int _REJECT_CHANCE = 5;
		// Chance to reject a request.

		private static Random _rng = new Random();
		// A random number generator used to randomly reject requests.


		// Utility

		private int genRandNum()
		{
			const int MAX_ROLL = 9;
			return _rng.Next(MAX_ROLL);
		}
		// DESCRIPTION:
		// * Generates a random number between and including 0 and the maximum
		// roll.
	}
}
