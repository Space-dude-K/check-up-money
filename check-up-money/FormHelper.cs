using System.Drawing;
using System.Windows.Forms;

namespace check_up_money
{
    class FormHelper : IFormHelper
    {
        public TableLayoutPanel CreateTableLayoutForFileCounters()
        {
            Color counterColor = Color.LightGray;

            int collCountForFirstRow = 6;
            int collCountForSecondRow = 6;
            int collCountForThirdRow = 6;
            int collCountForFourth = 6;
            int collCountForFifth = 6;
            int collCountForSix = 6;

            TableLayoutPanel tlp = new TableLayoutPanel();
            tlp.Name = "tableLayoutPanel_Main";
            tlp.AutoSize = true;
            tlp.Dock = DockStyle.Bottom;
            tlp.AutoScroll = false;
            tlp.VerticalScroll.Visible = false;
            tlp.HorizontalScroll.Visible = false;
            tlp.RowCount = 2;

            for (int c = 0; c < collCountForFirstRow; c++)
            {
                Label lb = new Label();
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.67f));
                lb.AutoSize = true;
                lb.Margin = new Padding(1);
                lb.Dock = DockStyle.Bottom;
                lb.BackColor = counterColor;
                lb.TextAlign = ContentAlignment.MiddleCenter;
                lb.Text = "-";
                tlp.Controls.Add(lb, c, 0);
            }

            for (int c = 0; c < collCountForSecondRow; c++)
            {
                Label lb = new Label();
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.67f));
                lb.AutoSize = true;
                lb.Margin = new Padding(1);
                lb.Dock = DockStyle.Bottom;
                lb.BackColor = counterColor;
                lb.TextAlign = ContentAlignment.MiddleCenter;
                lb.Text = "-";
                tlp.Controls.Add(lb, c, 1);
            }

            for (int c = 0; c < collCountForThirdRow; c++)
            {
                Label lb = new Label();
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.67f));
                lb.AutoSize = true;
                lb.Margin = new Padding(1);
                lb.Dock = DockStyle.Bottom;
                lb.BackColor = counterColor;
                lb.TextAlign = ContentAlignment.MiddleCenter;
                lb.Text = "-";
                tlp.Controls.Add(lb, c, 2);
            }

            for (int c = 0; c < collCountForFourth; c++)
            {
                Label lb = new Label();
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.67f));
                lb.AutoSize = true;
                lb.Margin = new Padding(1);
                lb.Dock = DockStyle.Bottom;
                lb.BackColor = counterColor;
                lb.TextAlign = ContentAlignment.MiddleCenter;
                lb.Text = "-";
                tlp.Controls.Add(lb, c, 3);
            }

            for (int c = 0; c < collCountForFifth; c++)
            {
                Label lb = new Label();
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.67f));
                lb.AutoSize = true;
                lb.Margin = new Padding(1);
                lb.Dock = DockStyle.Bottom;
                lb.BackColor = counterColor;
                lb.TextAlign = ContentAlignment.MiddleCenter;
                lb.Text = "-";
                tlp.Controls.Add(lb, c, 4);
            }

            for (int c = 0; c < collCountForSix; c++)
            {
                Label lb = new Label();
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.67f));
                lb.AutoSize = true;
                lb.Margin = new Padding(1);
                lb.Dock = DockStyle.Bottom;
                lb.BackColor = counterColor;
                lb.TextAlign = ContentAlignment.MiddleCenter;
                lb.Text = "-";
                tlp.Controls.Add(lb, c, 5);
            }

            // Merge cells.
            //tlp.SetColumnSpan(tlp.GetControlFromPosition(0, 1), 3);
            //tlp.SetColumnSpan(tlp.GetControlFromPosition(3, 1), 3);
            //tlp.SetColumnSpan(tlp.GetControlFromPosition(0, 2), 6);

            //this.Controls.Add(tlp);

            return tlp;
        }
    }
}