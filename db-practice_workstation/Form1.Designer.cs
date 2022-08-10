namespace arm
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.bLogIn = new System.Windows.Forms.Button();
            this.bGuest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bLogIn
            // 
            this.bLogIn.Location = new System.Drawing.Point(69, 39);
            this.bLogIn.Name = "bLogIn";
            this.bLogIn.Size = new System.Drawing.Size(115, 23);
            this.bLogIn.TabIndex = 0;
            this.bLogIn.Text = "Вход/Регистрация";
            this.bLogIn.UseVisualStyleBackColor = true;
            this.bLogIn.Click += new System.EventHandler(this.bLogIn_Click);
            // 
            // bGuest
            // 
            this.bGuest.Location = new System.Drawing.Point(69, 68);
            this.bGuest.Name = "bGuest";
            this.bGuest.Size = new System.Drawing.Size(115, 23);
            this.bGuest.TabIndex = 1;
            this.bGuest.Text = "Гостевой вход";
            this.bGuest.UseVisualStyleBackColor = true;
            this.bGuest.Click += new System.EventHandler(this.bGuest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 131);
            this.Controls.Add(this.bGuest);
            this.Controls.Add(this.bLogIn);
            this.Name = "Form1";
            this.Text = "Регистрация/аутентификация";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bLogIn;
        private System.Windows.Forms.Button bGuest;
    }
}

