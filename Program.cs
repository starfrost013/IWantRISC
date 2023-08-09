// See https://aka.ms/new-console-template for more information

// IWantRISC
// 8086 emulator

using IWantRISC;

Logger.Init();

Console.WriteLine("I want RISC");

// TEMPORARY
Emulator.Start(new IBM5150(1048576));
