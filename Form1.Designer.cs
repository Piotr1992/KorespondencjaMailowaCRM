namespace KorespondencjaMailowaCRM
{
    partial class Form1
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
            this.dsProperties = new KorespondencjaMailowaCRM.DataSet1();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Drukuj = new System.Windows.Forms.Button();
            this.btnZaznaczAll = new System.Windows.Forms.Button();
            this.btnOdznaczAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dsProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dsProperties
            // 
            this.dsProperties.DataSetName = "DataSet1";
            this.dsProperties.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 7);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1086, 425);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            // 
            // Drukuj
            // 
            this.Drukuj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Drukuj.Location = new System.Drawing.Point(866, 441);
            this.Drukuj.Name = "Drukuj";
            this.Drukuj.Size = new System.Drawing.Size(226, 48);
            this.Drukuj.TabIndex = 1;
            this.Drukuj.Text = "Drukuj";
            this.Drukuj.UseVisualStyleBackColor = true;
            this.Drukuj.Click += new System.EventHandler(this.DrukujButton_Click);
            // 
            // btnZaznaczAll
            // 
            this.btnZaznaczAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZaznaczAll.Location = new System.Drawing.Point(12, 438);
            this.btnZaznaczAll.Name = "btnZaznaczAll";
            this.btnZaznaczAll.Size = new System.Drawing.Size(104, 25);
            this.btnZaznaczAll.TabIndex = 2;
            this.btnZaznaczAll.Text = "Zaznacz wszystko";
            this.btnZaznaczAll.UseVisualStyleBackColor = true;
            this.btnZaznaczAll.Click += new System.EventHandler(this.btnZaznaczAll_Click);
            // 
            // btnOdznaczAll
            // 
            this.btnOdznaczAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOdznaczAll.Location = new System.Drawing.Point(12, 469);
            this.btnOdznaczAll.Name = "btnOdznaczAll";
            this.btnOdznaczAll.Size = new System.Drawing.Size(104, 25);
            this.btnOdznaczAll.TabIndex = 3;
            this.btnOdznaczAll.Text = "Odznacz wszystko";
            this.btnOdznaczAll.UseVisualStyleBackColor = true;
            this.btnOdznaczAll.Click += new System.EventHandler(this.btnOdznaczAll_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 500);
            this.Controls.Add(this.btnOdznaczAll);
            this.Controls.Add(this.btnZaznaczAll);
            this.Controls.Add(this.Drukuj);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Korespondencja mailowa CRM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dsProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataSet1 dsProperties;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button Drukuj;
        private System.Windows.Forms.Button btnZaznaczAll;
        private System.Windows.Forms.Button btnOdznaczAll;
    }
}

