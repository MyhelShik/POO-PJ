using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;

namespace EditorDeTextoSimples
{
    class Programa
    {
        [STAThread]  // Necessário para Windows Forms
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormularioEditorDeTexto());
        }
    }

    public class FormularioEditorDeTexto : Form
    {
        private RichTextBox caixaDeTexto { get; set; }
        private MenuStrip barraDeMenus;
        private ToolStripMenuItem menuFicheiro;
        private ToolStripMenuItem itemAbrir;
        private ToolStripMenuItem itemGuardar;
        private ToolStripMenuItem itemGuardarComo;
        private ToolStripMenuItem itemImprimir;

        private int ultimaPosicaoPesquisa = 0; // Posição da última pesquisa
        private string textoProcurado = ""; // Texto que está sendo procurado

        public FormularioEditorDeTexto()
        {
            // Configurar o formulário
            this.Text = "Editor de Texto Simples";
            this.Width = 800;
            this.Height = 600;

            // Criar uma caixa de texto
            caixaDeTexto = new RichTextBox();
            caixaDeTexto.Multiline = true;
            caixaDeTexto.Dock = DockStyle.Fill;
            caixaDeTexto.ScrollBars = RichTextBoxScrollBars.Both;

            // Criar uma barra de menus
            barraDeMenus = new MenuStrip();
            menuFicheiro = new ToolStripMenuItem("Ficheiro");
            itemAbrir = new ToolStripMenuItem("Abrir");
            itemGuardar = new ToolStripMenuItem("Guardar");
            itemGuardarComo = new ToolStripMenuItem("Guardar Como");
            itemImprimir = new ToolStripMenuItem("Imprimir");

            // Adicionar eventos
            itemAbrir.Click += AbrirFicheiro;
            itemGuardar.Click += GuardarFicheiro;
            itemGuardarComo.Click += GuardarComoFicheiro;
            itemImprimir.Click += ImprimirFicheiro;
            this.FormClosing += ConfirmarFecho;

            // Montar o menu
            menuFicheiro.DropDownItems.Add(itemAbrir);
            menuFicheiro.DropDownItems.Add(itemGuardar);
            menuFicheiro.DropDownItems.Add(itemGuardarComo);

            // Adicionar um separador
            menuFicheiro.DropDownItems.Add(new ToolStripSeparator());

            menuFicheiro.DropDownItems.Add(itemImprimir);

            // Adicionar outro separador
            menuFicheiro.DropDownItems.Add(new ToolStripSeparator());

            ToolStripMenuItem itemSair = new ToolStripMenuItem("Sair");
            itemSair.Click += SairPrograma;
            menuFicheiro.DropDownItems.Add(itemSair);

            ToolStripMenuItem itemFecharJanela = new ToolStripMenuItem("Fechar Janela");
            itemFecharJanela.Click += FecharJanela;
            menuFicheiro.DropDownItems.Add(itemFecharJanela);

            ToolStripMenuItem itemNovaJanela = new ToolStripMenuItem("Nova Janela");
            itemNovaJanela.Click += AbrirNovaJanela;
            menuFicheiro.DropDownItems.Add(new ToolStripSeparator()); // Adicionar um separador
            menuFicheiro.DropDownItems.Add(itemNovaJanela);

            menuFicheiro.DropDownItems.Add(itemFecharJanela);
            menuFicheiro.DropDownItems.Add(itemSair);

            barraDeMenus.Items.Add(menuFicheiro);

            // Adicionar o menu "Editar"
            ToolStripMenuItem menuEditar = new ToolStripMenuItem("Editar");

            // Opção para mudar o tipo de letra
            ToolStripMenuItem itemTipoDeLetra = new ToolStripMenuItem("Tipo de Letra");
            itemTipoDeLetra.Click += MudarTipoDeLetra;
            menuEditar.DropDownItems.Add(itemTipoDeLetra);

            // Opção para mudar o tamanho da letra
            ToolStripMenuItem itemTamanhoLetra = new ToolStripMenuItem("Tamanho da Letra");
            itemTamanhoLetra.Click += MudarTamanhoLetra;
            menuEditar.DropDownItems.Add(itemTamanhoLetra);

            // Separador
            menuEditar.DropDownItems.Add(new ToolStripSeparator());

            // Opção para negrito
            ToolStripMenuItem itemNegrito = new ToolStripMenuItem("Negrito");
            itemNegrito.Click += AlternarNegrito;
            menuEditar.DropDownItems.Add(itemNegrito);

            // Opção para itálico
            ToolStripMenuItem itemItalico = new ToolStripMenuItem("Itálico");
            itemItalico.Click += AlternarItalico;
            menuEditar.DropDownItems.Add(itemItalico);

            // Adicionar a opção "Localizar" no menu "Editar"
            ToolStripMenuItem itemLocalizar = new ToolStripMenuItem("Localizar");
            itemLocalizar.Click += LocalizarTexto;
            menuEditar.DropDownItems.Add(itemLocalizar);

            // Adicionar a opção "Substituir" no menu "Editar"
            ToolStripMenuItem itemSubstituir = new ToolStripMenuItem("Substituir");
            itemSubstituir.Click += SubstituirTexto;
            menuEditar.DropDownItems.Add(itemSubstituir);

            // Adicionar o menu "Editar" à barra de menus
            barraDeMenus.Items.Add(menuEditar);

            // Adicionar controlos ao formulário
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
/*
        private void GuardarCtrlS(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                Console.WriteLine("asd");
        }
*/

        private void GuardarFicheiro(object sender, EventArgs e)
        {
            SaveFileDialog dialogoGuardarFicheiro = new SaveFileDialog();
            dialogoGuardarFicheiro.Filter = "Ficheiros RTF (*.rtf)|*.rtf|Ficheiros de Texto (*.txt)|*.txt|Todos os Ficheiros (*.*)|*.*";

            if (dialogoGuardarFicheiro.ShowDialog() == DialogResult.OK)
            {
                if (dialogoGuardarFicheiro.FilterIndex == 1) // RTF format selected
                {
                    caixaDeTexto.SaveFile(dialogoGuardarFicheiro.FileName, RichTextBoxStreamType.RichText);
                }
                else // Plain text format
                {
                    File.WriteAllText(dialogoGuardarFicheiro.FileName, caixaDeTexto.Text);
                }
            }
        }

        private void GuardarComoFicheiro(object sender, EventArgs e)
        {
            SaveFileDialog dialogoGuardarComoFicheiro = new SaveFileDialog();
            dialogoGuardarComoFicheiro.Filter = "Ficheiros RTF (*.rtf)|*.rtf|Ficheiros de Texto (*.txt)|*.txt|Todos os Ficheiros (*.*)|*.*";

            if (dialogoGuardarComoFicheiro.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialogoGuardarComoFicheiro.FileName, caixaDeTexto.Text);
            }
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
                GuardarFicheiro(sender, e); // Chama o método para guardar o ficheiro
            }
            else if (resultado == DialogResult.Cancel)
            {
                e.Cancel = true; // Cancela o fecho do programa
            }
        }

        private void SairPrograma(object sender, EventArgs e)
        {
            Application.Exit(); // Fecha todo o programa
        }

        private void FecharJanela(object sender, EventArgs e)
        {
            this.Close(); // Fecha apenas a janela atual
        }

        private void AbrirNovaJanela(object sender, EventArgs e)
        {
            FormularioEditorDeTexto novaJanela = new FormularioEditorDeTexto();
            novaJanela.Show(); // Exibe a nova janela
        }

        private void MudarTipoDeLetra(object sender, EventArgs e)
        {
            FontDialog dialogoFonte = new FontDialog();
            dialogoFonte.Font = caixaDeTexto.SelectionFont; // Define a fonte inicial como a do texto selecionado

            if (dialogoFonte.ShowDialog() == DialogResult.OK)
            {
                // Aplica a nova fonte apenas ao texto selecionado
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
            FontStyle novoEstilo = estiloAtual ^ FontStyle.Bold; // Alterna o estilo negrito
            caixaDeTexto.SelectionFont = new Font(caixaDeTexto.SelectionFont ?? caixaDeTexto.Font, novoEstilo);

            // Atualizar o estado do checkmark
            ToolStripMenuItem itemNegrito = sender as ToolStripMenuItem;
            itemNegrito.Checked = !itemNegrito.Checked;
        }

        private void AlternarItalico(object sender, EventArgs e)
        {
            FontStyle estiloAtual = caixaDeTexto.SelectionFont?.Style ?? FontStyle.Regular;
            FontStyle novoEstilo = estiloAtual ^ FontStyle.Italic; // Alterna o estilo itálico
            caixaDeTexto.SelectionFont = new Font(caixaDeTexto.SelectionFont ?? caixaDeTexto.Font, novoEstilo);

            // Atualizar o estado do checkmark
            ToolStripMenuItem itemItalico = sender as ToolStripMenuItem;
            itemItalico.Checked = !itemItalico.Checked;
        }

        private void LocalizarTexto(object sender, EventArgs e)
        {
            DialogoLocalizar dialogo = new DialogoLocalizar();
            dialogo.TextoProcuradoAtualizado += (texto) =>
            {
                if (!string.IsNullOrEmpty(texto))
                {
                    // Atualizar o texto procurado e reiniciar a posição de pesquisa se o texto mudou
                    if (texto != textoProcurado)
                    {
                        textoProcurado = texto;
                        ultimaPosicaoPesquisa = 0;
                    }

                    // Procurar o texto no RichTextBox
                    int posicao = caixaDeTexto.Find(textoProcurado, ultimaPosicaoPesquisa, RichTextBoxFinds.None);

                    if (posicao >= 0)
                    {
                        // Seleciona o texto encontrado
                        caixaDeTexto.Select(posicao, textoProcurado.Length);
                        caixaDeTexto.Focus();

                        // Atualizar a posição para a próxima pesquisa
                        ultimaPosicaoPesquisa = posicao + textoProcurado.Length;
                    }
                    else
                    {
                        // Reiniciar a pesquisa se chegar ao final
                        MessageBox.Show("Fim da pesquisa. Reiniciando do início.", "Localizar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ultimaPosicaoPesquisa = 0;
                    }
                }
            };

            dialogo.Show(this); // Exibe o diálogo sem bloquear o formulário principal
        }

        private int ContarOcorrencias(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return 0; // Retorna 0 se o texto a ser procurado for vazio
            }

            int contagem = 0;
            int posicao = 0;

            while (posicao < caixaDeTexto.TextLength)
            {
                // Procurar o texto a partir da posição atual
                int novaPosicao = caixaDeTexto.Find(texto, posicao, RichTextBoxFinds.None);

                if (novaPosicao >= 0)
                {
                    contagem++;
                    posicao = novaPosicao + texto.Length; // Avançar a posição para evitar contagem repetida
                }
                else
                {
                    break; // Sai do loop se não houver mais ocorrências
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
                if (caixaDeTexto.SelectedText == textoProcurado)
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

    public class DialogoLocalizar : Form
    {
        public event Action<string> TextoProcuradoAtualizado; // Evento para enviar o texto ao formulário principal
        private TextBox caixaTexto;
        public DialogoLocalizar()
        {
            this.Text = "Localizar";
            this.Width = 300;
            this.Height = 150;
            this.TopMost = true; // Mantém a janela sempre no topo

            Label label = new Label();
            label.Text = "Insira o texto a procurar:";
            label.Dock = DockStyle.Top;
            label.Padding = new Padding(10);

            caixaTexto = new TextBox();
            caixaTexto.Dock = DockStyle.Top;
            caixaTexto.Padding = new Padding(10);



            Button botaoFechar = new Button();
            botaoFechar.Text = "Fechar";
            botaoFechar.Dock = DockStyle.Bottom;
            botaoFechar.Click += (s, e) => this.Close();

            this.Controls.Add(label);
            this.Controls.Add(caixaTexto);
            this.Controls.Add(botaoFechar);



            //COnfigurar o evento de ENTER
            caixaTexto.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    TextoProcuradoAtualizado?.Invoke(caixaTexto.Text); // Dispara o evento com o texto atual
                    e.SuppressKeyPress = true; // Impede o som de "ding" ao pressionar Enter
                    return;
                }
            };
            // Configurar o foco no TextBox ao abrir o diálogo
            this.Load += (s, e) => caixaTexto.Focus();
        }
    }
}