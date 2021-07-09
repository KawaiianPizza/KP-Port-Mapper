
namespace KP_Port_Mapper
{
    partial class formKPPortMapper
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelPublicIP = new System.Windows.Forms.Label();
            this.dataGridPortsView = new System.Windows.Forms.DataGridView();
            this.labelPrivateIP = new System.Windows.Forms.Label();
            this.textboxPrivatePortMin = new System.Windows.Forms.TextBox();
            this.textboxPrivatePortMax = new System.Windows.Forms.TextBox();
            this.textboxPublicPortMin = new System.Windows.Forms.TextBox();
            this.textboxPublicPortMax = new System.Windows.Forms.TextBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelPrivatePorts = new System.Windows.Forms.Label();
            this.labelPublicPorts = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelPrivatePortsSeparator = new System.Windows.Forms.Label();
            this.labelPublicPortsSeparator = new System.Windows.Forms.Label();
            this.buttonOpenPort = new System.Windows.Forms.Button();
            this.checkBoxTCP = new System.Windows.Forms.CheckBox();
            this.checkBoxUDP = new System.Windows.Forms.CheckBox();
            this.dataGridSuggestionView = new System.Windows.Forms.DataGridView();
            this.labelSuggestion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPortsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSuggestionView)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPublicIP
            // 
            this.labelPublicIP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPublicIP.AutoSize = true;
            this.labelPublicIP.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelPublicIP.Location = new System.Drawing.Point(162, 9);
            this.labelPublicIP.Name = "labelPublicIP";
            this.labelPublicIP.Size = new System.Drawing.Size(225, 19);
            this.labelPublicIP.TabIndex = 0;
            this.labelPublicIP.Text = "IP Address is: 127.0.0.1";
            // 
            // dataGridPortsView
            // 
            this.dataGridPortsView.AllowUserToResizeColumns = false;
            this.dataGridPortsView.AllowUserToResizeRows = false;
            this.dataGridPortsView.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridPortsView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridPortsView.ColumnHeadersHeight = 46;
            this.dataGridPortsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridPortsView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridPortsView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridPortsView.EnableHeadersVisualStyles = false;
            this.dataGridPortsView.Location = new System.Drawing.Point(12, 57);
            this.dataGridPortsView.Name = "dataGridPortsView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridPortsView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridPortsView.RowHeadersWidth = 24;
            this.dataGridPortsView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridPortsView.RowTemplate.Height = 25;
            this.dataGridPortsView.RowTemplate.ReadOnly = true;
            this.dataGridPortsView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridPortsView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridPortsView.Size = new System.Drawing.Size(536, 222);
            this.dataGridPortsView.TabIndex = 1;
            this.dataGridPortsView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridPortsView_CellFormatting);
            // 
            // labelPrivateIP
            // 
            this.labelPrivateIP.AutoSize = true;
            this.labelPrivateIP.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelPrivateIP.Location = new System.Drawing.Point(232, 33);
            this.labelPrivateIP.Name = "labelPrivateIP";
            this.labelPrivateIP.Size = new System.Drawing.Size(154, 15);
            this.labelPrivateIP.TabIndex = 2;
            this.labelPrivateIP.Text = "Private IP: 127.0.0.1";
            // 
            // textboxPrivatePortMin
            // 
            this.textboxPrivatePortMin.Location = new System.Drawing.Point(24, 326);
            this.textboxPrivatePortMin.MaxLength = 5;
            this.textboxPrivatePortMin.Name = "textboxPrivatePortMin";
            this.textboxPrivatePortMin.Size = new System.Drawing.Size(60, 26);
            this.textboxPrivatePortMin.TabIndex = 3;
            this.textboxPrivatePortMin.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
            this.textboxPrivatePortMin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Textbox_KeyDown);
            // 
            // textboxPrivatePortMax
            // 
            this.textboxPrivatePortMax.Location = new System.Drawing.Point(110, 326);
            this.textboxPrivatePortMax.MaxLength = 5;
            this.textboxPrivatePortMax.Name = "textboxPrivatePortMax";
            this.textboxPrivatePortMax.Size = new System.Drawing.Size(60, 26);
            this.textboxPrivatePortMax.TabIndex = 4;
            this.textboxPrivatePortMax.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
            this.textboxPrivatePortMax.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Textbox_KeyDown);
            // 
            // textboxPublicPortMin
            // 
            this.textboxPublicPortMin.Location = new System.Drawing.Point(24, 378);
            this.textboxPublicPortMin.MaxLength = 5;
            this.textboxPublicPortMin.Name = "textboxPublicPortMin";
            this.textboxPublicPortMin.Size = new System.Drawing.Size(60, 26);
            this.textboxPublicPortMin.TabIndex = 5;
            this.textboxPublicPortMin.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
            this.textboxPublicPortMin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Textbox_KeyDown);
            // 
            // textboxPublicPortMax
            // 
            this.textboxPublicPortMax.Location = new System.Drawing.Point(110, 378);
            this.textboxPublicPortMax.MaxLength = 5;
            this.textboxPublicPortMax.Name = "textboxPublicPortMax";
            this.textboxPublicPortMax.Size = new System.Drawing.Size(60, 26);
            this.textboxPublicPortMax.TabIndex = 6;
            this.textboxPublicPortMax.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
            this.textboxPublicPortMax.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Textbox_KeyDown);
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Location = new System.Drawing.Point(24, 430);
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(146, 26);
            this.textBoxDescription.TabIndex = 7;
            // 
            // labelPrivatePorts
            // 
            this.labelPrivatePorts.AutoSize = true;
            this.labelPrivatePorts.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelPrivatePorts.Location = new System.Drawing.Point(49, 308);
            this.labelPrivatePorts.Name = "labelPrivatePorts";
            this.labelPrivatePorts.Size = new System.Drawing.Size(98, 15);
            this.labelPrivatePorts.TabIndex = 8;
            this.labelPrivatePorts.Text = "Private Port:";
            // 
            // labelPublicPorts
            // 
            this.labelPublicPorts.AutoSize = true;
            this.labelPublicPorts.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelPublicPorts.Location = new System.Drawing.Point(49, 360);
            this.labelPublicPorts.Name = "labelPublicPorts";
            this.labelPublicPorts.Size = new System.Drawing.Size(91, 15);
            this.labelPublicPorts.TabIndex = 9;
            this.labelPublicPorts.Text = "Public Port:";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDescription.Location = new System.Drawing.Point(49, 412);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(91, 15);
            this.labelDescription.TabIndex = 10;
            this.labelDescription.Text = "Description:";
            // 
            // labelPrivatePortsSeparator
            // 
            this.labelPrivatePortsSeparator.AutoSize = true;
            this.labelPrivatePortsSeparator.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelPrivatePortsSeparator.Location = new System.Drawing.Point(90, 332);
            this.labelPrivatePortsSeparator.Name = "labelPrivatePortsSeparator";
            this.labelPrivatePortsSeparator.Size = new System.Drawing.Size(14, 15);
            this.labelPrivatePortsSeparator.TabIndex = 11;
            this.labelPrivatePortsSeparator.Text = "-";
            // 
            // labelPublicPortsSeparator
            // 
            this.labelPublicPortsSeparator.AutoSize = true;
            this.labelPublicPortsSeparator.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelPublicPortsSeparator.Location = new System.Drawing.Point(90, 384);
            this.labelPublicPortsSeparator.Name = "labelPublicPortsSeparator";
            this.labelPublicPortsSeparator.Size = new System.Drawing.Size(14, 15);
            this.labelPublicPortsSeparator.TabIndex = 12;
            this.labelPublicPortsSeparator.Text = "-";
            // 
            // buttonOpenPort
            // 
            this.buttonOpenPort.Location = new System.Drawing.Point(24, 491);
            this.buttonOpenPort.Name = "buttonOpenPort";
            this.buttonOpenPort.Size = new System.Drawing.Size(146, 28);
            this.buttonOpenPort.TabIndex = 13;
            this.buttonOpenPort.Text = "Add Port";
            this.buttonOpenPort.UseVisualStyleBackColor = true;
            this.buttonOpenPort.Click += new System.EventHandler(this.ButtonOpenPort_Click);
            // 
            // checkBoxTCP
            // 
            this.checkBoxTCP.AutoSize = true;
            this.checkBoxTCP.Checked = true;
            this.checkBoxTCP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTCP.Location = new System.Drawing.Point(24, 462);
            this.checkBoxTCP.Name = "checkBoxTCP";
            this.checkBoxTCP.Size = new System.Drawing.Size(55, 23);
            this.checkBoxTCP.TabIndex = 14;
            this.checkBoxTCP.Text = "TCP";
            this.checkBoxTCP.UseVisualStyleBackColor = true;
            // 
            // checkBoxUDP
            // 
            this.checkBoxUDP.AutoSize = true;
            this.checkBoxUDP.Checked = true;
            this.checkBoxUDP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUDP.Location = new System.Drawing.Point(115, 462);
            this.checkBoxUDP.Name = "checkBoxUDP";
            this.checkBoxUDP.Size = new System.Drawing.Size(55, 23);
            this.checkBoxUDP.TabIndex = 15;
            this.checkBoxUDP.Text = "UDP";
            this.checkBoxUDP.UseVisualStyleBackColor = true;
            // 
            // dataGridSuggestionView
            // 
            this.dataGridSuggestionView.AllowUserToAddRows = false;
            this.dataGridSuggestionView.AllowUserToDeleteRows = false;
            this.dataGridSuggestionView.AllowUserToResizeRows = false;
            this.dataGridSuggestionView.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridSuggestionView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSuggestionView.Location = new System.Drawing.Point(232, 326);
            this.dataGridSuggestionView.MultiSelect = false;
            this.dataGridSuggestionView.Name = "dataGridSuggestionView";
            this.dataGridSuggestionView.RowHeadersVisible = false;
            this.dataGridSuggestionView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridSuggestionView.RowTemplate.Height = 25;
            this.dataGridSuggestionView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridSuggestionView.Size = new System.Drawing.Size(316, 217);
            this.dataGridSuggestionView.TabIndex = 16;
            // 
            // labelSuggestion
            // 
            this.labelSuggestion.AutoSize = true;
            this.labelSuggestion.Location = new System.Drawing.Point(312, 294);
            this.labelSuggestion.Name = "labelSuggestion";
            this.labelSuggestion.Size = new System.Drawing.Size(153, 19);
            this.labelSuggestion.TabIndex = 17;
            this.labelSuggestion.Text = "Suggested Ports:";
            // 
            // formKPPortMapper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 555);
            this.Controls.Add(this.labelSuggestion);
            this.Controls.Add(this.dataGridSuggestionView);
            this.Controls.Add(this.checkBoxUDP);
            this.Controls.Add(this.checkBoxTCP);
            this.Controls.Add(this.buttonOpenPort);
            this.Controls.Add(this.labelPublicPortsSeparator);
            this.Controls.Add(this.labelPrivatePortsSeparator);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelPublicPorts);
            this.Controls.Add(this.labelPrivatePorts);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.textboxPublicPortMax);
            this.Controls.Add(this.textboxPublicPortMin);
            this.Controls.Add(this.textboxPrivatePortMax);
            this.Controls.Add(this.textboxPrivatePortMin);
            this.Controls.Add(this.labelPrivateIP);
            this.Controls.Add(this.dataGridPortsView);
            this.Controls.Add(this.labelPublicIP);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "formKPPortMapper";
            this.Text = "KP Port Mapper";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPortsView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSuggestionView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

