﻿using System;
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
        /// <param name="ramSize"></param>
        public PC(CPU cpu, int ramSize)
        {
            CPU = cpu;
            RAM = new byte[ramSize];
        }

        internal abstract void Boot();

        internal abstract void Run(); 
    }
}