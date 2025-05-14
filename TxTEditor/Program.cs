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
        private RichTextBox caixaDeTexto;
        private MenuStrip barraDeMenus;
        private ToolStripMenuItem menuFicheiro;
        private ToolStripMenuItem itemAbrir;
        private ToolStripMenuItem itemGuardar;
        private ToolStripMenuItem itemGuardarComo;
        private ToolStripMenuItem itemImprimir;

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
            dialogoAbrirFicheiro.Filter = "Ficheiros de Texto (*.txt)|*.txt|Todos os Ficheiros (*.*)|*.*";

            if (dialogoAbrirFicheiro.ShowDialog() == DialogResult.OK)
            {
                caixaDeTexto.Text = File.ReadAllText(dialogoAbrirFicheiro.FileName);
            }
        }

        private void GuardarFicheiro(object sender, EventArgs e)
        {
            SaveFileDialog dialogoGuardarFicheiro = new SaveFileDialog();
            dialogoGuardarFicheiro.Filter = "Ficheiros de Texto (*.txt)|*.txt|Todos os Ficheiros (*.*)|*.*";

            if (dialogoGuardarFicheiro.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialogoGuardarFicheiro.FileName, caixaDeTexto.Text);
            }
        }

        private void GuardarComoFicheiro(object sender, EventArgs e)
        {
            SaveFileDialog dialogoGuardarComoFicheiro = new SaveFileDialog();
            dialogoGuardarComoFicheiro.Filter = "Ficheiros de Texto (*.txt)|*.txt|Todos os Ficheiros (*.*)|*.*";

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
    }
}