using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RetailItemEntry
{
    public partial class RetailForm : Form
    {
        public RetailItem retailItem = null;
        public RetailForm()
        {
            InitializeComponent();
        }

        private void RetailForm_Load_1(object sender, EventArgs e)
        {
            this.AutoValidate = AutoValidate.Disable;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void descriptionTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (descriptionTextBox.Text.Length == 0)
            {
                errorProvider1.SetError(descriptionTextBox, "Item Description is required.");
                e.Cancel = true;
            }
            else
            {
                retailItem.Description = descriptionTextBox.Text;
                errorProvider1.SetError(descriptionTextBox, "");
                e.Cancel = false;
            }
        }
        private bool ValidateNumericTextBox(TextBox textBox, String fieldName, out double? value)
        {
            value = null;
            double doubleValue = 0;
            bool valid = false;

            if (textBox.Text.Length == 0)
                errorProvider1.SetError(textBox, $"{fieldName} is required.");
            else if (!double.TryParse(textBox.Text, out doubleValue))
                errorProvider1.SetError(textBox, $"{fieldName} is not a valid number.");
            else
            {
                valid = true;
                value = doubleValue;
                errorProvider1.SetError(textBox, "");
            }
            return valid;
        }

        private void unitsOnHandTextBox_Validating(object sender, CancelEventArgs e)
        {
            double? unitsOnHand = null;
            bool valid = ValidateNumericTextBox(unitsOnHandTextBox, "Units on Hand", out unitsOnHand);
            if (valid)
                retailItem.UnitsOnHand = unitsOnHand.Value;
            e.Cancel = !valid;
        }

        private void priceTextBox_Validating(object sender, CancelEventArgs e)
        {
            double? price = null;
            bool valid = ValidateNumericTextBox(priceTextBox, "Price", out price);
            if (valid)
                retailItem.Price = price.Value;
            e.Cancel = !valid;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            retailItem = new RetailItem();
            if (ValidateChildren())
            {
                this.Close();
            }
            else
            {
                retailItem = null;
            }
        }
    }
}
