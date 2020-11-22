using System;

namespace GBEmu {
    public interface IMemory {
        byte ReadByte(ushort address);
        void WriteByte(ushort address, byte content);
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
                if(fileStream.Length > ( (BootRomHi + 1)- BootRomLo)) {
                    throw new ArgumentException("BootRom exceeds maximum length.");
                }
                fileStream.Read(storage, BootRomLo, BootRomHi + 1);
            }
        }
    }
}