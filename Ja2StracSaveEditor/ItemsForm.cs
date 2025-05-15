using System.ComponentModel;
using Ja2StracSaveEditorLib.Managers;
using Zuby.ADGV;

namespace Ja2StracSaveEditor;

public partial class ItemsForm : Form
{
    private readonly DataGridX<Item> _grid = new DataGridX<Item>();

    private readonly BindingList<Item> _items = new BindingList<Item>();
    //private readonly BindingSource _source = new BindingSource();

    public Item Item { get; set; }

    public ItemsForm()
    {
        InitializeComponent();
        InitGrid();

        _grid.ConfigureDatasource(_items);
    }

    public ItemsForm(List<Item> items)
    {
        InitializeComponent();
        InitGrid();

        _grid.ConfigureDatasource(_items);

        if (items != null)
        {
            foreach (var item in items)
            {
                _items.Add(item);
            }
        }
    }

    private void InitGrid()
    {
        ((ISupportInitialize)_grid).BeginInit();
        SuspendLayout();
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        _grid.Dock = DockStyle.Fill;
        _grid.FilterAndSortEnabled = true;
        _grid.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
        _grid.Location = new Point(0, 27);
        _grid.MaxFilterButtonImageHeight = 23;
        _grid.MultiSelect = false;
        _grid.Name = "grid";
        _grid.ReadOnly = true;
        _grid.RightToLeft = RightToLeft.No;
        _grid.Size = new Size(800, 423);
        _grid.SortStringChangedInvokeBeforeDatasourceUpdate = true;
        _grid.TabIndex = 0;
        _grid.RowTemplate.Height = 50; // устанавливает шаблонную высоту
        _grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        Controls.Add(_grid);
        ResumeLayout(false);
        PerformLayout();
        ((ISupportInitialize)_grid).EndInit();
        _grid.BringToFront();
        _grid.CellDoubleClick += GridOnCellDoubleClick;
        _grid.CellFormatting += Grid_CellFormatting;
    }

    private void GridOnCellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || _grid.Rows[e.RowIndex].DataBoundItem is not Item item) return;

        DialogResult = DialogResult.OK;
        Item = item;
        Close();
    }

    private void ItemsForm_Load(object sender, EventArgs e)
    {
        var imageColumn = new DataGridViewImageColumn
        {
            Name = "Image",
            HeaderText = @"Image",
            ImageLayout = DataGridViewImageCellLayout.Zoom, // или Stretch
            Width = 150
        };
        _grid.Columns.Insert(0, imageColumn);

        var internalNameCol = _grid.Columns[nameof(Item.internalName)];
        if (internalNameCol != null)
        {
            internalNameCol.Width = 300;
            internalNameCol.DisplayIndex = 1;
        }

        var hiddenColumns = new HashSet<string>
        {
            nameof(Item.inventoryGraphics),
            nameof(Item.tileGraphic),
        };

        foreach (var col in hiddenColumns
                     .Select(colName => _grid.Columns[colName])
                     .Where(col => col != null))
        {
            col.Visible = false;
        }

        search.SetColumns(_grid.Columns);
        search.Search += SearchOnSearch;
    }

    private void SearchOnSearch(object sender, AdvancedDataGridViewSearchToolBarSearchEventArgs e)
    {
        var startColumn = 0;
        var startRow = 0;
        if (!e.FromBegin)
        {
            var endcol = _grid.CurrentCell.ColumnIndex + 1 >= _grid.ColumnCount;
            var endrow = _grid.CurrentCell.RowIndex + 1 >= _grid.RowCount;

            if (endcol && endrow)
            {
                startColumn = _grid.CurrentCell.ColumnIndex;
                startRow = _grid.CurrentCell.RowIndex;
            }
            else
            {
                startColumn = endcol ? 0 : _grid.CurrentCell.ColumnIndex + 1;
                startRow = _grid.CurrentCell.RowIndex + (endcol ? 1 : 0);
            }
        }

        var c = _grid.FindCell(
            e.ValueToSearch,
            e.ColumnToSearch?.Name,
            startRow,
            startColumn,
            e.WholeWord,
            e.CaseSensitive) ?? _grid.FindCell(
            e.ValueToSearch,
            e.ColumnToSearch?.Name,
            0,
            0,
            e.WholeWord,
            e.CaseSensitive);
        if (c != null)
            _grid.CurrentCell = c;
    }

    private void SelectRowByColumnValue(string columnName, object value)
    {
        foreach (DataGridViewRow row in _grid.Rows)
        {
            if (row.Cells[columnName].Value == null ||
                !row.Cells[columnName].Value.Equals(value)) continue;

            row.Selected = true;
            _grid.CurrentCell = row.Cells[columnName]; // Устанавливаем курсор
            _grid.FirstDisplayedScrollingRowIndex = row.Index; // Прокручиваем к строке
            break;
        }
    }

    private void ItemsForm_Shown(object sender, EventArgs e)
    {
        if (Item == null) return;
        SelectRowByColumnValue(nameof(Item.itemIndex), Item.itemIndex);
    }

    private readonly Dictionary<string, Image> _imageCache = new();
    private readonly Image _fallbackImage = Image.FromFile(Program.ImageNotFound);
    private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (_grid.Columns[e.ColumnIndex].Name != "Image" || e.RowIndex < 0)
            return;

        var row = _grid.Rows[e.RowIndex];
        if (row.DataBoundItem is not Item item)
            return;

        var path = Exts.GetItemImageFilename(item);

        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            e.Value = _fallbackImage;
            e.FormattingApplied = true;
            return;
        }

        if (_imageCache.TryGetValue(path, out var cachedImage))
        {
            e.Value = cachedImage;
        }
        else
        {
            try
            {
                using var temp = Image.FromFile(path);
                var bmp = new Bitmap(temp);
                _imageCache[path] = bmp;
                e.Value = bmp;
            }
            catch
            {
                e.Value = _fallbackImage;
            }
        }

        e.FormattingApplied = true;
    }
}