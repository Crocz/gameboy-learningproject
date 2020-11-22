using NUnit.Framework;

namespace GBEmu.Test {

    public class IntegrationTest {
        [Test]
        public void Test1() {
            var testmemory = new Memory();
            var gbCpu = new GbCpu(GbModel.DMG, testmemory);
            for(; ; ) {
                gbCpu.Tick();
            }
        }
    }
}
