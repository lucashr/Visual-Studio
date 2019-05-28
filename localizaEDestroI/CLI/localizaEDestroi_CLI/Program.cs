using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace localizaEDestroi
{

    class Program
    {
        static readonly object bloqueador = new object();
        static bool ok;

        private static string[] allfiles;  //Lista de diretorios pela pesquisa por extensão
        private static string[] allfiles2; //Lista de diretorios pela pesquisa por nome

        private static List<string> listaPorNome = new List<string>();     //List contendo o resultado da pesquisa por nome
        private static List<string> listaPorExtensao = new List<string>(); //List contendo o resultado da pesquisa por extensão
        private static List<string> logtxt = new List<string>();           //Juncao da lista por nome e lista por extensao
        private static List<string> tempAllfiles2 = new List<string>();
        private static List<string> listaEspecExclusao = new List<string>();
        private static List<string> caminhoAbsolutoExclusaoEspec = new List<string>();

        private static DirectoryInfo tamDir_verificador;

        private static long tamArqDir_somador;
        private static long qtdDir;
        private static long tamArqExt;
        private static long qtdArqExt;
        private static long tamArqExt_verificador;

        private static long tempoDeExecucao;
        private static string tamTotArqExt;
        private static string tamTotArqDir;        

        private static int tamLog;

        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            int milliseconds = 3000;

            diretorioRaiz();

            Console.WriteLine("------ Pesquisa iniciada ------");

            // dispara uma nova thread para executar 
            Thread tPe = new Thread(pesquisaPorExtensao);
            tPe.Start();
            // dispara uma nova thread para executar 
            Thread tPn = new Thread(pesquisaPorDiretorio);
            tPn.Start();

            while (tPe.IsAlive || tPn.IsAlive)
            {
                Console.Write(".");

                Thread.Sleep(milliseconds);
            }

            Console.WriteLine("------ Pesquisa concluida ------");

            Console.WriteLine("------ Exclusao iniciada ------");
            // dispara uma nova thread para executar 
            Thread tDe = new Thread(deletarPorExtensao);
            tDe.Start();
            //dispara uma nova thread para executar
            Thread tdn = new Thread(deletarPorDiretorio);
            tdn.Start();

            while (tDe.IsAlive || tdn.IsAlive)
            {
                Console.Write(".");

                Thread.Sleep(milliseconds);
            }

            Console.WriteLine("------ Especifica iniciada ------");
            exclusaoExpecifica();
            Console.WriteLine("------ Especifica concluida ------");

            Console.WriteLine("------ Exclusao concluida ------");
            tempoDeExecucao = sw.ElapsedMilliseconds;

            Console.WriteLine(tempoDeExecucao.ToString());
            tempoTotalDeExecucao();

            geratxt();
            geraLog();

        }

        public static void exclusaoExpecifica()
        {

            //string[] caminhoAbsolutoExclusaoEspec = new string[6];
            //string[] listaEspecExclusao = new string[2];
            //List<string> exclusaoEspecSplit = new List<string>();

            string[] dirCamSplit = diretorioRaiz().Split('\\');
            string nomeDaPasta = dirCamSplit.Last();

            string[]  linhas = File.ReadAllLines(@"C:\Users\lucas.rafael\Desktop\ambiente de teste\lista de exclusoes especificas.txt");

            foreach (string verificadorDir in linhas)
            {
                string[] tmp = verificadorDir.Split('\\'); //Variavel temporaria
            }

            //caminhoAbsolutoExclusaoEspec[0] = @"C:\InspecaoSonar\Dev\GIT\RICARDO-PUIG\PAY\java-paysrv\pay-services\src\main\java\br\com\santander\pay\Application.java";
            //caminhoAbsolutoExclusaoEspec[1] = @"C:\InspecaoSonar\Dev\GIT\RICARDO-PUIG\PAY\java-payappand\app\src\main\java\br\com\santander\ewallet\service\AppController.java";
            //caminhoAbsolutoExclusaoEspec[2] = @"C:\InspecaoSonar\Dev\RTC\KPV\ORA-KPVCLIORAC-SRC";
            //caminhoAbsolutoExclusaoEspec[3] = @"C:\InspecaoSonar\Dev\RTC\KPV\ORA-KPVCLITOOL-SRC";
            //caminhoAbsolutoExclusaoEspec[4] = @"C:\InspecaoSonar\Dev\RTC\KPV\VB6-KPVCLIKFIP-SRC";
            //caminhoAbsolutoExclusaoEspec[5] = @"C:\InspecaoSonar\Dev\RTC\KPV\VB6-KPVCLISYSE-SRC";

            int contadorLinhas = linhas.Count();

            if (contadorLinhas == 0)
            {
                return;
            }

            logtxt.Add("\r\n" + "***** Exclusao especifica *****" + "\r\n");

            foreach (string linha in linhas)
            {              

                string[] tmp = linha.Split('\\'); //Variavel temporaria

                foreach (string ex in tmp)
                {
                    if(ex == nomeDaPasta)
                    {

                    }
                    if (ex.Contains("@@@"))//Indice que sera uma arquivo a ser deletado
                    {
                        try
                        {
                            File.Delete(linha); //Deleta o arquivo através do caminho absoluto 
                            Console.WriteLine(linha);
                            //File.Delete(caminhoAbsolutoExclusaoEspec[1]);
                            //Console.WriteLine(caminhoAbsolutoExclusaoEspec[1]);
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

                        logtxt.Add(linha.Replace("@@@",""));

                    }
                    else if (ex.Contains("###"))
                    {
                        try
                        {
                            Directory.Delete(linha, true); //Deleta o diretorio recursivamente através do caminho absoluto    
                            Console.WriteLine(linha);
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

                        logtxt.Add(linha.Replace("###", ""));

                    }

                }

              }
            
        }
    
        public static string tempoTotalDeExecucao()
        {
            long horas, minutos, segundos, milisegundos;
            
            long auxm, auxs;
            segundos = tempoDeExecucao / 1000;
            milisegundos = (tempoDeExecucao % 1000);

            if (segundos >= 60)
            {
                segundos = segundos / 60;
            }

            auxs = segundos * 60;
            minutos = auxs / 60;

            if (minutos > 60)
            {
                minutos = minutos / 60;
            }

            auxm = minutos * 60;
            horas = minutos / 60;

            if (horas > 60)
            {
                horas = horas / 60;
            }

            return horas.ToString() + ":" + minutos.ToString() + ":" + segundos.ToString() + ":" + milisegundos.ToString();
        }

        public static string diretorioRaiz()
        {            
            string diretorio = Environment.CurrentDirectory;
            return diretorio;
        }
        
        public static void pesquisaPorExtensao()
        {
            string extensaoArq = "";
            //extensaoArq[0]="dat";
            //extensaoArq[1]= "zip";
            //extensaoArq[2]= "sql";
            //extensaoArq[3]= "jar";
            //extensaoArq[4]= "ts";
            //extensaoArq[5]= "rar";
            //extensaoArq[6]= "vb";
            //extensaoArq[7]= "json";
            //extensaoArq[8]= "SQL";

            string[] linhas = File.ReadAllLines(@"C:\Users\lucas.rafael\Desktop\ambiente de teste\lista de exclusoes por extensao.txt");

            int contadorLinhas = linhas.Count();

            if (contadorLinhas == 0)
            {
                return;
            }

            foreach (string item in linhas)
            {
                extensaoArq += item;
            }

            string[] listaDeArqPorExtensao = extensaoArq.Split(';');

            // ------------- Pesquisa por extensão ------------------ //
            foreach (var item in listaDeArqPorExtensao)
            {
                //allfiles recebe o caminho absoluto de cada arquivo encontrado a cada iteração do foreach                    
                allfiles = Directory.GetFiles(diretorioRaiz(), "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith("." + item.ToString(), StringComparison.CurrentCulture)).ToArray();

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
                        Console.WriteLine(file);
                    }

                }

            }

            lock (bloqueador)
            {
                if (!ok)
                {
                    logtxt.AddRange(listaPorExtensao);
                }
            }

            foreach (var arqExtTam in listaPorExtensao)
            {
                tamArqExt_verificador = new FileInfo(arqExtTam.ToString()).Length;
                tamArqExt += tamArqExt_verificador;
            }

            qtdArqExt = listaPorExtensao.Count;
            tamTotArqExt = FormataExibicaoTamanhoArquivo(tamArqExt);
        }

        private static long TamanhoTotalDiretorio(DirectoryInfo dInfo, bool includeSubDir)
        {
            //percorre os arquivos da pasta e calcula o tamanho somando o tamanho de cada arquivo
            long tamanhoTotal = dInfo.EnumerateFiles().Sum(file => file.Length);

            if (includeSubDir)
            {
                tamanhoTotal += dInfo.EnumerateDirectories().Sum(dir => TamanhoTotalDiretorio(dir, true));
            }

            return tamanhoTotal;
        }

        public static void pesquisaPorDiretorio()
        {            

            string extensaoArq = "";//17

            //diretorioArq[0] = "Test";
            //diretorioArq[1] = "Tests";
            //diretorioArq[2] = "Teste";
            //diretorioArq[3] = "Testes";
            //diretorioArq[4] = "lib";
            //diretorioArq[5] = "src-testes";
            //diretorioArq[6] = "enum";
            //diretorioArq[7] = "enums";
            //diretorioArq[8] = "enumeration";
            //diretorioArq[9] = "ios";
            //diretorioArq[10] = "build";
            //diretorioArq[11] = ".sonar";
            //diretorioArq[12] = ".scannerwork";
            //diretorioArq[13] = "ORA-KPVCLIORAC-SRC";
            //diretorioArq[14] = "ORA-KPVCLITOOL-SRC";
            //diretorioArq[15] = "VB6-KPVCLIKFIP-SRC";
            //diretorioArq[16] = "VB6-KPVCLISYSE-SRC";

            string[] linhas = File.ReadAllLines(@"C:\Users\lucas.rafael\Desktop\ambiente de teste\lista de exclusoes por diretorio.txt");

            int contadorLinhas = linhas.Count();

            if (contadorLinhas == 0)
            {
                return;
            }

            foreach (string item in linhas)
            {
                extensaoArq += item;
            }

            string[] listaDeArqPorExtensao = extensaoArq.Split(';');

            foreach (var item2 in listaDeArqPorExtensao)
            {
                //allfiles2 recebe o caminho absoluto de cada diretorio encontrado a cada iteração do foreach
                allfiles2 = Directory.GetDirectories(diretorioRaiz(), item2.ToString(), SearchOption.AllDirectories);

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
                    if (ch == "target" || ch == ".jazz5" || ch == ".git" || ch == ".metadata")
                    {
                        verificador = true;
                    }
                }

                if (!verificador)
                {
                    listaPorNome.Add(Convert.ToString(file2)); //listaPorNome recebe a variavel com o caminho absoluto
                    Console.WriteLine(file2);
                }

            }

            lock (bloqueador)
            {
                if (!ok)
                {
                    logtxt.AddRange(listaPorNome);
                }
            }        

            foreach (var arqDirTam in listaPorNome)
            {
                tamDir_verificador = new DirectoryInfo(arqDirTam);
                tamArqDir_somador += TamanhoTotalDiretorio(tamDir_verificador, true);
            }

            qtdDir += listaPorNome.Count;
            qtdDir.ToString();
            tamTotArqDir = FormataExibicaoTamanhoArquivo(tamArqDir_somador);

        }

        // O formato padrão é "0.### XB", Ex: "4.2 KB" ou "1.434 GB"
        public static string FormataExibicaoTamanhoArquivo(long i)
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

        

        public static void geraLog()
        {
            tamLog = logtxt.Count;

            if (tamLog == 0)
            {                
                File.AppendAllText(localLog(), "=== Clean ===" + "\r\n"); //Grava em um arquivo externo                

                return;                
            }

            File.AppendAllText(localLog(), "*********************************************************************************************************" + "\r\n");
            File.AppendAllText(localLog(), "* Quantidade de diretorios: " + qtdDir + "\r\n");
            File.AppendAllText(localLog(), "* Tamanho total dos diretorios: " + tamTotArqDir + "\r\n");
            File.AppendAllText(localLog(), "* Quantidade de arquivos: " + qtdArqExt + "\r\n");
            File.AppendAllText(localLog(), "* Tamanho total dos arquivos: " + tamTotArqExt + "\r\n");
            File.AppendAllText(localLog(), "* Tempo decorrido em HMSms: " + tempoTotalDeExecucao() + "\r\n");
            File.AppendAllText(localLog(), "* ------------------------------------------------------------------------------------------------------- " + "\r\n");
            File.AppendAllText(localLog(), "* Arquivos incluidos na pesquisa: " + "\r\n");
            File.AppendAllText(localLog(), "* Pastas: Test;Tests;Teste;Testes;lib;src-testes;" + "\r\n");
            File.AppendAllText(localLog(), "* enum;enums;enumeration;ios;build;.scannerwork;.sonar;" + "\r\n");
            File.AppendAllText(localLog(), "* ORA-KPVCLIORAC-SRC;ORA-KPVCLITOOL-SRC;VB6-KPVCLIKFIP-SRC;VB6-KPVCLISYSE-SRC" + "\r\n");
            File.AppendAllText(localLog(), "* Extensao: dat;zip;sql;jar;ts;rar;vb;json" + "\r\n");
            File.AppendAllText(localLog(), "*********************************************************************************************************" + "\r\n\r\n");

            foreach (var dir in logtxt)
            {                
                File.AppendAllText(localLog(), dir + "\r\n"); //Grava em um arquivo externo
            }                

            tamLog = 0;
            
        }

        public static void geratxt()
        {
            if (!File.Exists(diretorioRaiz())) //Se o arquivo de log não existir no diretorio informado, então ele é criado
            {
                Directory.CreateDirectory(DirLog());
            }

            if (!File.Exists(DirLog())) //Se o arquivo de log não existir no diretorio informado, então ele é criado
            {
                File.Create(localLog()).Close();
            }

            FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.Write, diretorioRaiz());

            try
            {
                File.Create(localLog()).Close();
            }
            catch (SecurityException s)
            {
                Console.WriteLine("Restricao: "+s.Message);
            }

        }

        //Diretorio do arquivo de log completo mais o nome do arquivo
        public static string localLog()
        {
            string localDoLog;
            string nomeArq = "_log_arquivos_deletados.txt";
            DateTime data = DateTime.Now;            
            localDoLog = DirLog() + "\\" + Convert.ToString(data.ToString("ddMMyy")+ data.Hour.ToString() + nomeArq);
            return localDoLog;
        }

        //Diretorio do LOG completo mais o nome da pasta
        public static string DirLog()
        {
            string DirLog;
            string nomePastaLog = "LogDeletados";
            DirLog = diretorioRaiz() + "\\" + nomePastaLog;
            return DirLog;
        }

        private void limpaVariaveis()
        {
            logtxt.Clear();

            tamTotArqDir = null;
            tempAllfiles2.Clear();
            listaPorExtensao.Clear(); //Limpa os elementos da list para uma nova pesquisa
            listaPorNome.Clear(); //Limpa os elementos da list para uma nova pesquisa

            allfiles = null; //Limpa os elementos do vetor para uma nova pesquisa
            allfiles2 = null; //Limpa os elementos do vetor para uma nova pesquisa

            qtdArqExt = 0;
        }       

        private static void deletarPorDiretorio()
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
                    Console.WriteLine(loc);
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

        private static void deletarPorExtensao()
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
                    Console.WriteLine(loc);
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

    }

}