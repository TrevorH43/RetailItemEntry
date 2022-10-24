using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace RetailItemEntry
{
    public partial class MainForm : Form
    {
        RetailItem retailItem;

        /*
         *  Declare a BindingList instead of List.  A BindingList is a base class which 
         *  supports data binding and will allow for binding to our datagrid. 
        */
        BindingList<RetailItem> retailItemList = new BindingList<RetailItem>();

        /*
         * The datagrid allows the assignment of a source which needs to be of type bindingSource.
         * We declare a BindingSource which we will create using our BindingList which will
         * allow us to view our Retail Item Objects, update and delete them in the datagridview.
        */
        BindingSource bindingSource;


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            try
            {
                // Give our retailItemList as the data source
                bindingSource = new BindingSource(retailItemList, null);

                // Now that we our source created we can assign that to our datagrid 
                dataGridView.DataSource = bindingSource;

                IWorkbook workBook;

                // Open the File.  NPOI requires a file stream.
                // Filestream physical byte level.  StreamReader is a helper class that
                // uses the file stream and converts the bytes into strings.
                using (FileStream fs = new FileStream("retail.xlsx", FileMode.Open, FileAccess.Read))
                {
                    // Create an instance of the XLSX workbook from the filestream
                    workBook = new XSSFWorkbook(fs);

                    // Set the sheet as the first sheet in the spreadsheet.  
                    // You can reference the sheet either by an index, 0 = first sheet in spreadsheet
                    ISheet sheet = workBook.GetSheetAt(0);

                    // Or by the explicit sheet name
                    //ISheet sheet = workBook.GetSheetAt("Sheet1");

                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        // Get the worksheet row
                        IRow curRow = sheet.GetRow(row);

                        // We shouldn't have a row that doesn't contain any cells with data
                        // If we do, we want to get out
                        if (curRow == null)
                            break;

                        // Otherwise we want to process the row
                        if (sheet.GetRow(row) != null)
                        {
                            // Each column must be access using an index --> it is an array
                            // You can get a column using the GetCell which returns an instance of the ICell
                            // If we explicitly want to get the cell, we can.
                            ICell column = curRow.GetCell(0);

                            // from the column, then we can use the properties to grab the value.
                            // You have to know what data you have in a column otherwise, if you try to
                            // get the wrong type out, it could cause a problem.  Our first column is a
                            // description, so we use the StringCellValue property to get the value out and
                            // then we are just going to trim the contents
                            string description = column.StringCellValue.Trim();

                            // the main properties used to extract data include the following. 
                            //DateTime myDate = column.DateCellValue
                            //bool myBoolean = column.BooleanCellValue
                            //double myNumber = column.NumericCellValue

                            // Generally we don't go through the separate step to get the column
                            // as a separate variable before retrieving the property value.  Here
                            // we show getting each cell (column) and then the numeric property value
                            // The NumericCellValue always returns a double
                            double unitsOnHand = curRow.GetCell(1).NumericCellValue;
                            double price = curRow.GetCell(2).NumericCellValue;

                            // create a new instance of the object using the all parameter constructor
                            retailItem = new RetailItem(description, unitsOnHand, price);

                            // add it to the list which is bound to the datagrid
                            retailItemList.Add(retailItem);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Excel read error");
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (dialogResult == DialogResult.Yes)
            {
                if (dataGridView.CurrentCell.RowIndex >= 0 && dataGridView.CurrentCell.RowIndex < retailItemList.Count)
                {
                    retailItemList.RemoveAt(dataGridView.CurrentCell.RowIndex);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentCell.RowIndex >= 0 && dataGridView.CurrentCell.RowIndex < retailItemList.Count)
            {
                // Get the current retail Item.  Think of this binding list as an array...
                // The dataGridView.CurrentCell.RowIndex has the index in the BindingList
                // that is bound to the datagridview that is currently selected.  We can
                // use this index, to access the array and pull out our value stored there.
                // In this case, the value is an RetailItem object.
                RetailItem retailItem = retailItemList[dataGridView.CurrentCell.RowIndex];

                // Now we can use the ToString method on the retailItem instance to show the 
                // row information
                MessageBox.Show(retailItem.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (RetailForm retailForm = new RetailForm())
            {
                retailForm.ShowDialog();
                if (retailForm.retailItem != null)
                {
                    retailItemList.Add(retailForm.retailItem);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //saveFileDialog1
            saveFileDialog1.Filter = "Excel Files|*.xlsx";
            saveFileDialog1.Title = "Save an Excel File";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Create an instance of a workbook
                IWorkbook workBook = new XSSFWorkbook();

                // Create a worksheet in the workbook named Sheet1
                ISheet sheet = workBook.CreateSheet("Sheet1");

                // Set initial values on the row number and column number
                int rowNum = 0, colNum = 0;

                // Create a row in the spreadsheet that will be used for writing the
                // datagrid column headers
                IRow row = sheet.CreateRow(rowNum++);

                // Now iterate over the column collection in the datagrid and get the column header
                foreach (DataGridViewColumn dataGridViewColumn in dataGridView.Columns)
                {
                    // Create a new cell (column) in the row based on the indicated column number
                    ICell cell = row.CreateCell(colNum++);

                    // Add the column Header Text to the cell
                    cell.SetCellValue(dataGridViewColumn.HeaderText);
                }

                // Now iterate over every retail item object in the bindinglist
                foreach (RetailItem retailItem in retailItemList)
                {
                    // Add another row to the spreadsheet
                    row = sheet.CreateRow(rowNum++);

                    // Reset the column index
                    colNum = 0;

                    // Since we don't want to do multiple instances of creating a
                    // cell and then setting the cell value, we can chain those
                    // operations into a single statement.  It will create the cell 
                    // first and then write the description to the cell
                    row.CreateCell(colNum++).SetCellValue(retailItem.Description);

                    // Create the column in the row and then write the Units on Hand value
                    row.CreateCell(colNum++).SetCellValue(retailItem.UnitsOnHand);

                    // This is the last column to write, so we don't need to increment the
                    // column counter when creating the Price column in the row and writing out
                    // the price
                    row.CreateCell(colNum).SetCellValue(retailItem.Price);
                }

                using (FileStream sw = File.Create(saveFileDialog1.FileName))
                {
                    workBook.Write(sw);
                    MessageBox.Show($"File, {saveFileDialog1.FileName}, was successfully created.");
                }
            }
        }
    }
}
  