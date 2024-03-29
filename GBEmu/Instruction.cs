﻿namespace GBEmu {
    public enum Instruction {        
        NOP = 0x00,
        LD_BC_d16 = 0x01,
        LD_pBC_A = 0x02,
        INC_BC = 0x03,
        INC_B = 0x04,
        DEC_B = 0x05,
        LD_B_d8 = 0x06,
        RLCA = 0x07,
        LD_pa16_SP = 0x08,
        ADD_HL_BC = 0x09,
        LD_A_pBC = 0x0A,
        DEC_BC = 0x0B,
        INC_C = 0x0C,
        DEC_C = 0x0D,
        LD_C_d8 = 0x0E,
        RRCA = 0x0F,

        STOP = 0x10,
        LD_DE_d16 = 0x11,
        LD_pDE_A = 0x12,
        INC_DE = 0x13,
        INC_D = 0x14,
        DEC_D = 0x15,
        LD_D_d8 = 0x16,
        RLA = 0x17,
        JR_r8 = 0x18,
        ADD_HL_DE = 0x19,
        LD_A_pDE = 0x1A,
        DEC_DE = 0x1B,
        INC_E = 0x1C,
        DEC_E = 0x1D,
        LD_E_d8 = 0x1E,
        RRA = 0x1F,

        JR_NZ_r8 = 0x20,
        LD_HL_d16 = 0x21,
        LD_pHLplus_A = 0x22,
        INC_HL = 0x23,
        INC_H = 0x24,
        DEC_H = 0x25,
        LD_H_d8 = 0x26,
        DAA = 0x27,
        JR_Z_r8 = 0x28,
        ADD_HL_HL = 0x29,
        LD_A_pHLplus = 0x2A,
        DEC_HL = 0x2B,
        INC_L = 0x2C,
        DEC_L = 0x2D,
        LD_L_d8 = 0x2E,
        CPL = 0x2F,

        JR_NC_r8 = 0x30,
        LD_SP_d16 = 0x31,
        LD_pHLminus_A = 0x32,
        INC_SP = 0x33,
        INC_pHL = 0x34,
        DEC_pHL = 0x35,
        LD_pHL_d8 = 0x36,
        SCF = 0x37,
        JR_C_r8 = 0x38,
        ADD_HL_SP = 0x39,
        LD_A_pHLminus = 0x3A,
        DEC_SP = 0x3B,
        INC_A = 0x3C,
        DEC_A = 0x3D,
        LD_A_d8 = 0x3E,
        CCF = 0x3F,

        LD_B_B = 0x40,
        LD_B_C = 0x41,
        LD_B_D = 0x42,
        LD_B_E = 0x43,
        LD_B_H = 0x44,
        LD_B_L = 0x45,
        LD_B_pHL = 0x46,
        LD_B_A = 0x47,
        LD_C_B = 0x48,
        LD_C_C = 0x49,
        LD_C_D = 0x4A,
        LD_C_E = 0x4B,
        LD_C_H = 0x4C,
        LD_C_L = 0x4D,
        LD_C_pHL = 0x4E,
        LD_C_A = 0x4F,

        LD_D_B = 0x50,
        LD_D_C = 0x51,
        LD_D_D = 0x52,
        LD_D_E = 0x53,
        LD_D_H = 0x54,
        LD_D_L = 0x55,
        LD_D_pHL = 0x56,
        LD_D_A = 0x57,
        LD_E_B = 0x58,
        LD_E_C = 0x59,
        LD_E_D = 0x5A,
        LD_E_E = 0x5B,
        LD_E_H = 0x5C,
        LD_E_L = 0x5D,
        LD_E_pHL = 0x5E,
        LD_E_A = 0x5F,

        LD_H_B = 0x60,
        LD_H_C = 0x61,
        LD_H_D = 0x62,
        LD_H_E = 0x63,
        LD_H_H = 0x64,
        LD_H_L = 0x65,
        LD_H_pHL = 0x66,
        LD_H_A = 0x67,
        LD_L_B = 0x68,
        LD_L_C = 0x69,
        LD_L_D = 0x6A,
        LD_L_E = 0x6B,
        LD_L_H = 0x6C,
        LD_L_L = 0x6D,
        LD_L_pHL = 0x6E,
        LD_L_A = 0x6F,

        LD_pHL_B = 0x70,
        LD_pHL_C = 0x71,
        LD_pHL_D = 0x72,
        LD_pHL_E = 0x73,
        LD_pHL_H = 0x74,
        LD_pHL_L = 0x75,
        HALT = 0x76,
        LD_pHL_A = 0x77,
        LD_A_B = 0x78,
        LD_A_C = 0x79,
        LD_A_D = 0x7A,
        LD_A_E = 0x7B,
        LD_A_H = 0x7C,
        LD_A_L = 0x7D,
        LD_A_pHL = 0x7E,
        LD_A_A = 0x7F,

        ADD_A_B = 0x80,
        ADD_A_C = 0x81,
        ADD_A_D = 0x82,
        ADD_A_E = 0x83,
        ADD_A_H = 0x84,
        ADD_A_L = 0x85,
        ADD_A_pHL = 0x86,
        ADD_A_A = 0x87,
        ADC_A_B = 0x88,
        ADC_A_C = 0x89,
        ADC_A_D = 0x8A,
        ADC_A_E = 0x8B,
        ADC_A_H = 0x8C,
        ADC_A_L = 0x8D,
        ADC_A_pHL = 0x8E,
        ADC_A_A = 0x8F,

        SUB_B = 0x90,
        SUB_C = 0x91,
        SUB_D = 0x92,
        SUB_E = 0x93,
        SUB_H = 0x94,
        SUB_L = 0x95,
        SUB_pHL = 0x96,
        SUB_A = 0x97,
        SBC_A_B = 0x98,
        SBC_A_C = 0x99,
        SBC_A_D = 0x9A,
        SBC_A_E = 0x9B,
        SBC_A_H = 0x9C,
        SBC_A_L = 0x9D,
        SBC_A_pHL = 0x9E,
        SBC_A_A = 0x9F,

        AND_B = 0xA0,
        AND_C = 0xA1,
        AND_D = 0xA2,
        AND_E = 0xA3,
        AND_H = 0xA4,
        AND_L = 0xA5,
        AND_pHL = 0xA6,
        AND_A = 0xA7,
        XOR_B = 0xA8,
        XOR_C = 0xA9,
        XOR_D = 0xAA,
        XOR_E = 0xAB,
        XOR_H = 0xAC,
        XOR_L = 0xAD,
        XOR_pHL = 0xAE,
        XOR_A = 0xAF,

        OR_B = 0xB0,
        OR_C = 0xB1,
        OR_D = 0xB2,
        OR_E = 0xB3,
        OR_H = 0xB4,
        OR_L = 0xB5,
        OR_pHL = 0xB6,
        OR_A = 0xB7,
        CP_B = 0xB8,
        CP_C = 0xB9,
        CP_D = 0xBA,
        CP_E = 0xBB,
        CP_H = 0xBC,
        CP_L = 0xBD,
        CP_pHL = 0xBE,
        CP_A = 0xBF,

        RET_NZ = 0xC0,
        POP_BC = 0xC1,
        JP_NZ_a16 = 0xC2,
        JP_a16 = 0xC3,
        CALL_NZ_a16 = 0xC4,
        PUSH_BC = 0xC5,
        ADD_A_d8 = 0xC6,
        RST_00H = 0xC7,
        RET_Z = 0xC8,
        RET = 0xC9,
        JP_Z_a16 = 0xCA,
        PREFIX_CB = 0xCB,
        CALL_Z_a16 = 0xCC,
        CALL_a16 = 0xCD,
        ADC_A_d8 = 0xCE,
        RST_08H = 0xCF,

        RET_NC = 0xD0,
        POP_DE = 0xD1,
        JP_NC_a16 = 0xD2,
        UNUSED_0xD3 = 0xD3,
        CALL_NC_a16 = 0xD4,
        PUSH_DE = 0xD5,
        SUB_d8 = 0xD6,
        RST_10H = 0xD7,
        RET_C = 0xD8,
        RETI = 0xD9,
        JP_C_a16 = 0xDA,
        UNUSED_0xDB = 0xDB,
        CALL_C_a16 = 0xDC,
        UNUSED_0xDD = 0xDD,
        SBC_A_d8 = 0xDE,
        RST_18H = 0xDF,

        LDH_pa8_A = 0xE0,
        POP_HL = 0xE1,
        LD_pC_A = 0xE2,
        UNUSED_0xE3 = 0xE3,
        UNUSED_0xE4 = 0xE4,
        PUSH_HL = 0xE5,
        AND_d8 = 0xE6,
        RST_20H = 0xE7,
        ADD_SP_r8 = 0xE8,
        JP_pHL = 0xE9,
        LD_pa16_A = 0xEA,
        UNUSED_0xEB = 0xEB,
        UNUSED_0xEC = 0xEC,
        UNUSED_0xED = 0xED,
        XOR_d8 = 0xEE,
        RST_28H = 0xEF,

        LDH_A_pa8 = 0xF0,
        POP_AF = 0xF1,
        LD_A_pC = 0xF2,
        DI = 0xF3,
        UNUSED_0xF4 = 0xF4,
        PUSH_AF = 0xF5,
        OR_d8 = 0xF6,
        RST_30H = 0xF7,
        LD_HL_SPplusr8 = 0xF8,
        LD_SP_HL = 0xF9,
        LD_A_pa16 = 0xFA,
        EI = 0xFB,
        UNUSED_0xFC = 0xFC,
        UNUSED_0xFD = 0xFD,
        CP_d8 = 0xFE,
        RST_38H = 0xFF,
    }
}
