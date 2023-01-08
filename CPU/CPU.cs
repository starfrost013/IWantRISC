

namespace IWantRISC
{
    /// <summary>
    /// <para>CPU</para>
    /// 
    /// <para>A base class for the 808x-architecture CPU</para>
    /// 
    /// <para>Notes:</para>
    /// <para>20-bit addressing</para>
    /// <para>Insane segmentation system</para>
    /// <para>Completely fucked up instruction encoding</para>
    /// <para>Program counter is for nerds (CS+IP = PC)</para>
    /// </summary>
    internal abstract class CPU
    {

        #region bad silicon design (registers)

        /// <summary>
        /// The size of one paragraph.
        /// 
        /// You decrement a segment register by one, it actually moves 16 bytes and you have to apply the offset.
        /// Fuck sake intel
        /// </summary>
        private const int PARAGRAPH_SIZE = 16;

        /* order of stack push:
         * 
         * AX
         * CX
         * DX
         * BX
         * SP
         * BP
         * SI
         * DI
         * 
         */
        /// <summary>
        /// General-purpose register 1
        /// 
        /// They wanted you to use this for accumulation.
        /// </summary>
        public ushort AX { get; set; }

        /// <summary>
        /// Low half of <see cref="AX"/>.
        /// </summary>
        public byte AL => (byte)(AX & 0xFF);

        /// <summary>
        /// High half of <see cref="AX"/>.
        /// </summary>
        public byte AH => (byte)((AX >> 8) & 0xFF);

        /// <summary>
        /// General-purpose register 2
        /// 
        /// Base Register
        /// </summary>
        public ushort BX { get; set; }

        /// <summary>
        /// Low half of <see cref="BX"/>.
        /// </summary>
        public byte BL => (byte)(AX & 0xFF);

        /// <summary>
        /// High half of <see cref="BX"/>.
        /// </summary>
        public byte BH => (byte)((AX >> 8) & 0xFF);
       
        /// <summary>
        /// It's a register! They wanted you to use this for loops.
        /// </summary>
        public ushort CX { get; set; }

        /// <summary>
        /// Low half of <see cref="CX"/>.
        /// </summary>
        public byte CL => (byte)(AX & 0xFF);

        /// <summary>
        /// High half of <see cref="CX"/>.
        /// </summary>
        public byte CH => (byte)((AX >> 8) & 0xFF);

        /// <summary>
        /// DX (General purpose register 4)
        /// 
        /// They wanted you to use this for data
        /// </summary>
        public ushort DX { get; set; }

        /// <summary>
        /// Low half of <see cref="DX"/>.
        /// </summary>
        public byte DL => (byte)(AX & 0xFF);

        /// <summary>
        /// High half of <see cref="DX"/>.
        /// </summary>
        public byte DH => (byte)((AX >> 8) & 0xFF);

        /// <summary>
        /// Stack pointer register. 
        /// 
        /// Offset of the end of the stack from the value of <see cref="SS"/>.
        /// </summary>
        public ushort SP { get; set; }

        internal ushort StackTop => (ushort)((SS * PARAGRAPH_SIZE) + SP);

        /// <summary>
        /// Low half of <see cref="SP"/>
        /// </summary>
        public byte SPL => (byte)(SP & 0xFF);

        /// <summary>
        /// Base pointer register.
        /// </summary>
        public ushort BP { get; set; }

        /// <summary>
        /// Low half of <see cref="BP"/>
        /// </summary>
        public byte BPL => (byte)(BP & 0xFF);

        /// <summary>
        /// Source index register.
        /// </summary>
        public ushort SI { get; set; }

        /// <summary>
        /// Low half of <see cref="SIL"/>
        /// </summary>
        public byte SIL => (byte)(SI & 0xFF);

        /// <summary>
        /// Destination index register
        /// </summary>
        public ushort DI { get; set; }

        /// <summary>
        /// Low half of <see cref="DI"/>.
        /// </summary>
        public byte DIL => (byte)(DI & 0xFF);

        /// <summary>
        /// Pointer to the stack segment.
        /// </summary>
        public ushort SS { get; set; }


        /// <summary>
        /// Pointer to the code segment.
        /// </summary>
        public ushort CS { get; set; }

        /// <summary>
        /// Instruction pointer
        /// </summary>
        public ushort IP { get; set; }

        /// <summary>
        /// Fake register used to make my life easier because unIntelligent says no
        /// </summary>
        internal int PC => ((CS * PARAGRAPH_SIZE) + IP) & 0xFFFFF; // 20-bit address space wraparound

        /// <summary>
        /// See <see cref="PC"/>.
        /// </summary>
        private string CurPC => $"{CS:X4}:{IP:X4} (0x{PC:X})";

        /// <summary>
        /// Pointer to the data segment.
        /// </summary>
        public ushort DS { get; set; }

        /// <summary>
        /// Pointer to the extra segment.
        /// </summary>
        public ushort ES { get; set; }

        /// <summary>
        /// Overflow flag (a mathematical operation overflowed)
        /// </summary>
        public bool OF { get; set; } // bit11

        /// <summary>
        /// Direction flag for string instructions
        /// </summary>
        public bool DF { get; set; } // bit10

        /// <summary>
        /// Interrupt enable flag
        /// </summary>
        public bool IF { get; set; } // bit9

        /// <summary>
        /// Trap flag (single-step execution)
        /// </summary>
        public bool TF { get; set; } // bit8

        /// <summary>
        /// Sign flag (MSB of reuslt)
        /// </summary>
        public bool SF { get; set; } // bit7

        /// <summary>
        /// Zero flag (result = 0)
        /// </summary>
        public bool ZF { get; set; } // bit6

        /// <summary>
        /// Auxillary carry flag (BCD mode)
        /// </summary>
        public bool AF { get; set; } // bit4

        /// <summary>
        /// Parity flag (__even parity__)
        /// </summary>
        public bool PF { get; set; } // bit2

        /// <summary>
        /// Carry flag (if a carry occurred)
        /// </summary>
        public bool CF { get; set; } // bit0

        #endregion

        // FS and GS are 80386+ only

        #region worse silicon design

        /// <summary>
        /// Boot the cpu.
        /// </summary>
        internal abstract void Boot();

        /// <summary>
        /// Execute code
        /// </summary>
        internal abstract void Execute();

        internal virtual void RegisterDump()
        {
            NCLogging.Log($"\nGeneral: AX={AX:X} BX={BX:X} CX={CX:X} DX={DX:X}\n" +
                $"Index and pointer: SP={SP:X} BP={BP:X} SI={SI:X} DI={DI:X}\n" +
                $"Segment: CS={CS:X} IP={IP:X} (calculated PC={CurPC}) DS={DS:X} ES={ES:X}\n" +
                $"Stack: SS={SS:X} SP={SP:X} (calculated top of stack=0X{StackTop:X5})\n" +
                $"Flags: Overflow={OF} Direction={DF} Interrupt enable={IF} Trap flag={TF} Sign={SF}\n" +
                $"Zero flag={ZF} Aux carry={AF} Even parity={PF} Carry={CF}", "CPU Register Dump"); ;
        }

        #endregion
    }
}
