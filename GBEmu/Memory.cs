using System;
using System.Text;

namespace GBEmu {
    public interface IMemory {
        byte ReadByte(ushort address);
        void WriteByte(ushort address, byte content);
        string ContentsAsString { get; }
    }

    public class Memory : IMemory {
        private const string bootrompath = @"C:\Users\AdminApan\Source\Repos\gameboy-learningproject\binaries\bootrom\DMG_ROM.bin";

        private byte[] storage;

        // Memory range bounds
        private const ushort BootRomLo = 0x0000;
        private const ushort BootRomHi = 0x00FF;
        private const ushort Rom0Lo = 0x0000;
        private const ushort Rom0Hi = 0x3FFF;
        private const ushort RomXLo = 0x4000;
        private const ushort RomXHi = 0x7FFF;
        private const ushort VRamLo = 0x8000;
        private const ushort VRamHi = 0x9FFF;
        private const ushort SRamLo = 0xA000;
        private const ushort SRamHi = 0xBFFF;
        private const ushort WRam0Lo = 0xC000;
        private const ushort WRam0Hi = 0xCFFF;
        private const ushort WRamXLo = 0xD000;
        private const ushort WRamXHi = 0xDFFF;
        private const ushort EchoLo = 0xE000;
        private const ushort EchoHi = 0xFDFF;
        private const ushort OamLo = 0xFE00;
        private const ushort OamHi = 0xFE9F;
        private const ushort UnusedLo = 0xFEA0;
        private const ushort UnusedHi = 0xFEFF;
        private const ushort IORegLo = 0xFF00;
        private const ushort IORegHi = 0xFF7F;
        private const ushort HRamLo = 0xFF80;
        private const ushort HRamHi = 0xFFFE;
        private const ushort IEReg = 0xFFFF;

        public string ContentsAsString {
            get {
                StringBuilder stringBuilder = new StringBuilder();
                int x = 0;
                int next;
                int lines = 0;
                while (x < storage.Length) {
                    stringBuilder.AppendLine($"{PrettyPrint(x, out next)}, ${x.ToString("X4")}");
                    x = next;
                    lines++;
                }
                return stringBuilder.ToString();
            }
        }

        public Memory() {
            storage = new byte[0xFFFF];
            LoadBootRom(bootrompath);
        }
        public byte ReadByte(ushort address) {
            return storage[address];
        }

        public void WriteByte(ushort address, byte content) {
            storage[address] = content;
        }

        private void LoadBootRom(string path) {
            using (var fileStream = System.IO.File.OpenRead(path)) {
                if (fileStream.Length > ((BootRomHi + 1) - BootRomLo)) {
                    throw new ArgumentException("BootRom exceeds maximum length.");
                }
                fileStream.Read(storage, BootRomLo, BootRomHi + 1);
            }
        }

