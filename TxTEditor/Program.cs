using System;
using System.Windows.Forms;
using System.IO;

namespace SimpleTextEditor
{
    class Program
    {
        [STAThread]  // Required for Windows Forms
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new TextEditorForm());
        }
    }

    public class TextEditorForm : Form
    {
        private TextBox textBox;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem openItem;
        private ToolStripMenuItem saveItem;

        public TextEditorForm()
        {
            // Configure the form
            this.Text = "Simple Text Editor";
            this.Width = 800;
            this.Height = 600;

            // Create a textbox
            textBox = new TextBox();
            textBox.Multiline = true;
            textBox.Dock = DockStyle.Fill;
            textBox.ScrollBars = ScrollBars.Both;

            // Create a menu bar
            menuStrip = new MenuStrip();
            fileMenu = new ToolStripMenuItem("File");
            openItem = new ToolStripMenuItem("Open");
            saveItem = new ToolStripMenuItem("Save");

            // Add events
            openItem.Click += OpenFile;
            saveItem.Click += SaveFile;

            // Assemble the menu
            fileMenu.DropDownItems.Add(openItem);
            fileMenu.DropDownItems.Add(saveItem);
            menuStrip.Items.Add(fileMenu);

            // Add controls to the form
            this.Controls.Add(textBox);
            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void SaveFile(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, textBox.Text);
            }
        }
    }
}