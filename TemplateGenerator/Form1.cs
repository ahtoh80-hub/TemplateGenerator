using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OfficeOpenXml;

namespace TemplateGenerator
{
    public partial class Form1 : Form
    {
        #region Поля и свойства
        private DataTable replacementTable;
        private string templateContent = string.Empty;
        private string templateFilePath = string.Empty;
        private List<InstanceData> instances = new List<InstanceData>();
        private string excelFilePath = string.Empty;
        private bool convertTagsToUnderscore = true;
        private const int MAX_LOG_MESSAGES = 1000;
        private bool isGenerating = false;
        private Dictionary<int, string> activeReplacements = new Dictionary<int, string>();
        private ContextMenuStrip contextMenu;
        #endregion

        #region Конструктор
        public Form1()
        {
            try
            {
                InitializeComponent();
                
                if (dgvReplacements == null)
                {
                    MessageBox.Show("Ошибка: dgvReplacements не инициализирован", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                InitializeReplacementTable();
                InitializeContextMenu();
                
                convertTagsToUnderscore = true;
                btnConvertTags.Text = "Преобразование Вкл";
                btnConvertTags.BackColor = Color.FromArgb(46, 204, 113);
                btnConvertTags.ForeColor = Color.White;
                
                this.dgvReplacements.KeyDown += DgvReplacements_KeyDown;
                this.dgvReplacements.Enter += DgvReplacements_Enter;
                this.dgvReplacements.Leave += DgvReplacements_Leave;
                
                this.txtTemplatePreview.KeyDown += TxtTemplatePreview_KeyDown;
                this.txtTemplatePreview.Enter += TxtTemplatePreview_Enter;
                this.txtTemplatePreview.Leave += TxtTemplatePreview_Leave;
                
                this.txtMappingInfo.KeyDown += TxtMappingInfo_KeyDown;
                this.txtMappingInfo.Enter += TxtMappingInfo_Enter;
                this.txtMappingInfo.Leave += TxtMappingInfo_Leave;
                
                this.rtbLog.Enter += RtbLog_Enter;
                this.rtbLog.Leave += RtbLog_Leave;
                
                this.Show();
                this.BringToFront();
                
                AddLogMessage("Программа запущена", LogType.Info);
                AddLogMessage("Режим преобразования тегов включен по умолчанию", LogType.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в конструкторе Form1:\n{ex.Message}\n\n{ex.StackTrace}", 
                    "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Обработка входа/выхода для rtbLog
        private void RtbLog_Enter(object sender, EventArgs e)
        {
            DisableMenuShortcuts(true);
        }

        private void RtbLog_Leave(object sender, EventArgs e)
        {
            DisableMenuShortcuts(false);
        }
        #endregion

        #region Управление горячими клавишами MenuStrip
        private void DisableMenuShortcuts(bool disable)
        {
            if (menuStrip == null) return;
            
            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                DisableMenuShortcutsRecursive(item, disable);
            }
        }

        private void DisableMenuShortcutsRecursive(ToolStripMenuItem item, bool disable)
        {
            if (item == null) return;
            
            if (disable)
            {
                item.ShortcutKeys = Keys.None;
            }
            else
            {
                if (item.Text.Contains("Загрузить шаблон"))
                    item.ShortcutKeys = Keys.Control | Keys.O;
                else if (item.Text.Contains("Загрузить экземпляры"))
                    item.ShortcutKeys = Keys.Control | Keys.E;
                else if (item.Text.Contains("Проверить данные"))
                    item.ShortcutKeys = Keys.Control | Keys.V;
                else if (item.Text.Contains("Преобразование"))
                    item.ShortcutKeys = Keys.Control | Keys.T;
                else if (item.Text.Contains("Сгенерировать"))
                    item.ShortcutKeys = Keys.Control | Keys.G;
                else if (item.Text.Contains("Очистить все"))
                    item.ShortcutKeys = Keys.Control | Keys.C;
                else if (item.Text.Contains("Выход"))
                    item.ShortcutKeys = Keys.Alt | Keys.F4;
            }

            foreach (ToolStripItem subItem in item.DropDownItems)
            {
                if (subItem is ToolStripMenuItem subMenuItem)
                {
                    DisableMenuShortcutsRecursive(subMenuItem, disable);
                }
            }
        }
        #endregion

        #region Обработка входа/выхода для txtTemplatePreview
        private void TxtTemplatePreview_Enter(object sender, EventArgs e)
        {
            DisableMenuShortcuts(true);
        }

        private void TxtTemplatePreview_Leave(object sender, EventArgs e)
        {
            DisableMenuShortcuts(false);
        }

        private void TxtTemplatePreview_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtTemplatePreview.Focused)
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    return;
                }
                if (e.Control && e.KeyCode == Keys.V)
                {
                    return;
                }
                if (e.Control && e.KeyCode == Keys.A)
                {
                    return;
                }
            }
        }
        #endregion

        #region Обработка входа/выхода для txtMappingInfo
        private void TxtMappingInfo_Enter(object sender, EventArgs e)
        {
            DisableMenuShortcuts(true);
        }

        private void TxtMappingInfo_Leave(object sender, EventArgs e)
        {
            DisableMenuShortcuts(false);
        }

