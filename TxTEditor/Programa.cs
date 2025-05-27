using System;
using System.Windows.Forms;

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
}