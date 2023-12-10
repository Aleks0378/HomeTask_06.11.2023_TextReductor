using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace HomeTask_06._11._2023_TextReductor
{
    public partial class Form1 : Form
    {
        private TextBox editorTextBox;
        private int textBoxSelectionPosition;
        private Label wordsQuantLabel;
        private Panel panel;

        private void NewDocument()
        {
            editorTextBox.Text = string.Empty;
            this.Text = "Новый документ";
        }
        public Form1()
        {
            InitializeComponent();

            editorTextBox = new TextBox()
            {
                Multiline = true,
                Dock = DockStyle.Fill,
            };

            editorTextBox.TextChanged += (sender, e) =>
            {
                wordsQuantLabel.Text=$"Words: {editorTextBox.Text.Split(new char[] {' ',',','.',';',':','"','\''}, StringSplitOptions.RemoveEmptyEntries).Length}";
                if (editorTextBox.SelectionStart > 0)
                {
                    textBoxSelectionPosition = editorTextBox.SelectionStart;
                }
            };

            panel = new Panel() 
            {
              Height = 20,
              Dock = DockStyle.Bottom,  
            };
            
            wordsQuantLabel = new Label()
            {
                Dock = DockStyle.Left,
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           

            MenuStrip menuStrip = new MenuStrip();

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            ToolStripMenuItem editMenu = new ToolStripMenuItem("Edit");
            ToolStripMenuItem outputMenu = new ToolStripMenuItem("Output");

            ToolStripMenuItem newItem = new ToolStripMenuItem("New Document");
            ToolStripMenuItem openItem = new ToolStripMenuItem("Open");
            ToolStripMenuItem saveItem = new ToolStripMenuItem("Save");
            ToolStripMenuItem copyItem = new ToolStripMenuItem("Copy");
            ToolStripMenuItem pasteItem = new ToolStripMenuItem("Paste");
            ToolStripMenuItem cutItem = new ToolStripMenuItem("Cut");
            ToolStripMenuItem cancelItem = new ToolStripMenuItem("Cancel");
            ToolStripMenuItem settingsItem = new ToolStripMenuItem("Settings");
            ToolStripMenuItem printItem = new ToolStripMenuItem("Print");

            fileMenu.DropDownItems.AddRange(new ToolStripMenuItem[]
            {
                newItem, openItem, saveItem
            });

            editMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                copyItem, pasteItem, cutItem, cancelItem, settingsItem
            });

            outputMenu.DropDownItems.AddRange(new ToolStripItem[]
           {
               printItem
           });

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(editMenu);
            menuStrip.Items.Add(outputMenu);

            newItem.Click += (sender, e) =>
            {
                NewDocument();
            };

            saveItem.Click += (sender, e) =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Txt files|*.txt|All files|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                    string content = editorTextBox.Text;
                    File.WriteAllText(fileName, content);
                    this.Text = fileName;
                }
            };

            openItem.Click += (sender, e) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Txt files|*.txt|All files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = openFileDialog.FileName;
                    string content = File.ReadAllText(fileName);
                    editorTextBox.Text = content;
                    this.Text = fileName;
                }
            };

            void Copy()
            {
                if (editorTextBox.Text.Substring(editorTextBox.SelectionStart, editorTextBox.SelectionLength) != "")
                        Clipboard.SetText(editorTextBox.Text.Substring(editorTextBox.SelectionStart, editorTextBox.SelectionLength));
                else Clipboard.Clear();
            }

            void Paste()
            {
                editorTextBox.SelectionStart = textBoxSelectionPosition;
                editorTextBox.Text += Clipboard.GetText();
            }
            void Cut()
            {
                Copy();
                editorTextBox.Text = editorTextBox.Text.Remove(editorTextBox.SelectionStart, editorTextBox.SelectionLength);
            }
            void Cancel()
            {
                editorTextBox.Undo();
            }
            void Settings()
            {
                FontDialog fontDialog = new FontDialog();
                fontDialog.ShowDialog();
                editorTextBox.Font = fontDialog.Font;
            }

            copyItem.Click += (sender, e) =>
            {
                Copy();
            };
            pasteItem.Click += (sender, e) =>
            {
                Paste();
            };
            cutItem.Click += (sender, e) =>
            {
                Cut();
            };
            cancelItem.Click += (sender, e) =>
            {
                Cancel();
            };
            settingsItem.Click += (sender, e) =>
            {
                Settings();
            };
            printItem.Click += (sender, e) =>
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    PrintDocument pd = new PrintDocument();
                    pd.Print();
                }
            };
            editorTextBox.ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem copyItemContex = new ToolStripMenuItem("Copy");
            ToolStripMenuItem pasteItemContex = new ToolStripMenuItem("Paste");
            ToolStripMenuItem cutItemContex = new ToolStripMenuItem("Cut");
            ToolStripMenuItem cancelItemContex = new ToolStripMenuItem("Cancel");
            ToolStripMenuItem settingsItemContex = new ToolStripMenuItem("Settings");

            copyItemContex.Click += (sender, e) =>
            {
                Copy();
            };
            pasteItemContex.Click += (sender, e) =>
            {
                Paste();
            };
            cutItemContex.Click += (sender, e) =>
            {
                Cut();
            };
            cancelItemContex.Click += (sender, e) =>
            {
                Cancel();
            };
            settingsItemContex.Click += (sender, e) =>
            {
                Settings();
            };

            editorTextBox.ContextMenuStrip.Items.AddRange(new ToolStripItem[] { copyItemContex, cutItemContex, pasteItemContex, cancelItemContex, settingsItemContex });

            this.Controls.Add(editorTextBox);
            this.Controls.Add(menuStrip);
            panel.Controls.Add(wordsQuantLabel);
            this.Controls.Add(panel);
            NewDocument();
        }


    }
}
