// AUTHOR: Ryan McKenzie
// FILENAME: p6.cs
// DATE: June 7, 2018
// REVISION HISTORY: v1.0.0
// PLATFORM: MSBuild 15.7.179.6572

// DESCRIPTION:
// * Prints output to a log file "log.txt".
// * Begins by testing the numMixer capabilities of the numMixerTmVolatileSeer
// in its various parities.
// * Then prints stats listing how it responded to being pinged.
// * Finally, tests its capabilities as a seer and prints various stats
// depending on how it responded.

// ASSUMPTIONS:
// * A numMixerTmVolatileSeer assumes the functionality of all seer objects as
// well as a numMixer.
// * Tests are implemented in 2 stages.
// * 1st stage is testing the object as a numMixer.
// * 2nd stage is testing the object as a seer.


using p1;
using System;
using System.IO;


namespace p6
{
	class p6
	{
		struct Stats
		{
			public int activeCount;
			public int inactiveCount;
			public int rejectCount;
		}
		// Used to track a seer's response to being requested.


		static void Main()
		{
			const string FILE_NAME = "log.txt";
			StreamWriter sw = new StreamWriter(FILE_NAME);

			numMixerTmVolatileSeer numMixerTmVolatileSeerObj = new numMixerTmVolatileSeer(genRandNum());

			pingMixer(numMixerTmVolatileSeerObj, numMixer.OutputController.MIX, ref sw);
			pingMixer(numMixerTmVolatileSeerObj, numMixer.OutputController.EVEN, ref sw);
			pingMixer(numMixerTmVolatileSeerObj, numMixer.OutputController.ODD, ref sw);

			printStats(numMixerTmVolatileSeerObj, ref sw);

			const int PING_COUNT = 25;
			writeHeader(numMixerTmVolatileSeerObj.name(), ref sw);
			requestSeer(numMixerTmVolatileSeerObj, PING_COUNT, ref sw);

			sw.Close();
			Console.WriteLine("Data written to: {0}.", Path.GetFullPath(FILE_NAME));
			Console.WriteLine("Press any key to exit.");
			Console.ReadKey();
		}


		static void writeHeader(string header, ref StreamWriter sw)
		{
			const int WIDTH = 26;
			string border = new string('#', WIDTH);
			border = "#" + border + "#";
			string padding = new string(' ', WIDTH);
			padding = "#" + padding + "#";

			sw.WriteLine(border);
			sw.WriteLine(padding);

			double dynWidth = WIDTH - header.Length;
			int widthLeft = (int)(Math.Floor(dynWidth / 2));
			int widthRight = (int)(Math.Ceiling(dynWidth / 2));
			string headerLeft = new string(' ', widthLeft);
			string headerRight = new string(' ', widthRight);
			string headerLine = "#" + headerLeft + header + headerRight + "#";
			sw.WriteLine(headerLine);

			sw.WriteLine(padding);
			sw.WriteLine(border);
		}
		// Description:
		// * Writes a header using the StreamWriter object:
		// ############################
		// #                          #
		// #          header          #
		// #                          #
		// ############################
		//
		// Preconditions:
		// * Supplied string must be short enough to fit within header box.


		static Random rng = new Random();
		static int genRandNum()
		{
			const int MIN_ROLL = 10;
			const int MAX_ROLL = 20;
			return rng.Next(MIN_ROLL, MAX_ROLL);
		}
		// DESCRIPTION:
		// * Returns a random number between and including the MIN_ROLL and MAX_ROLL.