        private void TxtMappingInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtMappingInfo.Focused)
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    return;
                }
                if (e.Control && e.KeyCode == Keys.V)
                {
                    return;
                }
                if (e.Control && e.KeyCode == Keys.A)
                {
                    return;
                }
            }
        }
        #endregion

        #region Обработка входа/выхода для DataGridView
        private void DgvReplacements_Enter(object sender, EventArgs e)
        {
            DisableMenuShortcuts(true);
        }

        private void DgvReplacements_Leave(object sender, EventArgs e)
        {
            DisableMenuShortcuts(false);
        }

        private void DgvReplacements_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvReplacements.Focused)
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    CopySelectedCells();
                    return;
                }
                if (e.Control && e.KeyCode == Keys.V)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    PasteToSelectedCells();
                    return;
                }
                if (e.Control && e.KeyCode == Keys.X)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    CutSelectedCells();
                    return;
                }
                if (e.KeyCode == Keys.Delete)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    DeleteSelectedCells();
                    return;
                }
            }
        }
        #endregion

        #region Переопределение обработки клавиш на уровне формы
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (dgvReplacements != null && dgvReplacements.Focused)
            {
                if (keyData == (Keys.Control | Keys.C) ||
                    keyData == (Keys.Control | Keys.V) ||
                    keyData == (Keys.Control | Keys.X) ||
                    keyData == Keys.Delete)
                {
                    return true;
                }
            }
            
            if ((txtTemplatePreview != null && txtTemplatePreview.Focused) || 
                (txtMappingInfo != null && txtMappingInfo.Focused))
            {
                if (keyData == (Keys.Control | Keys.C) ||
                    keyData == (Keys.Control | Keys.V) ||
                    keyData == (Keys.Control | Keys.A))
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            
            if (rtbLog != null && rtbLog.Focused)
            {
                if (keyData == (Keys.Control | Keys.C) ||
                    keyData == (Keys.Control | Keys.V) ||
                    keyData == (Keys.Control | Keys.A))
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Методы копирования/вставки для DataGridView
        private void CopySelectedCells()
        {
            if (dgvReplacements.SelectedCells.Count == 0)
            {
                AddLogMessage("Нет выделенных ячеек для копирования", LogType.Warning);
                return;
            }

            try
            {
                var copiedData = new System.Text.StringBuilder();
                var selectedCells = dgvReplacements.SelectedCells;
                
                int minRow = int.MaxValue, maxRow = int.MinValue;
                int minCol = int.MaxValue, maxCol = int.MinValue;
                
                foreach (DataGridViewCell cell in selectedCells)
                {
                    if (cell.RowIndex < minRow) minRow = cell.RowIndex;
                    if (cell.RowIndex > maxRow) maxRow = cell.RowIndex;
                    if (cell.ColumnIndex < minCol) minCol = cell.ColumnIndex;
                    if (cell.ColumnIndex > maxCol) maxCol = cell.ColumnIndex;
                }
                
                var selectedSet = new HashSet<(int row, int col)>();
                foreach (DataGridViewCell cell in selectedCells)
                {
                    selectedSet.Add((cell.RowIndex, cell.ColumnIndex));
                }
                
                for (int row = minRow; row <= maxRow; row++)
                {
                    for (int col = minCol; col <= maxCol; col++)
                    {
                        if (selectedSet.Contains((row, col)))
                        {
                            object value = dgvReplacements.Rows[row].Cells[col].Value;
                            string cellValue = value?.ToString() ?? string.Empty;
                            copiedData.Append(cellValue);
                        }
                        if (col < maxCol)
                            copiedData.Append('\t');
                    }
                    if (row < maxRow)
                        copiedData.AppendLine();
                }
                
                Clipboard.SetText(copiedData.ToString());
                AddLogMessage($"Скопировано {selectedCells.Count} ячеек", LogType.Success);
            }
            catch (Exception ex)
            {
                AddLogMessage($"Ошибка при копировании: {ex.Message}", LogType.Error);
                MessageBox.Show($"Ошибка при копировании:\n{ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PasteToSelectedCells()
        {
            if (dgvReplacements.SelectedCells.Count == 0)
            {
                MessageBox.Show("Выберите ячейку для вставки.", "Вставка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string clipboardText = Clipboard.GetText();
                if (string.IsNullOrEmpty(clipboardText))
                {
                    MessageBox.Show("Буфер обмена пуст.", "Вставка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string[] lines = clipboardText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                string[][] cells = lines
                    .Where(line => !string.IsNullOrEmpty(line))
                    .Select(line => line.Split('\t'))
                    .ToArray();

                if (cells.Length == 0)
                {
                    MessageBox.Show("Нет данных для вставки.", "Вставка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataGridViewCell startCell = dgvReplacements.SelectedCells[0];
                int startRow = startCell.RowIndex;
                int startCol = startCell.ColumnIndex;

                dgvReplacements.CellValueChanged -= dgvReplacements_CellValueChanged;
                
                try
                {
                    for (int r = 0; r < cells.Length; r++)
                    {
                        int targetRow = startRow + r;
                        if (targetRow >= dgvReplacements.Rows.Count)
                            break;

                        for (int c = 0; c < cells[r].Length; c++)
                        {
                            int targetCol = startCol + c;
                            if (targetCol >= dgvReplacements.Columns.Count)
                                break;

                            if (targetCol == 0)
                                continue;
                            
                            if (targetCol == 2)
                            {
                                string value = cells[r][c].Trim().ToLower();
                                bool boolValue = (value == "true" || value == "да" || value == "1" || value == "+" || value == "☑" || value == "✔" || value == "✓");
                                dgvReplacements.Rows[targetRow].Cells[targetCol].Value = boolValue;
                            }
                            else
                            {
                                string value = cells[r][c].Trim();
                                dgvReplacements.Rows[targetRow].Cells[targetCol].Value = value;
                            }
                        }
                    }
                }
                finally
                {
                    dgvReplacements.CellValueChanged += dgvReplacements_CellValueChanged;
                }

                dgvReplacements.Refresh();
                UpdateActiveReplacements();
                UpdateMappingInfo();
                
                int totalCells = cells.Sum(row => row.Length);
                AddLogMessage($"Вставлено {totalCells} ячеек", LogType.Success);
            }
            catch (Exception ex)
            {
                AddLogMessage($"Ошибка при вставке: {ex.Message}", LogType.Error);
                MessageBox.Show($"Ошибка при вставке:\n{ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CutSelectedCells()
        {
            if (dgvReplacements.SelectedCells.Count == 0)
            {
                AddLogMessage("Нет выделенных ячеек для вырезания", LogType.Warning);
                return;
            }

            try
            {
                CopySelectedCells();
                
                foreach (DataGridViewCell cell in dgvReplacements.SelectedCells)
                {
                    if (cell.ColumnIndex != 0)
                    {
                        if (cell.ColumnIndex == 2)
                        {
                            cell.Value = false;
                        }
                        else
                        {
                            cell.Value = string.Empty;
                        }
                    }
                }
                
                dgvReplacements.Refresh();
                UpdateActiveReplacements();
                UpdateMappingInfo();
                AddLogMessage("Выделенные ячейки вырезаны", LogType.Info);
            }
            catch (Exception ex)
            {
                AddLogMessage($"Ошибка при вырезании: {ex.Message}", LogType.Error);
            }
        }

        private void DeleteSelectedCells()
        {
            if (dgvReplacements.SelectedCells.Count == 0)
                return;

            try
            {
                foreach (DataGridViewCell cell in dgvReplacements.SelectedCells)
                {
                    if (cell.ColumnIndex != 0)
                    {
                        if (cell.ColumnIndex == 2)
                        {
                            cell.Value = false;
                        }
                        else
                        {
                            cell.Value = string.Empty;
                        }
                    }
                }
                
                dgvReplacements.Refresh();
                UpdateActiveReplacements();
                UpdateMappingInfo();
                AddLogMessage("Выделенные ячейки очищены", LogType.Info);
            }
            catch (Exception ex)
            {
                AddLogMessage($"Ошибка при удалении: {ex.Message}", LogType.Error);
            }
        }
        #endregion

        #region Инициализация контекстного меню
        private void InitializeContextMenu()
        {
            contextMenu = new ContextMenuStrip();
            contextMenu.BackColor = Color.FromArgb(20, 50, 95);
            contextMenu.ForeColor = Color.Black;

            var menuLoadTemplate = new ToolStripMenuItem("Загрузить шаблон");
            menuLoadTemplate.ForeColor = Color.Black;
            menuLoadTemplate.Click += (s, e) => btnLoadTemplate.PerformClick();

            var menuLoadExcel = new ToolStripMenuItem("Загрузить экземпляры");
            menuLoadExcel.ForeColor = Color.Black;
            menuLoadExcel.Click += (s, e) => btnLoadExcel.PerformClick();

            var menuValidate = new ToolStripMenuItem("Проверить");
            menuValidate.ForeColor = Color.Black;
            menuValidate.Click += (s, e) => btnValidate.PerformClick();

            var menuConvertTags = new ToolStripMenuItem("Преобразование");
            menuConvertTags.ForeColor = Color.Black;
            menuConvertTags.Click += (s, e) => btnConvertTags.PerformClick();

            var menuGenerate = new ToolStripMenuItem("Сгенерировать");
            menuGenerate.ForeColor = Color.Black;
            menuGenerate.Click += (s, e) => btnGenerate.PerformClick();

            var menuClearAll = new ToolStripMenuItem("Очистить все");
            menuClearAll.ForeColor = Color.Black;
            menuClearAll.Click += (s, e) => btnClearAll.PerformClick();

            var separator1 = new ToolStripSeparator();
            var separator2 = new ToolStripSeparator();

            contextMenu.Items.AddRange(new ToolStripItem[] {
                menuLoadTemplate,
                menuLoadExcel,
                separator1,
                menuValidate,
                menuConvertTags,
                menuGenerate,
                separator2,
                menuClearAll
            });

            var copyMenuItem = new ToolStripMenuItem("Копировать (Ctrl+C)");
            copyMenuItem.ForeColor = Color.Black;
            copyMenuItem.Click += (s, e) => CopySelectedCells();

            var pasteMenuItem = new ToolStripMenuItem("Вставить (Ctrl+V)");
            pasteMenuItem.ForeColor = Color.Black;
            pasteMenuItem.Click += (s, e) => PasteToSelectedCells();

            var cutMenuItem = new ToolStripMenuItem("Вырезать (Ctrl+X)");
            cutMenuItem.ForeColor = Color.Black;
            cutMenuItem.Click += (s, e) => CutSelectedCells();

            var deleteMenuItem = new ToolStripMenuItem("Очистить (Delete)");
            deleteMenuItem.ForeColor = Color.Black;
            deleteMenuItem.Click += (s, e) => DeleteSelectedCells();

            var separator3 = new ToolStripSeparator();

            contextMenu.Items.Add(separator3);
            contextMenu.Items.Add(copyMenuItem);
            contextMenu.Items.Add(pasteMenuItem);
            contextMenu.Items.Add(cutMenuItem);
            contextMenu.Items.Add(deleteMenuItem);

            this.dgvReplacements.ContextMenuStrip = contextMenu;
            this.ContextMenuStrip = contextMenu;
        }
        #endregion

        #region Инициализация таблицы замен
        private void InitializeReplacementTable()
        {
            try
            {
                if (dgvReplacements == null)
                {
                    MessageBox.Show("dgvReplacements не инициализирован", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                replacementTable = new DataTable();
                replacementTable.Columns.Add("№", typeof(int));
                replacementTable.Columns.Add("Имя тега для поиска", typeof(string));
                replacementTable.Columns.Add("Использовать", typeof(bool));

                for (int i = 1; i <= 10; i++)
                {
                    replacementTable.Rows.Add(i, string.Empty, false);
                }
                replacementTable.Rows.Add(11, string.Empty, false);

                dgvReplacements.AutoGenerateColumns = false;
                dgvReplacements.Columns.Clear();
                
                DataGridViewTextBoxColumn colNumber = new DataGridViewTextBoxColumn();
                colNumber.DataPropertyName = "№";
                colNumber.Name = "№";
                colNumber.HeaderText = "№";
                colNumber.Width = 45;
                colNumber.ReadOnly = false;
                colNumber.DefaultCellStyle.ForeColor = Color.Black;

                DataGridViewTextBoxColumn colTagName = new DataGridViewTextBoxColumn();
                colTagName.DataPropertyName = "Имя тега для поиска";
                colTagName.Name = "Имя тега для поиска";
                colTagName.HeaderText = "Имя тега для поиска";
                colTagName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                colTagName.MinimumWidth = 150;
                colTagName.DefaultCellStyle.ForeColor = Color.Black;

                DataGridViewCheckBoxColumn colUse = new DataGridViewCheckBoxColumn();
                colUse.DataPropertyName = "Использовать";
                colUse.Name = "Использовать";
                colUse.HeaderText = "Использовать";
                colUse.Width = 60;
                colUse.DefaultCellStyle.ForeColor = Color.Black;

                dgvReplacements.Columns.AddRange(new DataGridViewColumn[] {
                    colNumber,
                    colTagName,
                    colUse
                });

                dgvReplacements.DataSource = replacementTable;
                dgvReplacements.RowHeadersVisible = false;
                dgvReplacements.AllowUserToAddRows = false;
                dgvReplacements.AllowUserToDeleteRows = false;
                dgvReplacements.EditMode = DataGridViewEditMode.EditOnEnter;
                
                dgvReplacements.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации таблицы замен:\n{ex.Message}\n\n{ex.StackTrace}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
        #endregion

        #region Загрузка шаблона
        private void btnLoadTemplate_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "TXT files (*.txt)|*.txt|XML files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Title = "Выберите файл шаблона";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        templateFilePath = openFileDialog.FileName;
                        templateContent = File.ReadAllText(templateFilePath, System.Text.Encoding.UTF8);

                        txtTemplatePreview.Text = templateContent;
                        txtTemplatePreview.SelectionStart = 0;
                        txtTemplatePreview.ScrollToCaret();

                        lblTemplateInfo.Text = $"Шаблон: {Path.GetFileName(templateFilePath)}";
                        AddLogMessage($"Шаблон загружен: {Path.GetFileName(templateFilePath)} ({templateContent.Length} байт)", LogType.Success);
                        AddLogMessage($"Тип шаблона: *{Path.GetExtension(templateFilePath)}", LogType.Info);

                        AutoDetectTags();
                    }
                    catch (Exception ex)
                    {
                        AddLogMessage($"Ошибка загрузки шаблона: {ex.Message}", LogType.Error);
                        MessageBox.Show($"Ошибка загрузки шаблона:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        #endregion

        #region Автоматическое определение тегов
        private void AutoDetectTags()
        {
            if (string.IsNullOrEmpty(templateContent))
                return;

            try
            {
                string pattern = @"_([0-9]{1,6})_([A-Za-z]{1,10})_([0-9][A-Za-z0-9]{0,9})(?=[^A-Za-z0-9_]|$|_)";
                var matches = Regex.Matches(templateContent, pattern);

                if (matches.Count > 0)
                {
                    var sortedTags = new List<string>();
                    var seenTags = new HashSet<string>();

                    foreach (Match match in matches)
                    {
                        string tag = match.Value;
                        if (!seenTags.Contains(tag))
                        {
                            seenTags.Add(tag);
                            sortedTags.Add(tag);
                            AddLogMessage($"Найден тег: {tag}", LogType.Info);
                        }
                    }

                    replacementTable.Clear();

                    int position = 1;
                    foreach (string tag in sortedTags)
                    {
                        replacementTable.Rows.Add(position, tag, true);
                        position++;
                    }

                    replacementTable.Rows.Add(position, string.Empty, false);

                    dgvReplacements.Refresh();
                    AddLogMessage($"Автоматически определено {sortedTags.Count} уникальных тегов для замены", LogType.Info);
                    AddLogMessage($"Добавлена пустая строка для нового тега (позиция {position})", LogType.Info);
                    UpdateActiveReplacements();
                    UpdateMappingInfo();
                    
                    if (sortedTags.Count > 0)
                    {
                        string tagList = string.Join(", ", sortedTags);
                        AddLogMessage($"Найденные теги: {tagList}", LogType.Info);
                    }
                }
                else
                {
                    AddLogMessage("Теги для автоматической замены не найдены", LogType.Warning);
                    AddLogMessage("Ожидаемый формат: _цифры_буквы_цифры+буквы", LogType.Info);
                    AddLogMessage("Пример: _2120_XZV_00204A", LogType.Info);
                    AddLogMessage("4-я часть (если есть) игнорируется", LogType.Info);
                }
            }
            catch (Exception ex)
            {
                AddLogMessage($"Ошибка автоматического определения тегов: {ex.Message}", LogType.Error);
            }
        }
        #endregion

        #region Обновление активных замен
        private void UpdateActiveReplacements()
        {
            activeReplacements.Clear();
            foreach (DataRow row in replacementTable.Rows)
            {
                int position = Convert.ToInt32(row[0]);
                string tagName = row[1]?.ToString()?.Trim() ?? string.Empty;
                bool isUsed = Convert.ToBoolean(row[2]);

                if (isUsed && !string.IsNullOrEmpty(tagName))
                {
                    activeReplacements[position] = tagName;
                }
            }

            lblActiveReplacements.Text = $"Найдено: {activeReplacements.Count}";
        }
        #endregion

        #region Загрузка Excel
        private void btnLoadExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Title = "Выберите Excel-файл с экземплярами";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        excelFilePath = openFileDialog.FileName;
                        LoadExcelData(excelFilePath);
                        lblExcelInfo.Text = $"Excel: {Path.GetFileName(excelFilePath)}";
                        AddLogMessage($"Excel файл загружен: {Path.GetFileName(excelFilePath)} ({instances.Count} строк, {instances.GroupBy(x => x.InstanceName).Count()} экземпляров)", LogType.Success);
                        UpdateMappingInfo();
                    }
                    catch (Exception ex)
                    {
                        AddLogMessage($"Ошибка загрузки Excel: {ex.Message}", LogType.Error);
                        MessageBox.Show($"Ошибка загрузки Excel:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadExcelData(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            instances.Clear();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string instanceName = worksheet.Cells[row, 1]?.Text?.Trim() ?? string.Empty;
                    string tagNo = worksheet.Cells[row, 2]?.Text?.Trim() ?? string.Empty;
                    string positionStr = worksheet.Cells[row, 3]?.Text?.Trim() ?? string.Empty;

                    if (!string.IsNullOrEmpty(instanceName) && !string.IsNullOrEmpty(tagNo) && int.TryParse(positionStr, out int position))
                    {
                        var instance = instances.FirstOrDefault(i => i.InstanceName == instanceName);
                        if (instance == null)
                        {
                            instance = new InstanceData(instanceName);
                            instances.Add(instance);
                        }
                        instance.Replacements[position] = tagNo;
                    }
                }
            }
        }
        #endregion

        #region Вспомогательные методы для работы с тегами

        private string GetSecondPartOfTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                return string.Empty;
            
            var parts = tag.Split('_');
            if (parts.Length >= 3)
            {
                return parts[2];
            }
            return string.Empty;
        }

        private string UpdateXmlNameAttribute(string content, string instanceName)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(instanceName))
                return content;
            
            try
            {
                string pattern = @"NAME\s*=\s*""([^""]*)""";
                var match = Regex.Match(content, pattern, RegexOptions.IgnoreCase);
                
                if (match.Success)
                {
                    string newValue = Path.GetFileNameWithoutExtension(instanceName);
                    
                    if (string.IsNullOrEmpty(newValue))
                        return content;
                    
                    string result = Regex.Replace(content, pattern, $"NAME=\"{newValue}\"", RegexOptions.IgnoreCase);
                    return result;
                }
                
                return content;
            }
            catch (Exception ex)
            {
                AddLogMessage($"Ошибка при изменении атрибута NAME в XML: {ex.Message}", LogType.Warning);
                return content;
            }
        }

        private string ConvertToRtf(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            string escapedText = text
                .Replace(@"\", @"\\")
                .Replace("{", @"\{")
                .Replace("}", @"\}");
            
            var parts = new List<string>();
            int currentIndex = 0;
            int startIndex;
            
            while ((startIndex = escapedText.IndexOf("[RED]", currentIndex, StringComparison.Ordinal)) != -1)
            {
                if (startIndex > currentIndex)
                {
                    parts.Add(escapedText.Substring(currentIndex, startIndex - currentIndex));
                }
                
                int endIndex = escapedText.IndexOf("[/RED]", startIndex + 5, StringComparison.Ordinal);
                if (endIndex == -1)
                {
                    parts.Add(escapedText.Substring(startIndex));
                    break;
                }
                
                string highlightedText = escapedText.Substring(startIndex + 5, endIndex - startIndex - 5);
                parts.Add($"\\cf1 {highlightedText}\\cf0");
                
                currentIndex = endIndex + 6;
            }
            
            if (currentIndex < escapedText.Length)
            {
                parts.Add(escapedText.Substring(currentIndex));
            }
            
            string content = string.Join("", parts);
            content = content.Replace("\r\n", "\\par ");
            content = content.Replace("\n", "\\par ");
            
            string rtf = $@"{{\rtf1\ansi\deff0
{{\fonttbl{{\f0\fnil\fcharset204 Consolas;}}}}
{{\colortbl;\red255\green0\blue0;}}
\viewkind4\uc1\pard\lang1049\f0\fs18
{content}
}}";
            
            return rtf;
        }
        #endregion

        #region Обновление информационной панели
        private void UpdateMappingInfo()
        {
            if (instances.Count == 0 || activeReplacements.Count == 0)
            {
                txtMappingInfo.Text = "Загрузите шаблон, Excel и заполните таблицу замен";
                return;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            sb.AppendLine(new string('=', 100));
            sb.AppendLine("                    Детали замен по экземплярам");
            sb.AppendLine(new string('=', 100));
            sb.AppendLine();

            var sortedPositions = activeReplacements.Keys.OrderBy(k => k).ToList();
            
            foreach (var instance in instances)
            {
                sb.AppendLine($"Экземпляр: {instance.InstanceName}");
                
                foreach (var pos in sortedPositions)
                {
                    string searchTag = activeReplacements[pos];
                    string replaceTag = instance.Replacements.ContainsKey(pos) ? instance.Replacements[pos] : "ОТСУТСТВУЕТ";

                    if (searchTag.StartsWith("_") && replaceTag != "ОТСУТСТВУЕТ")
                    {
                        if (!replaceTag.StartsWith("_"))
                        {
                            replaceTag = "_" + replaceTag;
                        }
                    }

                    if (convertTagsToUnderscore && replaceTag != "ОТСУТСТВУЕТ")
                    {
                        replaceTag = ConvertDashToUnderscore(replaceTag);
                    }

                    bool highlightMismatch = false;
                    if (replaceTag != "ОТСУТСТВУЕТ")
                    {
                        string searchSecondPart = GetSecondPartOfTag(searchTag);
                        string replaceSecondPart = GetSecondPartOfTag(replaceTag);
                        
                        if (!string.IsNullOrEmpty(searchSecondPart) && !string.IsNullOrEmpty(replaceSecondPart))
                        {
                            highlightMismatch = !searchSecondPart.Equals(replaceSecondPart, StringComparison.OrdinalIgnoreCase);
                        }
                    }

                    if (highlightMismatch)
                    {
                        string pattern = @"_([0-9]+)_([A-Za-z]+)_([0-9][A-Za-z0-9]*)";
                        var match = Regex.Match(replaceTag, pattern);
                        if (match.Success)
                        {
                            string part1 = match.Groups[1].Value;
                            string part2 = match.Groups[2].Value;
                            string part3 = match.Groups[3].Value;
                            
                            sb.AppendLine($"  Позиция {pos}: {searchTag} -> _{part1}_[RED]{part2}[/RED]_{part3}");
                        }
                        else
                        {
                            sb.AppendLine($"  Позиция {pos}: {searchTag} -> [RED]{replaceTag}[/RED]");
                        }
                    }
                    else
                    {
                        sb.AppendLine($"  Позиция {pos}: {searchTag} -> {replaceTag}");
                    }
                }
                
                sb.AppendLine(new string('-', 80));
            }

            sb.AppendLine();

            sb.AppendLine(new string('=', 100));
            sb.AppendLine("                              Статистика");
            sb.AppendLine(new string('=', 100));
            sb.AppendLine($"  Всего экземпляров:        {instances.Count}");
            sb.AppendLine($"  Активных позиций замен:   {activeReplacements.Count}");
            sb.AppendLine($"  Преобразование тегов:     {(convertTagsToUnderscore ? "ВКЛ" : "ВЫКЛ")}");

            string textWithMarkers = sb.ToString();
            string rtfText = ConvertToRtf(textWithMarkers);
            
            try
            {
                txtMappingInfo.Rtf = rtfText;
            }
            catch
            {
                string plainText = textWithMarkers.Replace("[RED]", "").Replace("[/RED]", "");
                txtMappingInfo.Text = plainText;
            }
            
            txtMappingInfo.SelectionStart = 0;
            txtMappingInfo.ScrollToCaret();
        }
        #endregion

        #region Преобразование тегов
        private string ConvertDashToUnderscore(string tag)
        {
            return tag?.Replace('-', '_') ?? string.Empty;
        }

        private void btnConvertTags_Click(object sender, EventArgs e)
        {
            convertTagsToUnderscore = !convertTagsToUnderscore;

            if (convertTagsToUnderscore)
            {
                btnConvertTags.Text = "Преобразование Вкл";
                btnConvertTags.BackColor = Color.FromArgb(46, 204, 113);
                btnConvertTags.ForeColor = Color.White;
                AddLogMessage("Режим преобразования включен", LogType.Process);
            }
            else
            {
                btnConvertTags.Text = "Преобразование Выкл";
                btnConvertTags.BackColor = Color.FromArgb(241, 196, 15);
                btnConvertTags.ForeColor = Color.Black;
                AddLogMessage("Режим преобразования выключен", LogType.Info);
            }

            UpdateMappingInfo();
        }
        #endregion

        #region Проверка данных
        private void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                var errors = new List<string>();
                var warnings = new List<string>();

                if (string.IsNullOrEmpty(templateContent))
                {
                    errors.Add("Шаблон не загружен");
                }

                UpdateActiveReplacements();
                if (activeReplacements.Count == 0)
                {
                    errors.Add("Нет активных позиций для замены");
                }

                if (instances.Count == 0)
                {
                    errors.Add("Excel-файл не загружен или не содержит данных");
                }

                if (instances.Count > 0 && activeReplacements.Count > 0)
                {
                    foreach (var instance in instances)
                    {
                        foreach (var pos in activeReplacements.Keys)
                        {
                            if (!instance.Replacements.ContainsKey(pos))
                            {
                                warnings.Add($"Для экземпляра {instance.InstanceName} отсутствует замена для позиции {pos}");
                            }
                        }
                    }
                }

                if (errors.Count == 0 && warnings.Count == 0)
                {
                    AddLogMessage("✅ Все данные корректны", LogType.Success);
                    MessageBox.Show("Все данные корректны. Можно выполнять генерацию.", "Проверка данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string message = "";
                    if (errors.Count > 0)
                    {
                        message += "ОШИБКИ:\n" + string.Join("\n", errors) + "\n\n";
                    }
                    if (warnings.Count > 0)
                    {
                        message += "ПРЕДУПРЕЖДЕНИЯ:\n" + string.Join("\n", warnings);
                    }

                    foreach (var error in errors)
                    {
                        AddLogMessage($"❌ {error}", LogType.Error);
                    }
                    foreach (var warning in warnings)
                    {
                        AddLogMessage($"⚠️ {warning}", LogType.Warning);
                    }

                    MessageBox.Show(message, "Результаты проверки", MessageBoxButtons.OK, errors.Count > 0 ? MessageBoxIcon.Error : MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                AddLogMessage($"Ошибка при проверке: {ex.Message}", LogType.Error);
                MessageBox.Show($"Ошибка при проверке:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Генерация файлов
        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            if (isGenerating)
                return;

            try
            {
                UpdateActiveReplacements();

                if (string.IsNullOrEmpty(templateContent))
                {
                    MessageBox.Show("Шаблон не загружен.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (activeReplacements.Count == 0)
                {
                    MessageBox.Show("Нет активных позиций для замены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (instances.Count == 0)
                {
                    MessageBox.Show("Excel-файл не загружен или не содержит данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Выберите папку для сохранения сгенерированных файлов";
                    if (folderDialog.ShowDialog() != DialogResult.OK)
                        return;

                    string outputFolder = folderDialog.SelectedPath;
                    string fileExtension = GetFileExtensionFromTemplate();
                    
                    AddLogMessage($"Начата генерация файлов в папку: {outputFolder}", LogType.Process);
                    AddLogMessage($"Тип выходных файлов: *{fileExtension}", LogType.Info);

                    isGenerating = true;
                    btnGenerate.Enabled = false;
                    btnGenerate.Text = "Генерация...";

                    int generatedCount = 0;
                    string historyFilePath = Path.Combine(outputFolder, "История замен.txt");

                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string historyHeader = new string('=', 100);
                    string historyEntry = $@"
{historyHeader}
Дата и время: {timestamp}
Количество сгенерированных файлов: {instances.Count}
Активных позиций замен: {activeReplacements.Count}
Преобразование тегов: {(convertTagsToUnderscore ? "ВКЛ" : "ВЫКЛ")}
Тип выходных файлов: *{fileExtension}
{historyHeader}

";

                    var sortedPositions = activeReplacements.Keys.OrderBy(k => k).ToList();
                    
                    historyEntry += "ДЕТАЛИ ЗАМЕН ПО ЭКЗЕМПЛЯРАМ:\n";
                    historyEntry += new string('-', 80) + "\n";

                    foreach (var instance in instances)
                    {
                        historyEntry += $"Экземпляр: {instance.InstanceName}\n";
                        foreach (var pos in sortedPositions)
                        {
                            string searchTag = activeReplacements[pos];
                            string replaceTag = instance.Replacements.ContainsKey(pos) ? instance.Replacements[pos] : "ОТСУТСТВУЕТ";

                            if (searchTag.StartsWith("_") && replaceTag != "ОТСУТСТВУЕТ")
                            {
                                if (!replaceTag.StartsWith("_"))
                                {
                                    replaceTag = "_" + replaceTag;
                                }
                            }

                            if (convertTagsToUnderscore && replaceTag != "ОТСУТСТВУЕТ")
                            {
                                replaceTag = ConvertDashToUnderscore(replaceTag);
                            }

                            historyEntry += $"  Позиция {pos}: {searchTag} -> {replaceTag}\n";
                        }
                        historyEntry += new string('-', 80) + "\n";
                    }

                    historyEntry += "\n";

                    await System.Threading.Tasks.Task.Run(() =>
                    {
                        foreach (var instance in instances)
                        {
                            try
                            {
                                string content = templateContent;

                                foreach (var pos in activeReplacements.Keys)
                                {
                                    string searchTag = activeReplacements[pos];
                                    string replaceTag = instance.Replacements.ContainsKey(pos) ? instance.Replacements[pos] : searchTag;

                                    if (searchTag.StartsWith("_") && replaceTag != "ОТСУТСТВУЕТ")
                                    {
                                        if (!replaceTag.StartsWith("_"))
                                        {
                                            replaceTag = "_" + replaceTag;
                                        }
                                    }

                                    if (convertTagsToUnderscore && replaceTag != "ОТСУТСТВУЕТ")
                                    {
                                        replaceTag = ConvertDashToUnderscore(replaceTag);
                                    }

                                    string escapedSearch = Regex.Escape(searchTag);
                                    content = Regex.Replace(content, escapedSearch, replaceTag);
                                }

                                if (fileExtension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                                {
                                    content = UpdateXmlNameAttribute(content, instance.InstanceName);
                                }

                                string fileName = instance.InstanceName;
                                string filePath = Path.Combine(outputFolder, fileName + fileExtension);

                                int counter = 1;
                                while (File.Exists(filePath))
                                {
                                    filePath = Path.Combine(outputFolder, $"{fileName}_{counter}" + fileExtension);
                                    counter++;
                                }

                                File.WriteAllText(filePath, content, System.Text.Encoding.UTF8);
                                generatedCount++;

                                this.Invoke((Action)(() =>
                                {
                                    AddLogMessage($"Создан файл: {Path.GetFileName(filePath)}", LogType.Action);
                                }));
                            }
                            catch (Exception ex)
                            {
                                this.Invoke((Action)(() =>
                                {
                                    AddLogMessage($"Ошибка при генерации для {instance.InstanceName}: {ex.Message}", LogType.Error);
                                }));
                            }
                        }

                        try
                        {
                            File.AppendAllText(historyFilePath, historyEntry, System.Text.Encoding.UTF8);
                            this.Invoke((Action)(() =>
                            {
                                AddLogMessage($"История замен сохранена: {Path.GetFileName(historyFilePath)}", LogType.Success);
                            }));
                        }
                        catch (Exception ex)
                        {
                            this.Invoke((Action)(() =>
                            {
                                AddLogMessage($"Ошибка сохранения истории замен: {ex.Message}", LogType.Error);
                            }));
                        }
                    });

                    AddLogMessage($"✅ Успешно сгенерировано {generatedCount} файлов", LogType.Success);
                    MessageBox.Show($"Генерация завершена!\nСоздано {generatedCount} файлов.\nТип файлов: *{fileExtension}\nИстория замен сохранена в файл: История замен.txt", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                AddLogMessage($"Ошибка при генерации: {ex.Message}", LogType.Error);
                MessageBox.Show($"Ошибка при генерации:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isGenerating = false;
                btnGenerate.Enabled = true;
                btnGenerate.Text = "Сгенерировать";
            }
        }

        private string GetFileExtensionFromTemplate()
        {
            if (string.IsNullOrEmpty(templateFilePath))
                return ".txt";
            
            string extension = Path.GetExtension(templateFilePath).ToLower();
            
            if (extension == ".txt" || extension == ".xml" || extension == ".csv" || extension == ".json")
                return extension;
            
            return ".txt";
        }
        #endregion

        #region Очистка всех данных
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите очистить все данные?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                try
                {
                    templateContent = string.Empty;
                    templateFilePath = string.Empty;
                    txtTemplatePreview.Clear();
                    lblTemplateInfo.Text = "Шаблон: -";

                    instances.Clear();
                    excelFilePath = string.Empty;
                    lblExcelInfo.Text = "Excel: -";

                    replacementTable.Clear();
                    for (int i = 1; i <= 10; i++)
                    {
                        replacementTable.Rows.Add(i, string.Empty, false);
                    }
                    replacementTable.Rows.Add(11, string.Empty, false);
                    
                    dgvReplacements.Refresh();
                    activeReplacements.Clear();

                    convertTagsToUnderscore = true;
                    btnConvertTags.Text = "Преобразование Вкл";
                    btnConvertTags.BackColor = Color.FromArgb(46, 204, 113);
                    btnConvertTags.ForeColor = Color.White;

                    txtMappingInfo.Clear();
                    rtbLog.Clear();

                    lblActiveReplacements.Text = "Найдено: 0";

                    AddLogMessage("🗑 Все данные очищены", LogType.Info);
                    AddLogMessage("Режим преобразования тегов включен по умолчанию", LogType.Info);
                }
                catch (Exception ex)
                {
                    AddLogMessage($"Ошибка при очистке: {ex.Message}", LogType.Error);
                }
            }
        }
        #endregion

        #region Логирование
        private void AddLogMessage(string message, LogType type)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke((Action)(() => AddLogMessage(message, type)));
                return;
            }

            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string prefix = type switch
                {
                    LogType.Info => "ℹ️",
                    LogType.Success => "✅",
                    LogType.Warning => "⚠️",
                    LogType.Error => "❌",
                    LogType.Process => "🚀",
                    LogType.Action => "📄",
                    _ => "ℹ️"
                };

                string logEntry = $"[{timestamp}] {prefix} {message}";

                Color color = type switch
                {
                    LogType.Info => Color.White,
                    LogType.Success => Color.FromArgb(46, 204, 113),
                    LogType.Warning => Color.FromArgb(241, 196, 15),
                    LogType.Error => Color.FromArgb(231, 76, 60),
                    LogType.Process => Color.FromArgb(52, 152, 219),
                    LogType.Action => Color.White,
                    _ => Color.White
                };

                rtbLog.SelectionStart = rtbLog.TextLength;
                rtbLog.SelectionLength = 0;
                rtbLog.SelectionColor = color;
                rtbLog.AppendText(logEntry + Environment.NewLine);
                rtbLog.SelectionStart = 0;
                rtbLog.SelectionLength = 0;
                rtbLog.ScrollToCaret();

                if (rtbLog.Lines.Length > MAX_LOG_MESSAGES)
                {
                    int linesToRemove = rtbLog.Lines.Length - MAX_LOG_MESSAGES;
                    int removeIndex = 0;
                    for (int i = 0; i < linesToRemove; i++)
                    {
                        removeIndex += rtbLog.Lines[i].Length + Environment.NewLine.Length;
                    }
                    if (removeIndex < rtbLog.TextLength)
                    {
                        rtbLog.Text = rtbLog.Text.Substring(removeIndex);
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Обработка изменений таблицы
        private void dgvReplacements_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                CheckAndAddNewRow();
                UpdateActiveReplacements();
                UpdateMappingInfo();
            }
        }

        private void dgvReplacements_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvReplacements.IsCurrentCellDirty)
            {
                dgvReplacements.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void CheckAndAddNewRow()
        {
            if (replacementTable.Rows.Count == 0)
                return;

            DataRow lastRow = replacementTable.Rows[replacementTable.Rows.Count - 1];
            string tagName = lastRow[1]?.ToString()?.Trim() ?? string.Empty;
            bool isUsed = Convert.ToBoolean(lastRow[2]);

            if (!string.IsNullOrEmpty(tagName) && isUsed)
            {
                bool hasEmptyRow = false;
                for (int i = replacementTable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = replacementTable.Rows[i];
                    string name = row[1]?.ToString()?.Trim() ?? string.Empty;
                    bool used = Convert.ToBoolean(row[2]);
                    
                    if (string.IsNullOrEmpty(name) && !used)
                    {
                        hasEmptyRow = true;
                        break;
                    }
                }
                
                if (!hasEmptyRow)
                {
                    int newPosition = replacementTable.Rows.Count + 1;
                    replacementTable.Rows.Add(newPosition, string.Empty, false);
                    dgvReplacements.Refresh();
                    AddLogMessage($"Добавлена новая позиция для замены: {newPosition}", LogType.Info);
                }
            }
        }
        #endregion
    }

    #region Вспомогательные классы
    public class InstanceData
    {
        public string InstanceName { get; set; }
        public Dictionary<int, string> Replacements { get; set; }

        public InstanceData(string name)
        {
            InstanceName = name;
            Replacements = new Dictionary<int, string>();
        }
    }

    public enum LogType
    {
        Info,
        Success,
        Warning,
        Error,
        Process,
        Action
    }
    #endregion
}