using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Text.RegularExpressions; // Добавлено для Regex

namespace EditorDeTextoSimples
{
    public class FormularioEditorDeTexto : Form
    {
        private RichTextBox caixaDeTexto { get; set; }
        private MenuStrip barraDeMenus;
        private ToolStripMenuItem menuFicheiro;
        private ToolStripMenuItem itemAbrir;
        private ToolStripMenuItem itemGuardar;
        private ToolStripMenuItem itemGuardarComo;
        private ToolStripMenuItem itemImprimir;

        private int ultimaPosicaoPesquisa = 0;
        private string textoProcurado = "";

        private string currentFilePath = null;
        private int currentFileFilterIndex = 1;

        public FormularioEditorDeTexto()
        {
            this.Text = "Editor de Texto Simples";
            this.Width = 800;
            this.Height = 600;

            caixaDeTexto = new RichTextBox();
            caixaDeTexto.Multiline = true;
            caixaDeTexto.Dock = DockStyle.Fill;
            caixaDeTexto.ScrollBars = RichTextBoxScrollBars.Both;
            caixaDeTexto.KeyDown += GuardarCtrlS;

            barraDeMenus = new MenuStrip();
            menuFicheiro = new ToolStripMenuItem("Ficheiro");
            itemAbrir = new ToolStripMenuItem("Abrir");
            itemGuardar = new ToolStripMenuItem("Guardar");
            itemGuardarComo = new ToolStripMenuItem("Guardar Como");
            itemImprimir = new ToolStripMenuItem("Imprimir");

            itemAbrir.Click += AbrirFicheiro;
            itemGuardar.Click += GuardarFicheiro;
            itemGuardarComo.Click += GuardarComoFicheiro;
            itemImprimir.Click += ImprimirFicheiro;
            this.FormClosing += ConfirmarFecho;

            menuFicheiro.DropDownItems.Add(itemAbrir);
            menuFicheiro.DropDownItems.Add(itemGuardar);
            menuFicheiro.DropDownItems.Add(itemGuardarComo);
            menuFicheiro.DropDownItems.Add(new ToolStripSeparator());
            menuFicheiro.DropDownItems.Add(itemImprimir);
            menuFicheiro.DropDownItems.Add(new ToolStripSeparator());

            ToolStripMenuItem itemSair = new ToolStripMenuItem("Sair");
            itemSair.Click += SairPrograma;
            menuFicheiro.DropDownItems.Add(itemSair);

            ToolStripMenuItem itemFecharJanela = new ToolStripMenuItem("Fechar Janela");
            itemFecharJanela.Click += FecharJanela;
            menuFicheiro.DropDownItems.Add(itemFecharJanela);

            ToolStripMenuItem itemNovaJanela = new ToolStripMenuItem("Nova Janela");
            itemNovaJanela.Click += AbrirNovaJanela;
            menuFicheiro.DropDownItems.Add(new ToolStripSeparator());
            menuFicheiro.DropDownItems.Add(itemNovaJanela);

            menuFicheiro.DropDownItems.Add(itemFecharJanela);
            menuFicheiro.DropDownItems.Add(itemSair);

            barraDeMenus.Items.Add(menuFicheiro);

            ToolStripMenuItem menuEditar = new ToolStripMenuItem("Editar");
            ToolStripMenuItem itemTipoDeLetra = new ToolStripMenuItem("Tipo de Letra");
            itemTipoDeLetra.Click += MudarTipoDeLetra;
            menuEditar.DropDownItems.Add(itemTipoDeLetra);

            ToolStripMenuItem itemTamanhoLetra = new ToolStripMenuItem("Tamanho da Letra");
            itemTamanhoLetra.Click += MudarTamanhoLetra;
            menuEditar.DropDownItems.Add(itemTamanhoLetra);

            menuEditar.DropDownItems.Add(new ToolStripSeparator());

            ToolStripMenuItem itemNegrito = new ToolStripMenuItem("Negrito");
            itemNegrito.Click += AlternarNegrito;
            menuEditar.DropDownItems.Add(itemNegrito);

            ToolStripMenuItem itemItalico = new ToolStripMenuItem("Itálico");
            itemItalico.Click += AlternarItalico;
            menuEditar.DropDownItems.Add(itemItalico);

            ToolStripMenuItem itemLocalizar = new ToolStripMenuItem("Localizar e Substituir");
            itemLocalizar.Click += LocalizarTexto;
            menuEditar.DropDownItems.Add(itemLocalizar);

            barraDeMenus.Items.Add(menuEditar);

            this.Controls.Add(caixaDeTexto);
            this.Controls.Add(barraDeMenus);
            this.MainMenuStrip = barraDeMenus;
        }

