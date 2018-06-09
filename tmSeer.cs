// AUTHOR: Ryan McKenzie
// FILENAME: p6.cs
// DATE: June 7, 2018
// REVISION HISTORY: v1.0.0
// PLATFORM: MSBuild 15.7.179.6572

// CLASS INVARIANT:
// * The tmSeer "dies" after so many cycles.
// * A tmSeer will mix the case of the hidden message.

// INTERFACE INVARIANT:
// * The tmSeer will "die" after a certain amount of cycles.
// * When the tmSeer is "dead", requests will always return empty.
// * A "dead" tmSeer is considered inactive.
// * Successful requests will mix the case of the message.

// IMPLEMENTATION INVARIANT:
// * A tmSeer will die after 2 * _k cycles.
// * An active seer will randomly mix the case of successful requests.
// * There is a 30% chance to mix a character's case.

// DESCRIPTION:
// * The tmSeer acts like a seer, but cannot change state after a certain
// number of cycles.

// ASSUMPTIONS:
// * k is defined as twice q.
// * A cycle is determined as turning on and off (i.e. 2 state changes == 1
// cycle).
// * When a tmSeer has exceeded its cycles, it is "dead" and can no longer be
// requested.
// * All requests to a "dead" tmSeer return empty;
// * The returned message is mixed arbitrarily via a random number generator.
// * It iterates through the string and has a 30% chance to mix that character's
// case.


using System;


namespace p3
{
	public class tmSeer : seer
	{
		// PUBLIC


		// Constructors

		public tmSeer(int q) : base(q)
		{
			_dead = false;
			_k = q * _Q_MULT;
			_stateChangeCount = 0;

			_name = "tmSeer";
		}
		// DESCRIPTION:
		// * Initializes _dead, _k, the state change count, the random number
		// generator, and the name.
		// * "q" is used to determine how many times the volatileSeer can be
		// requested before it flips state.
		// 
		// PRECONDITIONS:
		// * "q" must be greater than 0.
		// 
		// POSTCONDITIONS:
		// * _dead (whether the object can still change states) begins false.
		// * _k is set to a predetermined multiplier.
		// * The state change count begins at 0.
		// * The random number generator is initialized.
		// * _name is set to the class's name.
		// * All other inherited values are determined by their parent
		// constructor.


		// Functionality

		public override void request(ref string fetchedMessage)
		{
			if (_dead) {
				fetchedMessage = null;
			} else {
				bool lastState = _active;
				base.request(ref fetchedMessage);
				if (lastState != _active) {
					_stateChangeCount++;
					if (_stateChangeCount == _k * 2) {
						_dead = true;
					}
				}
				if (fetchedMessage != null) {
					invertStringCase(ref fetchedMessage);
				}
			}
		}
		// DESCRIPTION:
		// * If the tmSeer is not "dead" then it will fetch its hidden message,
		// dependant on it active state.
		// * If the state is active, the case of the message is arbitrarily
		// mixed.
		// * If the state is inactive, the message is returned as empty.
		// * If the state changes, the state change count increments.
		// * If the tmSeer is dead, the message will always be returned empty.
		// 
		// POSTCONDITIONS:
		// * If the request count matches _q, the state flips, and the request
		// count is reset.
		// * If the state flips, the state change count increments.
		// * If the state change count increments, the object will be
		// determined "dead" if the count matches twice _k.


		// Accessors

		public override bool active()
		{
			if (_dead) {
				return false;
			} else {
				return base.active();
			}
		}
		// DESCRIPTION:
		// * Checks whether the tmSeer is active.
		// * Returns true if the tmSeer is alive and active.
		// * Returns false if the tmSeer is dead or inactive.


		// PRIVATE


		// Members

		private const int _Q_MULT = 2;
		// Multiplier used to determines _k's values

		private static Random _rng = new Random();
		// A random number generator used to mix the message case.

		private bool _dead;
		// Determines whether the tmSeer can still be requested.

		private int _k;
		// How many times the tmSeer can cycle through states.

		private uint _stateChangeCount;
		// The number of times the state has changed.


		// Utility

		private void invertStringCase(ref string toInvert)
		{
			const int MIX_CHANCE = 3;
			char[] charArr = toInvert.ToCharArray();
			for (int i = 0; i < charArr.Length; i++) {
				if (genRandNum() < 10 - MIX_CHANCE) {
					continue;
				} else {
					if (char.IsUpper(charArr[i])) {
						charArr[i] = char.ToLower(charArr[i]);
					} else {
						charArr[i] = char.ToUpper(charArr[i]);
					}
				}
			}

			toInvert = "";
			foreach (char c in charArr) {
				toInvert += c;
			}
		}
		// DESCRIPTION:
		// * Inverts the case of the passed string.
		// * Interates through the string, and has a 30% chance to flip its
		// case.

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
