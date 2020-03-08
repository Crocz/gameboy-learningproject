using System;
using System.Collections.Generic;

namespace GBEmu {

    public interface IGbCpu {
        void Tick();
    }
    public class GbCpu : IGbCpu {
        private readonly IMemory memory;
        private readonly Queue<Action> steps = new Queue<Action>(8);

        //InstructionSteps
        private Action FetchInstruction;

        private ushort registerAF; //accumulator and flags
        private ushort registerBC;
        private ushort registerDE;
        private ushort registerHL;
        private ushort stackPointer;
        private ushort programCounter;

        private int workVariable;

        public GbCpu(GbModel model, IMemory memory) {
            this.memory = memory;
            Init(model);
            steps.Enqueue(FetchInstruction);
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

            FetchInstruction = new Action(FetchInstructionImpl);
        }

        public void Tick() {
            steps.Dequeue().Invoke();
        }

        private void FetchInstructionImpl() {
            workVariable = memory.GetByte(programCounter);
            switch (workVariable) {

            }
            steps.Enqueue(FetchInstruction);
        }
    }

}
