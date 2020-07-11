using System;
using System.Collections.Generic;
using System.Linq;

namespace GBEmu {

    public interface IGbCpu {
        void Tick();
    }
    public class GbCpu : IGbCpu {
        private readonly IMemory memory;
        private readonly Queue<Action> steps = new Queue<Action>(8);

        //InstructionSteps
        private Action FetchInstruction;
        private Action NoOperation;
        private Action Stop;
        private Action RelativeJump;
        private Action<ushort, ushort> LoadRegister;
        private Action LoadImmediate;
        private Action LoadAbsoluteAddress;
        private Action LoadH;
        private Action PushStack;
        private Action PopStack;

        private Action Call;

        private Action Add;
        private Action Subtract;
        private Action And;
        private Action Or;
        private Action Return;
        private Action ConditionalReturn;

        private ushort registerA; //accumulator and flags
        private ushort registerB;
        private ushort registerD;
        private ushort registerH;
        private ushort registerF;
        private ushort registerC;
        private ushort registerE;
        private ushort registerL;

        private const int UpperMask = 0xFF00;
        private const int LowerMask = 0x00FF;

        private ushort registerAF {
            get {
                return (ushort)((registerA << 8) + registerF);
            }
            set {
                registerF = (ushort)(value & LowerMask);
                registerA = (ushort)((value >> 8) & LowerMask);
            }
        }
        private ushort registerBC {
            get {
                return (ushort)((registerB << 8) + registerC);
            }
            set {
                registerC = (ushort)(value & LowerMask);
                registerB = (ushort)((value >> 8) & LowerMask);
            }
        }
        private ushort registerDE {
            get {
                return (ushort)((registerD << 8) + registerE);
            }
            set {
                registerE = (ushort)(value & LowerMask);
                registerD = (ushort)((value >> 8) & LowerMask);
            }
        }
        private ushort registerHL {
            get {
                return (ushort)((registerH << 8) + registerL);
            }
            set {
                registerL = (ushort)(value & LowerMask);
                registerH = (ushort)((value >> 8) & LowerMask);
            }
        }


        private ushort stackPointer;
        private ushort programCounter;

        private int workVariable;
        private Instruction fetchedInstruction;

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
            NoOperation = new Action(NoOpImpl);            
        }

        public void Tick() {
            steps.Dequeue().Invoke();
        }

        private void FetchInstructionImpl() {
            fetchedInstruction = (Instruction)memory.GetByte(programCounter);
            var actions = GetActionChain(fetchedInstruction);
            foreach(var action in actions) {
                steps.Enqueue(action);
            }
        }

        private IEnumerable<Action> GetActionChain(Instruction inst) {
            switch (inst) {
                case Instruction.NOP:
                    yield return NoOperation;
                    break;
                case Instruction.LD_A_A:
                    yield return () => registerA = registerA;
                    break;
                case Instruction.LD_A_B:
                    yield return () => registerA = registerB;
                    break;
                case Instruction.LD_A_C:
                    yield return () => registerA = registerC;
                    break;
                case Instruction.LD_A_D:
                    yield return () => registerA = registerD;
                    break;
                case Instruction.LD_A_E:
                    yield return () => registerA = registerE;
                    break;
                case Instruction.LD_A_H:
                    yield return () => registerA = registerH;
                    break;
                case Instruction.LD_A_L:
                    yield return () => registerA = registerL;
                    break;
                case Instruction.LD_B_A:
                    yield return () => registerB = registerA;
                    break;
                case Instruction.LD_B_B:
                    yield return () => registerB = registerB;
                    break;
                case Instruction.LD_B_C:
                    yield return () => registerB = registerC;
                    break;
                case Instruction.LD_B_D:
                    yield return () => registerB = registerD;
                    break;
                case Instruction.LD_B_E:
                    yield return () => registerB = registerE;
                    break;
                case Instruction.LD_B_H:
                    yield return () => registerB = registerH;
                    break;
                case Instruction.LD_B_L:
                    yield return () => registerB = registerL;
                    break;
                case Instruction.LD_C_A:
                    yield return () => registerC = registerA;
                    break;
                case Instruction.LD_C_B:
                    yield return () => registerC = registerB;
                    break;
                case Instruction.LD_C_C:
                    yield return () => registerC = registerC;
                    break;
                case Instruction.LD_C_D:
                    yield return () => registerC = registerD;
                    break;
                case Instruction.LD_C_E:
                    yield return () => registerC = registerE;
                    break;
                case Instruction.LD_C_H:
                    yield return () => registerC = registerH;
                    break;
                case Instruction.LD_C_L:
                    yield return () => registerC = registerL;
                    break;
                case Instruction.LD_D_A:
                    yield return () => registerD = registerA;
                    break;
                case Instruction.LD_D_B:
                    yield return () => registerD = registerB;
                    break;
                case Instruction.LD_D_C:
                    yield return () => registerD = registerC;
                    break;
                case Instruction.LD_D_D:
                    yield return () => registerD = registerD;
                    break;
                case Instruction.LD_D_E:
                    yield return () => registerD = registerE;
                    break;
                case Instruction.LD_D_H:
                    yield return () => registerD = registerH;
                    break;
                case Instruction.LD_D_L:
                    yield return () => registerD = registerL;
                    break;
                case Instruction.LD_E_A:
                    yield return () => registerE = registerA;
                    break;
                case Instruction.LD_E_B:
                    yield return () => registerE = registerB;
                    break;
                case Instruction.LD_E_C:
                    yield return () => registerE = registerC;
                    break;
                case Instruction.LD_E_D:
                    yield return () => registerE = registerD;
                    break;
                case Instruction.LD_E_E:
                    yield return () => registerE = registerE;
                    break;
                case Instruction.LD_E_H:
                    yield return () => registerE = registerH;
                    break;
                case Instruction.LD_E_L:
                    yield return () => registerE = registerL;
                    break;
                case Instruction.LD_H_A:
                    yield return () => registerH = registerA;
                    break;
                case Instruction.LD_H_B:
                    yield return () => registerH = registerB;
                    break;
                case Instruction.LD_H_C:
                    yield return () => registerH = registerC;
                    break;
                case Instruction.LD_H_D:
                    yield return () => registerH = registerD;
                    break;
                case Instruction.LD_H_E:
                    yield return () => registerH = registerE;
                    break;
                case Instruction.LD_H_H:
                    yield return () => registerH = registerH;
                    break;
                case Instruction.LD_H_L:
                    yield return () => registerH = registerL;
                    break;
                case Instruction.LD_L_A:
                    yield return () => registerL = registerA;
                    break;
                case Instruction.LD_L_B:
                    yield return () => registerL = registerB;
                    break;
                case Instruction.LD_L_C:
                    yield return () => registerL = registerC;
                    break;
                case Instruction.LD_L_D:
                    yield return () => registerL = registerD;
                    break;
                case Instruction.LD_L_E:
                    yield return () => registerL = registerE;
                    break;
                case Instruction.LD_L_H:
                    yield return () => registerL = registerH;
                    break;
                case Instruction.LD_L_L:
                    yield return () => registerL = registerL;
                    break;
                default: throw new NotImplementedException();
            }
        }

        private void NoOpImpl() {

        }

    }

}
