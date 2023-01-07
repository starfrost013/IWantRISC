
using System.Diagnostics;

namespace IWantRISC
{
    /// <summary>
    /// x86-16 ModRM decoder
    /// 
    /// We don't care about the first byte of the instruction (already figured out through opcode charts basically)
    /// </summary>
    internal struct Instruction
    {
        /// <summary>
        /// 6-bit opcode
        /// 
        /// (incorporates S and W bits)
        /// </summary>
        public byte Opcode;

        /// <summary>
        /// Determines a lot of stuff.
        /// 
        /// IF MOD=0, RM=6, DIRECT MEMORY ADDRESSING!!!!!
        /// </summary>
        public byte Mod;

        /// <summary>
        /// If:
        /// Mod=00->10: register ID
        /// Mod=11->opcode extension
        /// </summary>
        public byte RegOpcode;

        /// <summary>
        /// Register/Memory byte
        /// 
        /// if Mod=0..2 then this is a register pair 
        /// where the values of the registers indicate where we 
        /// need to go
        /// if Mod=3 then this is a register (see <see cref="Register"/>);
        /// </summary>
        public byte RM;

        /// <summary>
        /// Relative address of the opcode that will be operated on.
        /// </summary>
        public ushort Displacement;

        public Instruction(byte[] instruction)
        {
            Opcode = instruction[0];

            byte modrmByte = instruction[1];

            Mod = (byte)(modrmByte & 0x03);
            RegOpcode = (byte)((modrmByte >> 2) & 0x07);
            RM = (byte)((modrmByte >> 5) & 0x07);

            switch (instruction.Length)
            {
                case 2:
                    break;
                case 3:
                    byte displacementByte = instruction[2];
                    Displacement = displacementByte;
                    break;
                case 4:
                    Displacement = BitConverter.ToUInt16(instruction.AsSpan()[2..3]);
                    break;
            }
        }
    }
}
