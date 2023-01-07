// See https://aka.ms/new-console-template for more information

// IWantRISC
// 8086 emulator

using IWantRISC;

NCLogging.Init();

Console.WriteLine("I want RISC");

// TEMPORARY
Emulator.CurMachine = new IBM5150(1048576);
Emulator.CurMachine.Boot();