        private string PrettyPrint(int x, out int nextOffset) {
            if(x > (storage.Length-1)) {
                throw new ArgumentException($"Requested memory cell {x}");
            }
            Instruction i = (Instruction)storage[x];
            if (i == Instruction.PREFIX_CB) {
                nextOffset = x + 2;
                return ((CBInstruction)storage[x+1]).ToString();
            }
            switch (i) {
                case Instruction.NOP:
                case Instruction.LD_A_A:
                case Instruction.LD_A_B:
                case Instruction.LD_A_C:
                case Instruction.LD_A_D:
                case Instruction.LD_A_E:
                case Instruction.LD_A_H:
                case Instruction.LD_A_L:
                case Instruction.LD_B_A:
                case Instruction.LD_B_B:
                case Instruction.LD_B_C:
                case Instruction.LD_B_D:
                case Instruction.LD_B_E:
                case Instruction.LD_B_H:
                case Instruction.LD_B_L:
                case Instruction.LD_C_A:
                case Instruction.LD_C_B:
                case Instruction.LD_C_C:
                case Instruction.LD_C_D:
                case Instruction.LD_C_E:
                case Instruction.LD_C_H:
                case Instruction.LD_C_L:
                case Instruction.LD_D_A:
                case Instruction.LD_D_B:
                case Instruction.LD_D_C:
                case Instruction.LD_D_D:
                case Instruction.LD_D_E:
                case Instruction.LD_D_H:
                case Instruction.LD_D_L:
                case Instruction.LD_E_A:
                case Instruction.LD_E_B:
                case Instruction.LD_E_C:
                case Instruction.LD_E_D:
                case Instruction.LD_E_E:
                case Instruction.LD_E_H:
                case Instruction.LD_E_L:
                case Instruction.LD_H_A:
                case Instruction.LD_H_B:
                case Instruction.LD_H_C:
                case Instruction.LD_H_D:
                case Instruction.LD_H_E:
                case Instruction.LD_H_H:
                case Instruction.LD_H_L:
                case Instruction.LD_L_A:
                case Instruction.LD_L_B:
                case Instruction.LD_L_C:
                case Instruction.LD_L_D:
                case Instruction.LD_L_E:
                case Instruction.LD_L_H:
                case Instruction.LD_L_L:
                case Instruction.LD_A_pHL:
                case Instruction.LD_B_pHL:
                case Instruction.LD_C_pHL:
                case Instruction.LD_D_pHL:
                case Instruction.LD_E_pHL:
                case Instruction.LD_H_pHL:
                case Instruction.LD_L_pHL:
                case Instruction.LD_pHL_A:
                case Instruction.LD_pHL_B:
                case Instruction.LD_pHL_C:
                case Instruction.LD_pHL_D:
                case Instruction.LD_pHL_E:
                case Instruction.LD_pHL_H:
                case Instruction.LD_pHL_L:
                case Instruction.LD_A_pHLminus:
                case Instruction.LD_A_pHLplus:
                case Instruction.LD_pHLminus_A:
                case Instruction.LD_pHLplus_A:
                case Instruction.LD_A_pBC:
                case Instruction.LD_A_pDE:
                case Instruction.LD_pBC_A:
                case Instruction.LD_pDE_A:
                case Instruction.INC_A:
                case Instruction.INC_B:
                case Instruction.INC_C:
                case Instruction.INC_D:
                case Instruction.INC_E:
                case Instruction.INC_H:
                case Instruction.INC_L:
                case Instruction.INC_pHL:
                case Instruction.INC_BC:
                case Instruction.INC_DE:
                case Instruction.INC_HL:
                case Instruction.INC_SP:
                case Instruction.DEC_A:
                case Instruction.DEC_B:
                case Instruction.DEC_C:
                case Instruction.DEC_D:
                case Instruction.DEC_E:
                case Instruction.DEC_H:
                case Instruction.DEC_L:
                case Instruction.DEC_pHL:
                case Instruction.DEC_BC:
                case Instruction.DEC_DE:
                case Instruction.DEC_HL:
                case Instruction.DEC_SP:
                case Instruction.CPL:
                case Instruction.CCF:
                case Instruction.ADD_A_A:
                case Instruction.ADD_A_B:
                case Instruction.ADD_A_C:
                case Instruction.ADD_A_D:
                case Instruction.ADD_A_E:
                case Instruction.ADD_A_H:
                case Instruction.ADD_A_L:
                case Instruction.ADD_A_pHL:
                case Instruction.ADC_A_A:
                case Instruction.ADC_A_B:
                case Instruction.ADC_A_C:
                case Instruction.ADC_A_D:
                case Instruction.ADC_A_E:
                case Instruction.ADC_A_H:
                case Instruction.ADC_A_L:
                case Instruction.ADC_A_pHL:
                case Instruction.SUB_A:
                case Instruction.SUB_B:
                case Instruction.SUB_C:
                case Instruction.SUB_D:
                case Instruction.SUB_E:
                case Instruction.SUB_H:
                case Instruction.SUB_L:
                case Instruction.SUB_pHL:
                case Instruction.SBC_A_A:
                case Instruction.SBC_A_B:
                case Instruction.SBC_A_C:
                case Instruction.SBC_A_D:
                case Instruction.SBC_A_E:
                case Instruction.SBC_A_H:
                case Instruction.SBC_A_L:
                case Instruction.SBC_A_pHL:
                case Instruction.AND_A:
                case Instruction.AND_B:
                case Instruction.AND_C:
                case Instruction.AND_D:
                case Instruction.AND_E:
                case Instruction.AND_H:
                case Instruction.AND_L:
                case Instruction.AND_pHL:
                case Instruction.XOR_A:
                case Instruction.XOR_B:
                case Instruction.XOR_C:
                case Instruction.XOR_D:
                case Instruction.XOR_E:
                case Instruction.XOR_H:
                case Instruction.XOR_L:
                case Instruction.XOR_pHL:
                case Instruction.OR_A:
                case Instruction.OR_B:
                case Instruction.OR_C:
                case Instruction.OR_D:
                case Instruction.OR_E:
                case Instruction.OR_H:
                case Instruction.OR_L:
                case Instruction.OR_pHL:
                case Instruction.CP_A:
                case Instruction.CP_B:
                case Instruction.CP_C:
                case Instruction.CP_D:
                case Instruction.CP_E:
                case Instruction.CP_H:
                case Instruction.CP_L:
                case Instruction.CP_pHL:
                case Instruction.POP_AF:
                case Instruction.POP_BC:
                case Instruction.POP_DE:
                case Instruction.POP_HL:
                case Instruction.PUSH_AF:
                case Instruction.PUSH_BC:
                case Instruction.PUSH_DE:
                case Instruction.PUSH_HL:
                case Instruction.RET:
                case Instruction.RETI:
                case Instruction.RET_Z:
                case Instruction.RET_C:
                case Instruction.RET_NZ:
                case Instruction.RET_NC:
                case Instruction.RLA:
                case Instruction.RLCA:
                    nextOffset = x + 1;
                    return i.ToString();

                case Instruction.LD_A_d8:
                case Instruction.LD_B_d8:
                case Instruction.LD_C_d8:
                case Instruction.LD_D_d8:
                case Instruction.LD_E_d8:
                case Instruction.LD_H_d8:
                case Instruction.LD_L_d8:
                case Instruction.LD_pHL_d8:
                case Instruction.LD_A_pC:
                case Instruction.LD_pC_A:
                case Instruction.LDH_A_pa8:
                case Instruction.LDH_pa8_A:
                case Instruction.JR_NZ_r8:
                case Instruction.JR_NC_r8:
                case Instruction.CP_d8:
                case Instruction.JR_r8:
                case Instruction.JR_Z_r8:
                case Instruction.JR_C_r8:
                case Instruction.ADC_A_d8:
                case Instruction.SBC_A_d8:
                case Instruction.XOR_d8:
                case Instruction.AND_d8:
                    nextOffset = x + 2;
                    return i.ToString();

                case Instruction.LD_A_pa16:
                case Instruction.LD_pa16_A:
                case Instruction.LD_pa16_SP:
                case Instruction.LD_BC_d16:
                case Instruction.LD_DE_d16:
                case Instruction.LD_HL_d16:
                case Instruction.LD_SP_d16:
                case Instruction.CALL_a16:
                case Instruction.CALL_C_a16:
                case Instruction.CALL_NC_a16:
                case Instruction.CALL_Z_a16:
                case Instruction.CALL_NZ_a16:
                    nextOffset = x + 3;
                    return i.ToString();

                    

                default: throw new NotImplementedException($"Instruction: {i}");
            }
        }
    }
}