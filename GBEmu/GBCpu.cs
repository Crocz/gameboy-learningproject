using System;
using System.Collections.Generic;

namespace GBEmu {

    public interface IGbCpu {
        void Tick();
    }
    public class GbCpu : IGbCpu {
        private readonly IMemory memory;
        private int NumberOfCycles;

        private byte registerA; //accumulator and flags
        private byte registerB;
        private byte registerD;
        private byte registerH;
        private byte registerF;
        private byte registerC;
        private byte registerE;
        private byte registerL;

        private void SetFlag(CpuFlags flag, bool newValue) {
            byte mask = GetMask(flag);
            registerF = newValue ? (byte)(registerF | mask) : (byte)(registerF & ~mask);
        }

        private bool IsFlagSet(CpuFlags flag) {
            byte mask = GetMask(flag);
            return (registerF & mask) == mask;
        }

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
        private const byte LeastSignificantBitMask = 0b_0000_0001;
        private byte MostSignificantByte(ushort UnsignedShort) => (byte)((UnsignedShort >> 8) & LeastSignificantByteMask);
        private byte LeastSignificantByte(ushort UnsignedShort) => (byte)(UnsignedShort & LeastSignificantByteMask);
        private bool IsMostSignificantBitSet(byte b) => IsLeastSignificantBitSet((byte)(b >> 7));
        private bool IsLeastSignificantBitSet(byte b) => (b & LeastSignificantBitMask) == LeastSignificantBitMask;


        private ushort stackPointer;

        private string ProgramCounter => $"${programCounter.ToString("X4")}";

        private const int Increment = 1;
        private const int Decrement = -1;

        private ushort programCounter;

        private int workVariable;
        private int workVariable2;

        public GbCpu(GbModel model, IMemory memory) {
            this.memory = memory;
            Init(model);
            NumberOfCycles = 0;
        }

        private void Init(GbModel model) {
            if (model != GbModel.DMG) {
                throw new ArgumentOutOfRangeException("Currently only supports DMG model.");
            }
            registerAF = 0x01b0;
            registerBC = 0x0013;
            registerDE = 0x00d8;
            registerHL = 0x014d;
            stackPointer = 0xfffe;
            programCounter = 0x0000;
        }

        public void Tick() {
            if(--NumberOfCycles > 0) {
                return;
            }
            Instruction instruction = (Instruction)memory.ReadByte(programCounter++);
            NumberOfCycles = ExecuteInstruction(instruction);
        }

