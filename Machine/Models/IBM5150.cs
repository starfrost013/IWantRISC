namespace IWantRISC
{
    /// <summary>
    /// <para>The good old IBM PC. The one that started it all...for better or worse.</para>
    /// 
    /// <para>Intel 8088</para>
    /// <para>64-640KB RAM</para>
    /// <para>8KB bios</para>
    /// <para>64-640KB RAM</para>
    /// <para>64-640KB RAM</para>
    /// </summary>
    internal class IBM5150 : PC
    {
        public IBM5150(int ramSize) : base(new CPU8086(), ramSize)
        {
            Debug.Assert(ramSize >= 65535, "IBM 5150 requires >64kb ram!"); 
        }

        internal override void Boot()
        {

            CPU.Boot();
        }

        internal override void Run()
        {
            throw new NotImplementedException();
        }
    }
}
