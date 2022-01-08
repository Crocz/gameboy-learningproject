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
        private byte stackPointerUpper;
        private byte stackPointerLower;

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
        private ushort stackPointer {
            get {
                return Combine(stackPointerUpper, stackPointerLower);
            }
            set {
                stackPointerLower = LeastSignificantByte(value);
                stackPointerUpper = MostSignificantByte(value);
            }
        }
        private ushort Combine(byte upperByte, byte lowerByte) => (ushort)((upperByte << 8) + lowerByte);
        private const int LeastSignificantByteMask = 0x00FF;
        private const byte LeastSignificantBitMask = 0b_0000_0001;
        private byte MostSignificantByte(ushort UnsignedShort) => (byte)((UnsignedShort >> 8) & LeastSignificantByteMask);
        private byte LeastSignificantByte(ushort UnsignedShort) => (byte)(UnsignedShort & LeastSignificantByteMask);
        private bool IsMostSignificantBitSet(byte b) => IsLeastSignificantBitSet((byte)(b >> 7));
        private bool IsLeastSignificantBitSet(byte b) => (b & LeastSignificantBitMask) == LeastSignificantBitMask;

        public string ProgramCounter => $"${programCounter.ToString("X4")}";
        public ushort PCAsShort => programCounter;
        public string NextInstruction => ((Instruction)memory.ReadByte(programCounter)).ToString();

        private ushort programCounter;

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
            switch (instruction) {
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
                case Instruction.LD_A_pC:
                    return Load_Target_MemoryAddr(ref registerA, (ushort)((0xFF << 8) + registerC));
                case Instruction.LD_pC_A:
                    return Load_MemoryAddr_SourceRegister((ushort)((0xFF << 8) + registerC), registerA);
                case Instruction.LDH_A_pa8:
                    return Load_Target_Direct8MemoryAddr(ref registerA);
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
                    return XorWithRegisterA(registerA);
                case Instruction.XOR_B:
                    return XorWithRegisterA(registerB);
                case Instruction.XOR_C:
                    return XorWithRegisterA(registerC);
                case Instruction.XOR_D:
                    return XorWithRegisterA(registerD);
                case Instruction.XOR_E:
                    return XorWithRegisterA(registerE);
                case Instruction.XOR_H:
                    return XorWithRegisterA(registerH);
                case Instruction.XOR_L:
                    return XorWithRegisterA(registerL);
                case Instruction.LD_BC_d16:
                    return Load_Target_Direct8(ref registerC) + Load_Target_Direct8(ref registerB);
                case Instruction.LD_DE_d16:
                    return Load_Target_Direct8(ref registerD) + Load_Target_Direct8(ref registerE);
                case Instruction.LD_HL_d16:
                    return Load_Target_Direct8(ref registerL) + Load_Target_Direct8(ref registerH);
                case Instruction.LD_SP_d16:
                    return Load_Target_Direct8(ref stackPointerLower) + Load_Target_Direct8(ref stackPointerUpper);

                case Instruction.CALL_a16:
                    return Call_Direct16();
                case Instruction.CALL_C_a16:
                case Instruction.CALL_NC_a16:
                case Instruction.CALL_Z_a16:
                case Instruction.CALL_NZ_a16:
                    return Call_Conditional_Direct16(instruction);

                case Instruction.JR_r8:
                    return Jump_Direct8();
                case Instruction.JR_C_r8:
                case Instruction.JR_NC_r8:
                case Instruction.JR_Z_r8:
                case Instruction.JR_NZ_r8:
                    return Jump_Conditonal_Direct8(instruction);

                case Instruction.PUSH_AF:
                    return Push(registerAF);
                case Instruction.PUSH_BC:
                    return Push(registerBC);
                case Instruction.PUSH_DE:
                    return Push(registerDE);
                case Instruction.PUSH_HL:
                    return Push(registerHL);

                case Instruction.POP_AF:
                case Instruction.POP_BC:
                case Instruction.POP_DE:
                case Instruction.POP_HL:
                    return Pop(instruction);

                case Instruction.RLA:
                    return Rotate_Target_Left(ref registerA, clearZeroFlag: true);
                case Instruction.RLCA:

                default: throw new NotImplementedException($"Instruction: {instruction}");
            }
        }

        private int ExecuteCbInstruction(CBInstruction cbInstruction) {
            switch (cbInstruction) {
                case CBInstruction.BIT_1_A:
                    return Bit_X_Source(1, registerA);
                case CBInstruction.BIT_1_B:
                    return Bit_X_Source(1, registerB);
                case CBInstruction.BIT_1_C:
                    return Bit_X_Source(1, registerC);
                case CBInstruction.BIT_1_D:
                    return Bit_X_Source(1, registerD);
                case CBInstruction.BIT_1_E:
                    return Bit_X_Source(1, registerE);
                case CBInstruction.BIT_1_H:
                    return Bit_X_Source(1, registerH);
                case CBInstruction.BIT_1_L:
                    return Bit_X_Source(1, registerL);
                case CBInstruction.BIT_2_A:
                    return Bit_X_Source(2, registerA);
                case CBInstruction.BIT_2_B:
                    return Bit_X_Source(2, registerB);
                case CBInstruction.BIT_2_C:
                    return Bit_X_Source(2, registerC);
                case CBInstruction.BIT_2_D:
                    return Bit_X_Source(2, registerD);
                case CBInstruction.BIT_2_E:
                    return Bit_X_Source(2, registerE);
                case CBInstruction.BIT_2_H:
                    return Bit_X_Source(2, registerH);
                case CBInstruction.BIT_2_L:
                    return Bit_X_Source(2, registerL);
                case CBInstruction.BIT_3_A:
                    return Bit_X_Source(3, registerA);
                case CBInstruction.BIT_3_B:
                    return Bit_X_Source(3, registerB);
                case CBInstruction.BIT_3_C:
                    return Bit_X_Source(3, registerC);
                case CBInstruction.BIT_3_D:
                    return Bit_X_Source(3, registerD);
                case CBInstruction.BIT_3_E:
                    return Bit_X_Source(3, registerE);
                case CBInstruction.BIT_3_H:
                    return Bit_X_Source(3, registerH);
                case CBInstruction.BIT_3_L:
                    return Bit_X_Source(3, registerL);
                case CBInstruction.BIT_4_A:
                    return Bit_X_Source(4, registerA);
                case CBInstruction.BIT_4_B:
                    return Bit_X_Source(4, registerB);
                case CBInstruction.BIT_4_C:
                    return Bit_X_Source(4, registerC);
                case CBInstruction.BIT_4_D:
                    return Bit_X_Source(4, registerD);
                case CBInstruction.BIT_4_E:
                    return Bit_X_Source(4, registerE);
                case CBInstruction.BIT_4_H:
                    return Bit_X_Source(4, registerH);
                case CBInstruction.BIT_4_L:
                    return Bit_X_Source(4, registerL);
                case CBInstruction.BIT_5_A:
                    return Bit_X_Source(5, registerA);
                case CBInstruction.BIT_5_B:
                    return Bit_X_Source(5, registerB);
                case CBInstruction.BIT_5_C:
                    return Bit_X_Source(5, registerC);
                case CBInstruction.BIT_5_D:
                    return Bit_X_Source(5, registerD);
                case CBInstruction.BIT_5_E:
                    return Bit_X_Source(5, registerE);
                case CBInstruction.BIT_5_H:
                    return Bit_X_Source(5, registerH);
                case CBInstruction.BIT_5_L:
                    return Bit_X_Source(5, registerL);
                case CBInstruction.BIT_6_A:
                    return Bit_X_Source(6, registerA);
                case CBInstruction.BIT_6_B:
                    return Bit_X_Source(6, registerB);
                case CBInstruction.BIT_6_C:
                    return Bit_X_Source(6, registerC);
                case CBInstruction.BIT_6_D:
                    return Bit_X_Source(6, registerD);
                case CBInstruction.BIT_6_E:
                    return Bit_X_Source(6, registerE);
                case CBInstruction.BIT_6_H:
                    return Bit_X_Source(6, registerH);
                case CBInstruction.BIT_6_L:
                    return Bit_X_Source(6, registerL);
                case CBInstruction.BIT_7_A:
                    return Bit_X_Source(7, registerA);
                case CBInstruction.BIT_7_B:
                    return Bit_X_Source(7, registerB);
                case CBInstruction.BIT_7_C:
                    return Bit_X_Source(7, registerC);
                case CBInstruction.BIT_7_D:
                    return Bit_X_Source(7, registerD);
                case CBInstruction.BIT_7_E:
                    return Bit_X_Source(7, registerE);
                case CBInstruction.BIT_7_H:
                    return Bit_X_Source(7, registerH);
                case CBInstruction.BIT_7_L:
                    return Bit_X_Source(7, registerL);

                case CBInstruction.RL_A:
                    return Rotate_Target_Left(ref registerA);
                case CBInstruction.RL_B:
                    return Rotate_Target_Left(ref registerB);
                case CBInstruction.RL_C:
                    return Rotate_Target_Left(ref registerC);
                case CBInstruction.RL_D:
                    return Rotate_Target_Left(ref registerD);
                case CBInstruction.RL_E:
                    return Rotate_Target_Left(ref registerE);
                case CBInstruction.RL_H:
                    return Rotate_Target_Left(ref registerH);
                case CBInstruction.RL_L:
                    return Rotate_Target_Left(ref registerL);

                case CBInstruction.RLC_A:
                case CBInstruction.RLC_B:
                case CBInstruction.RLC_C:
                case CBInstruction.RLC_D:
                case CBInstruction.RLC_E:
                case CBInstruction.RLC_H:
                case CBInstruction.RLC_L:

                default: throw new NotImplementedException($"CbInstruction: {cbInstruction}");
            }
        }

        private ushort ReadDirect16() => (ushort)(ReadDirect8() << ReadDirect8());

        private byte ReadDirect8() => memory.ReadByte(programCounter++);

        #region Instruction implementations

        private byte AddCore(byte a, byte b, out bool halfCarry, out bool carry) {
            const int nybbleMask = 0x0f;
            const int halfCarryBit = 0x10;
            halfCarry = (((a & nybbleMask) + (b & nybbleMask)) & halfCarryBit) == halfCarryBit;
            const int carryBit = 0x100;
            var sum = a + b;
            carry = (sum & carryBit) == carryBit;
            return (byte)sum;
        }

        //change the impl. of this method to be different from AddCore.
        private byte SubCore(byte a, byte b, out bool halfCarry, out bool carry) {
            const int nybbleMask = 0x0f;
            const int halfCarryBit = 0x10;
            halfCarry = (((a & nybbleMask) + (b & nybbleMask)) & halfCarryBit) == halfCarryBit; //TODO : change the implementation to do the Right Thing.
            const int carryBit = 0x100;
            var sum = a + b;
            carry = (sum & carryBit) == carryBit;
            return (byte)sum;
        }

        private int Decrement(ref byte target) {
            bool halfCarry;
            target = SubCore(target, 1, out halfCarry, out _);
            SetFlag(CpuFlags.Zero, target == 0);
            SetFlag(CpuFlags.AddSub, false);
            SetFlag(CpuFlags.HalfCarry, halfCarry);
            // Carry flag (strangely) not affected by Dec.
            return 4;
        }

        private int Increment(ref byte target) {
            bool halfCarry;
            target = AddCore(target, 1, out halfCarry, out _);
            SetFlag(CpuFlags.Zero, target == 0);
            SetFlag(CpuFlags.AddSub, false);
            SetFlag(CpuFlags.HalfCarry, halfCarry);
            // Carry flag (strangely) not affected by Inc.
            return 4;
        }

        private int Load_Direct8MemoryAddr_Source(byte source) {
            ushort memoryAddr = (ushort)(ReadDirect8() + 0xFF << 8);
            return Load_MemoryAddr_SourceRegister(memoryAddr, source) + 8;
        }

        private int Load_Direct16MemoryAddr_Source(byte source) {
            ushort memoryAddr = ReadDirect16();
            return Load_MemoryAddr_SourceRegister(memoryAddr, source) + 8;
        }

        private int Load_Target_Direct8MemoryAddr(ref byte target) {
            ushort memoryAddr = (ushort)(ReadDirect8() + 0xFF << 8);
            return Load_Target_MemoryAddr(ref target, memoryAddr) + 8;
        }

        private int Load_Target_Direct16MemoryAddr(ref byte target) {
            ushort memoryAddr = ReadDirect16();
            return Load_Target_MemoryAddr(ref target, memoryAddr) + 8;
        }

        private int Load_MemoryAddr_Direct8(ushort addr) {
            memory.WriteByte(addr, ReadDirect8());
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
            target = ReadDirect8();
            return 4;
        }

        private int Load_Target_Source(ref byte target, byte source) {
            target = source;
            return 4;
        }

        private byte XorWithRegisterA(byte data) {
            registerA = (byte)(data ^ registerA);
            SetFlag(CpuFlags.Zero, registerA == 0);
            SetFlag(CpuFlags.AddSub, false);
            SetFlag(CpuFlags.HalfCarry, false);
            SetFlag(CpuFlags.Carry, false);
            return 4;
        }

        private int Call_Direct16() => Conditional_Call_Direct16(true);

        private int Call_Conditional_Direct16(Instruction instruction) => Conditional_Call_Direct16(EvaluateCondition(instruction));

        private int Conditional_Call_Direct16(bool call) {
            int cycles = 4;
            ushort addr = ReadDirect16();
            if (call) {
                WriteAddressToStack(programCounter);
                programCounter = addr;
                cycles += 8;
            }
            return cycles;
        }

        private int Push(ushort addr) {
            WriteAddressToStack(addr);
            return 4;
        }

        
        private int Pop(Instruction instruction) {
            var addr = ReadAddressFromStack();
            switch (instruction) {
                case Instruction.POP_AF:
                    registerAF = addr;
                    break;
                case Instruction.POP_BC:
                    registerBC = addr;
                    break;
                case Instruction.POP_DE:
                    registerDE = addr;
                    break;
                case Instruction.POP_HL:
                    registerHL = addr;
                    break;
            }
            return 4;
        }

        private void WriteAddressToStack(ushort addr) {
            memory.WriteByte(stackPointer, MostSignificantByte(addr));
            memory.WriteByte((ushort)(stackPointer - 1), LeastSignificantByte(addr));
            stackPointer = (ushort)(stackPointer - 2);
        }

        private ushort ReadAddressFromStack() {
            byte leastSignificantByte = memory.ReadByte((ushort)(stackPointer + 1));
            byte mostSignificantByte = memory.ReadByte((ushort)(stackPointer + 2));
            stackPointer = (ushort)(stackPointer + 2);
            return Combine(mostSignificantByte, leastSignificantByte);
        }

        private int Jump_Direct8() => Conditional_Jump_Direct8(true);

        private int Jump_Conditonal_Direct8(Instruction instruction) => Conditional_Jump_Direct8(EvaluateCondition(instruction));

        private int Conditional_Jump_Direct8(bool call) {
            int cycles = 4;
            sbyte signedOffset = (sbyte)ReadDirect8();
            if (call) {
                programCounter = (ushort)(programCounter + signedOffset);
                cycles += 8;
            }
            return cycles;
        }
        #endregion

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

        private int Bit_X_Source(int x, byte source) {
            var isBitXSet = IsLeastSignificantBitSet((byte)(source >> x));
            SetFlag(CpuFlags.Zero, isBitXSet);
            SetFlag(CpuFlags.AddSub, false);
            SetFlag(CpuFlags.HalfCarry, true);
            //Carry not affected
            return 8;
        }

        private int Rotate_Target_Left(ref byte target) => Rotate_Target_Left(ref target, false);

        private int Rotate_Target_Left(ref byte target, bool clearZeroFlag) {
            bool msbValue = IsMostSignificantBitSet(target);
            target = (byte)(target << 1);
            if (msbValue) {
                target = (byte)(target | LeastSignificantBitMask);
            }
            SetFlag(CpuFlags.Zero, clearZeroFlag ? false : target == 0);
            SetFlag(CpuFlags.AddSub, false);
            SetFlag(CpuFlags.HalfCarry, false);
            SetFlag(CpuFlags.Carry, msbValue);
            return 8;
        }

        #region CpuFlags
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
        #endregion
    }
}