        private int ExecuteInstruction(Instruction instruction) {
            int cycles = 0;
            switch (inst) {
                case Instruction.NOP:
                    return 4;
                case Instruction.PREFIX_CB:
                    return ExecuteCbInstruction((CBInstruction)memory.ReadByte(programCounter++));
                case Instruction.LD_A_A:
                    return Load_Target_Source(ref registerA, registerA);
                case Instruction.LD_A_B:
                    return Load_Target_Source(ref registerA, registerB);
                case Instruction.LD_A_C:
                    return Load_Target_Source(ref registerA, registerC);
                case Instruction.LD_A_D:
                    return Load_Target_Source(ref registerA, registerD);
                case Instruction.LD_A_E:
                    return Load_Target_Source(ref registerA, registerE);
                case Instruction.LD_A_H:
                    return Load_Target_Source(ref registerA, registerH);
                case Instruction.LD_A_L:
                    return Load_Target_Source(ref registerA, registerL);
                case Instruction.LD_B_A:
                    return Load_Target_Source(ref registerB, registerA);
                case Instruction.LD_B_B:
                    return Load_Target_Source(ref registerB, registerB);
                case Instruction.LD_B_C:
                    return Load_Target_Source(ref registerB, registerC);
                case Instruction.LD_B_D:
                    return Load_Target_Source(ref registerB, registerD);
                case Instruction.LD_B_E:
                    return Load_Target_Source(ref registerB, registerE);
                case Instruction.LD_B_H:
                    return Load_Target_Source(ref registerB, registerH);
                case Instruction.LD_B_L:
                    return Load_Target_Source(ref registerB, registerL);
                case Instruction.LD_C_A:
                    return Load_Target_Source(ref registerC, registerA);
                case Instruction.LD_C_B:
                    return Load_Target_Source(ref registerC, registerB);
                case Instruction.LD_C_C:
                    return Load_Target_Source(ref registerC, registerC);
                case Instruction.LD_C_D:
                    return Load_Target_Source(ref registerC, registerD);
                case Instruction.LD_C_E:
                    return Load_Target_Source(ref registerC, registerE);
                case Instruction.LD_C_H:
                    return Load_Target_Source(ref registerC, registerH);
                case Instruction.LD_C_L:
                    return Load_Target_Source(ref registerC, registerL);
                case Instruction.LD_D_A:
                    return Load_Target_Source(ref registerD, registerA);
                case Instruction.LD_D_B:
                    return Load_Target_Source(ref registerD, registerB);
                case Instruction.LD_D_C:
                    return Load_Target_Source(ref registerD, registerC);
                case Instruction.LD_D_D:
                    return Load_Target_Source(ref registerD, registerD);
                case Instruction.LD_D_E:
                    return Load_Target_Source(ref registerD, registerE);
                case Instruction.LD_D_H:
                    return Load_Target_Source(ref registerD, registerH);
                case Instruction.LD_D_L:
                    return Load_Target_Source(ref registerD, registerL);
                case Instruction.LD_E_A:
                    return Load_Target_Source(ref registerE, registerA);
                case Instruction.LD_E_B:
                    return Load_Target_Source(ref registerE, registerB);
                case Instruction.LD_E_C:
                    return Load_Target_Source(ref registerE, registerC);
                case Instruction.LD_E_D:
                    return Load_Target_Source(ref registerE, registerD);
                case Instruction.LD_E_E:
                    return Load_Target_Source(ref registerE, registerE);
                case Instruction.LD_E_H:
                    return Load_Target_Source(ref registerE, registerH);
                case Instruction.LD_E_L:
                    return Load_Target_Source(ref registerE, registerL);
                case Instruction.LD_H_A:
                    return Load_Target_Source(ref registerH, registerA);
                case Instruction.LD_H_B:
                    return Load_Target_Source(ref registerH, registerB);
                case Instruction.LD_H_C:
                    return Load_Target_Source(ref registerH, registerC);
                case Instruction.LD_H_D:
                    return Load_Target_Source(ref registerH, registerD);
                case Instruction.LD_H_E:
                    return Load_Target_Source(ref registerH, registerE);
                case Instruction.LD_H_H:
                    return Load_Target_Source(ref registerH, registerH);
                case Instruction.LD_H_L:
                    return Load_Target_Source(ref registerH, registerL);
                case Instruction.LD_L_A:
                    return Load_Target_Source(ref registerL, registerA);
                case Instruction.LD_L_B:
                    return Load_Target_Source(ref registerL, registerB);
                case Instruction.LD_L_C:
                    return Load_Target_Source(ref registerL, registerC);
                case Instruction.LD_L_D:
                    return Load_Target_Source(ref registerL, registerD);
                case Instruction.LD_L_E:
                    return Load_Target_Source(ref registerL, registerE);
                case Instruction.LD_L_H:
                    return Load_Target_Source(ref registerL, registerH);
                case Instruction.LD_L_L:
                    return Load_Target_Source(ref registerL, registerL);
                case Instruction.LD_A_d8:
                    return Load_Target_Direct8(ref registerA);
                case Instruction.LD_B_d8:
                    return Load_Target_Direct8(ref registerB);
                case Instruction.LD_C_d8:
                    return Load_Target_Direct8(ref registerC);
                case Instruction.LD_D_d8:
                    return Load_Target_Direct8(ref registerD);
                case Instruction.LD_E_d8:
                    return Load_Target_Direct8(ref registerE);
                case Instruction.LD_H_d8:
                    return Load_Target_Direct8(ref registerH);
                case Instruction.LD_L_d8:
                    return Load_Target_Direct8(ref registerL);
                case Instruction.LD_A_pHL:
                    return Load_Target_MemoryAddr(ref registerA, registerHL);
                case Instruction.LD_B_pHL:
                    return Load_Target_MemoryAddr(ref registerB, registerHL);
                case Instruction.LD_C_pHL:
                    return Load_Target_MemoryAddr(ref registerC, registerHL);
                case Instruction.LD_D_pHL:
                    return Load_Target_MemoryAddr(ref registerD, registerHL);
                case Instruction.LD_E_pHL:
                    return Load_Target_MemoryAddr(ref registerE, registerHL);
                case Instruction.LD_H_pHL:
                    return Load_Target_MemoryAddr(ref registerH, registerHL);
                case Instruction.LD_L_pHL:
                    return Load_Target_MemoryAddr(ref registerL, registerHL);
                case Instruction.LD_pHL_A:
                    return Load_MemoryAddr_SourceRegister(registerHL, registerA);
                case Instruction.LD_pHL_B:
                    return Load_MemoryAddr_SourceRegister(registerHL, registerB);
                case Instruction.LD_pHL_C:
                    return Load_MemoryAddr_SourceRegister(registerHL, registerC);
                case Instruction.LD_pHL_D:
                    return Load_MemoryAddr_SourceRegister(registerHL, registerD);
                case Instruction.LD_pHL_E:
                    return Load_MemoryAddr_SourceRegister(registerHL, registerE);
                case Instruction.LD_pHL_H:
                    return Load_MemoryAddr_SourceRegister(registerHL, registerH);
                case Instruction.LD_pHL_L:
                    return Load_MemoryAddr_SourceRegister(registerHL, registerL);
                case Instruction.LD_A_pHLminus:
                    return Load_Target_pHLminus(ref registerA);
                case Instruction.LD_A_pHLplus:
                    return Load_Target_pHLplus(ref registerA);
                case Instruction.LD_pHLminus_A:
                    cycles = Load_MemoryAddr_SourceRegister(registerHL, registerA);
                    registerHL = (ushort)(registerHL - 1);
                    return cycles;
                case Instruction.LD_pHLplus_A:
                    cycles = Load_MemoryAddr_SourceRegister(registerHL, registerA);
                    registerHL = (ushort)(registerHL + 1);
                    return cycles;
                case Instruction.LD_pHL_d8:
                    return Load_MemoryAddr_Direct8(registerHL);
                case Instruction.LD_A_pBC:
                    return Load_Target_MemoryAddr(ref registerA, registerBC);
                case Instruction.LD_A_pDE:
                    return Load_Target_MemoryAddr(ref registerA, registerDE);
                case Instruction.LD_pBC_A:
                    return Load_MemoryAddr_SourceRegister(registerBC, registerA);
                case Instruction.LD_pDE_A:
                    return Load_MemoryAddr_SourceRegister(registerDE, registerA);
                case Instruction.LD_A_pa16:
                    return Load_Target_Direct16MemoryAddr(ref registerA);
                case Instruction.LD_pa16_A:
                    return Load_Direct16MemoryAddr_Source(registerA);
                    break;
                case Instruction.LD_A_pC:
                    return Load_Target_MemoryAddr(ref registerA, (ushort)((0xFF << 8) + registerC));
                case Instruction.LD_pC_A:
                    return Load_MemoryAddr_SourceRegister((ushort)((0xFF << 8) + registerC), registerA);
                case Instruction.LDH_A_pa8:
                    return Load_Target_Direct8MemoryAddr(ref registerA);
                    break;
                case Instruction.LDH_pa8_A:
                    return Load_Direct8MemoryAddr_Source(registerA);
                case Instruction.INC_A:
                    return Increment(ref registerA);
                case Instruction.INC_B:
                    return Increment(ref registerB);
                case Instruction.INC_C:
                    return Increment(ref registerC);
                case Instruction.INC_D:
                    return Increment(ref registerD);
                case Instruction.INC_E:
                    return Increment(ref registerE);
                case Instruction.INC_H:
                    return Increment(ref registerH);
                case Instruction.INC_L:
                    return Increment(ref registerL);
                case Instruction.DEC_A:
                    return Decrement(ref registerA);
                case Instruction.DEC_B:
                    return Decrement(ref registerB);
                case Instruction.DEC_C:
                    return Decrement(ref registerC);
                case Instruction.DEC_D:
                    return Decrement(ref registerD);
                case Instruction.DEC_E:
                    return Decrement(ref registerE);
                case Instruction.DEC_H:
                    return Decrement(ref registerH);
                case Instruction.DEC_L:
                    return Decrement(ref registerL);
                case Instruction.XOR_A:
                    yield return () => registerA = XorWithRegisterA(registerA);
                    yield return () => SetFlag(CpuFlags.Zero, registerA == 0);
                    break;
                case Instruction.XOR_B:
                    yield return () => registerA = XorWithRegisterA(registerB);
                    yield return () => SetFlag(CpuFlags.Zero, registerA == 0);
                    break;
                case Instruction.XOR_C:
                    yield return () => registerA = XorWithRegisterA(registerC);
                    yield return () => SetFlag(CpuFlags.Zero, registerA == 0);
                    break;
                case Instruction.XOR_D:
                    yield return () => registerA = XorWithRegisterA(registerD);
                    yield return () => SetFlag(CpuFlags.Zero, registerA == 0);
                    break;
                case Instruction.XOR_E:
                    yield return () => registerA = XorWithRegisterA(registerE);
                    yield return () => SetFlag(CpuFlags.Zero, registerA == 0);
                    break;
                case Instruction.XOR_H:
                    yield return () => registerA = XorWithRegisterA(registerH);
                    yield return () => SetFlag(CpuFlags.Zero, registerA == 0);
                    break;
                case Instruction.XOR_L:
                    yield return () => registerA = XorWithRegisterA(registerL);
                    yield return () => SetFlag(CpuFlags.Zero, registerA == 0);
                    break;
                case Instruction.LD_BC_d16:
                case Instruction.LD_DE_d16:
                case Instruction.LD_HL_d16:
                case Instruction.LD_SP_d16:
                    foreach (var action in LD_rr_nn(inst)) {
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
                    foreach (var action in Call_cc_nn(inst)) {
                        yield return action;
                    }
                    break;



                case Instruction.JR_C_r8:
                case Instruction.JR_NC_r8:
                case Instruction.JR_Z_r8:
                case Instruction.JR_NZ_r8:
                    foreach (var action in JR_cc_n(inst)) {
                        yield return action;
                    }
                    break;

                case Instruction.JR_r8:

                default: throw new NotImplementedException($"Instruction: {inst}");
            }
        }

        private int Decrement(ref byte target) {
            target--;
            return 4;
        }

        private int Increment(ref byte target) {
            target++;
            return 4;
        }

        private int Load_Direct8MemoryAddr_Source(byte source) {
            ushort memoryAddr = (ushort)(memory.ReadByte(programCounter++) + 0xFF << 8);
            return Load_MemoryAddr_SourceRegister(memoryAddr, source) + 8;
        }

        private int Load_Direct16MemoryAddr_Source(byte source) {
            ushort memoryAddr = (ushort)(memory.ReadByte(programCounter++) + memory.ReadByte(programCounter++) << 8);
            return Load_MemoryAddr_SourceRegister(memoryAddr, source) + 8;
        }

        private int Load_Target_Direct8MemoryAddr(ref byte target) {
            ushort memoryAddr = (ushort)(memory.ReadByte(programCounter++) + 0xFF << 8);
            return Load_Target_MemoryAddr(ref target, memoryAddr) + 8;
        }

        private int Load_Target_Direct16MemoryAddr(ref byte target) {
            ushort memoryAddr = (ushort)(memory.ReadByte(programCounter++) + memory.ReadByte(programCounter++) << 8);
            return Load_Target_MemoryAddr(ref target, memoryAddr) + 8;
        }

        private int Load_MemoryAddr_Direct8(ushort addr) {
            var data = memory.ReadByte(programCounter++);
            memory.WriteByte(addr, data);
            return 4;
        }

        private int Load_Target_pHL(ref byte target) => Load_Target_MemoryAddr(ref target, registerHL);

        private int Load_Target_pHLplus(ref byte target) {
            var cycles = Load_Target_pHL(ref target);
            registerHL = (ushort)(registerHL + 1);
            return cycles;
        }

        private int Load_Target_pHLminus(ref byte target) {
            var cycles = Load_Target_pHL(ref target);
            registerHL = (ushort)(registerHL - 1);
            return cycles;
        }

        private int Load_Target_MemoryAddr(ref byte target, ushort addr) {
            target = memory.ReadByte(addr);
            return 4;
        }

        private int Load_MemoryAddr_SourceRegister(ushort addr, byte source) {
            memory.WriteByte(addr, source);
            return 4;
        }


        private int Load_Target_Direct8(ref byte target) {
            target = memory.ReadByte(programCounter++);
            return 4;
        }

        private int ExecuteCbInstruction(CBInstruction cBInstruction) {
            throw new NotImplementedException();
        }

        private int Load_Target_Source(ref byte target, byte source) {
            target = source;
            return 4;
        }

        private IEnumerable<Action> JR_cc_n(Instruction instruction) {
            foreach (var step in Read16Bit()) {
                yield return step;
            }
            if (EvaluateCondition(instruction)) {
                yield return () => programCounter = (ushort)(programCounter + workVariable);
            } else {
                //waste a cycle?
            }
        }

        private IEnumerable<Action> GetCbActionChain() {
            switch (CbInstruction) {
                case CBInstruction.RLC_B:
                    yield return () => RotateRegisterLeft(ref registerB);
                    break;

                default: throw new NotImplementedException($"CbInstruction: {CbInstruction}");
            }
        }

        private byte XorWithRegisterA(byte register) => (byte)(register ^ registerA);

        private void RotateRegisterLeft(ref byte register) {
            bool msbValue = IsMostSignificantBitSet(register);
            register = (byte)(register << 1);
            if (msbValue) {
                register = (byte)(register | LeastSignificantBitMask);
            }
            SetFlag(CpuFlags.Zero, register == 0);
            SetFlag(CpuFlags.AddSub, false);
            SetFlag(CpuFlags.HalfCarry, false);
            SetFlag(CpuFlags.Carry, msbValue);
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
                case Instruction.CALL_C_a16:
                case Instruction.JR_C_r8: return IsFlagSet(CpuFlags.Carry);
                case Instruction.CALL_NC_a16:
                case Instruction.JR_NC_r8: return !IsFlagSet(CpuFlags.Carry);
                case Instruction.CALL_Z_a16:
                case Instruction.JR_Z_r8: return IsFlagSet(CpuFlags.Zero);
                case Instruction.CALL_NZ_a16:
                case Instruction.JR_NZ_r8: return !IsFlagSet(CpuFlags.Zero);
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

        private byte GetMask(CpuFlags flag) => flag switch {
            CpuFlags.Zero => ZeroFlagMask,
            CpuFlags.AddSub => AddSubFlagMask,
            CpuFlags.HalfCarry => HalfCarryFlagMask,
            CpuFlags.Carry => CarryFlagMask,
            _ => throw new NotImplementedException($"Unrecognized flag '{flag}'."),
        };

        private enum CpuFlags {
            Zero,
            AddSub,
            HalfCarry,
            Carry,
        }

        private const byte ZeroFlagMask = 0b_1000_0000;
        private const byte AddSubFlagMask = 0b_0100_0000;
        private const byte HalfCarryFlagMask = 0b_0010_0000;
        private const byte CarryFlagMask = 0b_0001_0000;
    }
}
