using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace localizaEDestroi
{

    class Program
    {        

        private static List<string> listaPorNome = new List<string>();     //List contendo o resultado da pesquisa por nome
        private static List<string> listaPorExtensao = new List<string>(); //List contendo o resultado da pesquisa por extensão
        private static List<string> logtxt = new List<string>();           //Juncao da lista por nome e lista por extensao

        private static long tamArqDir_somador, qtdDir, tamArqExt, qtdArqExt, tamArqExt_verificador, tempoDeExecucao;

        private static string tamTotArqExt, tamTotArqDir;              

        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            int milliseconds = 3000;

            diretorioRaiz(); //Obtem o diretorio de execucao do programa
            criacaoArqParametrosPesquisa(); //Cria diretorio de parametros de pesquisa do programa

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

            geratxt(); //Cria o arquivo de log txt
            geraLog(); //Armazena as informacoes das delecoes no arquivo log

        }

        //Cria diretorio e arquivos com parametros de pesquisa padrao
        public static void criacaoArqParametrosPesquisa()
        {
            if (!File.Exists(dirParametrosDePesquisa())) //Se o diretorio de configuracao nao existir, entao ele sera criado
            {
                Directory.CreateDirectory(dirParametrosDePesquisa());
            }

            if (!File.Exists(localArqListExEspecificas())) //Verifica se o arquivo lista de exclusoes especificas existe
            {
                List<string> lista = new List<string>();

                lista.Add(@"C:\InspecaoSonar\Dev\GIT\RICARDO-PUIG\PAY\java-paysrv\pay-services\src\main\java\br\com\santander\pay\Application.java@@@");
                lista.Add(@"C:\InspecaoSonar\Dev\GIT\RICARDO-PUIG\PAY\java-payappand\app\src\main\java\br\com\santander\ewallet\service\AppController.java@@@");
                lista.Add(@"C:\InspecaoSonar\Dev\RTC\KPV\ORA-KPVCLIORAC-SRC###");
                lista.Add(@"C:\InspecaoSonar\Dev\RTC\KPV\ORA-KPVCLITOOL-SRC###");
                lista.Add(@"C:\InspecaoSonar\Dev\RTC\KPV\VB6-KPVCLIKFIP-SRC###");
                lista.Add(@"C:\InspecaoSonar\Dev\RTC\KPV\VB6-KPVCLISYSE-SRC###");

                File.Create(localArqListExEspecificas()).Close();

                foreach (var linha in lista)
                {
                    File.AppendAllText(localArqListExEspecificas(), linha + "\r\n"); //Grava em um arquivo externo
                }
                
            }

            if (!File.Exists(localArqListExDiretorio())) //Verifica se o arquivo lista de exclusoes por diretorio existe
            {
                string lista = "Test;Tests;Teste;Testes;lib;src-testes;enum;enums;enumeration;ios;build;.scannerwork;.sonar";

                File.Create(localArqListExDiretorio()).Close();
                File.AppendAllText(localArqListExDiretorio(), lista + "\r\n"); //Grava em um arquivo externo
            }

            if (!File.Exists(localArqListExExtensao())) //Verifica se o arquivo lista de exclusoes por extensao existe
            {
                string lista = "dat;zip;sql;jar;ts;rar;vb;json;SQL";

                File.Create(localArqListExExtensao()).Close();
                File.AppendAllText(localArqListExExtensao(), lista + "\r\n"); //Grava em um arquivo externo
            }

        }

        //Metodo para exclusao de arquivos pontuais
        public static void exclusaoExpecifica()
        {
            string[] dirCamSplit = diretorioRaiz().Split('\\'); //Quebra a string do diretorio de execucao do programa
            string nomeDaPasta = dirCamSplit.Last();            //Obtem o nome da sigla (pasta)

            string[]  linhas = File.ReadAllLines(localArqListExEspecificas()); //Efetua a leitura de todas as linhas do arquivo externo txt

            int contadorLinhas = linhas.Count();

            //Verifica se o arquivo esta vazio ou nao
            if (contadorLinhas == 0)
            {
                return;
            }

            logtxt.Add("\r\n" + "***** Exclusao especifica *****" + "\r\n");

            foreach (string linha in linhas)
            {              

                string[] tmp = linha.Split('\\');

                foreach (string ex in tmp)
                {
                    if(ex == nomeDaPasta) //Verifica se o diretorio atual (pasta) faz parte da exclusao especifica
                    {                       

                        if (ex.Contains("@@@")) //No final da string indica que e um arquivo
                        {
                            string caminho = linha.Replace("@@@", "");

                            try
                            {
                                File.Delete(caminho); //Deleta o arquivo através do caminho absoluto                                
                                Console.WriteLine(caminho);                                
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

                            logtxt.Add(caminho); //Armazena o caminho completo para ser mostrado no arquivo de log

                            break;

                        }
                    
                        else if (ex.Contains("###")) //No final da string indica que e uma pasta
                        {
                            string caminho = linha.Replace("###", ""); 

                            try
                            {                                
                                Directory.Delete(caminho, true); //Deleta o diretorio recursivamente através do caminho absoluto                               
                                Console.WriteLine(caminho);
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

                            logtxt.Add(caminho); //Armazena o caminho completo para ser mostrado no arquivo de log                       

                        }
                    }

                }

              }
            
        }
    
        //Calculo do tempo total de execucao do programa
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

        //Metodo responsavel por gravar as informacoes no arquivo txt
        public static void geraLog()
        {
            int tamLog = logtxt.Count;

            if (tamLog == 0)
            {
                string[] vetorListaDiretorio2 = File.ReadAllLines(localArqListExDiretorio());
                string[] vetorListaExtensao2 = File.ReadAllLines(localArqListExExtensao());

                string listaDiretorio2 = "";
                string listaExtensao2 = "";

                foreach (var linha in vetorListaDiretorio2)
                {
                    listaDiretorio2 += linha;
                }

                foreach (var linha in vetorListaExtensao2)
                {
                    listaExtensao2 += linha;
                }

                File.AppendAllText(localLog(), "*********************************************************************************************************" + "\r\n");
                File.AppendAllText(localLog(), "* Quantidade de diretorios: " + qtdDir + "\r\n");
                File.AppendAllText(localLog(), "* Tamanho total dos diretorios: " + tamTotArqDir + "\r\n");
                File.AppendAllText(localLog(), "* Quantidade de arquivos: " + qtdArqExt + "\r\n");
                File.AppendAllText(localLog(), "* Tamanho total dos arquivos: " + tamTotArqExt + "\r\n");
                File.AppendAllText(localLog(), "* Tempo decorrido em HMSms: " + tempoTotalDeExecucao() + "\r\n");
                File.AppendAllText(localLog(), "* ------------------------------------------------------------------------------------------------------- " + "\r\n");
                File.AppendAllText(localLog(), "* Arquivos incluidos na pesquisa: " + "\r\n");
                File.AppendAllText(localLog(), "* <<< Por pastas >>>" + "\r\n");
                File.AppendAllText(localLog(), "* " + listaDiretorio2 + "\r\n");
                File.AppendAllText(localLog(), "* <<< Por extensao de arquivo >>>" + "\r\n");
                File.AppendAllText(localLog(), "* " + listaExtensao2 + "\r\n");
                File.AppendAllText(localLog(), "*********************************************************************************************************" + "\r\n\r\n");
                File.AppendAllText(localLog(), "=== Clean ===" + "\r\n"); //Grava em um arquivo externo   

                return;                
            }

            string[] vetorListaDiretorio = File.ReadAllLines(localArqListExDiretorio());
            string[] vetorListaExtensao = File.ReadAllLines(localArqListExExtensao());

            string listaDiretorio = "";
            string listaExtensao = "";

            foreach (var linha in vetorListaDiretorio)
            {
                listaDiretorio += linha;
            }

            foreach (var linha in vetorListaExtensao)
            {
                listaExtensao += linha;
            }

            File.AppendAllText(localLog(), "*********************************************************************************************************" + "\r\n");
            File.AppendAllText(localLog(), "* Quantidade de diretorios: " + qtdDir + "\r\n");
            File.AppendAllText(localLog(), "* Tamanho total dos diretorios: " + tamTotArqDir + "\r\n");
            File.AppendAllText(localLog(), "* Quantidade de arquivos: " + qtdArqExt + "\r\n");
            File.AppendAllText(localLog(), "* Tamanho total dos arquivos: " + tamTotArqExt + "\r\n");
            File.AppendAllText(localLog(), "* Tempo decorrido em HMSms: " + tempoTotalDeExecucao() + "\r\n");
            File.AppendAllText(localLog(), "* ------------------------------------------------------------------------------------------------------- " + "\r\n");
            File.AppendAllText(localLog(), "* Arquivos incluidos na pesquisa: " + "\r\n");
            File.AppendAllText(localLog(), "* <<< Por pastas >>>" + "\r\n");
            File.AppendAllText(localLog(), "* " + listaDiretorio + "\r\n");
            File.AppendAllText(localLog(), "* <<< Por extensao de arquivo >>>" + "\r\n");
            File.AppendAllText(localLog(), "* " + listaExtensao + "\r\n");
            File.AppendAllText(localLog(), "*********************************************************************************************************" + "\r\n\r\n");

            foreach (var dir in logtxt)
            {                
                File.AppendAllText(localLog(), dir + "\r\n"); //Grava em um arquivo externo
            }

            tamLog = 0;
            
        }

        //Obtem o diretorio de execucao do programa
        public static string diretorioRaiz()
        {
            string diretorio = Environment.CurrentDirectory;
            return diretorio;
        }

        //Diretorio do arquivo de log completo mais o nome do arquivo
        public static string localLog()
        {
            string nomeArq = "_log_arquivos_deletados.txt";
            DateTime data = DateTime.Now;
            string localDoLog = DirLog() + "\\" + Convert.ToString(data.ToString("ddMMyy")+ data.Hour.ToString() + nomeArq);
            return localDoLog;
        }

        //Caminho do diretorio do LOG completo mais o nome da pasta
        public static string DirLog()
        {
            string nomePastaLog = "LogDeletados";
            string DirLog = diretorioRaiz() + "\\" + nomePastaLog;
            return DirLog;
        }

        //Diretorio do arquivo de lista de exclusoes especificas completo mais o nome do arquivo
        public static string localArqListExEspecificas()
        {
            string nomeArq = "lista de exclusoes especificas.txt";
            string localArq = dirParametrosDePesquisa() + "\\" + nomeArq;
            return localArq;
        }

        //Diretorio do arquivo de lista de exclusoes por diretorio completo mais o nome do arquivo
        public static string localArqListExDiretorio()
        {
            string nomeArq = "lista de exclusoes por diretorio.txt";
            string localArq = dirParametrosDePesquisa() + "\\" + nomeArq;
            return localArq;
        }

        //Diretorio do arquivo de lista de exclusoes por extensao completo mais o nome do arquivo
        public static string localArqListExExtensao()
        {
            string nomeArq = "lista de exclusoes por extensao.txt";
            string localArq = dirParametrosDePesquisa() + "\\" + nomeArq;
            return localArq;
        }       

        //Caminho do diretorio do parametro de pesquisa completo mais o nome da pasta
        public static string dirParametrosDePesquisa()
        {
            string pastaArqDeConf = "Parametros pesquisa localizaEDestroi";
            string DirLog = diretorioRaiz() + "\\" + pastaArqDeConf;
            return DirLog;
        }

        //Metodo para pesquisa por extensao de arquivos
        public static void pesquisaPorExtensao()
        {
            string[] allfiles;  //Lista de diretorios pela pesquisa por extensão
            string[] linhas = File.ReadAllLines(localArqListExExtensao()); //Efetua a leitura do arquivo externo com os parametros da pesquisa
            
            string extensaoArq = "";

            int contadorLinhas = linhas.Count();

            //Verifica se o arquivo esta vazio
            if (contadorLinhas == 0)
            {
                return;
            }

            foreach (string item in linhas)
            {
                extensaoArq += item;
            }

            string[] listaDeArqPorExtensao = extensaoArq.Split(';'); //Quebra o texto para obter os parametros de pesquisa por extensao

            foreach (var item in listaDeArqPorExtensao)
            {
                //allfiles recebe o caminho absoluto de cada arquivo encontrado a cada iteração do foreach                    
                allfiles = Directory.GetFiles(diretorioRaiz(), "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith("." + item.ToString(), StringComparison.CurrentCulture)).ToArray();

                foreach (var file in allfiles)
                {
                    string[] teste = Convert.ToString(file).Split('\\');
                    bool verificador = false; //Usado para verificar se o caminho possui algum dos parametros do filtro

                    foreach (var ch in teste)
                    {
                        //Filtro usado para ignorar determinadas pastas na pesquisa
                        if (ch == "target" || ch == ".jazz5" || ch == ".git" || ch == ".metadata")
                        {
                            verificador = true;
                        }
                    }

                    //Entrara na lista apenas se o caminho nao conter nenhum dos parametros do filtro
                    if (!verificador)
                    {
                        listaPorExtensao.Add(Convert.ToString(file)); //listaPorExtensao recebe a variavel com o caminho absoluto
                        Console.WriteLine(file);
                    }

                }

            }

            //Apos o termino da pesquisa, a lista de pesquisa atual e adicionada na lista central para gerar as informacoes no arquivo de log
            logtxt.AddRange(listaPorExtensao);

            foreach (var arqExtTam in listaPorExtensao)
            {
                tamArqExt_verificador = new FileInfo(arqExtTam.ToString()).Length;
                tamArqExt += tamArqExt_verificador;
            }

            qtdArqExt = listaPorExtensao.Count; //Obtem a quantidade de arquivos por extensao encontrados na pesquisa
            tamTotArqExt = FormataExibicaoTamanhoArquivo(tamArqExt); //Obtem o tamanho total dos arquivos encontrados na pesquisa
        }

        //Responsavel por calcular o tamanho dos diretorios encontrados na pesquisa
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

        //Metodo para pesquisa por diretorio de arquivos
        public static void pesquisaPorDiretorio()
        {
            List<string> tempAllfiles2 = new List<string>(); //Armazena partes de um caminho para iteracao de arquivos

            string[] allfiles2; //Lista de diretorios pela pesquisa por nome

            DirectoryInfo tamDir_verificador;

            string extensaoArq = "";
            string[] linhas = File.ReadAllLines(localArqListExDiretorio());

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

            logtxt.AddRange(listaPorNome);

            foreach (var arqDirTam in listaPorNome)
            {
                tamDir_verificador = new DirectoryInfo(arqDirTam);
                tamArqDir_somador += TamanhoTotalDiretorio(tamDir_verificador, true);
            }

            qtdDir += listaPorNome.Count;
            qtdDir.ToString();
            tamTotArqDir = FormataExibicaoTamanhoArquivo(tamArqDir_somador);

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

        //Metodo responsavel por criar o arquivo de texto do log
        public static void geratxt()
        {
            if (!File.Exists(diretorioRaiz())) //Se o diretorio de log não existir, então ele é criado
            {
                Directory.CreateDirectory(DirLog());
            }

            File.Create(localLog()).Close();

        }

    }

}