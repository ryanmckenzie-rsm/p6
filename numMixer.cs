// AUTHOR: Ryan McKenzie
// FILENAME: numMixer.cs
// DATE: June 7, 2018
// REVISION HISTORY: v5.0.0
// PLATFORM: MSBuild 15.7.179.6572

// DESCRIPTION:
// * This class outputs a random set of integers selected either from user
// seeded input, or from an auto-generated set.
// * This class can be placed into an illegal state should the user provide a
// array of size zero on object creation.
// * Or should the user request a parity they did not provide (i.e. even
// numbers when they did not provide even numbers).
// * This class is anticipated to be used as a data sink.

// ASSUMPTIONS: 
// * This class has one hard requirement for correct usage: the user MUST
// provide a array of size > 0 or else the program can fail. Otherwise, this
// class protects against invalid states (i.e. requesting even/odd numbers when
// the user hasn't provided any).
// * This class will build its dataset either from a user provided array, or
// from a pre-defined, valid array. The array must be size > 0, and should
// have even and odd numbers if the user wishes to ping them.
// * The user can change the output controller via a public mutator.
// * The state change counter will only increment should the state ACTUALLY
// change.
// * To ping the numMixer, the user must provide a array of the size they
// want returned. The class protects against arrays of size = 0. Pinging the
// numMixer can fail (return false) in two cases:
//   1. The numMixer is inactive.
//   2. The user has requested even/odd integers when they did not provide any.
// * The numMixer returns integers by randomly selecting them from the set
// provided at object creation. It randomly selects indexes from the array
// until it finds a valid number (e.g. an even number if the controller is set
// to even) and stores them in a user provided array.
// * The "active" state of the numMixer is tracked via a counter which is
// set by the user at object creation. It is decremented everytime the user
// successfully pings the numMixer.
// * The user can check whether the numMixer is active via a public accessor
// which returns true when the counter > 0.
// * Comparison (==) operators are supported.
// * Comparison is performed on all data members.
// * Relational (<) operators are supported.
// * Relations are assessed on the countdown.
// * Addition (+) operators are supported.
// * Addition is performed on all members except the state controller.
// * The dataset of the right hand side is appended to the end of the left
// hand side.


using System;


namespace p1
{
	public class numMixer
	{
		// PUBLIC


		// Types

		public enum OutputController { MIX, EVEN, ODD };
		// Valid states the output controller can be set to.


		// Constructors

		public numMixer(int countDown)
		{
			_evenValid = true;
			_oddValid = true;
			_countDown = countDown;
			_stateChangeCount = 0;
			_dataset = null;
			_controllerState = OutputController.MIX;

			// generate valid dataset
			const int SIZE = 100;
			_dataset = new int[SIZE];
			for (int i = 0; i < SIZE; i++) {
				_dataset[i] = i + 1;
			}
		}
		// DESCRIPTION:
		// * This constructor creates a dataset to choose from of values 1-100.
		// * The numMixer can be called "countDown" times.
		// * Calls for integers of any parity are valid.
		// * The output controller defaults to "MIX".
		// 
		// Postconditions:
		// * The controller state is set to "MIX".
		// * The state change count is set to 0.
		// * The dataset is intialized with integers 1-100.
		// * The countdown is randomly set to "countDown"
		// * Calls for integers of even parity are valid.
		// * Calls for integers of odd parity are valid.

		public numMixer(int countDown, int[] dataset)
		{
			_evenValid = false;
			_oddValid = false;
			_countDown = countDown;
			_stateChangeCount = 0;
			_dataset = null;
			_controllerState = OutputController.MIX;

			// copy dataset
			_dataset = (int[])dataset.Clone();

			// validate dataset
			foreach (int numMixer in _dataset) {
				if (numMixer % 2 == 0) {
					_evenValid = true;
				} else if (numMixer % 2 == 1) {
					_oddValid = true;
				}
			}
		}
		// DESCRIPTION:
		// * This constructor creates a dataset using "dataset".
		// * The numMixer can be called "countDown" times.
		// * The validity of various parity calls are evaluated on the dataset
		// provided.
		// * The output controller defaults to "Mix".
		//
		// PRECONDITIONS:
		// * "dataset" must be of size > 0.
		// * "dataset" must contain integers of a parity the user wishes to
		// request.
		//
		// Postconditions:
		// * The controller state is set to "Mix".
		// * The state change count is set to 0.
		// * The dataset is intialized with "dataset".
		// * The countdown is randomly set to "countDown"
		// * Calls for integers of even parity may or may not be valid.
		// * Calls for integers of odd parity may or may not be valid.