		static void requestSeer(ISeer seerObj, uint count, ref StreamWriter sw)
		{
			sw.WriteLine("Requesting messages...\r\n");
			sw.WriteLine("== {0} is now active. ==", seerObj.name());
			string message = "";
			Stats stats;
			stats.activeCount = 0;
			stats.inactiveCount = 0;
			stats.rejectCount = 0;
			bool lastState;
			while (count > 0) {
				message = "Lorem ipsum dolor.";
				lastState = seerObj.active();
				seerObj.request(ref message);
				testStateActive(lastState, message, ref stats, ref sw);
				testStateChanged(lastState, seerObj, ref sw);
				count--;
			}
			sw.WriteLine("\r\n{0} was inactive for \"{1}\" requests.",
						 seerObj.name(),
						 stats.inactiveCount);
			sw.WriteLine("{0} was active for \"{1}\" requests and rejected " +
						 "\"{2}\" of them.\r\n",
						 seerObj.name(),
						 stats.activeCount,
						 stats.rejectCount);
		}
		// DESCRIPTION:
		// * Requests a seer objects.
		// * Tracks state changes in the seer object and prints data on how often it
		// was active, inactive, and how many requests it rejected.
		// 
		// PRECONDITIONS:
		// * "count" should be > 0.
		// * The seer object should not be null.
		// 
		// POSTCONDITIONS:
		// * The seer object may be in a different state than when it was passed.


		static void testStateActive(bool lastState,
									string message,
									ref Stats stats,
									ref StreamWriter sw)
		{
			if (lastState) {
				if (message != null) {
					sw.WriteLine(message);
				} else {
					sw.WriteLine("Rejected...");
					stats.rejectCount++;
				}
				stats.activeCount++;
			} else {
				sw.WriteLine("Inactive...");
				stats.inactiveCount++;
			}
		}
		// DESCRIPTION:
		// * Checks if the state of the seer object changed, and records data
		// depending on the results.


		static void testStateChanged(bool lastState,
									 ISeer seerObj,
									 ref StreamWriter sw)
		{
			if (lastState != seerObj.active()) {
				if (seerObj.active()) {
					sw.WriteLine("\r\n== {0} is now active. ==", seerObj.name());
				} else {
					sw.WriteLine("\r\n== {0} is now inactive. ==", seerObj.name());
				}
			}
		}
		// DESCRIPTION:
		// * Lets the client know how the state of the seerObj has changed.
		// 
		// PRECONDITIONS:
		// * seerObj must not be null.

		static void pingMixer(numMixer numMixerObj,
							  numMixer.OutputController parity,
							  ref StreamWriter sw)
		{
			testParity(numMixerObj, parity, ref sw);

			const int SIZE = 10;
			int[] pingedData = new int[SIZE];
			if (numMixerObj.ping(ref pingedData)) {
				sw.WriteLine("Ping successful ({0} elements requested):", SIZE);
				for (int i = 0; i < pingedData.Length; i++) {
					sw.WriteLine(pingedData[i]);
				}
			} else {
				sw.WriteLine("Ping failed.");
			}

			sw.WriteLine("\r\n");
		}
		// DESCRIPTION:
		// * Tests the numMixer object with the passed state.
		// * Pings the numMixer and prints returned values, if it succeeds.
		//
		// PRECONDITIONS:
		// * numMixerObj must not be null.
		// * numMixerObj must have a dataset containing the requested parity.
		//
		// POSTCONDITIONS:
		// * numMixer may no longer be active.


		static void testParity(numMixer numMixerObj,
							   numMixer.OutputController parity,
							   ref StreamWriter sw)
		{
			if (parity == numMixerObj.getControllerState()) {
				writeHeader("PING " + numMixerObj.getControllerStateName(), ref sw);
				sw.WriteLine("State change failed (already set).");
			} else {
				numMixerObj.setControllerState(parity);
				writeHeader("PING " + numMixerObj.getControllerStateName(), ref sw);
				sw.WriteLine("State set to {0}.", numMixerObj.getControllerStateName());
			}
		}
		// DESCRIPTION:
		// * Tests if the numMixer successfully changes to the specified parity.
		// 
		// POSTCONDITIONS:
		// * numMixer may have changed parity.


		static void printStats(numMixer numMixerObj, ref StreamWriter sw)
		{
			writeHeader("STATS", ref sw);
			sw.WriteLine("Output controller state change count: {0}.",
						 numMixerObj.stateChangeCount());
			string state = "Output controller is ";
			if (numMixerObj.isActive()) {
				state += "active";
			} else {
				state += "inactive.";
			}
			sw.WriteLine("{0}\r\n\r\n", state);
		}
		// DESCRIPTION:
		// * Prints the number of times the output controller has changed state and
		// whether the numMixer object is active/inactive.
	}
}
