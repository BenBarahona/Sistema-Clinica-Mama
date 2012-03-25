namespace TC
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
            this.btn_afiliado = new System.Windows.Forms.Button();
            this.btn_empleado = new System.Windows.Forms.Button();
            this.btn_interes = new System.Windows.Forms.Button();
            this.txtb_ingresar = new System.Windows.Forms.TextBox();
            this.btn_ocupacion = new System.Windows.Forms.Button();
            this.btn_motivo = new System.Windows.Forms.Button();
            this.btn_monto = new System.Windows.Forms.Button();
            this.btn_login = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_afiliado
            // 
            this.btn_afiliado.Location = new System.Drawing.Point(12, 12);
            this.btn_afiliado.Name = "btn_afiliado";
            this.btn_afiliado.Size = new System.Drawing.Size(75, 23);
            this.btn_afiliado.TabIndex = 0;
            this.btn_afiliado.Text = "Afiliado";
            this.btn_afiliado.UseVisualStyleBackColor = true;
            this.btn_afiliado.Click += new System.EventHandler(this.btn_afiliado_Click);
            // 
            // btn_empleado
            // 
            this.btn_empleado.Location = new System.Drawing.Point(93, 12);
            this.btn_empleado.Name = "btn_empleado";
            this.btn_empleado.Size = new System.Drawing.Size(75, 23);
            this.btn_empleado.TabIndex = 1;
            this.btn_empleado.Text = "Empleado";
            this.btn_empleado.UseVisualStyleBackColor = true;
            this.btn_empleado.Click += new System.EventHandler(this.btn_empleado_Click);
            // 
            // btn_interes
            // 
            this.btn_interes.Location = new System.Drawing.Point(12, 54);
            this.btn_interes.Name = "btn_interes";
            this.btn_interes.Size = new System.Drawing.Size(75, 23);
            this.btn_interes.TabIndex = 2;
            this.btn_interes.Text = "Interes";
            this.btn_interes.UseVisualStyleBackColor = true;
            this.btn_interes.Click += new System.EventHandler(this.btn_interes_Click);
            // 
            // txtb_ingresar
            // 
            this.txtb_ingresar.Location = new System.Drawing.Point(84, 217);
            this.txtb_ingresar.Name = "txtb_ingresar";
            this.txtb_ingresar.Size = new System.Drawing.Size(100, 20);
            this.txtb_ingresar.TabIndex = 3;
            // 
            // btn_ocupacion
            // 
            this.btn_ocupacion.Location = new System.Drawing.Point(93, 54);
            this.btn_ocupacion.Name = "btn_ocupacion";
            this.btn_ocupacion.Size = new System.Drawing.Size(75, 23);
            this.btn_ocupacion.TabIndex = 4;
            this.btn_ocupacion.Text = "Ocupacion";
            this.btn_ocupacion.UseVisualStyleBackColor = true;
            this.btn_ocupacion.Click += new System.EventHandler(this.btn_ocupacion_Click);
            // 
            // btn_motivo
            // 
            this.btn_motivo.Location = new System.Drawing.Point(174, 54);
            this.btn_motivo.Name = "btn_motivo";
            this.btn_motivo.Size = new System.Drawing.Size(75, 23);
            this.btn_motivo.TabIndex = 5;
            this.btn_motivo.Text = "Motivo";
            this.btn_motivo.UseVisualStyleBackColor = true;
            this.btn_motivo.Click += new System.EventHandler(this.btn_motivo_Click);
            // 
            // btn_monto
            // 
            this.btn_monto.Location = new System.Drawing.Point(12, 93);
            this.btn_monto.Name = "btn_monto";
            this.btn_monto.Size = new System.Drawing.Size(75, 23);
            this.btn_monto.TabIndex = 6;
            this.btn_monto.Text = "Monto";
            this.btn_monto.UseVisualStyleBackColor = true;
            this.btn_monto.Click += new System.EventHandler(this.btn_monto_Click);
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(93, 93);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(75, 23);
            this.btn_login.TabIndex = 7;
            this.btn_login.Text = "Login";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.btn_monto);
            this.Controls.Add(this.btn_motivo);
            this.Controls.Add(this.btn_ocupacion);
            this.Controls.Add(this.txtb_ingresar);
            this.Controls.Add(this.btn_interes);
            this.Controls.Add(this.btn_empleado);
            this.Controls.Add(this.btn_afiliado);
            this.Name = "Form1";
            this.Text = "TC";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_afiliado;
        private System.Windows.Forms.Button btn_empleado;
        private System.Windows.Forms.Button btn_interes;
        private System.Windows.Forms.TextBox txtb_ingresar;
        private System.Windows.Forms.Button btn_ocupacion;
        private System.Windows.Forms.Button btn_motivo;
        private System.Windows.Forms.Button btn_monto;
        private System.Windows.Forms.Button btn_login;
    }
}