        private void AbrirFicheiro(object sender, EventArgs e)
        {
            OpenFileDialog dialogoAbrirFicheiro = new OpenFileDialog();
            dialogoAbrirFicheiro.Filter = "Ficheiros RTF (*.rtf)|*.rtf|Ficheiros de Texto (*.txt)|*.txt|Todos os Ficheiros (*.*)|*.*";

            if (dialogoAbrirFicheiro.ShowDialog() == DialogResult.OK)
            {
                if (dialogoAbrirFicheiro.FileName.EndsWith(".rtf", StringComparison.OrdinalIgnoreCase))
                {
                    caixaDeTexto.LoadFile(dialogoAbrirFicheiro.FileName, RichTextBoxStreamType.RichText);
                }
                else
                {
                    caixaDeTexto.Text = File.ReadAllText(dialogoAbrirFicheiro.FileName);
                }
            }
        }

        private void GuardarFicheiro(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath) && File.Exists(currentFilePath))
            {
                SaveToFile(currentFilePath);
                ShowNotification("Ficheiro guardado!");
            }
            else
            {
                GuardarComoFicheiro(sender, e);
            }
        }

        private void GuardarComoFicheiro(object sender, EventArgs e)
        {
            SaveFileDialog dialogoGuardarFicheiro = new SaveFileDialog();
            dialogoGuardarFicheiro.FilterIndex = 1;

            if (ContainsFormatting(caixaDeTexto))
            {
                dialogoGuardarFicheiro.Filter = "Ficheiros RTF (*.rtf)|*.rtf|Ficheiros de Texto (*.txt)|*.txt|Todos os Ficheiros (*.*)|*.*";
            }
            else
            {
                dialogoGuardarFicheiro.Filter = "Ficheiros de Texto (*.txt)|*.txt|Ficheiros RTF (*.rtf)|*.rtf|Todos os Ficheiros (*.*)|*.*";
            }

            if (dialogoGuardarFicheiro.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = dialogoGuardarFicheiro.FileName;
                SaveToFile(currentFilePath);
                ShowNotification("Ficheiro guardado!");
            }
        }

        private void GuardarCtrlS(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                GuardarFicheiro(sender, e);
                e.Handled = true;
            }
        }

        private void SaveToFile(string filePath)
        {
            if (filePath.EndsWith(".rtf", StringComparison.OrdinalIgnoreCase))
                caixaDeTexto.SaveFile(filePath, RichTextBoxStreamType.RichText);
            else
                File.WriteAllText(filePath, caixaDeTexto.Text);
        }

        private bool ContainsFormatting(RichTextBox rtb)
        {
            rtb.SelectAll();
            Font currentFont = rtb.SelectionFont;
            if (currentFont == null) return true;
            bool hasFormatting = currentFont.Bold || currentFont.Italic || currentFont.Underline;
            rtb.DeselectAll();
            return hasFormatting;
        }

        private void ImprimirFicheiro(object sender, EventArgs e)
        {
            PrintDocument documentoImpressao = new PrintDocument();
            documentoImpressao.PrintPage += (s, ev) =>
            {
                ev.Graphics.DrawString(caixaDeTexto.Text, new Font("Arial", 12), Brushes.Black, new RectangleF(0, 0, ev.PageBounds.Width, ev.PageBounds.Height));
            };

            PrintDialog dialogoImpressao = new PrintDialog();
            dialogoImpressao.Document = documentoImpressao;

            if (dialogoImpressao.ShowDialog() == DialogResult.OK)
            {
                documentoImpressao.Print();
            }
        }

        private void ConfirmarFecho(object sender, FormClosingEventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "Deseja guardar as alterações antes de sair?",
                "Confirmar Saída",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                GuardarFicheiro(sender, e);
            }
            else if (resultado == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void ShowNotification(string message)
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Visible = true;
            notifyIcon.Icon = SystemIcons.Information;
            notifyIcon.BalloonTipTitle = "Editor de Texto Simples";
            notifyIcon.BalloonTipText = message;
            notifyIcon.ShowBalloonTip(3000);

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 3000;
            timer.Tick += (s, args) =>
            {
                notifyIcon.Dispose();
                timer.Dispose();
            };
            timer.Start();
        }
        private void SairPrograma(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FecharJanela(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AbrirNovaJanela(object sender, EventArgs e)
        {
            FormularioEditorDeTexto novaJanela = new FormularioEditorDeTexto();
            novaJanela.Show();
        }

        private void MudarTipoDeLetra(object sender, EventArgs e)
        {
            FontDialog dialogoFonte = new FontDialog();
            dialogoFonte.Font = caixaDeTexto.SelectionFont;

            if (dialogoFonte.ShowDialog() == DialogResult.OK)
            {
                caixaDeTexto.SelectionFont = dialogoFonte.Font;
            }
        }

        private void MudarTamanhoLetra(object sender, EventArgs e)
        {
            try
            {
                string tamanho = Microsoft.VisualBasic.Interaction.InputBox("Insira o tamanho da letra:", "Tamanho da Letra", caixaDeTexto.Font.Size.ToString());
                if (!string.IsNullOrEmpty(tamanho))
                {
                    float novoTamanho = float.Parse(tamanho);
                    caixaDeTexto.Font = new Font(caixaDeTexto.Font.FontFamily, novoTamanho, caixaDeTexto.Font.Style);
                }
            }
            catch
            {
                MessageBox.Show("Tamanho inválido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AlternarNegrito(object sender, EventArgs e)
        {
            FontStyle estiloAtual = caixaDeTexto.SelectionFont?.Style ?? FontStyle.Regular;
            FontStyle novoEstilo = estiloAtual ^ FontStyle.Bold;
            caixaDeTexto.SelectionFont = new Font(caixaDeTexto.SelectionFont ?? caixaDeTexto.Font, novoEstilo);

            ToolStripMenuItem itemNegrito = sender as ToolStripMenuItem;
            itemNegrito.Checked = !itemNegrito.Checked;
        }

        private void AlternarItalico(object sender, EventArgs e)
        {
            FontStyle estiloAtual = caixaDeTexto.SelectionFont?.Style ?? FontStyle.Regular;
            FontStyle novoEstilo = estiloAtual ^ FontStyle.Italic;
            caixaDeTexto.SelectionFont = new Font(caixaDeTexto.SelectionFont ?? caixaDeTexto.Font, novoEstilo);

            ToolStripMenuItem itemItalico = sender as ToolStripMenuItem;
            itemItalico.Checked = !itemItalico.Checked;
        }

        // Новый метод поиска только целых слов!
        private int FindWholeWord(string text, string word, int startIndex)
        {
            if (string.IsNullOrEmpty(word))
                return -1;

            var regex = new Regex(@"\b" + Regex.Escape(word) + @"\b", RegexOptions.IgnoreCase);
            Match match = regex.Match(text, startIndex);
            return match.Success ? match.Index : -1;
        }

        private void HighlightCurrent(int posicao, int length)
        {
            // Сбросить старую подсветку
            int selectionStart = caixaDeTexto.SelectionStart;
            int selectionLength = caixaDeTexto.SelectionLength;
            caixaDeTexto.SelectAll();
            caixaDeTexto.SelectionBackColor = caixaDeTexto.BackColor;
            caixaDeTexto.DeselectAll();

            // Подсветить только найденное слово
            if (posicao >= 0 && length > 0)
            {
                caixaDeTexto.Select(posicao, length);
                caixaDeTexto.SelectionBackColor = Color.LightSkyBlue;
            }

            // Вернуть пользователю его выделение (если нужно)
            caixaDeTexto.SelectionStart = selectionStart;
            caixaDeTexto.SelectionLength = selectionLength;
        }

        private void LocalizarTexto(object sender, EventArgs e)
        {
            DialogoLocalizar dialogo = new DialogoLocalizar();
            dialogo.TextoProcuradoAtualizado += (texto) =>
            {
                if (!string.IsNullOrEmpty(texto))
                {
                    if (texto != textoProcurado)
                    {
                        textoProcurado = texto;
                        ultimaPosicaoPesquisa = 0;
                    }

                    int posicao = FindWholeWord(caixaDeTexto.Text, textoProcurado, ultimaPosicaoPesquisa);

                    if (posicao >= 0)
                    {
                        HighlightCurrent(posicao, textoProcurado.Length); // Подсветить только найденное слово
                        ultimaPosicaoPesquisa = posicao + textoProcurado.Length;
                    }
                    else
                    {
                        MessageBox.Show("Fim da pesquisa. Reiniciando do início.", "Localizar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ultimaPosicaoPesquisa = 0;
                        HighlightCurrent(-1, 0); // Снять подсветку при отсутствии совпадений
                    }
                }
                else
                {
                    HighlightCurrent(-1, 0); // Сбросить подсветку если строка поиска пуста
                }
            };

            dialogo.SubstituirTextoRequisitado += (textoSubstituir) =>
            {
                if (!string.IsNullOrEmpty(textoProcurado) && !string.IsNullOrEmpty(textoSubstituir))
                {
                    if (caixaDeTexto.SelectedText.Equals(textoProcurado, StringComparison.CurrentCultureIgnoreCase))
                    {
                        caixaDeTexto.SelectedText = textoSubstituir;
                        // После замены заново подсветить новое совпадение
                        int posicao = FindWholeWord(caixaDeTexto.Text, textoProcurado, ultimaPosicaoPesquisa - textoProcurado.Length);
                        HighlightCurrent(posicao, textoProcurado.Length);
                    }
                    else
                    {
                        MessageBox.Show("Nenhuma palavra selecionada para substituir.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Nenhuma palavra foi localizada para substituir.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            dialogo.Show(this);
        }

        private int ContarOcorrencias(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return 0;
            }

            int contagem = 0;
            int posicao = 0;

            while (posicao < caixaDeTexto.TextLength)
            {
                int novaPosicao = FindWholeWord(caixaDeTexto.Text, texto, posicao);

                if (novaPosicao >= 0)
                {
                    contagem++;
                    posicao = novaPosicao + texto.Length;
                }
                else
                {
                    break;
                }
            }

            return contagem;
        }

        private void SubstituirTexto(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textoProcurado))
            {
                MessageBox.Show("Nenhuma palavra foi localizada para substituir.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string textoSubstituir = Microsoft.VisualBasic.Interaction.InputBox(
                "Insira o texto para substituir:",
                "Substituir Texto",
                ""
            );

            if (!string.IsNullOrEmpty(textoSubstituir))
            {
                if (caixaDeTexto.SelectedText.Equals(textoProcurado, StringComparison.CurrentCultureIgnoreCase))
                {
                    caixaDeTexto.SelectedText = textoSubstituir;
                }
                else
                {
                    MessageBox.Show("Nenhuma palavra selecionada para substituir.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}