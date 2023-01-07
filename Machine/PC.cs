using System;
using System.Collections.Generic;

namespace IWantRISC
{
    /// <summary>
    /// this is a pc
    /// </summary>
    internal abstract class PC
    {
        /// <summary>
        /// the cpu of the pc
        /// </summary>
        public CPU CPU { get; set; }

        // GPU, interrupt controller, blah blah blah 

        public byte[] RAM { get; set; }

        /// <summary>
        /// Create a new PC.
        /// </summary>
        /// <param name="cpu">The CPU of the PC.</param>
        /// <param name="addressSpaceSize"></param>
        public PC(CPU cpu, int addressSpaceSize, int ramSize = 65536)
        {
            CPU = cpu;
            RAM = new byte[addressSpaceSize];
        }

        internal abstract void Boot();

        internal abstract void Run(); 
    }
}
