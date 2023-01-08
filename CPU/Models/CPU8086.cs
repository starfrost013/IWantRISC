namespace IWantRISC
{
    internal class CPU8086 : CPU
    {
        /// <summary>
        /// If this is true, don't do anything until an external interrupt.
        /// </summary>
        private bool Halt { get; set; }

        internal override void Boot()
        {
            NCLogging.Log("808x RESET", "Hope it boots this time", ConsoleColor.Blue);

            //All of the FLAGS
            //are initialised to zero,
            //we just simulate them with booleans
            //so they all start as FALSE
            CS = 0xFFFF;
            DS = 0x0;
            SS = 0x0;
            ES = 0x0;

            RegisterDump();

            Execute();
        }

        internal override void Execute()
        {
            while (!Halt)
            {

                // get the next instruction
                byte next = Emulator.CurMachine.AddressSpace[PC];

                switch (next)
                {
                    case 0x40: // INC AX
                        AX++;
                        break;
                    case 0x41: // INC CX
                        CX++;
                        break; 
                    case 0x42: // INC DX
                        DX++;
                        break;
                    case 0x43: // INC BX
                        BX++;
                        break;
                    case 0x44: // INC SP
                        SP++;
                        break;
                    case 0x45: // INC BP
                        BP++;
                        break;
                    case 0x46: // INC SI
                        SI++;
                        break;
                    case 0x47: // INC DI
                        DI++;
                        break;
                    case 0x48: // DEC AX
                        AX--;
                        break;
                    case 0x49: // DEC CX
                        CX--;
                        break;
                    case 0x4A: // DEC DX
                        DX--;
                        break;
                    case 0x4B: // DEC BX
                        BX--;
                        break;
                    case 0x4C: // DEC SP
                        SP--;
                        break;
                    case 0x4D: // DEC BP
                        BP--;
                        break;
                    case 0x4E: // DEC SI
                        SI--;
                        break;
                    case 0x4F: // DEC DI
                        DI--;
                        break;
                    case 0x90: // NOP (16-bit)
                        break;
                    case 0x91: // XCHG CX, AX
                        ushort tempCx = CX;
                        CX = AX;
                        AX = tempCx;
                        break;
                    case 0x92: // XCHG DX, AX
                        ushort tempDx = DX;
                        DX = AX;
                        AX = tempDx;
                        break;
                    case 0x93: // XCHG BX, AX
                        ushort tempBx = BX;
                        BX = AX;
                        AX = tempBx;
                        break;
                    case 0x94: // XCHG SP, AX
                        ushort tempSp = SP;
                        SP = AX; 
                        AX = tempSp;
                        break;
                    case 0x95: // XCHG BP, AX
                        ushort tempBp = BP;
                        BP = AX;
                        AX = tempBp;
                        break;
                    case 0x96: // XCHG SI, AX
                        ushort tempSi = SI;
                        SI = AX;
                        AX = tempSi;
                        break;
                    case 0x97: // XCHG DI, AX
                        ushort tempDi = DI;
                        DI = AX;
                        AX = tempDi;
                        break;
                    case 0xEA: // FAR JMP (Absolute far jump)
                        ushort newOffset = Read16(); // read offset
                        ushort newSegment = Read16();

                        IP = newOffset;
                        CS = newSegment;

                        NCLogging.Log($"FAR JUMP to {CS:X4}:{IP:X4}", "Functioning as intended?", ConsoleColor.Blue);
                        break;
                    case 0xF4: // HLT
                        Halt = true; 
                        break;
                    case 0xF5: // CMC
                        CF = !CF;
                        break;
                    case 0xF8: // CLC
                        // cf = 2^11 so we just subtract if it set
                        CF = false;
                        break;
                    case 0xF9: // STC
                        CF = true;
                        break;
                    case 0xFA: // CLI
                        IF = false;
                        break;
                    case 0xFB: // STI
                        IF = true;
                        break;
                    case 0xFC: // CLD
                        DF = false;
                        break;
                    case 0xFD: // STD
                        DF = true;
                        break;
                    default:
                        NCError.ShowErrorBox($"unknown opcode lol (0x{next:X})", 7000, $"CPU8086::Execute - Unknown Opcode encountered (probably not implemented) - (0x{next:X})",
                            NCErrorSeverity.Error, null, true);
                        break;
                }

                RegisterDump();

                // 64kb segment wraparound
                if (IP == ushort.MaxValue) // 0xFFFF
                {
                    IP = 0;
                }
                else
                {
                    IP++;
                }
            }
        }

        private ushort Read16()
        {
            IP++;
            byte byte1 = Emulator.CurMachine.AddressSpace[PC];
            IP++;
            byte byte2 = Emulator.CurMachine.AddressSpace[PC];
            // seems smart to stackalloc this
            return BitConverter.ToUInt16(stackalloc byte[] { byte1, byte2 });
        }
    }
}
