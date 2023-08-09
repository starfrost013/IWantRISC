namespace IWantRISC
{
    /// <summary>
    /// Temporary class.
    /// </summary>
    internal static class Emulator
    {
        /// <summary>
        /// Stores the current machine.
        /// </summary>
        internal static Machine CurrentMachine { get; set; }

        static Emulator()
        {
            CurrentMachine = new IBM5150();
        }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="machine">The machine to run</param>
        public static void Start(Machine machine)
        { 
            CurrentMachine = machine;
            CurrentMachine.Start();
        }
    }
}
