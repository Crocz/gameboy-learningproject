using NUnit.Framework;

namespace GBEmu.Test
{
    public class GbCpuTest
    {

        [Test]
        public void Test1()
        {
            var sut = new GbCpu(GbModel.DMG, new Memory());
            sut.Tick();
            sut.Tick();
            sut.Tick();
            sut.Tick();
        }
    }
}