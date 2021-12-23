using GBEmu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GBEmuWinDesktopGUI {
    public partial class Form1 : Form {
        IMemory memory;
        IGbCpu GbCpu;

        public Form1() {
            InitializeComponent();
            memory = new Memory();
            GbCpu = new GbCpu(GbModel.DMG, memory);
        }

        private void tickButton_Click(object sender, EventArgs e) {
            GbCpu.Tick();
            memContentsTextBox.Text = memory.ContentsAsString;
        }
    }
}
