using NUnit.Framework;

namespace GBEmu.Test {

    public class IntegrationTest {
        [Test]
        public void Test1() {
            var testmemory = new Memory();
            var blah = testmemory.ContentsAsString;
            var gbCpu = new GbCpu(GbModel.DMG, testmemory);
            int x = 0;
            for(; ; ) {
                gbCpu.Tick();
                x++;
            }
        }
    }
}
