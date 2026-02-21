namespace IBIMCA.Forms.Bases
{
    partial class BaseEnterValue
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
            labelTooltip = new Label();
            textBox = new TextBox();
            btnCancel = new Button();
            btnSelect = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // labelTooltip
            // 
            labelTooltip.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelTooltip.AutoSize = true;
            labelTooltip.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelTooltip.Location = new System.Drawing.Point(24, 25);
            labelTooltip.Margin = new Padding(24, 25, 24, 6);
            labelTooltip.Name = "labelTooltip";
            labelTooltip.Size = new Size(650, 61);
            labelTooltip.TabIndex = 0;
            labelTooltip.Text = "Default Message";
            labelTooltip.TextAlign = ContentAlignment.MiddleCenter;
            labelTooltip.Click += labelTooltip_Click;
            // 
            // textBox
            // 
            textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox.Location = new System.Drawing.Point(24, 98);
            textBox.Margin = new Padding(24, 6, 24, 6);
            textBox.MaxLength = 100;
            textBox.Multiline = true;
            textBox.Name = "textBox";
            textBox.Size = new Size(650, 84);
            textBox.TabIndex = 1;
            textBox.TextAlign = HorizontalAlignment.Center;
            textBox.WordWrap = false;
            textBox.KeyPress += TextBox_KeyPress;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnCancel.AutoSize = true;
            btnCancel.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.Location = new System.Drawing.Point(331, 0);
            btnCancel.Margin = new Padding(6, 0, 0, 0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(319, 61);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSelect
            // 
            btnSelect.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnSelect.AutoSize = true;
            btnSelect.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSelect.Location = new System.Drawing.Point(0, 0);
            btnSelect.Margin = new Padding(0, 0, 6, 0);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(319, 61);
            btnSelect.TabIndex = 2;
            btnSelect.Text = "OK";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 2);
            tableLayoutPanel1.Controls.Add(textBox, 0, 1);
            tableLayoutPanel1.Controls.Add(labelTooltip, 0, 0);
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            tableLayoutPanel1.Size = new Size(698, 280);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(btnCancel, 1, 0);
            tableLayoutPanel2.Controls.Add(btnSelect, 0, 0);
            tableLayoutPanel2.Location = new System.Drawing.Point(24, 194);
            tableLayoutPanel2.Margin = new Padding(24, 6, 24, 25);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(650, 61);
            tableLayoutPanel2.TabIndex = 5;
            // 
            // BaseEnterValue
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(696, 278);
            Controls.Add(tableLayoutPanel1);
            ImeMode = ImeMode.Off;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(711, 313);
            Name = "BaseEnterValue";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Default Title";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTooltip;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelect;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
    }
}