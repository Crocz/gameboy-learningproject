
namespace GBEmuWinDesktopGUI {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.memContentsTextBox = new System.Windows.Forms.RichTextBox();
            this.tickButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // memContentsTextBox
            // 
            this.memContentsTextBox.Location = new System.Drawing.Point(12, 12);
            this.memContentsTextBox.Name = "memContentsTextBox";
            this.memContentsTextBox.Size = new System.Drawing.Size(873, 596);
            this.memContentsTextBox.TabIndex = 0;
            this.memContentsTextBox.Text = "";
            // 
            // tickButton
            // 
            this.tickButton.Location = new System.Drawing.Point(31, 625);
            this.tickButton.Name = "tickButton";
            this.tickButton.Size = new System.Drawing.Size(116, 88);
            this.tickButton.TabIndex = 1;
            this.tickButton.Text = "Tick";
            this.tickButton.UseVisualStyleBackColor = true;
            this.tickButton.Click += new System.EventHandler(this.tickButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 729);
            this.Controls.Add(this.tickButton);
            this.Controls.Add(this.memContentsTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox memContentsTextBox;
        private System.Windows.Forms.Button tickButton;
    }
}

