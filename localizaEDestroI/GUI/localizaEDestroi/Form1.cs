using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace localizaEDestroi
{
    public partial class Form1 : Form
    {
        FolderBrowserDialog dir = new FolderBrowserDialog(); //Cria uma janela para usuario informar o diretorio raiz

        private string[] allfiles;  //Lista de diretorios pela pesquisa por extensão
        private string[] allfiles2; //Lista de diretorios pela pesquisa por nome
        private string dirRaiz;            //Diretorio raiz
        private string usuarioDamaquina;

        private List<string> listaPorNome = new List<string>();     //List contendo o resultado da pesquisa por nome
        private List<string> listaPorExtensao = new List<string>(); //List contendo o resultado da pesquisa por extensão
        private List<string> logtxt = new List<string>();
        private List<string> tempAllfiles2 = new List<string>();

        private BackgroundWorker myWorker_pesquisa_Ext = new BackgroundWorker();
        private BackgroundWorker myWorker_pesquisa_Dir = new BackgroundWorker();
        private BackgroundWorker myWorker_deletar_nome = new BackgroundWorker();
        private BackgroundWorker myWorker_deletar_extensao = new BackgroundWorker();

        DirectoryInfo tamTotArqDir;

        private bool flag_PesquisaRealizada = false;

        private long totDir;
        private long totArquivosPorExt;

        private static long tamArqExt;
        private static long tamDir;
        private static int tamLog;

        public Form1()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false; //Evita que fique aparecendo mensagem por chamadas incorretas no thread 
            
            myWorker_pesquisa_Dir.DoWork += new DoWorkEventHandler(myWorker_DoWork_pesquisa_nome);
            myWorker_pesquisa_Dir.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myWorker_RunWorkerCompleted_pesquisa_nome);
            myWorker_pesquisa_Dir.ProgressChanged += new ProgressChangedEventHandler(myWorker_ProgressChanged_pesquisa_extensao);
            myWorker_pesquisa_Dir.WorkerReportsProgress = true;
            myWorker_pesquisa_Dir.WorkerSupportsCancellation = true;

            myWorker_pesquisa_Ext.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myWorker_RunWorkerCompleted_pesquisa_extensao);
            myWorker_pesquisa_Ext.DoWork += new DoWorkEventHandler(myWorker_DoWork_pesquisa_extensao);
            myWorker_pesquisa_Ext.ProgressChanged += new ProgressChangedEventHandler(myWorker_ProgressChanged_pesquisa_extensao);
            myWorker_pesquisa_Ext.WorkerReportsProgress = true;
            myWorker_pesquisa_Ext.WorkerSupportsCancellation = true;


            myWorker_deletar_nome.DoWork += new DoWorkEventHandler(myWorker_DoWork_deletar_nome);
            myWorker_deletar_nome.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myWorker_RunWorkerCompleted_deletar_nome);
            myWorker_deletar_nome.ProgressChanged += new ProgressChangedEventHandler(myWorker_ProgressChanged_deletar_nome);
            myWorker_deletar_nome.WorkerReportsProgress = true;
            myWorker_deletar_nome.WorkerSupportsCancellation = true;

            myWorker_deletar_extensao.DoWork += new DoWorkEventHandler(myWorker_DoWork_deletar_extensao);            
            myWorker_deletar_extensao.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myWorker_RunWorkerCompleted_deletar_extensao);            
            myWorker_deletar_extensao.ProgressChanged += new ProgressChangedEventHandler(myWorker_ProgressChanged_deletar_extensao);            
            myWorker_deletar_extensao.WorkerReportsProgress = true;            
            myWorker_deletar_extensao.WorkerSupportsCancellation = true;
            usuarioDamaquina = Environment.UserName;
        }

        private void gravaDgv()
        {
            if (logtxt == null)
            {
                return;
            }

            foreach (var item in logtxt)
            {
                dgv.Rows.Add(item);
            }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            ChooseFolder(); //Chama função para escolha do diretorio
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            limpaVariaveis();
            limpaCampos();                                   
            verificaEstadobusy_btnpesquisar();            
        }

        private void verificaEstadobusy_btnpesquisar()
        {
            object[] arrObjects = new object[] { txtNomePasta };//Declare the array of objects
            object[] arrObjects2 = new object[] { txtExtArq };//Declare the array of objects

            if (!myWorker_pesquisa_Dir.IsBusy || !myWorker_pesquisa_Ext.IsBusy)//Check if the worker is already in progress
            {

                if(txtNomePasta.Text != "" || txtExtArq.Text != "")
                {
                    lblStatus.Text = "Pesquisando...";
                    desabilitaBotoes();
                    flag_PesquisaRealizada = true;

                    if (txtNomePasta.Text != "")
                    {
                        myWorker_pesquisa_Dir.RunWorkerAsync(arrObjects);//Call the background worker
                    }

                    if (txtExtArq.Text != "")
                    {
                        myWorker_pesquisa_Ext.RunWorkerAsync(arrObjects2);//Call the background worker
                    }
                }
                
            }
        }        

        protected void myWorker_DoWork_pesquisa_nome(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker sendingWorker =
            (BackgroundWorker)sender;//Capture the BackgroundWorker that fired the event
            object[] arrObjects =
            (object[])e.Argument;//Collect the array of objects the we received from the main thread

            //int maxValue = (int)arrObjects[0];//Get the numeric value 
            //from inside the objects array, don't forget to cast
            //StringBuilder sb = new StringBuilder();//Declare a new string builder to store the result.

            //for (int i = 1; i <= maxValue; i++)//Start a for loop
            //{
            if (!sendingWorker.CancellationPending)//At each iteration of the loop, 
                                                   //check if there is a cancellation request pending 
            {
                pesquisaPorNomeDir();
                                
                //flg_gravaDgv = true;
                //sb.Append(string.Format("Counting number: {0}{1}",
                //pesquisaPorExtensao, Environment.NewLine));//Append the result to the string builder
                //sendingWorker.ReportProgress(i);//Report our progress to the main thread
            }

            else
            {
                e.Cancel = true;//If a cancellation request is pending, assign this flag a value of true
                                //break;// If a cancellation request is pending, break to exit the loop
            }
            //}

            //e.Result = sb.ToString();// Send our result to the main thread!
        }

        protected void myWorker_DoWork_pesquisa_extensao(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker sendingWorker =
            (BackgroundWorker)sender;//Capture the BackgroundWorker that fired the event
            object[] arrObjects =
            (object[])e.Argument;//Collect the array of objects the we received from the main thread

            //int maxValue = (int)arrObjects[0];//Get the numeric value 
            //from inside the objects array, don't forget to cast
            //StringBuilder sb = new StringBuilder();//Declare a new string builder to store the result.

            //for (int i = 1; i <= maxValue; i++)//Start a for loop
            //{
            if (!sendingWorker.CancellationPending)//At each iteration of the loop, 
                                                   //check if there is a cancellation request pending 
            {
                pesquisaPorExtensao();
                //flg_gravaDgv = true;
                //sb.Append(string.Format("Counting number: {0}{1}",
                //pesquisaPorExtensao, Environment.NewLine));//Append the result to the string builder
                //sendingWorker.ReportProgress(i);//Report our progress to the main thread
            }
            else
            {
                e.Cancel = true;//If a cancellation request is pending, assign this flag a value of true
                                //break;// If a cancellation request is pending, break to exit the loop
            }
            //}

            //e.Result = sb.ToString();// Send our result to the main thread!
        }

        protected void myWorker_ProgressChanged_pesquisa_nome(object sender, ProgressChangedEventArgs e)
        {
            //Show the progress to the user based on the input we got from the background worker
            lblStatus.Text = string.Format("Counting number: {0}...", e.ProgressPercentage);
        }

        protected void myWorker_ProgressChanged_pesquisa_extensao(object sender, ProgressChangedEventArgs e)
        {
            //Show the progress to the user based on the input we got from the background worker
            lblStatus.Text = string.Format("Counting number: {0}...", e.ProgressPercentage);
        }

        protected void myWorker_RunWorkerCompleted_pesquisa_nome(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)//Check if the worker has been canceled or if an error occurred
            {
                string result = (string)e.Result;//Get the result from the background thread

                if (!myWorker_pesquisa_Ext.IsBusy)
                {
                    lblStatus.Text = "Pesquisa completa";
                    gravaDgv();
                    habilitarBotoes();
                }
            }

            else if (e.Cancelled)
            {
                lblStatus.Text = "User Canceled";
            }

            else
            {
                lblStatus.Text = "An error has occurred";
            }
            
        }

        private void habilitarBotoes()
        {
            btnPesquisar.Enabled = true;//Re enable the start button
            btnBrowse.Enabled = true;//Disable the Start button
            btnDeletar.Enabled = true;//Disable the Start button
        }

        protected void myWorker_RunWorkerCompleted_pesquisa_extensao(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)//Check if the worker has been canceled or if an error occurred
            {
                string result = (string)e.Result;//Get the result from the background thread
                
                if (!myWorker_pesquisa_Dir.IsBusy)
                {
                    lblStatus.Text = "Pesquisa completa";
                    gravaDgv();
                    habilitarBotoes();
                }
            }

            else if (e.Cancelled)
            {
                lblStatus.Text = "User Canceled";
            }

            else
            {
                lblStatus.Text = "An error has occurred";
            }
            
        }

        protected void myWorker_DoWork_deletar_nome(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker sendingWorker =
            (BackgroundWorker)sender;//Capture the BackgroundWorker that fired the event
            object[] arrObjects =
            (object[])e.Argument;//Collect the array of objects the we received from the main thread

            //int maxValue = (int)arrObjects[0];//Get the numeric value 
            //from inside the objects array, don't forget to cast
            //StringBuilder sb = new StringBuilder();//Declare a new string builder to store the result.

            //for (int i = 1; i <= maxValue; i++)//Start a for loop
            //{

            if (!sendingWorker.CancellationPending)//At each iteration of the loop, 
                                                   //check if there is a cancellation request pending 
            {
                deletarPorDiretorio();
                //flg_gravaDgv = true;
                //sb.Append(string.Format("Counting number: {0}{1}",
                //pesquisaPorExtensao, Environment.NewLine));//Append the result to the string builder
                //sendingWorker.ReportProgress(i);//Report our progress to the main thread
            }
            else
            {
                e.Cancel = true;//If a cancellation request is pending, assign this flag a value of true
                                //break;// If a cancellation request is pending, break to exit the loop
            }
            //}

            //e.Result = sb.ToString();// Send our result to the main thread!
        }

        protected void myWorker_DoWork_deletar_extensao(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker sendingWorker =
            (BackgroundWorker)sender;//Capture the BackgroundWorker that fired the event
            object[] arrObjects =
            (object[])e.Argument;//Collect the array of objects the we received from the main thread

            //int maxValue = (int)arrObjects[0];//Get the numeric value 
            //from inside the objects array, don't forget to cast
            //StringBuilder sb = new StringBuilder();//Declare a new string builder to store the result.

            //for (int i = 1; i <= maxValue; i++)//Start a for loop
            //{

            if (!sendingWorker.CancellationPending)//At each iteration of the loop, 
                                                   //check if there is a cancellation request pending 
            {
                deletarPorExtensao();
                //flg_gravaDgv = true;
                //sb.Append(string.Format("Counting number: {0}{1}",
                //pesquisaPorExtensao, Environment.NewLine));//Append the result to the string builder
                //sendingWorker.ReportProgress(i);//Report our progress to the main thread
            }
            else
            {
                e.Cancel = true;//If a cancellation request is pending, assign this flag a value of true
                                //break;// If a cancellation request is pending, break to exit the loop
            }
            //}

            //e.Result = sb.ToString();// Send our result to the main thread!
        }

        protected void myWorker_ProgressChanged_deletar_nome(object sender, ProgressChangedEventArgs e)
        {
            //Show the progress to the user based on the input we got from the background worker
            lblStatus.Text = string.Format("Counting number: {0}...", e.ProgressPercentage);
        }

        protected void myWorker_ProgressChanged_deletar_extensao(object sender, ProgressChangedEventArgs e)
        {
            //Show the progress to the user based on the input we got from the background worker
            lblStatus.Text = string.Format("Counting number: {0}...", e.ProgressPercentage);
        }

        protected void myWorker_RunWorkerCompleted_deletar_nome(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)//Check if the worker has been canceled or if an error occurred
            {
                string result = (string)e.Result;//Get the result from the background thread

                if (!myWorker_deletar_extensao.IsBusy)
                {
                    lblStatus.Text = "Deleção completa. Gerando log...";
                    geraLog();
                    lblStatus.Text = "Log concluido";
                    habilitarBotoes();
                }
            }

            else if (e.Cancelled)
            {
                lblStatus.Text = "User Canceled";
            }

            else
            {
                lblStatus.Text = "An error has occurred";
            }
            
        }

        protected void myWorker_RunWorkerCompleted_deletar_extensao(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)//Check if the worker has been canceled or if an error occurred
            {
                string result = (string)e.Result;//Get the result from the background thread             

                if (!myWorker_deletar_nome.IsBusy)
                {
                    lblStatus.Text = "Deleção completa. Gerando log...";
                    geraLog();
                    lblStatus.Text = "Log concluido";
                    habilitarBotoes();
                }
            }

            else if (e.Cancelled)
            {
                lblStatus.Text = "User Canceled";
            }

            else
            {
                lblStatus.Text = "An error has occurred";
            }
            
        }       

        private void pesquisaPorExtensao()
        {
            string txtExt = txtExtArq.Text;   //Recebe o conjunto de extensões do textbox delimitados por (.)            
            string[] ext = txtExt.Split('.'); //Quebra a string nas delimitações (.) e armazena em um vetor
            
            //var listaClonada = new List<Vao> { new Vao { Quantidade = 2, Medida = 2 }, new Vao { Quantidade = 0, Medida = 0 }, new Vao { Quantidade = 1, Medida = 1 } };
            long tamTotArqExt;

            if (txtExtArq.Text == "")
            {
                return;
            }

            
            // ------------- Pesquisa por extensão ------------------ //
            foreach (var item in ext)
            {
                //allfiles recebe o caminho absoluto de cada arquivo encontrado a cada iteração do foreach                    
                allfiles = Directory.GetFiles(dirRaiz, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith("." + item.ToString(), StringComparison.CurrentCulture)).ToArray();

                foreach (var file in allfiles)
                {
                    string[] teste = Convert.ToString(file).Split('\\');
                    bool verificador = false;

                    foreach (var ch in teste)
                    {
                        if (ch == "target" || ch == ".jazz5" || ch == ".git" || ch == ".metadata")
                        {
                            verificador = true;
                        }
                    }

                    if (!verificador)
                    {                        
                        listaPorExtensao.Add(Convert.ToString(file)); //listaPorExtensao recebe a variavel com o caminho absoluto
                    }

                }

            }

            logtxt.AddRange(listaPorExtensao);

            foreach (var arqExtTam in listaPorExtensao)
            {
                tamTotArqExt = new FileInfo(arqExtTam.ToString()).Length;
                tamArqExt += tamTotArqExt;                
            }

            totArquivosPorExt = listaPorExtensao.Count;
            lblTotArq.Text = totArquivosPorExt.ToString();
            lblTamTotArq.Text = FormataExibicaoTamanhoArquivo(tamArqExt);                   
        }

        private void pesquisaPorNomeDir()
        {
            string txtExt = txtExtArq.Text;   //Recebe o conjunto de extensões do textbox delimitados por (.)            
            string[] ext = txtExt.Split('.'); //Quebra a string nas delimitações (.) e armazena em um vetor

            //var listaClonada = new List<Vao> { new Vao { Quantidade = 2, Medida = 2 }, new Vao { Quantidade = 0, Medida = 0 }, new Vao { Quantidade = 1, Medida = 1 } };
            long tamTotArqExt;

            if (txtExtArq.Text == "")
            {
                return;
            }


            // ------------- Pesquisa por extensão ------------------ //
            foreach (var item in ext)
            {
                //allfiles recebe o caminho absoluto de cada arquivo encontrado a cada iteração do foreach                    
                allfiles = Directory.GetFiles(dirRaiz, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith("." + item.ToString(), StringComparison.CurrentCulture)).ToArray();

                foreach (var file in allfiles)
                {
                    string[] teste = Convert.ToString(file).Split('\\');
                    bool verificador = false;

                    foreach (var ch in teste)
                    {
                        if (ch == "target" || ch == ".jazz5" || ch == ".git" || ch == ".metadata")
                        {
                            verificador = true;
                        }
                    }

                    if (!verificador)
                    {
                        listaPorExtensao.Add(Convert.ToString(file)); //listaPorExtensao recebe a variavel com o caminho absoluto
                    }

                }

            }

            logtxt.AddRange(listaPorExtensao);

            foreach (var arqExtTam in listaPorExtensao)
            {
                tamTotArqExt = new FileInfo(arqExtTam.ToString()).Length;
                tamArqExt += tamTotArqExt;
            }

            totArquivosPorExt = listaPorExtensao.Count;
            lblTotArq.Text = totArquivosPorExt.ToString();
            lblTamTotArq.Text = FormataExibicaoTamanhoArquivo(tamArqExt);
        }

        // O formato padrão é "0.### XB", Ex: "4.2 KB" ou "1.434 GB"
        public string FormataExibicaoTamanhoArquivo(long i)
        {
            // Obtém o valor absoluto
            long i_absoluto = (i < 0 ? -i : i);

            // Determina o sufixo e o valor
            string sufixo;
            double leitura;

            if (i_absoluto >= 0x1000000000000000) // Exabyte
            {
                sufixo = "EB";
                leitura = (i >> 50);
            }

            else if (i_absoluto >= 0x4000000000000) // Petabyte
            {
                sufixo = "PB";
                leitura = (i >> 40);
            }

            else if (i_absoluto >= 0x10000000000) // Terabyte
            {
                sufixo = "TB";
                leitura = (i >> 30);
            }

            else if (i_absoluto >= 0x40000000) // Gigabyte
            {
                sufixo = "GB";
                leitura = (i >> 20);
            }

            else if (i_absoluto >= 0x100000) // Megabyte
            {
                sufixo = "MB";
                leitura = (i >> 10);
            }

            else if (i_absoluto >= 0x400) // Kilobyte
            {
                sufixo = "KB";
                leitura = i;
            }

            else
            {
                return i.ToString("0 bytes"); // Byte
            }

            // Divide por 1024 para obter o valor fracionário
            leitura = (leitura / 1024);
            // retorna o número formatado com sufixo
            return leitura.ToString("0.### ") + sufixo;
        }

        private void limpaCampos()
        {
            dgv.Rows.Clear();

            tamTotArqDir = null;

            lblTotDir.Text = "---";
            lblTamTotDir.Text = "---";
            lblTotArq.Text = "---";
            lblTamTotArq.Text = "---";

            tamDir = 0;
            tamArqExt = 0;
            totDir = 0;
            totArquivosPorExt = 0;
        }

        private void limpaVariaveis()
        {        
            logtxt.Clear();
            
            tamTotArqDir = null;

            listaPorExtensao.Clear(); //Limpa os elementos da list para uma nova pesquisa
            listaPorNome.Clear(); //Limpa os elementos da list para uma nova pesquisa

            allfiles = null; //Limpa os elementos do vetor para uma nova pesquisa
            allfiles2 = null; //Limpa os elementos do vetor para uma nova pesquisa
            tempAllfiles2.Clear();

            totArquivosPorExt = 0;
            tamArqExt = 0;
            totDir = 0;
            tamDir = 0;

            lblTotArq.Text = "---";
            lblTamTotArq.Text = "---";
            lblStatus.Text = "---";
            lblTotDir.Text = "---";
            lblTamTotDir.Text = "---";
        }

        private void desabilitaBotoes()
        {
            btnPesquisar.Enabled = false;//Disable the Start button
            btnBrowse.Enabled = false;//Disable the Start button
            btnDeletar.Enabled = false;//Disable the Start button
        }

        private void pesquisaPorNomeArq()
        {
            string txtNom = txtNomePasta.Text;  //Recebe o conjunto de nomes do textbox delimitados por (.)
            string[] nom = txtNom.Split('.'); //Quebra a string nas delimitações (.) e armazena em um vetor
            
            //Se o textbox txtNomePasta estiver vazio então finaliza o metodo
            if (txtNomePasta.Text == "")
            {                
                return;
            }            

            // ------------- Pesquisa por nome ------------------ //
            foreach (var item2 in nom)
            {
                //allfiles2 recebe o caminho absoluto de cada diretorio encontrado a cada iteração do foreach
                //allfiles2 = Directory.GetDirectories(dirRaiz, item2.ToString(), SearchOption.AllDirectories);
                //

                allfiles2 = Directory.GetDirectories(dirRaiz, item2, SearchOption.AllDirectories);
                
                foreach (var lst in allfiles2)
                {
                    string[] temp = Convert.ToString(lst).Split('\\');
                    
                    foreach (var verifNome in temp)
                    {                        
                        
                        if (item2.Equals(verifNome, StringComparison.CurrentCulture))
                        {
                            tempAllfiles2.Add(lst);
                        }
                    }
                    
                }                

            }

            foreach (var file2 in tempAllfiles2)
            {
                string[] teste = Convert.ToString(file2).Split('\\');
                bool verificador = false;

                foreach (var ch in teste)
                {
                    if (ch == "target" || ch == ".jazz5" || ch == ".git" || ch == ".scannerwork" || ch == ".metadata")
                    {
                        verificador = true;
                    }
                }

                if (!verificador)
                {
                    listaPorNome.Add(Convert.ToString(file2)); //listaPorNome recebe a variavel com o caminho absoluto
                }

            }

            logtxt.AddRange(listaPorNome);

            foreach (var arqDirTam in listaPorNome)
            {
                tamTotArqDir = new DirectoryInfo(arqDirTam);
                tamDir += TamanhoTotalDiretorio(tamTotArqDir, true);                
            }

            totDir += listaPorNome.Count;
            lblTotDir.Text = totDir.ToString();
            lblTamTotDir.Text = FormataExibicaoTamanhoArquivo(tamDir);         
        }


        private long TamanhoTotalDiretorio(DirectoryInfo dInfo, bool includeSubDir)
        {
            //percorre os arquivos da pasta e calcula o tamanho somando o tamanho de cada arquivo
            long tamanhoTotal = dInfo.EnumerateFiles().Sum(file => file.Length);

            if (includeSubDir)
            {
                tamanhoTotal += dInfo.EnumerateDirectories().Sum(dir => TamanhoTotalDiretorio(dir, true));
            }
            return tamanhoTotal;
        }

        public void ChooseFolder()
        {
            if (dir.ShowDialog() == DialogResult.OK)
            {
                dirRaiz = dir.SelectedPath;
                txtCaminho.Text = dirRaiz;
            }
        }

        private string localLog()
        {
            string localDoLog;
            string nomeArq = "_log_arquivos_deletados.txt";
            DateTime data = DateTime.Now;

            localDoLog = @"C:\\Users\\" + usuarioDamaquina + "\\Desktop\\" + Convert.ToString(data.ToString("ddMMyyyy") + nomeArq);
            return localDoLog;
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {            
            geratxt();
            verificaEstadobusy_btnDeletar();
        }

        private void geratxt()
        {
            if(flag_PesquisaRealizada)
            {
                if (!File.Exists(dirRaiz)) //Se o arquivo de log não existir no diretorio informado, então ele é criado
                {
                    File.Create(localLog()).Close();
                }
            }
            
        }

        private void verificaEstadobusy_btnDeletar()
        {
            if (flag_PesquisaRealizada)
            {
                object[] arrObjects = new object[] { logtxt };//Declare the array of objects

                if (!myWorker_deletar_nome.IsBusy && !myWorker_deletar_extensao.IsBusy)//Check if the worker is already in progress
                {
                    lblStatus.Text = "Deleção iniciada...";
                    desabilitaBotoes();

                    myWorker_deletar_nome.RunWorkerAsync(arrObjects);//Call the background worker
                    myWorker_deletar_extensao.RunWorkerAsync(arrObjects);//Call the background worker
                }
            }
            
        }

        private void deletarPorDiretorio()
        {
            //Se a string allfiles2 estiver null então retorna do metodo
            if (listaPorNome == null)
            {
                return;
            }

            int cont = listaPorNome.Count;            

                foreach (var loc in listaPorNome)
                {
                    try
                    {
                        Directory.Delete(loc, true); //Deleta o diretorio recursivamente através do caminho absoluto                        
                    }
                    catch (DirectoryNotFoundException)
                    {

                    }
                    catch (IOException)
                    {

                    }
                    catch (Exception)
                    {

                    }

            }

        }

        private void deletarPorExtensao()
        {
            //Se a string allfiles estiver null então retorna do metodo
            if (listaPorExtensao == null)
            {
                return;
            }

            int cont = listaPorExtensao.Count;

                foreach (var loc in listaPorExtensao)
                {
                    try
                    {
                        File.Delete(loc); //Deleta o arquivo através do caminho absoluto                        
                    }
                    catch (FileNotFoundException)
                    {
                        
                    }
                    catch (IOException)
                    {

                    }
                    catch (Exception)
                    {

                    }

            }         

        }

        private void geraLog()
        {

            if (flag_PesquisaRealizada)
            {
                tamLog = logtxt.Count;

                if (tamLog == 0)
                {
                    File.AppendAllText(localLog(), "=== Clean ===" + "\r\n"); //Grava em um arquivo externo
                    return;
                }

                File.WriteAllLines(localLog(), logtxt.ToArray());

                flag_PesquisaRealizada = false;
                tamLog = 0;
            }
        }
        
    }
}