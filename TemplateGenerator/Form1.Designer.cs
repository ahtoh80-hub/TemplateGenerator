using System;
using System.Windows.Forms;

namespace TemplateGenerator
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelButtons;
        
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem toolsMenu;
        private System.Windows.Forms.ToolStripMenuItem helpMenu;
        private System.Windows.Forms.ToolStripMenuItem loadTemplateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadExcelMenuItem;
        private System.Windows.Forms.ToolStripMenuItem validateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertTagsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTemplateInfo;
        private System.Windows.Forms.Label lblExcelInfo;
        private System.Windows.Forms.Label lblActiveReplacements;
        
        private System.Windows.Forms.Button btnLoadTemplate;
        private System.Windows.Forms.Button btnLoadExcel;
        private System.Windows.Forms.Button btnConvertTags;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnValidate;
        
        private System.Windows.Forms.DataGridView dgvReplacements;
        private System.Windows.Forms.Label lblReplacementsTitle;
        
        private System.Windows.Forms.TextBox txtTemplatePreview;
        private System.Windows.Forms.Label lblPreviewTitle;
        
        private System.Windows.Forms.RichTextBox txtMappingInfo;
        private System.Windows.Forms.Label lblMappingTitle;
        
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Label lblLogTitle;
        
        private System.Windows.Forms.Splitter splitterRight;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.Panel();
            
            this.splitterRight = new System.Windows.Forms.Splitter();
            
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTemplateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadExcelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.validateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertTagsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTemplateInfo = new System.Windows.Forms.Label();
            this.lblExcelInfo = new System.Windows.Forms.Label();
            this.lblActiveReplacements = new System.Windows.Forms.Label();
            
            this.btnLoadTemplate = new System.Windows.Forms.Button();
            this.btnLoadExcel = new System.Windows.Forms.Button();
            this.btnConvertTags = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            
            this.dgvReplacements = new System.Windows.Forms.DataGridView();
            this.lblReplacementsTitle = new System.Windows.Forms.Label();
            
            this.txtTemplatePreview = new System.Windows.Forms.TextBox();
            this.lblPreviewTitle = new System.Windows.Forms.Label();
            
            this.txtMappingInfo = new System.Windows.Forms.RichTextBox();
            this.lblMappingTitle = new System.Windows.Forms.Label();
            
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.lblLogTitle = new System.Windows.Forms.Label();
            
            // MenuStrip
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(0, 80, 150);
            this.menuStrip.ForeColor = System.Drawing.Color.Black;
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.fileMenu,
                this.toolsMenu,
                this.helpMenu
            });
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1400, 28);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            
            // File Menu
            this.fileMenu.Text = "📁 Файл";
            this.fileMenu.ForeColor = System.Drawing.Color.Black;
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.loadTemplateMenuItem,
                this.loadExcelMenuItem,
                new System.Windows.Forms.ToolStripSeparator(),
                this.exitMenuItem
            });
            
            this.loadTemplateMenuItem.Text = "📄 Загрузить шаблон";
            this.loadTemplateMenuItem.ForeColor = System.Drawing.Color.Black;
            this.loadTemplateMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            this.loadTemplateMenuItem.Click += new System.EventHandler((s, e) => btnLoadTemplate.PerformClick());
            
            this.loadExcelMenuItem.Text = "📊 Загрузить экземпляры";
            this.loadExcelMenuItem.ForeColor = System.Drawing.Color.Black;
            this.loadExcelMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E;
            this.loadExcelMenuItem.Click += new System.EventHandler((s, e) => btnLoadExcel.PerformClick());
            
            this.exitMenuItem.Text = "❌ Выход";
            this.exitMenuItem.ForeColor = System.Drawing.Color.Black;
            this.exitMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            this.exitMenuItem.Click += new System.EventHandler((s, e) => Application.Exit());
            
            // Tools Menu
            this.toolsMenu.Text = "🛠 Инструменты";
            this.toolsMenu.ForeColor = System.Drawing.Color.Black;
            this.toolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.validateMenuItem,
                this.convertTagsMenuItem,
                new System.Windows.Forms.ToolStripSeparator(),
                this.generateMenuItem,
                this.clearAllMenuItem
            });
            
            this.validateMenuItem.Text = "✅ Проверить данные";
            this.validateMenuItem.ForeColor = System.Drawing.Color.Black;
            this.validateMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            this.validateMenuItem.Click += new System.EventHandler((s, e) => btnValidate.PerformClick());
            
            this.convertTagsMenuItem.Text = "🔄 Преобразовать тэг";
            this.convertTagsMenuItem.ForeColor = System.Drawing.Color.Black;
            this.convertTagsMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T;
            this.convertTagsMenuItem.Click += new System.EventHandler((s, e) => btnConvertTags.PerformClick());
            
            this.generateMenuItem.Text = "🚀 Сгенерировать";
            this.generateMenuItem.ForeColor = System.Drawing.Color.Black;
            this.generateMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G;
            this.generateMenuItem.Click += new System.EventHandler((s, e) => btnGenerate.PerformClick());
            
            this.clearAllMenuItem.Text = "🗑 Очистить все";
            this.clearAllMenuItem.ForeColor = System.Drawing.Color.Black;
            this.clearAllMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            this.clearAllMenuItem.Click += new System.EventHandler((s, e) => btnClearAll.PerformClick());
            
            // Help Menu
            this.helpMenu.Text = "❓ Помощь";
            this.helpMenu.ForeColor = System.Drawing.Color.Black;
            this.helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.aboutMenuItem
            });
            
            this.aboutMenuItem.Text = "ℹ️ О программе";
            this.aboutMenuItem.ForeColor = System.Drawing.Color.Black;
            this.aboutMenuItem.Click += new System.EventHandler((s, e) => {
                MessageBox.Show(
                    "Генератор экземпляров по шаблону\nВерсия 2.3\n\nРазработчик: Антон Решетов\nЗаказчик: ТЭКОН-Системы\n\n© 2026",
                    "О программе",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            });
            
            this.SuspendLayout();
            
            // Form1
            this.Text = "Генератор экземпляров по шаблону";
            this.BackColor = System.Drawing.Color.FromArgb(13, 37, 63);
            this.ForeColor = System.Drawing.Color.White;
            this.Size = new System.Drawing.Size(1400, 900);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MainMenuStrip = this.menuStrip;
            
            // panelTop
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Height = 60;
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(0, 80, 150);
            this.panelTop.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            
            this.lblTitle.Text = "Генератор экземпляров по шаблону";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Height = 35;
            
            this.lblTemplateInfo.Text = "Шаблон: -";
            this.lblTemplateInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTemplateInfo.ForeColor = System.Drawing.Color.White;
            this.lblTemplateInfo.AutoSize = true;
            this.lblTemplateInfo.Location = new System.Drawing.Point(20, 40);
            
            this.lblExcelInfo.Text = "Excel: -";
            this.lblExcelInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblExcelInfo.ForeColor = System.Drawing.Color.White;
            this.lblExcelInfo.AutoSize = true;
            this.lblExcelInfo.Location = new System.Drawing.Point(220, 40);
            
            this.lblActiveReplacements.Text = "Найдено: 0";
            this.lblActiveReplacements.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblActiveReplacements.ForeColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.lblActiveReplacements.AutoSize = true;
            this.lblActiveReplacements.Location = new System.Drawing.Point(450, 40);
            this.lblActiveReplacements.BackColor = System.Drawing.Color.Transparent;
            
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.lblTemplateInfo);
            this.panelTop.Controls.Add(this.lblExcelInfo);
            this.panelTop.Controls.Add(this.lblActiveReplacements);
            
            // panelMain
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(13, 37, 63);
            this.panelMain.Padding = new System.Windows.Forms.Padding(5);
            
            // panelLeft - Таблица замен
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Width = 380;
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(20, 50, 95);
            this.panelLeft.Padding = new System.Windows.Forms.Padding(5);
            
            var panelLeftContainer = new System.Windows.Forms.Panel();
            panelLeftContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            panelLeftContainer.BackColor = System.Drawing.Color.FromArgb(20, 50, 95);
            
            this.lblReplacementsTitle.Text = "Позиции для поиска (номер совпадает с Excel)";
            this.lblReplacementsTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblReplacementsTitle.ForeColor = System.Drawing.Color.White;
            this.lblReplacementsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblReplacementsTitle.Height = 35;
            this.lblReplacementsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblReplacementsTitle.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblReplacementsTitle.BackColor = System.Drawing.Color.FromArgb(0, 80, 150);
            
            // Настройка dgvReplacements
            this.dgvReplacements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReplacements.BackgroundColor = System.Drawing.Color.White;
            this.dgvReplacements.ForeColor = System.Drawing.Color.Black;
            this.dgvReplacements.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvReplacements.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvReplacements.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvReplacements.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.dgvReplacements.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvReplacements.RowHeadersVisible = false;
            this.dgvReplacements.AllowUserToAddRows = false;
            this.dgvReplacements.AllowUserToDeleteRows = false;
            this.dgvReplacements.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            
            // Создаем колонки вручную
            DataGridViewTextBoxColumn colNumber = new DataGridViewTextBoxColumn();
            colNumber.Name = "№";
            colNumber.HeaderText = "№";
            colNumber.Width = 45;
            colNumber.ReadOnly = true;
            colNumber.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;

            DataGridViewTextBoxColumn colTagName = new DataGridViewTextBoxColumn();
            colTagName.Name = "Имя тега для поиска";
            colTagName.HeaderText = "Имя тега для поиска";
            colTagName.Width = 200;
            colTagName.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;

            DataGridViewCheckBoxColumn colUse = new DataGridViewCheckBoxColumn();
            colUse.Name = "Использовать";
            colUse.HeaderText = "Использовать";
            colUse.Width = 70;
            colUse.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;

            this.dgvReplacements.Columns.AddRange(new DataGridViewColumn[] {
                colNumber,
                colTagName,
                colUse
            });
            
            panelLeftContainer.Controls.Add(this.dgvReplacements);
            panelLeftContainer.Controls.Add(this.lblReplacementsTitle);
            this.panelLeft.Controls.Add(panelLeftContainer);
            
            // panelRight - Информация о заменах
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Width = 450;
            this.panelRight.BackColor = System.Drawing.Color.FromArgb(20, 50, 95);
            this.panelRight.Padding = new System.Windows.Forms.Padding(5);
            this.panelRight.MinimumSize = new System.Drawing.Size(250, 0);
            
            var panelRightContainer = new System.Windows.Forms.Panel();
            panelRightContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            panelRightContainer.BackColor = System.Drawing.Color.FromArgb(20, 50, 95);
            
            this.lblMappingTitle.Text = "Детали замен по экземплярам";
            this.lblMappingTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMappingTitle.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.lblMappingTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMappingTitle.Height = 35;
            this.lblMappingTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMappingTitle.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblMappingTitle.BackColor = System.Drawing.Color.FromArgb(0, 80, 150);
            
            this.txtMappingInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMappingInfo.BackColor = System.Drawing.Color.White;
            this.txtMappingInfo.ForeColor = System.Drawing.Color.Black;
            this.txtMappingInfo.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtMappingInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
            this.txtMappingInfo.ReadOnly = true;
            this.txtMappingInfo.WordWrap = false;
            this.txtMappingInfo.DetectUrls = false;
            this.txtMappingInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            
            panelRightContainer.Controls.Add(this.txtMappingInfo);
            panelRightContainer.Controls.Add(this.lblMappingTitle);
            this.panelRight.Controls.Add(panelRightContainer);
            
            // Splitter
            this.splitterRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterRight.Width = 6;
            this.splitterRight.BackColor = System.Drawing.Color.FromArgb(0, 80, 150);
            this.splitterRight.MinSize = 250;
            
            // panelCenter - Предпросмотр
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.BackColor = System.Drawing.Color.FromArgb(20, 50, 95);
            this.panelCenter.Padding = new System.Windows.Forms.Padding(5);
            
            this.lblPreviewTitle.Text = "Предпросмотр шаблона";
            this.lblPreviewTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPreviewTitle.ForeColor = System.Drawing.Color.White;
            this.lblPreviewTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPreviewTitle.Height = 30;
            this.lblPreviewTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            this.txtTemplatePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTemplatePreview.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtTemplatePreview.ForeColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.txtTemplatePreview.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtTemplatePreview.Multiline = true;
            this.txtTemplatePreview.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTemplatePreview.ReadOnly = true;
            this.txtTemplatePreview.WordWrap = false;
            
            this.panelCenter.Controls.Add(this.lblPreviewTitle);
            this.panelCenter.Controls.Add(this.txtTemplatePreview);
            
            // Сборка panelMain
            this.panelMain.Controls.Add(this.panelCenter);
            this.panelMain.Controls.Add(this.splitterRight);
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Controls.Add(this.panelLeft);
            
            // panelButtons
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Height = 50;
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(0, 80, 150);
            this.panelButtons.Padding = new System.Windows.Forms.Padding(10);
            
            this.btnLoadTemplate.Text = "📁 Загрузить шаблон";
            this.btnLoadTemplate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLoadTemplate.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnLoadTemplate.ForeColor = System.Drawing.Color.White;
            this.btnLoadTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadTemplate.FlatAppearance.BorderSize = 0;
            this.btnLoadTemplate.Size = new System.Drawing.Size(150, 35);
            this.btnLoadTemplate.Location = new System.Drawing.Point(10, 8);
            this.btnLoadTemplate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoadTemplate.Click += new System.EventHandler(this.btnLoadTemplate_Click);
            
            this.btnLoadExcel.Text = "📊 Загрузить экземпляры";
            this.btnLoadExcel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLoadExcel.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnLoadExcel.ForeColor = System.Drawing.Color.White;
            this.btnLoadExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadExcel.FlatAppearance.BorderSize = 0;
            this.btnLoadExcel.Size = new System.Drawing.Size(160, 35);
            this.btnLoadExcel.Location = new System.Drawing.Point(170, 8);
            this.btnLoadExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoadExcel.Click += new System.EventHandler(this.btnLoadExcel_Click);
            
            this.btnValidate.Text = "✅ Проверить";
            this.btnValidate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnValidate.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.btnValidate.ForeColor = System.Drawing.Color.White;
            this.btnValidate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidate.FlatAppearance.BorderSize = 0;
            this.btnValidate.Size = new System.Drawing.Size(120, 35);
            this.btnValidate.Location = new System.Drawing.Point(340, 8);
            this.btnValidate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            
            this.btnConvertTags.Text = "🔄 Преобразовать тэг";
            this.btnConvertTags.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnConvertTags.BackColor = System.Drawing.Color.FromArgb(241, 196, 15);
            this.btnConvertTags.ForeColor = System.Drawing.Color.Black;
            this.btnConvertTags.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConvertTags.FlatAppearance.BorderSize = 0;
            this.btnConvertTags.Size = new System.Drawing.Size(160, 35);
            this.btnConvertTags.Location = new System.Drawing.Point(520, 8);
            this.btnConvertTags.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConvertTags.Click += new System.EventHandler(this.btnConvertTags_Click);
            
            this.btnGenerate.Text = "🚀 Сгенерировать";
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.BackColor = System.Drawing.Color.FromArgb(243, 156, 18);
            this.btnGenerate.ForeColor = System.Drawing.Color.White;
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.FlatAppearance.BorderSize = 0;
            this.btnGenerate.Size = new System.Drawing.Size(140, 35);
            this.btnGenerate.Location = new System.Drawing.Point(780, 8);
            this.btnGenerate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            
            this.btnClearAll.Text = "🗑 Очистить все";
            this.btnClearAll.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClearAll.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.btnClearAll.ForeColor = System.Drawing.Color.White;
            this.btnClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAll.FlatAppearance.BorderSize = 0;
            this.btnClearAll.Size = new System.Drawing.Size(120, 35);
            this.btnClearAll.Location = new System.Drawing.Point(1050, 8);
            this.btnClearAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            
            this.panelButtons.Controls.Add(this.btnLoadTemplate);
            this.panelButtons.Controls.Add(this.btnLoadExcel);
            this.panelButtons.Controls.Add(this.btnValidate);
            this.panelButtons.Controls.Add(this.btnConvertTags);
            this.panelButtons.Controls.Add(this.btnGenerate);
            this.panelButtons.Controls.Add(this.btnClearAll);
            
            // panelBottom - Лог
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Height = 150;
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(10, 25, 45);
            this.panelBottom.Padding = new System.Windows.Forms.Padding(5);
            
            this.lblLogTitle.Text = "Окно событий и ошибок";
            this.lblLogTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLogTitle.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.lblLogTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLogTitle.Height = 25;
            this.lblLogTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.BackColor = System.Drawing.Color.FromArgb(10, 25, 45);
            this.rtbLog.ForeColor = System.Drawing.Color.White;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLog.ReadOnly = true;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            
            this.panelBottom.Controls.Add(this.rtbLog);
            this.panelBottom.Controls.Add(this.lblLogTitle);
            
            // Добавление элементов на форму
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.menuStrip);
            
            // Подписка на события DataGridView
            this.dgvReplacements.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReplacements_CellValueChanged);
            this.dgvReplacements.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvReplacements_CurrentCellDirtyStateChanged);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}