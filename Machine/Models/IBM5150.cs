namespace IWantRISC
{
    /// <summary>
    /// <para>The good old IBM PC. The one that started it all...for better or worse.</para>
    /// 
    /// <para>Intel 8088</para>
    /// <para>16-640KB RAM</para>
    /// <para>8KB bios</para>
    /// <para>8253 PIC</para>
    /// <para>MDA or CGA card</para>
    /// 
    /// etc.
    /// </summary>
    internal class IBM5150 : Machine
    {
        public IBM5150(uint ramSize = 16384) : base(new CPU8086(), 1048576, ramSize)
        {
            Debug.Assert(ramSize >= 16384, "IBM 5150 requires >64kb ram!"); 
        }

        internal override void Start()
        {
            Logger.Log("Loading BIOS", "IBM 5150");

            byte[] bios = File.ReadAllBytes("Content\\BIOS\\Machine\\BIOS_IBM5150_27OCT82_1501476_U33.BIN");

            // check bios
            Debug.Assert(bios.Length == 8192, "wrong size to be a 5150 bios what are you doing");

            for (int curByte = 0xFE000; curByte <= 0xFFFFF; curByte++)
            {
                AddressSpace[curByte] = bios[curByte & 0x1FFF];
            }

            Logger.Log("Loaded BIOS!", "IBM 5150", ConsoleColor.Green);

            CPU.Boot();
        }
    }
}
