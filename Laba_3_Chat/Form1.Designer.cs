namespace KSIS_3
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
            this.btnSend = new System.Windows.Forms.Button();
            this.textInputMessage = new System.Windows.Forms.TextBox();
            this.listChat = new System.Windows.Forms.ListBox();
            this.listUsers = new System.Windows.Forms.ListBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelUsers = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelPanel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(470, 307);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 20);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // textInputMessage
            // 
            this.textInputMessage.Location = new System.Drawing.Point(12, 307);
            this.textInputMessage.Name = "textInputMessage";
            this.textInputMessage.Size = new System.Drawing.Size(452, 20);
            this.textInputMessage.TabIndex = 1;
            // 
            // listChat
            // 
            this.listChat.FormattingEnabled = true;
            this.listChat.Location = new System.Drawing.Point(165, 37);
            this.listChat.Name = "listChat";
            this.listChat.Size = new System.Drawing.Size(379, 251);
            this.listChat.TabIndex = 4;
            // 
            // listUsers
            // 
            this.listUsers.FormattingEnabled = true;
            this.listUsers.Location = new System.Drawing.Point(12, 92);
            this.listUsers.Name = "listUsers";
            this.listUsers.Size = new System.Drawing.Size(147, 199);
            this.listUsers.TabIndex = 5;
            // 
            // btnFind
            // 
            this.btnFind.Enabled = false;
            this.btnFind.Location = new System.Drawing.Point(104, 37);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(55, 23);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "Поиск абонентов";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(12, 37);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(86, 20);
            this.textBoxName.TabIndex = 7;
            this.textBoxName.Text = "Имя";
            this.textBoxName.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // labelUsers
            // 
            this.labelUsers.AutoSize = true;
            this.labelUsers.Location = new System.Drawing.Point(0, 76);
            this.labelUsers.Name = "labelUsers";
            this.labelUsers.Size = new System.Drawing.Size(161, 13);
            this.labelUsers.TabIndex = 8;
            this.labelUsers.Text = "Подключенные пользователи:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(9, 21);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(107, 13);
            this.labelName.TabIndex = 9;
            this.labelName.Text = "Введите ваше имя: ";
            // 
            // labelPanel
            // 
            this.labelPanel.AutoSize = true;
            this.labelPanel.Location = new System.Drawing.Point(165, 20);
            this.labelPanel.Name = "labelPanel";
            this.labelPanel.Size = new System.Drawing.Size(108, 13);
            this.labelPanel.TabIndex = 10;
            this.labelPanel.Text = "Панель сообщений:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 340);
            this.Controls.Add(this.labelPanel);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.labelUsers);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.listUsers);
            this.Controls.Add(this.listChat);
            this.Controls.Add(this.textInputMessage);
            this.Controls.Add(this.btnSend);
            this.Name = "Form1";
            this.Text = "Чат";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox textInputMessage;
        private System.Windows.Forms.ListBox listChat;
        private System.Windows.Forms.ListBox listUsers;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelUsers;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelPanel;
    }
}

