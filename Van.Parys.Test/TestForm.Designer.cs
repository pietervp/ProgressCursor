﻿namespace Van.Parys.Test
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.changeCursorButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // changeCursorButton
            // 
            this.changeCursorButton.Location = new System.Drawing.Point(12, 12);
            this.changeCursorButton.Name = "changeCursorButton";
            this.changeCursorButton.Size = new System.Drawing.Size(95, 23);
            this.changeCursorButton.TabIndex = 0;
            this.changeCursorButton.Text = "ChangeCursor";
            this.changeCursorButton.UseVisualStyleBackColor = true;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.changeCursorButton);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button changeCursorButton;
    }
}