		// Functionality

		public virtual bool ping(ref int[] returnValues)
		{
			if (isActive() && checkStateValid()) {
				for (int i = 0; i < returnValues.Length; i++) {
					returnValues[i] = genRandNum();
				}
				_countDown--;
				return true;
			} else {
				return false;
			}
		}
		// DESCRIPTION:
		// * Stores a random selection of integers from the dataset into
		// "returnValues".
		// * The amount of integers stored is equal to the size of
		// "returnValues".
		// * The parity of the integers is determined by the controller state.
		// * Returns true if the pinged numMixer is active and the requested
		// parity exists within the dataset.
		// * Returns false if the numMixer is inactive, or if the requested
		// parity does not exist within the dataset.
		//
		// PRECONDITIONS:
		// * The numMixer must be in an active state.
		// * The user must provide an array of size > 0.
		// * The user must have provided integers of the requested parity at
		// object creation.
		//
		// Postconditions:
		// * If the call succeeds, the countdown is decremented.


		// Accessors

		public virtual bool isActive()
		{
			return (_countDown > 0);
		}
		// DESCRIPTION:
		// * Returns whether the numMixer is still active.
		// * Returns true if the numMixer is active.
		// * Returns false if the numMixer is inactive.

		public int stateChangeCount()
		{
			return _stateChangeCount;
		}
		// DESCRIPTION:
		// * Returns how many times the output controller has changed state.

		public OutputController getControllerState()
		{
			return _controllerState;
		}
		// DESCRIPTION:
		// * Returns the state of the OutputController.

		public string getControllerStateName()
		{
			switch (_controllerState) {
				case OutputController.MIX:
					return "MIX";
				case OutputController.EVEN:
					return "EVEN";
				case OutputController.ODD:
					return "ODD";
				default:
					return "UNKNOWN";
			}
		}
		// DESCRIPTION:
		// * Returns the name of the controller state, for templating purposes.

		// Mutators

		public void setControllerState(OutputController state)
		{
			if (getControllerState() != state) {
				_controllerState = state;
				_stateChangeCount++;
			}
		}
		// DESCRIPTION:
		// * Changes the output controller to "state" if it is not already set.
		//
		// PRECONDITIONS:
		// * The output controller should be set to a state that is different
		// than "state".
		//
		// Postconditions:
		// * The output controller changes state.


		// PRIVATE

		// Utility

		private bool checkStateValid()
		{
			switch (_controllerState) {
				case OutputController.MIX:
					return true;
				case OutputController.EVEN:
					return _evenValid;
				case OutputController.ODD:
					return _oddValid;
				default:
					return false;
			}
		}
		// DESCRIPTION:
		// * Checks whether a ping call is valid depending on the output
		// controller state.
		// * Returns true if the call is valid.
		// * Returns false if the call is invalid.

		private int genRandNum()
		{
			int upperBound = _dataset.Length - 1;
			int i = _rng.Next(upperBound);
			switch (_controllerState) {
				case OutputController.MIX:
					return _dataset[i];
				case OutputController.EVEN:
					while (_dataset[i] % 2 != 0) {
						i = _rng.Next(upperBound);
					}
					return _dataset[i];
				case OutputController.ODD:
					while (_dataset[i] % 2 != 1) {
						i = _rng.Next(upperBound);
					}
					return _dataset[i];
				default:
					return 0;
			}
		}
		// DESCRIPTION:
		// * Selects random values from the dataset and returns them depending
		// on the state of the output controller.
		//
		// PRECONDITIONS:
		// * The dataset must be of size > 0.
		// * The dataset must contain numbers of the requested parity.


		// Members

		private static Random _rng = new Random();
		// Used to seed the dataset, countDown, and randomly select values from
		// the dataset.

		private bool _evenValid;
		// Determines whether pings for even values can be evaluated.

		private bool _oddValid;
		// Determines whether pings for odd values can be evaluated.

		protected int _countDown;
		// Determines how many times the numMixer can be pinged.

		private int _stateChangeCount;
		// Stores how many times the OutputController has changed state.

		private int[] _dataset;
		// Stores the values to be randomly returned in pings.

		private OutputController _controllerState;
		// Determines the parity of the integers to be returned.
	}
}
