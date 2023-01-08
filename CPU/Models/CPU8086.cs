using System.Reflection.PortableExecutable;
using System.Xml.Schema;

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
                    case 0x06: // PUSH ES
                        SP -= 2;
                        Write16(StackTop, ES);
                        break;
                    case 0x07: // POP ES
                        ES = Pop();
                        break;
                    case 0x0E: // PUSH CS
                        SP -= 2;
                        Write16(StackTop, CS);
                        break;
                    case 0x0F: // POP CS (useless but maybe people want to use it)
                        CS = Pop();
                        break;
                    case 0x16: // PUSH SS
                        SP -= 2;
                        Write16(StackTop, SS);
                        break;
                    case 0x17: // POP SS
                        SS = Pop();
                        break;
                    case 0x1E: // PUSH DS
                        SP -= 2;
                        Write16(StackTop, DS);
                        break;
                    case 0x1F: // POP DS
                        DS = Pop();
                        break;
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
                    case 0x50: // PUSH AX
                        SP -= 2;
                        Write16(StackTop, AX); 
                        break;
                    case 0x51: // PUSH CX
                        SP -= 2;
                        Write16(StackTop, CX);
                        break;
                    case 0x52: // PUSH DX
                        SP -= 2;
                        Write16(StackTop, DX);
                        break;
                    case 0x53: // PUSH BX
                        SP -= 2;
                        Write16(StackTop, BX);
                        break;
                    case 0x54: // PUSH SP
                        SP -= 2;
                        Write16(StackTop, SP);
                        break;
                    case 0x55: // PUSH BP
                        SP -= 2;
                        Write16(StackTop, BP);
                        break;
                    case 0x56: // PUSH SI
                        SP -= 2;
                        Write16(StackTop, SI);
                        break;
                    case 0x57: // PUSH DI
                        SP -= 2;
                        Write16(StackTop, DI);
                        break;
                    case 0x58: // POP AX
                        AX = Pop();
                        break;
                    case 0x59: // POP CX
                        CX = Pop();
                        break;
                    case 0x5A: // POP DX
                        DX = Pop();
                        break;
                    case 0x5B: // POP BX
                        BX = Pop();
                        break;
                    case 0x5C: // POP SP
                        SP = Pop();
                        break;
                    case 0x5D: // POP BP
                        BP = Pop();
                        break;
                    case 0x5E: // POP SI
                        SI = Pop();
                        break;
                    case 0x5F: // POP DI
                        DI = Pop(); 
                        break;
                    case 0x90: // NOP (16-bit)
                        // Technically this is XCHG AX, AX. 
                        // Do any apps depend on this behaviour? I can imagine them doing so...x86...ugh
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
                        // continue because we don't want IP to increase
                        continue; 
                    case 0xF4: // HLT
                        Halt = true;
                        NCLogging.Log("CPU HALT", "It might have crashed", ConsoleColor.Red);
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
                        NCLogging.Log("Interrupts disabled", ConsoleColor.Blue);
                        IF = false;
                        break;
                    case 0xFB: // STI
                        NCLogging.Log("Interrupts enabled", ConsoleColor.Blue);
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

                // 64kb segment wraparound
                if (IP == ushort.MaxValue) // 0xFFFF
                {
                    IP = 0;
                }
                else
                {
                    IP++;
                }

                RegisterDump();

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

        private ushort Pop()
        {
            byte byte1 = Emulator.CurMachine.AddressSpace[StackTop];
            byte byte2 = Emulator.CurMachine.AddressSpace[StackTop+1];

            // INCREMENT sp by 2 AFTER reading
            SP += 2;

            // seems smart to stackalloc this
            return BitConverter.ToUInt16(stackalloc byte[] { byte1, byte2 });
        }

        private void Write16(int position, ushort value)
        {
            // little endian,
            // so write in reverse order.
            Emulator.CurMachine.AddressSpace[position] = (byte)(value & 0xFF); // LSB
            Emulator.CurMachine.AddressSpace[position] = (byte)((value >> 8) & 0xFF); // MSB
        }
    }
}
