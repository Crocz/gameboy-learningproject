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

        private byte registerA; //accumulator and flags
        private byte registerB;
        private byte registerD;
        private byte registerH;
        private byte registerF;
        private byte registerC;
        private byte registerE;
        private byte registerL;

        private const short ZeroFlagMask = 0b_0000_0000_1000_0000;
        private const short AddSubFlagMask = 0b_0000_0000_0100_0000;
        private const short HalfCarryFlagMask = 0b_0000_0000_0010_0000;
        private const short CarryFlagMask = 0b_0000_0000_0001_0000;

        private bool IsZeroFlagSet => (registerF & ZeroFlagMask) == ZeroFlagMask;
        private bool IsCarryFlagSet => (registerF & CarryFlagMask) == CarryFlagMask;

        private ushort registerAF {
            get {
                return Combine(registerA, registerF);
            }
            set {
                registerF = LeastSignificantByte(value);
                registerA = MostSignificantByte(value);
            }
        }
        private ushort registerBC {
            get {
                return Combine(registerB, registerC);
            }
            set {
                registerC = LeastSignificantByte(value);
                registerB = MostSignificantByte(value);
            }
        }
        private ushort registerDE {
            get {
                return Combine(registerD, registerE);
            }
            set {
                registerE = LeastSignificantByte(value);
                registerD = MostSignificantByte(value);
            }
        }
        private ushort registerHL {
            get {
                return Combine(registerH, registerL);
            }
            set {
                registerL = LeastSignificantByte(value);
                registerH = MostSignificantByte(value);
            }
        }
        private ushort Combine(byte upperByte, byte lowerByte) => (ushort)((upperByte << 8) + lowerByte);
        private const int LeastSignificantByteMask = 0x00FF;
        private byte MostSignificantByte(ushort UnsignedShort) => (byte)((UnsignedShort >> 8) & LeastSignificantByteMask);
        private byte LeastSignificantByte(ushort UnsignedShort) => (byte)(UnsignedShort & LeastSignificantByteMask);


        private ushort stackPointer;
        private ushort programCounter;

        private int workVariable;
        private int workVariable2;
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
        }

        public void Tick() {
            steps.Dequeue().Invoke();
        }

        private void FetchInstructionImpl() {
            fetchedInstruction = (Instruction)memory.ReadByte(programCounter++);
            var actions = GetActionChain(fetchedInstruction);
            foreach(var action in actions) {
                steps.Enqueue(action);
            }
            steps.Enqueue(FetchInstruction);
        }

        private IEnumerable<Action> GetActionChain(Instruction inst) {
            switch (inst) {
                case Instruction.NOP:
                    yield return () => { };
                    break;
#pragma warning disable 1717
                case Instruction.LD_A_A:
                    yield return () => registerA = registerA;
                    break;
#pragma warning restore 1717
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
#pragma warning disable 1717
                case Instruction.LD_B_B:
                    yield return () => registerB = registerB;
                    break;
#pragma warning restore 1717
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
#pragma warning disable 1717
                case Instruction.LD_C_C:
                    yield return () => registerC = registerC;
                    break;
#pragma warning restore 1717
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
#pragma warning disable 1717
                case Instruction.LD_D_D:
                    yield return () => registerD = registerD;
                    break;
#pragma warning restore 1717
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
#pragma warning disable 1717
                case Instruction.LD_E_E:
                    yield return () => registerE = registerE;
                    break;
#pragma warning restore 1717
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
#pragma warning disable 1717
                case Instruction.LD_H_H:
                    yield return () => registerH = registerH;
                    break;
#pragma warning restore 1717
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
#pragma warning disable 1717
                case Instruction.LD_L_L:
                    yield return () => registerL = registerL;
                    break;
#pragma warning restore 1717
                case Instruction.LD_A_d8:
                    yield return () => workVariable = memory.ReadByte(programCounter++);
                    yield return () => registerA = (byte)workVariable;
                    break;
                case Instruction.LD_B_d8:
                    yield return () => workVariable = memory.ReadByte(programCounter++);
                    yield return () => registerB = (byte)workVariable;
                    break;
                case Instruction.LD_C_d8:
                    yield return () => workVariable = memory.ReadByte(programCounter++);
                    yield return () => registerC = (byte)workVariable;
                    break;
                case Instruction.LD_D_d8:
                    yield return () => workVariable = memory.ReadByte(programCounter++);
                    yield return () => registerD = (byte)workVariable;
                    break;
                case Instruction.LD_E_d8:
                    yield return () => workVariable = memory.ReadByte(programCounter++);
                    yield return () => registerE = (byte)workVariable;
                    break;
                case Instruction.LD_H_d8:
                    yield return () => workVariable = memory.ReadByte(programCounter++);
                    yield return () => registerH = (byte)workVariable;
                    break;
                case Instruction.LD_L_d8:
                    yield return () => workVariable = memory.ReadByte(programCounter++);
                    yield return () => registerL = (byte)workVariable;
                    break;
                case Instruction.LD_A_pHL:
                    yield return () => workVariable = memory.ReadByte(registerHL);
                    yield return () => registerA = (byte)workVariable;
                    break;
                case Instruction.LD_B_pHL:
                    yield return () => workVariable = memory.ReadByte(registerHL);
                    yield return () => registerB = (byte)workVariable;
                    break;
                case Instruction.LD_C_pHL:
                    yield return () => workVariable = memory.ReadByte(registerHL);
                    yield return () => registerC = (byte)workVariable;
                    break;
                case Instruction.LD_D_pHL:
                    yield return () => workVariable = memory.ReadByte(registerHL);
                    yield return () => registerD = (byte)workVariable;
                    break;
                case Instruction.LD_E_pHL:
                    yield return () => workVariable = memory.ReadByte(registerHL);
                    yield return () => registerE = (byte)workVariable;
                    break;
                case Instruction.LD_H_pHL:
                    yield return () => workVariable = memory.ReadByte(registerHL);
                    yield return () => registerH = (byte)workVariable;
                    break;
                case Instruction.LD_L_pHL:
                    yield return () => workVariable = memory.ReadByte(registerHL);
                    yield return () => registerL = (byte)workVariable;
                    break;
                case Instruction.LD_pHL_A:
                    yield return () => workVariable = registerA;
                    yield return () => memory.WriteByte(registerHL, (byte)workVariable);
                    break;
                case Instruction.LD_pHL_B:
                    yield return () => workVariable = registerB;
                    yield return () => memory.WriteByte(registerHL, (byte)workVariable);
                    break;
                case Instruction.LD_pHL_C:
                    yield return () => workVariable = registerC;
                    yield return () => memory.WriteByte(registerHL, (byte)workVariable);
                    break;
                case Instruction.LD_pHL_D:
                    yield return () => workVariable = registerD;
                    yield return () => memory.WriteByte(registerHL, (byte)workVariable);
                    break;
                case Instruction.LD_pHL_E:
                    yield return () => workVariable = registerE;
                    yield return () => memory.WriteByte(registerHL, (byte)workVariable);
                    break;
                case Instruction.LD_pHL_H:
                    yield return () => workVariable = registerH;
                    yield return () => memory.WriteByte(registerHL, (byte)workVariable);
                    break;
                case Instruction.LD_pHL_L:
                    yield return () => workVariable = registerL;
                    yield return () => memory.WriteByte(registerHL, (byte)workVariable);
                    break;
                case Instruction.LD_A_pHLminus:
                    yield return () => registerA = memory.ReadByte(registerHL);
                    yield return () => registerHL--;
                    break;
                case Instruction.LD_A_pHLplus:
                    yield return () => registerA = memory.ReadByte(registerHL);
                    yield return () => registerHL++;
                    break;
                case Instruction.LD_pHLminus_A:
                    yield return () => memory.WriteByte(registerHL, registerA);
                    yield return () => registerHL--;
                    break;
                case Instruction.LD_pHLplus_A:
                    yield return () => memory.WriteByte(registerHL, registerA);
                    yield return () => registerHL++;
                    break;
                case Instruction.LD_pHL_d8:
                    yield return () => workVariable = memory.ReadByte(programCounter++);
                    yield return () => memory.WriteByte(registerHL, (byte)workVariable);
                    break;
                case Instruction.LD_A_pBC:
                    yield return () => workVariable = memory.ReadByte(registerBC);
                    yield return () => registerA = (byte)workVariable;
                    break;
                case Instruction.LD_A_pDE:
                    yield return () => workVariable = memory.ReadByte(registerDE);
                    yield return () => registerA = (byte)workVariable;
                    break;
                case Instruction.LD_pBC_A:
                    yield return () => workVariable = registerA;
                    yield return () => memory.WriteByte(registerBC, (byte)workVariable);
                    break;
                case Instruction.LD_pDE_A:
                    yield return () => workVariable = registerA;
                    yield return () => memory.WriteByte(registerDE, (byte)workVariable);
                    break;
                case Instruction.LD_A_pa16:
                    yield return () => workVariable = memory.ReadByte(programCounter++);
                    yield return () => workVariable += memory.ReadByte(programCounter++) << 8;
                    yield return () => workVariable = memory.ReadByte((ushort)workVariable);
                    yield return () => registerA = (byte)workVariable;
                    break;
                case Instruction.LD_pa16_A:
                    yield return () => workVariable = memory.ReadByte(programCounter++);
                    yield return () => workVariable += memory.ReadByte(programCounter++) << 8;
                    yield return () => workVariable2 = registerA;
                    yield return () => memory.WriteByte((ushort)workVariable, (byte)workVariable2);
                    break;
                case Instruction.LD_A_pC:
                    yield return () => workVariable = memory.ReadByte((ushort)((0xFF << 8) + registerC));
                    yield return () => registerA = (byte)workVariable;
                    break;
                case Instruction.LD_pC_A:
                    yield return () => workVariable = registerA;
                    yield return () => memory.WriteByte((ushort)((0xFF << 8) + registerC), (byte)workVariable);
                    break;
                case Instruction.LDH_A_pa8:
                    yield return () => workVariable = memory.ReadByte(programCounter++);
                    yield return () => workVariable = memory.ReadByte((ushort)((0xFF << 8) + workVariable));
                    yield return () => registerA = (byte)workVariable;
                    break;
                case Instruction.LDH_pa8_A:
                    yield return () => workVariable = memory.ReadByte(programCounter++);
                    yield return () => workVariable2 = registerA;
                    yield return () => memory.WriteByte((ushort)((0xFF << 8) + workVariable), (byte)workVariable2);
                    break;
                //case Instruction.LD_DE_d16:
                //    yield return () => registerD = memory.ReadByte(programCounter++);
                //    yield return () => registerE = memory.ReadByte(programCounter++);
                //    break;
                case Instruction.INC_A:
                    yield return () => registerA = IncrementRegister(registerA);
                    break;
                case Instruction.INC_B:
                    yield return () => registerB = IncrementRegister(registerB);
                    break;
                case Instruction.INC_C:
                    yield return () => registerC = IncrementRegister(registerC);
                    break;
                case Instruction.INC_D:
                    yield return () => registerD = IncrementRegister(registerD);
                    break;
                case Instruction.INC_E:
                    yield return () => registerE = IncrementRegister(registerE);
                    break;
                case Instruction.INC_H:
                    yield return () => registerH = IncrementRegister(registerH);
                    break;
                case Instruction.INC_L:
                    yield return () => registerL = IncrementRegister(registerL);
                    break;
                case Instruction.DEC_A:
                    yield return () => registerA = DecrementRegister(registerA);
                    break;
                case Instruction.DEC_B:
                    yield return () => registerB = DecrementRegister(registerB);
                    break;
                case Instruction.DEC_C:
                    yield return () => registerC = DecrementRegister(registerC);
                    break;
                case Instruction.DEC_D:
                    yield return () => registerD = DecrementRegister(registerD);
                    break;
                case Instruction.DEC_E:
                    yield return () => registerE = DecrementRegister(registerE);
                    break;
                case Instruction.DEC_H:
                    yield return () => registerH = DecrementRegister(registerH);
                    break;
                case Instruction.DEC_L:
                    yield return () => registerL = DecrementRegister(registerL);
                    break;
                case Instruction.LD_BC_d16:
                case Instruction.LD_DE_d16:
                case Instruction.LD_HL_d16:
                case Instruction.LD_SP_d16:
                    foreach(var action in LD_rr_nn(inst)) {
                        yield return action;
                    }
                    break;
                case Instruction.CALL_a16:
                    foreach (var action in Call_nn()) {
                        yield return action;
                    }
                    break;

                case Instruction.CALL_C_a16:
                case Instruction.CALL_NC_a16:
                case Instruction.CALL_Z_a16:
                case Instruction.CALL_NZ_a16:
                

                default: throw new NotImplementedException($"Instruction: {inst}");
            }
        }

        private IEnumerable<Action> Call_nn() {
            foreach (var step in Read16Bit()) {
                yield return step;
            }
            //waste a cycle?
            foreach (var step in WritePCToStack()) {
                yield return step;
            }
            yield return () => programCounter = (ushort)workVariable;
        }
        private IEnumerable<Action> Call_cc_nn(Instruction instruction) {
            foreach (var step in Read16Bit()) {
                yield return step;
            }
            if (EvaluateCondition(instruction)) {
                foreach (var step in WritePCToStack()) {
                    yield return step;
                }
                yield return () => programCounter = (ushort)workVariable;
            } else {
                //waste a cycle?
            }
        }


        private IEnumerable<Action> LD_rr_nn(Instruction instruction) {
            foreach(var step in Read16Bit()) {
                yield return step;
            }
            switch (instruction) {
                case Instruction.LD_HL_d16:
                    yield return () => registerHL = (ushort)workVariable;
                    break;
            }
        }

        private IEnumerable<Action> Read16Bit() {
            yield return () => workVariable = memory.ReadByte(programCounter++);
            yield return () => workVariable = workVariable + memory.ReadByte(programCounter++) << 8;
        }

        private IEnumerable<Action> WritePCToStack() {
            yield return () => memory.WriteByte(--stackPointer, LeastSignificantByte(programCounter));
            yield return () => memory.WriteByte(--stackPointer, MostSignificantByte(programCounter));
        }

        private bool EvaluateCondition(Instruction instruction) {
            switch (instruction) {
                case Instruction.CALL_C_a16: return IsCarryFlagSet;
                case Instruction.CALL_NC_a16: return !IsCarryFlagSet;
                case Instruction.CALL_Z_a16: return IsZeroFlagSet;
                case Instruction.CALL_NZ_a16: return !IsZeroFlagSet;
                default: throw new ArgumentException($"Wrong instruction '{instruction}'.");
            }
        }

        private byte IncrementRegister(byte register) {
            workVariable = register + 1;
            if(workVariable == 0) {
                registerF = (byte)(registerF | ZeroFlagMask);
            } else {
                registerF = (byte)(registerF & ~ZeroFlagMask); //not sure if should be cleared, but seems reasonable.
            }
            registerF = (byte)(registerF & ~HalfCarryFlagMask); // set to zero on additions/increments
            //something with carryflagmask too
            return (byte)workVariable;
        }

        private byte DecrementRegister(byte register) {
            workVariable = register - 1;
            if (workVariable == 0) {
                registerF = (byte)(registerF | ZeroFlagMask);
            } else {
                registerF = (byte)(registerF & ~ZeroFlagMask); //not sure if should be cleared, but seems reasonable.
            }
            registerF = (byte)(registerF | HalfCarryFlagMask); // set to one on subtractions/decrements
            //something with carryflagmask too
            return (byte)workVariable;
        }
    }

}
