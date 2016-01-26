using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathEditor {
    public partial class Form1 : Form {
        private const string TEXTO_CONFIRMACION_ELIMINAR = "¿Esta seguro que desea eliminar el elemento?";
        private const string TEXTO_CONFIRMACION_EDITAR = "¿Esta seguro que desea modificar el elemento?";
        private const string TEXTO_CONFIRMACION_GUARDAR = "¿Esta seguro que desea guardar los cambios en el sistema?";
        private const string TEXTO_ERROR_LISTA = "Debe seleccionar un elemento de la lista.";
        private const string TEXTO_ERROR_TEXTO = "Debe escribir la ruta en el cuadro de texto.";
        private const string TEXTO_LISTA_VACIA = "Lista vacia.";
        private const string CAPTION_ADVERTENCIA = "Advertencia";
 
        public Form1() {
            InitializeComponent();
            llenarLista();
        }

        public void backup() {
            string cadena = convertirCadena(getValues());
            string fecha = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Year.ToString();
            string hora = DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
            string path = "BACKUP-"+ fecha + "-"+ hora +".txt";
            System.IO.File.WriteAllText(@path, cadena);
        }

        public string convertirCadena(string[] valores) {
            string completo = "";
            foreach (string s in valores) {
                if (valores.Last() != s) {
                    completo += s + ";";
                } else {
                    completo += s;
                }
            }
            return completo;
        }

        public string[] getValues() {
            return Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine).Split(';');
        }

        public void setValues(string[] valores) {
            string valor = convertirCadena(valores);
            Environment.SetEnvironmentVariable("Path", valor, EnvironmentVariableTarget.Machine);
        }

        public string[] listaArray() {
            string[] valores = new string[lista.Items.Count];
            for (int i = 0; i < valores.Length; i++) {
                valores[i] = lista.Items[i].ToString();
            }
            return valores;
        }

        public void llenarLista() {
            lista.Items.Clear();
            lista.Items.AddRange(getValues());
        }

        private void lista_SelectedIndexChanged(object sender, EventArgs e) {
            if (lista.SelectedItem != null) {
                txtValor.Text = lista.SelectedItem.ToString();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e) {
            if (!txtValor.Text.Equals(String.Empty)) {
                if (lista.SelectedItem != null) {
                    DialogResult resultado = MessageBox.Show(TEXTO_CONFIRMACION_EDITAR, CAPTION_ADVERTENCIA, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes) {
                        int indexActual = lista.SelectedIndex;
                        lista.Items.RemoveAt(lista.SelectedIndex);
                        lista.Items.Insert(indexActual, txtValor.Text);
                        txtValor.Text = String.Empty;
                    }
                } else {
                    MessageBox.Show(TEXTO_ERROR_LISTA, CAPTION_ADVERTENCIA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            } else {
                MessageBox.Show(TEXTO_ERROR_TEXTO, CAPTION_ADVERTENCIA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        private void btnAgregar_Click(object sender, EventArgs e) {
            if (!txtValor.Text.Equals(String.Empty)) {
                lista.Items.Add(txtValor.Text);
                txtValor.Text = String.Empty;
            } else {
                MessageBox.Show(TEXTO_ERROR_TEXTO, CAPTION_ADVERTENCIA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e) {
            if (lista.SelectedItem != null) {
                DialogResult resultado = MessageBox.Show(TEXTO_CONFIRMACION_ELIMINAR, CAPTION_ADVERTENCIA, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes) {
                    lista.Items.Remove(lista.SelectedItem);
                    txtValor.Text = String.Empty;
                }
            }else {
                MessageBox.Show(TEXTO_ERROR_LISTA, CAPTION_ADVERTENCIA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e) {
            if (lista.Items.Count > 0) {
                DialogResult resultado = MessageBox.Show(TEXTO_CONFIRMACION_GUARDAR, CAPTION_ADVERTENCIA, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes) {
                    backup();
                    setValues(listaArray());
                    llenarLista();
                }
            } else {
                MessageBox.Show(TEXTO_LISTA_VACIA, CAPTION_ADVERTENCIA, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
