using System;

namespace GBEmu {

    public interface IGbCpu {
        void Tick();
    }
    public class GbCpu : IGbCpu {
        private readonly IMemory memory;

        ushort registerAF; //accumulator and flags
        ushort registerBC;
        ushort registerDE;
        ushort registerHL;
        ushort stackPointer;
        ushort programCounter;

        public GbCpu(GbModel model, IMemory memory) {
            this.memory = memory;
            Init(model);
        }

        private void Init(GbModel model) {
            if(model != GbModel.DMG) {
                throw new ArgumentOutOfRangeException("Currently only supports DMG model.");
            }
            registerAF = 0x01b0;
            registerBC = 0x0013;
            registerDE = 0x00d8;
            registerHL = 0x014d;
            stackPointer = 0xfffe;
            programCounter = 0x0010;
        }

        public void Tick() {
            throw new NotImplementedException();
        }
    }

}
