namespace IWantRISC
{
    /// <summary>
    /// this is a pc
    /// </summary>
    internal abstract class Machine
    {
        /// <summary>
        /// the cpu of the pc
        /// </summary>
        public CPU CPU { get; set; }

        /// <summary>
        /// Address space: CPU, GPU, interrupt controller
        /// </summary>
        internal byte[] AddressSpace;

        /// <summary>
        /// Ram in bytes.
        /// </summary>
        public uint Ram { get; set; }

        /// <summary>
        /// Start of actual ram.
        /// 0x0 - 0x400 - Interrupts
        /// 0x400 - 0x500 - BIOS Data Area
        /// </summary>
        const uint RamStart = 0x500;

        /// <summary>
        /// End of actual ram.
        /// </summary>
        protected uint RamEnd = 0x500 + RamStart;

        /// <summary>
        /// Create a new PC.
        /// </summary>
        /// <param name="cpu">The CPU of the PC.</param>
        public Machine(CPU cpu, uint ramSize = 65536)
        {
            CPU = cpu;
            AddressSpace = new byte[1048576]; // 20-bit address space for 8086. Make this better when we have more cpus
            Ram = ramSize;
        }

        /// <summary>
        /// Boot the machine.
        /// </summary>
        internal abstract void Start();
    }
}
