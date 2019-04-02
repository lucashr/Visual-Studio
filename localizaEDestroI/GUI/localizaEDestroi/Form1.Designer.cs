namespace localizaEDestroi
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtCaminho = new System.Windows.Forms.TextBox();
            this.btnPesquisar = new System.Windows.Forms.Button();
            this.btnDeletar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.caminhoabs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtNomePasta = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtExtArq = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTotDir = new System.Windows.Forms.Label();
            this.lblTamTotDir = new System.Windows.Forms.Label();
            this.lblTamTotArq = new System.Windows.Forms.Label();
            this.lblTotArq = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtNomeArq = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(486, 74);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(112, 35);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtCaminho
            // 
            this.txtCaminho.Location = new System.Drawing.Point(38, 78);
            this.txtCaminho.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCaminho.Name = "txtCaminho";
            this.txtCaminho.Size = new System.Drawing.Size(416, 26);
            this.txtCaminho.TabIndex = 1;
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.Location = new System.Drawing.Point(38, 140);
            this.btnPesquisar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Size = new System.Drawing.Size(112, 35);
            this.btnPesquisar.TabIndex = 2;
            this.btnPesquisar.Text = "Pesquisar";
            this.btnPesquisar.UseVisualStyleBackColor = true;
            this.btnPesquisar.Click += new System.EventHandler(this.btnPesquisar_Click);
            // 
            // btnDeletar
            // 
            this.btnDeletar.Location = new System.Drawing.Point(38, 308);
            this.btnDeletar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDeletar.Name = "btnDeletar";
            this.btnDeletar.Size = new System.Drawing.Size(112, 35);
            this.btnDeletar.TabIndex = 3;
            this.btnDeletar.Text = "Deletar";
            this.btnDeletar.UseVisualStyleBackColor = true;
            this.btnDeletar.Click += new System.EventHandler(this.btnDeletar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 54);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(182, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Selecione o diretorio raiz";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.caminhoabs});
            this.dgv.Location = new System.Drawing.Point(38, 352);
            this.dgv.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.Size = new System.Drawing.Size(1126, 275);
            this.dgv.TabIndex = 7;
            // 
            // caminhoabs
            // 
            this.caminhoabs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.caminhoabs.HeaderText = "Caminho absoluto";
            this.caminhoabs.Name = "caminhoabs";
            this.caminhoabs.ReadOnly = true;
            // 
            // txtNomePasta
            // 
            this.txtNomePasta.Location = new System.Drawing.Point(38, 225);
            this.txtNomePasta.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNomePasta.Name = "txtNomePasta";
            this.txtNomePasta.Size = new System.Drawing.Size(416, 26);
            this.txtNomePasta.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 200);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Delete arquivos por nome de pasta";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(548, 200);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(215, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Delete arquivos por extensão";
            // 
            // txtExtArq
            // 
            this.txtExtArq.Location = new System.Drawing.Point(552, 225);
            this.txtExtArq.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtExtArq.Name = "txtExtArq";
            this.txtExtArq.Size = new System.Drawing.Size(416, 26);
            this.txtExtArq.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(674, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(187, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Quantidade de diretorios:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(674, 54);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(181, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Quantidade de arquivos:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(674, 89);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(214, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Tamanho total dos diretorios:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(674, 126);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(208, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Tamanho total dos arquivos:";
            // 
            // lblTotDir
            // 
            this.lblTotDir.AutoSize = true;
            this.lblTotDir.Location = new System.Drawing.Point(870, 14);
            this.lblTotDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotDir.Name = "lblTotDir";
            this.lblTotDir.Size = new System.Drawing.Size(24, 20);
            this.lblTotDir.TabIndex = 16;
            this.lblTotDir.Text = "---";
            // 
            // lblTamTotDir
            // 
            this.lblTamTotDir.AutoSize = true;
            this.lblTamTotDir.Location = new System.Drawing.Point(894, 89);
            this.lblTamTotDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTamTotDir.Name = "lblTamTotDir";
            this.lblTamTotDir.Size = new System.Drawing.Size(24, 20);
            this.lblTamTotDir.TabIndex = 17;
            this.lblTamTotDir.Text = "---";
            // 
            // lblTamTotArq
            // 
            this.lblTamTotArq.AutoSize = true;
            this.lblTamTotArq.Location = new System.Drawing.Point(894, 126);
            this.lblTamTotArq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTamTotArq.Name = "lblTamTotArq";
            this.lblTamTotArq.Size = new System.Drawing.Size(24, 20);
            this.lblTamTotArq.TabIndex = 18;
            this.lblTamTotArq.Text = "---";
            // 
            // lblTotArq
            // 
            this.lblTotArq.AutoSize = true;
            this.lblTotArq.Location = new System.Drawing.Point(870, 54);
            this.lblTotArq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotArq.Name = "lblTotArq";
            this.lblTotArq.Size = new System.Drawing.Size(24, 20);
            this.lblTotArq.TabIndex = 19;
            this.lblTotArq.Text = "---";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(1068, 14);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(24, 20);
            this.lblStatus.TabIndex = 20;
            this.lblStatus.Text = "---";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1004, 14);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 20);
            this.label8.TabIndex = 21;
            this.label8.Text = "Status:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(34, 256);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(190, 20);
            this.label9.TabIndex = 23;
            this.label9.Text = "Delete arquivos por nome";
            // 
            // txtNomeArq
            // 
            this.txtNomeArq.Location = new System.Drawing.Point(39, 281);
            this.txtNomeArq.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNomeArq.Name = "txtNomeArq";
            this.txtNomeArq.Size = new System.Drawing.Size(416, 26);
            this.txtNomeArq.TabIndex = 22;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtNomeArq);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTotArq);
            this.Controls.Add(this.lblTamTotArq);
            this.Controls.Add(this.lblTamTotDir);
            this.Controls.Add(this.lblTotDir);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtExtArq);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNomePasta);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnDeletar);
            this.Controls.Add(this.btnPesquisar);
            this.Controls.Add(this.txtCaminho);
            this.Controls.Add(this.btnBrowse);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "LocalizaEDestroI";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtCaminho;
        private System.Windows.Forms.Button btnPesquisar;
        private System.Windows.Forms.Button btnDeletar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.TextBox txtNomePasta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtExtArq;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTotDir;
        private System.Windows.Forms.Label lblTamTotDir;
        private System.Windows.Forms.Label lblTamTotArq;
        private System.Windows.Forms.Label lblTotArq;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn caminhoabs;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtNomeArq;
    }
}