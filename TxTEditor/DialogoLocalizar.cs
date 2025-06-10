using System;
using System.Windows.Forms;

public class DialogoLocalizar : Form
{
    public event Action<string> TextoProcuradoAtualizado;
    public event Action<string> SubstituirTextoRequisitado;
    public event Action<bool> HighlightAllToggled;
    public event Action<string> ReplaceAllRequested;

    private TextBox textBoxFind;
    private TextBox textBoxReplace;
    private Button buttonFindNext;
    private Button buttonReplace;
    private Button buttonReplaceAll;
    private CheckBox checkHighlightAll;

    public DialogoLocalizar()
    {
        this.Text = "Localizar e Substituir";
        this.Width = 425;
        this.Height = 155;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        Label labelFind = new Label() { Text = "Localizar:", Left = 10, Top = 15, Width = 60 };
        textBoxFind = new TextBox() { Left = 75, Top = 12, Width = 170 };

        Label labelReplace = new Label() { Text = "Substituir:", Left = 10, Top = 45, Width = 65 };
        textBoxReplace = new TextBox() { Left = 75, Top = 42, Width = 170 };

        buttonFindNext = new Button() { Text = "Localizar próximo", Left = 250, Top = 10, Width = 90 };
        buttonReplace = new Button() { Text = "Substituir", Left = 250, Top = 40, Width = 90 };
        buttonReplaceAll = new Button() { Text = "Substituir tudo", Left = 250, Top = 70, Width = 100 };

        checkHighlightAll = new CheckBox() { Text = "Localizar tudo", Left = 75, Top = 75, Width = 135 };

        this.Controls.Add(labelFind);
        this.Controls.Add(textBoxFind);
        this.Controls.Add(labelReplace);
        this.Controls.Add(textBoxReplace);
        this.Controls.Add(buttonFindNext);
        this.Controls.Add(buttonReplace);
        this.Controls.Add(buttonReplaceAll);
        this.Controls.Add(checkHighlightAll);

        buttonFindNext.Click += (s, e) => TextoProcuradoAtualizado?.Invoke(textBoxFind.Text);
        buttonReplace.Click += (s, e) => SubstituirTextoRequisitado?.Invoke(textBoxReplace.Text);
        buttonReplaceAll.Click += (s, e) => ReplaceAllRequested?.Invoke(textBoxReplace.Text);
        checkHighlightAll.CheckedChanged += (s, e) => HighlightAllToggled?.Invoke(checkHighlightAll.Checked);

        textBoxFind.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) buttonFindNext.PerformClick(); };
        textBoxReplace.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) buttonReplace.PerformClick(); };
    }
}