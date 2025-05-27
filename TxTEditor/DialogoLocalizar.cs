using System;
using System.Windows.Forms;
using System.Drawing;

namespace EditorDeTextoSimples
{
    public class DialogoLocalizar : Form
    {
        public event Action<string> TextoProcuradoAtualizado;
        public event Action<string> SubstituirTextoRequisitado;

        private TextBox caixaTexto;
        private Button botaoProcurarSeguinte;

        public DialogoLocalizar()
        {
            this.Text = "Localizar";
            this.Width = 300;
            this.Height = 210;
            this.TopMost = true;

            Label label = new Label();
            label.Text = "Insira o texto a procurar:";
            label.Dock = DockStyle.Top;
            label.Padding = new Padding(10);

            caixaTexto = new TextBox();
            caixaTexto.Dock = DockStyle.Top;
            caixaTexto.Padding = new Padding(10);

            botaoProcurarSeguinte = new Button();
            botaoProcurarSeguinte.Text = "Procurar seguinte";
            botaoProcurarSeguinte.Dock = DockStyle.Top;
            botaoProcurarSeguinte.Click += (s, e) =>
            {
                TextoProcuradoAtualizado?.Invoke(caixaTexto.Text);
            };

            Button botaoSubstituir = new Button();
            botaoSubstituir.Text = "Substituir";
            botaoSubstituir.Dock = DockStyle.Top;
            botaoSubstituir.Click += (s, e) =>
            {
                string textoSubstituir = Microsoft.VisualBasic.Interaction.InputBox(
                    "Insira o texto para substituir:",
                    "Substituir Texto",
                    ""
                );
                SubstituirTextoRequisitado?.Invoke(textoSubstituir);
            };

            Button botaoFechar = new Button();
            botaoFechar.Text = "Fechar";
            botaoFechar.Dock = DockStyle.Bottom;
            botaoFechar.Click += (s, e) => this.Close();

            this.Controls.Add(label);
            this.Controls.Add(caixaTexto);
            this.Controls.Add(botaoProcurarSeguinte);
            this.Controls.Add(botaoSubstituir);
            this.Controls.Add(botaoFechar);

            caixaTexto.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TextoProcuradoAtualizado?.Invoke(caixaTexto.Text);
                    e.SuppressKeyPress = true;
                }
            };

            this.Load += (s, e) => caixaTexto.Focus();
        }
    }
}