
namespace WindowsFormsApp
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.第一种映射方式 = new System.Windows.Forms.Button();
            this.第二种映射方式 = new System.Windows.Forms.Button();
            this.第三种映射方式 = new System.Windows.Forms.Button();
            this.第四种映射方式 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 12F);
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(209, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // 第一种映射方式
            // 
            this.第一种映射方式.Font = new System.Drawing.Font("宋体", 12F);
            this.第一种映射方式.Location = new System.Drawing.Point(12, 59);
            this.第一种映射方式.Name = "第一种映射方式";
            this.第一种映射方式.Size = new System.Drawing.Size(209, 41);
            this.第一种映射方式.TabIndex = 1;
            this.第一种映射方式.Text = "第一种映射方式";
            this.第一种映射方式.UseVisualStyleBackColor = true;
            this.第一种映射方式.Click += new System.EventHandler(this.第一种映射方式_Click);
            // 
            // 第二种映射方式
            // 
            this.第二种映射方式.Font = new System.Drawing.Font("宋体", 12F);
            this.第二种映射方式.Location = new System.Drawing.Point(12, 106);
            this.第二种映射方式.Name = "第二种映射方式";
            this.第二种映射方式.Size = new System.Drawing.Size(209, 41);
            this.第二种映射方式.TabIndex = 2;
            this.第二种映射方式.Text = "第二种映射方式";
            this.第二种映射方式.UseVisualStyleBackColor = true;
            this.第二种映射方式.Click += new System.EventHandler(this.第二种映射方式_Click);
            // 
            // 第三种映射方式
            // 
            this.第三种映射方式.Font = new System.Drawing.Font("宋体", 12F);
            this.第三种映射方式.Location = new System.Drawing.Point(12, 153);
            this.第三种映射方式.Name = "第三种映射方式";
            this.第三种映射方式.Size = new System.Drawing.Size(209, 41);
            this.第三种映射方式.TabIndex = 3;
            this.第三种映射方式.Text = "第三种映射方式";
            this.第三种映射方式.UseVisualStyleBackColor = true;
            this.第三种映射方式.Click += new System.EventHandler(this.第三种映射方式_Click);
            // 
            // 第四种映射方式
            // 
            this.第四种映射方式.Font = new System.Drawing.Font("宋体", 12F);
            this.第四种映射方式.Location = new System.Drawing.Point(12, 200);
            this.第四种映射方式.Name = "第四种映射方式";
            this.第四种映射方式.Size = new System.Drawing.Size(209, 41);
            this.第四种映射方式.TabIndex = 4;
            this.第四种映射方式.Text = "第四种映射方式";
            this.第四种映射方式.UseVisualStyleBackColor = true;
            this.第四种映射方式.Click += new System.EventHandler(this.第四种映射方式_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 255);
            this.Controls.Add(this.第四种映射方式);
            this.Controls.Add(this.第三种映射方式);
            this.Controls.Add(this.第二种映射方式);
            this.Controls.Add(this.第一种映射方式);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button 第一种映射方式;
        private System.Windows.Forms.Button 第二种映射方式;
        private System.Windows.Forms.Button 第三种映射方式;
        private System.Windows.Forms.Button 第四种映射方式;
    }
}

