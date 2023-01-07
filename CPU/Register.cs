namespace IWantRISC
{
    /// <summary>
    /// ModRM register IDs
    /// 
    /// These are dependent on the W bit.
    /// </summary>
    internal enum Register : byte
    {
        AX = 0,

        AL = AX,

        CX = 1,

        CL = CX,

        DX = 2,

        DL = DX,

        BX = 3,

        BL = BX,

        SP = 4,

        AH = SP,

        BP = 5,

        CH = SP,

        SI = 6,

        DH = SP,

        DI = 7,

        BH = DI,
    }
}
