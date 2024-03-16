
using System.Windows.Forms;

namespace KP_Port_Mapper
{
    partial class FormKPPortMapper
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormKPPortMapper));
            labelPublicIP = new Label();
            dataGridPortsView = new DataGridView();
            labelPrivateIP = new Label();
            textboxPrivatePortMin = new TextBox();
            textboxPrivatePortMax = new TextBox();
            textboxPublicPortMin = new TextBox();
            textboxPublicPortMax = new TextBox();
            textBoxDescription = new TextBox();
            labelPrivatePorts = new Label();
            labelPublicPorts = new Label();
            labelDescription = new Label();
            labelPrivatePortsSeparator = new Label();
            labelPublicPortsSeparator = new Label();
            buttonOpenPort = new Button();
            checkBoxTCP = new CheckBox();
            checkBoxUDP = new CheckBox();
            dataGridSuggestionView = new DataGridView();
            labelSuggestion = new Label();
            hideApplicationButton = new Button();
            labelNATPMP = new Label();
            labelDisabled = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridPortsView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridSuggestionView).BeginInit();
            SuspendLayout();
            // 
            // labelPublicIP
            // 
            labelPublicIP.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelPublicIP.AutoSize = true;
            labelPublicIP.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            labelPublicIP.Location = new System.Drawing.Point(162, 9);
            labelPublicIP.Name = "labelPublicIP";
            labelPublicIP.Size = new System.Drawing.Size(225, 19);
            labelPublicIP.TabIndex = 0;
            labelPublicIP.Text = "IP Address is: 127.0.0.1";
            labelPublicIP.DoubleClick += CopyIPFromLabel_DoubleClick;
            // 
            // dataGridPortsView
            // 
            dataGridPortsView.AllowUserToResizeColumns = false;
            dataGridPortsView.AllowUserToResizeRows = false;
            dataGridPortsView.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Consolas", 12F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dataGridPortsView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridPortsView.ColumnHeadersHeight = 46;
            dataGridPortsView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Consolas", 12F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dataGridPortsView.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridPortsView.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridPortsView.EnableHeadersVisualStyles = false;
            dataGridPortsView.Location = new System.Drawing.Point(12, 57);
            dataGridPortsView.Name = "dataGridPortsView";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Consolas", 12F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dataGridPortsView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dataGridPortsView.RowHeadersWidth = 24;
            dataGridPortsView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridPortsView.RowTemplate.ReadOnly = true;
            dataGridPortsView.ScrollBars = ScrollBars.Vertical;
            dataGridPortsView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridPortsView.Size = new System.Drawing.Size(536, 222);
            dataGridPortsView.TabIndex = 1;
            dataGridPortsView.CellDoubleClick += DataGridPortsView_CellDoubleClick;
            dataGridPortsView.CellFormatting += DataGridPortsView_CellFormatting;
            dataGridPortsView.UserDeletingRow += DataGridPortsView_UserDeletingRow;
            // 
            // labelPrivateIP
            // 
            labelPrivateIP.AutoSize = true;
            labelPrivateIP.Font = new System.Drawing.Font("Consolas", 9.75F);
            labelPrivateIP.Location = new System.Drawing.Point(232, 33);
            labelPrivateIP.Name = "labelPrivateIP";
            labelPrivateIP.Size = new System.Drawing.Size(154, 15);
            labelPrivateIP.TabIndex = 2;
            labelPrivateIP.Text = "Private IP: 127.0.0.1";
            labelPrivateIP.DoubleClick += CopyIPFromLabel_DoubleClick;
            // 
            // textboxPrivatePortMin
            // 
            textboxPrivatePortMin.Location = new System.Drawing.Point(24, 326);
            textboxPrivatePortMin.MaxLength = 5;
            textboxPrivatePortMin.Name = "textboxPrivatePortMin";
            textboxPrivatePortMin.Size = new System.Drawing.Size(60, 26);
            textboxPrivatePortMin.TabIndex = 3;
            textboxPrivatePortMin.TextChanged += Textbox_TextChanged;
            textboxPrivatePortMin.KeyDown += Textbox_KeyDown;
            // 
            // textboxPrivatePortMax
            // 
            textboxPrivatePortMax.Location = new System.Drawing.Point(110, 326);
            textboxPrivatePortMax.MaxLength = 5;
            textboxPrivatePortMax.Name = "textboxPrivatePortMax";
            textboxPrivatePortMax.Size = new System.Drawing.Size(60, 26);
            textboxPrivatePortMax.TabIndex = 4;
            textboxPrivatePortMax.TextChanged += Textbox_TextChanged;
            textboxPrivatePortMax.KeyDown += Textbox_KeyDown;
            // 
            // textboxPublicPortMin
            // 
            textboxPublicPortMin.Location = new System.Drawing.Point(24, 378);
            textboxPublicPortMin.MaxLength = 5;
            textboxPublicPortMin.Name = "textboxPublicPortMin";
            textboxPublicPortMin.Size = new System.Drawing.Size(60, 26);
            textboxPublicPortMin.TabIndex = 5;
            textboxPublicPortMin.TextChanged += Textbox_TextChanged;
            textboxPublicPortMin.KeyDown += Textbox_KeyDown;
            // 
            // textboxPublicPortMax
            // 
            textboxPublicPortMax.Location = new System.Drawing.Point(110, 378);
            textboxPublicPortMax.MaxLength = 5;
            textboxPublicPortMax.Name = "textboxPublicPortMax";
            textboxPublicPortMax.Size = new System.Drawing.Size(60, 26);
            textboxPublicPortMax.TabIndex = 6;
            textboxPublicPortMax.TextChanged += Textbox_TextChanged;
            textboxPublicPortMax.KeyDown += Textbox_KeyDown;
            // 
            // textBoxDescription
            // 
            textBoxDescription.Location = new System.Drawing.Point(24, 430);
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.Size = new System.Drawing.Size(146, 26);
            textBoxDescription.TabIndex = 7;
            // 
            // labelPrivatePorts
            // 
            labelPrivatePorts.AutoSize = true;
            labelPrivatePorts.Font = new System.Drawing.Font("Consolas", 9.75F);
            labelPrivatePorts.Location = new System.Drawing.Point(49, 308);
            labelPrivatePorts.Name = "labelPrivatePorts";
            labelPrivatePorts.Size = new System.Drawing.Size(98, 15);
            labelPrivatePorts.TabIndex = 8;
            labelPrivatePorts.Text = "Private Port:";
            // 
            // labelPublicPorts
            // 
            labelPublicPorts.AutoSize = true;
            labelPublicPorts.Font = new System.Drawing.Font("Consolas", 9.75F);
            labelPublicPorts.Location = new System.Drawing.Point(49, 360);
            labelPublicPorts.Name = "labelPublicPorts";
            labelPublicPorts.Size = new System.Drawing.Size(91, 15);
            labelPublicPorts.TabIndex = 9;
            labelPublicPorts.Text = "Public Port:";
            // 
            // labelDescription
            // 
            labelDescription.AutoSize = true;
            labelDescription.Font = new System.Drawing.Font("Consolas", 9.75F);
            labelDescription.Location = new System.Drawing.Point(49, 412);
            labelDescription.Name = "labelDescription";
            labelDescription.Size = new System.Drawing.Size(91, 15);
            labelDescription.TabIndex = 10;
            labelDescription.Text = "Description:";
            // 
            // labelPrivatePortsSeparator
            // 
            labelPrivatePortsSeparator.AutoSize = true;
            labelPrivatePortsSeparator.Font = new System.Drawing.Font("Consolas", 9.75F);
            labelPrivatePortsSeparator.Location = new System.Drawing.Point(90, 332);
            labelPrivatePortsSeparator.Name = "labelPrivatePortsSeparator";
            labelPrivatePortsSeparator.Size = new System.Drawing.Size(14, 15);
            labelPrivatePortsSeparator.TabIndex = 11;
            labelPrivatePortsSeparator.Text = "-";
            // 
            // labelPublicPortsSeparator
            // 
            labelPublicPortsSeparator.AutoSize = true;
            labelPublicPortsSeparator.Font = new System.Drawing.Font("Consolas", 9.75F);
            labelPublicPortsSeparator.Location = new System.Drawing.Point(90, 384);
            labelPublicPortsSeparator.Name = "labelPublicPortsSeparator";
            labelPublicPortsSeparator.Size = new System.Drawing.Size(14, 15);
            labelPublicPortsSeparator.TabIndex = 12;
            labelPublicPortsSeparator.Text = "-";
            // 
            // buttonOpenPort
            // 
            buttonOpenPort.Location = new System.Drawing.Point(24, 491);
            buttonOpenPort.Name = "buttonOpenPort";
            buttonOpenPort.Size = new System.Drawing.Size(146, 28);
            buttonOpenPort.TabIndex = 13;
            buttonOpenPort.Text = "Add Port";
            buttonOpenPort.UseVisualStyleBackColor = true;
            buttonOpenPort.Click += ButtonOpenPort_Click;
            // 
            // checkBoxTCP
            // 
            checkBoxTCP.AutoSize = true;
            checkBoxTCP.Checked = true;
            checkBoxTCP.CheckState = CheckState.Checked;
            checkBoxTCP.Location = new System.Drawing.Point(24, 462);
            checkBoxTCP.Name = "checkBoxTCP";
            checkBoxTCP.Size = new System.Drawing.Size(55, 23);
            checkBoxTCP.TabIndex = 14;
            checkBoxTCP.Text = "TCP";
            checkBoxTCP.UseVisualStyleBackColor = true;
            // 
            // checkBoxUDP
            // 
            checkBoxUDP.AutoSize = true;
            checkBoxUDP.Checked = true;
            checkBoxUDP.CheckState = CheckState.Checked;
            checkBoxUDP.Location = new System.Drawing.Point(115, 462);
            checkBoxUDP.Name = "checkBoxUDP";
            checkBoxUDP.Size = new System.Drawing.Size(55, 23);
            checkBoxUDP.TabIndex = 15;
            checkBoxUDP.Text = "UDP";
            checkBoxUDP.UseVisualStyleBackColor = true;
            // 
            // dataGridSuggestionView
            // 
            dataGridSuggestionView.AllowUserToAddRows = false;
            dataGridSuggestionView.AllowUserToDeleteRows = false;
            dataGridSuggestionView.AllowUserToResizeRows = false;
            dataGridSuggestionView.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            dataGridSuggestionView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridSuggestionView.Location = new System.Drawing.Point(232, 326);
            dataGridSuggestionView.MultiSelect = false;
            dataGridSuggestionView.Name = "dataGridSuggestionView";
            dataGridSuggestionView.RowHeadersVisible = false;
            dataGridSuggestionView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridSuggestionView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridSuggestionView.Size = new System.Drawing.Size(316, 217);
            dataGridSuggestionView.TabIndex = 16;
            dataGridSuggestionView.CellDoubleClick += DataGridSuggestionView_CellDoubleClick;
            // 
            // labelSuggestion
            // 
            labelSuggestion.AutoSize = true;
            labelSuggestion.Location = new System.Drawing.Point(234, 299);
            labelSuggestion.Name = "labelSuggestion";
            labelSuggestion.Size = new System.Drawing.Size(153, 19);
            labelSuggestion.TabIndex = 17;
            labelSuggestion.Text = "Suggested Ports:";
            // 
            // hideApplicationButton
            // 
            hideApplicationButton.Location = new System.Drawing.Point(402, 294);
            hideApplicationButton.Name = "hideApplicationButton";
            hideApplicationButton.Size = new System.Drawing.Size(146, 28);
            hideApplicationButton.TabIndex = 18;
            hideApplicationButton.Text = "Add Port";
            hideApplicationButton.UseVisualStyleBackColor = true;
            hideApplicationButton.Visible = false;
            // 
            // labelNATPMP
            // 
            labelNATPMP.AutoSize = true;
            labelNATPMP.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Bold);
            labelNATPMP.ForeColor = System.Drawing.Color.Peru;
            labelNATPMP.Location = new System.Drawing.Point(9, 28);
            labelNATPMP.Name = "labelNATPMP";
            labelNATPMP.Size = new System.Drawing.Size(217, 26);
            labelNATPMP.TabIndex = 19;
            labelNATPMP.Text = "INFO: UPnP disabled on the router\r\nLimited functionality using NAT-PMP";
            // 
            // labelDisabled
            // 
            labelDisabled.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelDisabled.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelDisabled.Location = new System.Drawing.Point(9, 9);
            labelDisabled.Name = "labelDisabled";
            labelDisabled.Size = new System.Drawing.Size(539, 537);
            labelDisabled.TabIndex = 20;
            labelDisabled.Text = "UPnP and NAT-PMP is disabled on the router\r\nmaking this app is useless";
            labelDisabled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormKPPortMapper
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(560, 555);
            Controls.Add(labelDisabled);
            Controls.Add(labelNATPMP);
            Controls.Add(hideApplicationButton);
            Controls.Add(labelSuggestion);
            Controls.Add(dataGridSuggestionView);
            Controls.Add(checkBoxUDP);
            Controls.Add(checkBoxTCP);
            Controls.Add(buttonOpenPort);
            Controls.Add(labelPublicPortsSeparator);
            Controls.Add(labelPrivatePortsSeparator);
            Controls.Add(labelDescription);
            Controls.Add(labelPublicPorts);
            Controls.Add(labelPrivatePorts);
            Controls.Add(textBoxDescription);
            Controls.Add(textboxPublicPortMax);
            Controls.Add(textboxPublicPortMin);
            Controls.Add(textboxPrivatePortMax);
            Controls.Add(textboxPrivatePortMin);
            Controls.Add(labelPrivateIP);
            Controls.Add(dataGridPortsView);
            Controls.Add(labelPublicIP);
            Font = new System.Drawing.Font("Consolas", 12F);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            Name = "FormKPPortMapper";
            Text = "KP Port Mapper";
            Load += FormKPPortMapper_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridPortsView).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridSuggestionView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelPublicIP;
        private System.Windows.Forms.DataGridView dataGridPortsView;
        private System.Windows.Forms.Label labelPrivateIP;
        private System.Windows.Forms.TextBox textboxPrivatePortMin;
        private System.Windows.Forms.TextBox textboxPrivatePortMax;
        private System.Windows.Forms.TextBox textboxPublicPortMin;
        private System.Windows.Forms.TextBox textboxPublicPortMax;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelPrivatePorts;
        private System.Windows.Forms.Label labelPublicPorts;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelPrivatePortsSeparator;
        private System.Windows.Forms.Label labelPublicPortsSeparator;
        private System.Windows.Forms.Button buttonOpenPort;
        private System.Windows.Forms.CheckBox checkBoxTCP;
        private System.Windows.Forms.CheckBox checkBoxUDP;
        private System.Windows.Forms.DataGridView dataGridSuggestionView;
        private System.Windows.Forms.Label labelSuggestion;
        private Button hideApplicationButton;
        private Label labelNATPMP;
        private Label labelDisabled;
    }
}

