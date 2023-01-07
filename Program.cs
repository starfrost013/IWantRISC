// See https://aka.ms/new-console-template for more information

// IWantRISC
// 8086 emulator

using IWantRISC;

NCLogging.Init();

Console.WriteLine("I want RISC");

IBM5150 ibm5150 = new(65536);
ibm5150.Boot();