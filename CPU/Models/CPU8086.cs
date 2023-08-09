
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
            Logger.Log("808x RESET", "Hope it boots this time", ConsoleColor.Blue);

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
                byte next = Emulator.CurrentMachine.AddressSpace[PC];

                // Reset the segment override prefix to its default value (DS)
                ushort OS = DS;

                // reset the repeat prefix
                int repeatState = 0;

                // Handle prefix override.
                switch (next)
                {
                    case 0x26: // ES prefix override.
                        OS = ES;
                        IP++;
                        break;
                    case 0x2E: // CS prefix override.
                        OS = CS;
                        IP++;
                        break;
                    case 0x36: // SS prefix override.
                        OS = SS;
                        IP++;
                        break;
                    case 0x3E: // DS prefix override.
                        OS = DS;
                        IP++;
                        break;
                    case 0xF2: // REPNE/REPNZ (non-zer0)
                        repeatState = 2;
                        IP++;
                        break;
                    case 0xF3: // REP/REPE/REPZ (zero)
                        repeatState = 1;
                        IP++;
                        break;

                }

                // Handle the rest of the instructions.
                switch (next)
                {
                    case 0x06: // PUSH ES
                        SP -= 2;
                        Write16(StackTop, ES);
                        IP++;
                        break;
                    case 0x07: // POP ES
                        ES = Pop();
                        IP++;
                        break;
                    case 0x0E: // PUSH CS
                        SP -= 2;
                        Write16(StackTop, CS);
                        IP++;
                        break;
                    case 0x0F: // POP CS
                        
                        CS = Pop();
                        IP++;
                        break;
                    case 0x16: // PUSH SS
                        SP -= 2;
                        Write16(StackTop, SS);
                        IP++;
                        break;
                    case 0x17: // POP SS
                        SS = Pop();
                        IP++;
                        break;
                    case 0x1E: // PUSH DS
                        SP -= 2;
                        Write16(StackTop, DS);
                        IP++;
                        break;
                    case 0x1F: // POP DS
                        DS = Pop();
                        IP++;
                        break;
                    case 0x40: // INC AX
                        AX++;
                        IP++;
                        break;
                    case 0x41: // INC CX
                        CX++;
                        IP++;
                        break; 
                    case 0x42: // INC DX
                        DX++;
                        IP++;
                        break;
                    case 0x43: // INC BX
                        BX++;
                        IP++;
                        break;
                    case 0x44: // INC SP
                        SP++;
                        IP++;
                        break;
                    case 0x45: // INC BP
                        BP++;
                        IP++;
                        break;
                    case 0x46: // INC SI
                        SI++;
                        IP++;
                        break;
                    case 0x47: // INC DI
                        DI++;
                        IP++;
                        break;
                    case 0x48: // DEC AX
                        AX--;
                        IP++;
                        break;
                    case 0x49: // DEC CX
                        CX--;
                        IP++;
                        break;
                    case 0x4A: // DEC DX
                        DX--;
                        IP++;
                        break;
                    case 0x4B: // DEC BX
                        BX--;
                        IP++;
                        break;
                    case 0x4C: // DEC SP
                        SP--;
                        IP++;
                        break;
                    case 0x4D: // DEC BP
                        BP--;
                        IP++;
                        break;
                    case 0x4E: // DEC SI
                        SI--;
                        IP++;
                        break;
                    case 0x4F: // DEC DI
                        DI--;
                        IP++;
                        break;
                    case 0x50: // PUSH AX
                        SP -= 2;
                        Write16(StackTop, AX);
                        IP++;
                        break;
                    case 0x51: // PUSH CX
                        SP -= 2;
                        Write16(StackTop, CX);
                        IP++;
                        break;
                    case 0x52: // PUSH DX
                        SP -= 2;
                        Write16(StackTop, DX);
                        IP++;
                        break;
                    case 0x53: // PUSH BX
                        SP -= 2;
                        Write16(StackTop, BX);
                        IP++;
                        break;
                    case 0x54: // PUSH SP
                        SP -= 2;
                        Write16(StackTop, SP);
                        IP++;
                        break;
                    case 0x55: // PUSH BP
                        SP -= 2;
                        Write16(StackTop, BP);
                        IP++;
                        break;
                    case 0x56: // PUSH SI
                        SP -= 2;
                        Write16(StackTop, SI);
                        IP++;
                        break;
                    case 0x57: // PUSH DI
                        SP -= 2;
                        Write16(StackTop, DI);
                        IP++;
                        break;
                    case 0x58: // POP AX
                        AX = Pop();
                        IP++;
                        break;
                    case 0x59: // POP CX
                        CX = Pop();
                        IP++;
                        break;
                    case 0x5A: // POP DX
                        DX = Pop();
                        IP++;
                        break;
                    case 0x5B: // POP BX
                        BX = Pop();
                        IP++;
                        break;
                    case 0x5C: // POP SP
                        SP = Pop();
                        IP++;
                        break;
                    case 0x5D: // POP BP
                        BP = Pop();
                        IP++;
                        break;
                    case 0x5E: // POP SI
                        SI = Pop();
                        IP++;
                        break;
                    case 0x5F: // POP DI
                        DI = Pop();
                        IP++;
                        break;
                    case 0x72: // JNBE (Jump short if not below or equal)
                        if (CF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2;
                        break;
                    case 0x73: // JNB (Jump short if not below or equal)
                        if (!CF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2; 
                        break;
                    case 0x74: // JZ (Jump if zero)
                        if (ZF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2; 
                        break;
                    case 0x75: // JNE (Jump short if not equal) / JNZ 
                        if (!ZF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2;
                        break;
                    case 0x76: // JBE (Jump short if below or equal) 
                        if (CF || ZF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2; 
                        break;
                    case 0x77: // JZ (Jump short if zero)
                        if (!CF && !ZF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2;
                        break;
                    case 0x78: // JS (Jump short if sign)
                        if (SF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2;
                        break;
                    case 0x79: // JNS (Jump short if not sign)
                        if (!SF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2;
                        break;
                    case 0x7A: // JPE (Jump short if parity even)
                        if (PF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2;
                        break;
                    case 0x7B: // JPO (Jump short if parity odd)
                        if (!PF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2;
                        break;
                    case 0x7C: // JPE (Jump short if parity even)
                        if (SF != OF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2;
                        break;
                    case 0x7D: // JNE (Jump short if not less)
                        if (SF == OF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2;
                        break;
                    case 0x7E: // JNE (Jump short if less or equal)
                        if (ZF || (SF != OF)) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2;
                        break;
                    case 0x7F: // JNLE (Jump short if not less or equal)
                        if (!ZF && SF == OF) IP = (ushort)(IP + (sbyte)Read8());
                        IP += 2;
                        break;
                    case 0x90: // NOP (16-bit)
                        // Technically this is XCHG AX, AX. 
                        // Do any apps depend on this behaviour? I can imagine them doing so...x86...ugh
                        IP++;
                        break;
                    case 0x91: // XCHG CX, AX
                        ushort tempCx = CX;
                        CX = AX;
                        AX = tempCx;
                        IP++;
                        break;
                    case 0x92: // XCHG DX, AX
                        ushort tempDx = DX;
                        DX = AX;
                        AX = tempDx;
                        IP++;
                        break;
                    case 0x93: // XCHG BX, AX
                        ushort tempBx = BX;
                        BX = AX;
                        AX = tempBx;
                        IP++;
                        break;
                    case 0x94: // XCHG SP, AX
                        ushort tempSp = SP;
                        SP = AX; 
                        AX = tempSp;
                        IP++;
                        break;
                    case 0x95: // XCHG BP, AX
                        ushort tempBp = BP;
                        BP = AX;
                        AX = tempBp;
                        IP++;
                        break;
                    case 0x96: // XCHG SI, AX
                        ushort tempSi = SI;
                        SI = AX;
                        AX = tempSi;
                        IP++;
                        break;
                    case 0x97: // XCHG DI, AX
                        ushort tempDi = DI;
                        DI = AX;
                        AX = tempDi;
                        IP++;
                        break;
                    case 0x9E: // SAHF
                        // per 808x manual:
                        // bit7 = SF
                        // bit6 = ZF
                        // bit4 = AF
                        // bit2 = PF
                        // bit0 = CF
                        SF = ((AH >> 7) & 1) == 1;
                        ZF = ((AH >> 6) & 1) == 1;
                        AF = ((AH >> 4) & 1) == 1;
                        PF = ((AH >> 2) & 1) == 1;
                        CF = (AH & 1) == 1;
                        IP++;
                        break;
                    case 0x9F: // LAHF
                        // bit0 = CF
                        // bit1 = 1
                        // bit2 = PF
                        // bit3 = 0
                        // bit4 = AF
                        // bit5 = 0
                        // bit6 = ZF
                        // bit7 = SF
                        AX &= 0x00FF; // mask off AH
                        // for some reason no cast lol
                        AX |= Convert.ToUInt16(CF);
                        AX |= 1 << 1; 
                        if (PF) AX |= 1 << 2;
                        if (AF) AX |= 1 << 4;
                        if (ZF) AX |= 1 << 6;
                        if (SF) AX |= 1 << 7;
                        AX <<= 8;
                        IP++;
                        break;  
                    case 0xA0: // MOV AL, (8-bit signed offset from current overridden segment)
                        ushort alOffset = Read16();
                        AX |= (ushort)((Emulator.CurrentMachine.AddressSpace[(OS + alOffset) % ushort.MaxValue]) & 0xFF);
                        IP += 3;
                        break;
                    case 0xB0: // MOV AL, (8-bit immediate) 
                        AX = (ushort)((AX & 0xFF00) + Read8());
                        IP += 2; 
                        break;
                    case 0xB1: // MOV CL, (8-bit immediate)
                        CX = (ushort)((CX & 0xFF00) + Read8());
                        IP += 2; 
                        break;
                    case 0xB2: // MOV DL, (8-bit immediate)
                        DX = (ushort)((DX & 0xFF00) + Read8());
                        IP += 2; 
                        break;
                    case 0xB3: // MOV BL, (8-bit immediate)
                        BX = (ushort)((BX & 0xFF00) + Read8());
                        IP += 2;
                        break;
                    case 0xB4: // MOV AH, (8-bit immediate) 
                        // stupid way of setting AH
                        AX = (ushort)((Read8() << 8) | (AX & 0x00FF));
                        IP += 2; 
                        break;
                    case 0xB5: // MOV CH, (8-bit immediate) 
                        CX = (ushort)((Read8() << 8) | (CX & 0x00FF));
                        IP += 2; 
                        break;
                    case 0xB6: // MOV DH, (8-bit immediate) 
                        DX = (ushort)((Read8() << 8) | (AX & 0x00FF));
                        IP += 2; 
                        break;
                    case 0xB7: // MOV BH, (8-bit immediate) 
                        BX = (ushort)((Read8() << 8) | (BX & 0x00FF));
                        IP += 2; 
                        break;
                    case 0xB8: // MOV AX, (16-bit immediate) 
                        AX = Read16();
                        IP += 3;
                        break;
                    case 0xB9: // MOV CX, (16-bit immediate) 
                        CX = Read16();
                        IP += 3;
                        break;
                    case 0xBA: // MOV DX, (16-bit immediate) 
                        DX = Read16();
                        IP += 3;
                        break;
                    case 0xBB: // MOV BX, (16-bit immediate) 
                        BX = Read16();
                        IP += 3;
                        break;
                    case 0xBC: // MOV SP, (16-bit immediate) 
                        SP = Read16();
                        IP += 3;
                        break;
                    case 0xBD: // MOV BP, (16-bit immediate) 
                        BP = Read16();
                        IP += 3;
                        break;
                    case 0xBE: // MOV SI, (16-bit immediate) 
                        SI = Read16();
                        IP += 3;
                        break;
                    case 0xBF: // MOV DI, (16-bit immediate) 
                        DI = Read16();
                        IP += 3;
                        break;
                    case 0xEA: // FAR JMP (Absolute far jump)
                        ushort newOffset = Read16(); // read offset
                        IP += 2; 
                        ushort newSegment = Read16();
                        IP += 2; 

                        IP = newOffset;
                        CS = newSegment;

                        Logger.Log($"FAR JUMP to {CS:X4}:{IP:X4}", "Functioning as intended?", ConsoleColor.Blue);
                        // continue because we don't want IP to increase
                        continue; 
                    case 0xF4: // HLT
                        Halt = true;
                        Logger.Log("CPU HALT", "It might have crashed", ConsoleColor.Red);
                        break;
                    case 0xF5: // CMC
                        CF = !CF;
                        IP++;
                        break;
                    case 0xF8: // CLC
                        CF = false;
                        IP++;
                        break;
                    case 0xF9: // STC
                        CF = true;
                        IP++;
                        break;
                    case 0xFA: // CLI
                        Logger.Log("Interrupts disabled", ConsoleColor.Blue);
                        IF = false;
                        IP++;
                        break;
                    case 0xFB: // STI
                        Logger.Log("Interrupts enabled", ConsoleColor.Blue);
                        IF = true;
                        IP++;
                        break;
                    case 0xFC: // CLD
                        DF = false;
                        IP++;
                        break;
                    case 0xFD: // STD
                        DF = true;
                        IP++;
                        break;
                    default:
                        Logger.LogError($"Unknown Opcode (0x{next:X}) - likely unimplemented", 7000, LoggerSeverity.Error, null, true);
                        break;
                }

                // Enforce segment wraparound.
                IP %= 0xFFFF;

                RegisterDump();

                // Trap Flag - single step mode
                if (TF) Console.ReadLine();
            }
        }


        internal override void RegisterDump()
        {
            Logger.Log($"\n\nGeneral: AX={AX:X4} BX={BX:X4} CX={CX:X4} DX={DX:X4}\n" +
                $"Index and pointer: SP={SP:X4} BP={BP:X4} SI={SI:X4} DI={DI:X4}\n" +
                $"Segment: CS={CS:X4} IP={IP:X4} Absolute PC={CurPC:X5} DS={DS:X4} ES={ES:X4}\n" +
                $"Stack: SS={SS:X4} SP={SP:X4} Absolute SP=0x{StackTop:X5})\n" +
                $"Flags: Overflow={OF} Direction={DF} Interrupt enable={IF} Trap flag={TF} Sign={SF}\n" +
                $"Zero={ZF} Aux carry={AF} Even parity={PF} Carry={CF}\n\nLAST OPCODE={Emulator.CurrentMachine.AddressSpace[PC]:X}\n");
        }

        private byte Read8()
        {
            return Emulator.CurrentMachine.AddressSpace[PC+1];
        }

        private ushort Read16()
        {
            byte byte1 = Emulator.CurrentMachine.AddressSpace[PC+1];
            byte byte2 = Emulator.CurrentMachine.AddressSpace[PC+2];
            // seems smart to stackalloc this
            return BitConverter.ToUInt16(stackalloc byte[] { byte1, byte2 });
        }

        private ushort Pop()
        {
            byte byte1 = Emulator.CurrentMachine.AddressSpace[StackTop];
            byte byte2 = Emulator.CurrentMachine.AddressSpace[StackTop+1];

            // INCREMENT sp by 2 AFTER reading
            SP += 2;

            // seems smart to stackalloc this
            return BitConverter.ToUInt16(stackalloc byte[] { byte1, byte2 });
        }

        private void Write16(int position, ushort value)
        {
            // little endian,
            // so write in reverse order.
            Emulator.CurrentMachine.AddressSpace[position] = (byte)(value & 0xFF); // LSB
            Emulator.CurrentMachine.AddressSpace[position] = (byte)((value >> 8) & 0xFF); // MSB
        }
    }
}
