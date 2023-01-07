

namespace IWantRISC
{
    internal enum AddressingMode : byte
    {
        Immediate = 0,

        Register = 1,

        Direct = 2,

        IndirectBase = 3,

        IndirectIndex = 4,

        IndirectBasePlusIndex = 5,

        IndirectBasePlusDisplacement = 6,

        IndirectIndexPlusDisplacement = 7,

        IndirectBasePlusIndexPlusDisplacement = 8,
    }
}
