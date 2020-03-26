using localizaEDestroi_CLI.Pesquisa;
using System;
using System.Collections.Generic;
using System.IO;


namespace localizaEDestroi_CLI.Log
{
    class GeraTxt
    {
        static readonly CriaArquivosConfiguracao dirRaiz = new CriaArquivosConfiguracao();
   
        //Metodo responsavel por criar o arquivo de texto do log
        public void Geratxt()
        {

            if (!File.Exists(dirRaiz.DiretorioRaiz())) //Se o diretorio de log não existir, então ele é criado
            {
                Directory.CreateDirectory(DirLog());
            }

            File.Create(LocalLog()).Close();

        }

        public static string DirLog()
        {
            string nomePastaLog = "LogDeletados";
            string DirLog = dirRaiz.DiretorioRaiz() + "\\" + nomePastaLog;
            return DirLog;
        }

        public static string LocalLog()
        {
            string nomeArq = "_log_arquivos_deletados.txt";
            DateTime data = DateTime.Now;
            string localDoLog = DirLog() + "\\" + Convert.ToString(data.ToString("ddMMyy") + data.Hour.ToString() + nomeArq);
            return localDoLog;
        }

        public void GeraLog(List<string> listaArquivosDir, List<string> listaArquivosExt,
            long qtdDir, string tamTotArqDir, long qtdArqExt, string tamTotArqExt, long tempoDeExecucao, List<string> listaExclusaoEspec)
        {
            List<string> logtxt = new List<string>();
            clsPesquisa pesquisa = new clsPesquisa();

            string[] diretoriosIgnorados = File.ReadAllLines(pesquisa.LocalArqListDiretoriosIgnorados());
            string ignoradosNaPesquisa = "";

            logtxt.AddRange(listaArquivosDir);
            logtxt.AddRange(listaArquivosExt);
            logtxt.AddRange(listaExclusaoEspec);

            foreach (var dir in diretoriosIgnorados)
            {
                ignoradosNaPesquisa += dir;
            }

            int tamLog = logtxt.Count;

            if (tamLog == 0)
            {
                string[] vetorListaDiretorio2 = File.ReadAllLines(dirRaiz.LocalArqListExDiretorio());
                string[] vetorListaExtensao2 = File.ReadAllLines(dirRaiz.LocalArqListExExtensao());

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

                File.AppendAllText(LocalLog(), "*********************************************************************************************************" + "\r\n");
                File.AppendAllText(LocalLog(), "* Quantidade de diretorios: " + qtdDir + "\r\n");
                File.AppendAllText(LocalLog(), "* Tamanho total dos diretorios: " + tamTotArqDir + "\r\n");
                File.AppendAllText(LocalLog(), "* Quantidade de arquivos: " + qtdArqExt + "\r\n");
                File.AppendAllText(LocalLog(), "* Tamanho total dos arquivos: " + tamTotArqExt + "\r\n");
                File.AppendAllText(LocalLog(), "* Tempo decorrido em HMSms: " + TempoTotalDeExecucao(tempoDeExecucao) + "\r\n");
                File.AppendAllText(LocalLog(), "* ------------------------------------------------------------------------------------------------------- " + "\r\n");
                File.AppendAllText(LocalLog(), "* << Ignorados na pesquisa >> " + "\r\n");
                File.AppendAllText(LocalLog(), "* " + ignoradosNaPesquisa + "\r\n");
                File.AppendAllText(LocalLog(), "* Arquivos incluidos na pesquisa: " + "\r\n");
                File.AppendAllText(LocalLog(), "* <<< Por pastas >>>" + "\r\n");
                File.AppendAllText(LocalLog(), "* " + listaDiretorio2 + "\r\n");
                File.AppendAllText(LocalLog(), "* <<< Por extensao de arquivo >>>" + "\r\n");
                File.AppendAllText(LocalLog(), "* " + listaExtensao2 + "\r\n");
                File.AppendAllText(LocalLog(), "*********************************************************************************************************" + "\r\n\r\n");
                File.AppendAllText(LocalLog(), "=== Clean ===" + "\r\n"); //Grava em um arquivo externo   

                return;
            }

            string[] vetorListaDiretorio = File.ReadAllLines(dirRaiz.LocalArqListExDiretorio());
            string[] vetorListaExtensao = File.ReadAllLines(dirRaiz.LocalArqListExExtensao());

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

            File.AppendAllText(LocalLog(), "*********************************************************************************************************" + "\r\n");
            File.AppendAllText(LocalLog(), "* Quantidade de diretorios: " + qtdDir + "\r\n");
            File.AppendAllText(LocalLog(), "* Tamanho total dos diretorios: " + tamTotArqDir + "\r\n");
            File.AppendAllText(LocalLog(), "* Quantidade de arquivos: " + qtdArqExt + "\r\n");
            File.AppendAllText(LocalLog(), "* Tamanho total dos arquivos: " + tamTotArqExt + "\r\n");
            File.AppendAllText(LocalLog(), "* Tempo decorrido em HMS: " + TempoTotalDeExecucao(tempoDeExecucao) + "\r\n");
            File.AppendAllText(LocalLog(), "* ------------------------------------------------------------------------------------------------------- " + "\r\n");
            File.AppendAllText(LocalLog(), "* << Ignorados na pesquisa >> " + "\r\n");
            File.AppendAllText(LocalLog(), "* " + ignoradosNaPesquisa + "\r\n");
            File.AppendAllText(LocalLog(), "* Arquivos incluidos na pesquisa: " + "\r\n");
            File.AppendAllText(LocalLog(), "* <<< Por pastas >>>" + "\r\n");
            File.AppendAllText(LocalLog(), "* " + listaDiretorio + "\r\n");
            File.AppendAllText(LocalLog(), "* <<< Por extensao de arquivo >>>" + "\r\n");
            File.AppendAllText(LocalLog(), "* " + listaExtensao + "\r\n");
            File.AppendAllText(LocalLog(), "*********************************************************************************************************" + "\r\n\r\n");

            foreach (var dir in logtxt)
            {
                File.AppendAllText(LocalLog(), dir + "\r\n"); //Grava em um arquivo externo
            }


        }

        //Calculo do tempo total de execucao do programa
        public string TempoTotalDeExecucao(long t)
        {
            long horas, minutos, segundos;

            segundos = (t / 1000) % 60;      // se não precisar de segundos, basta remover esta linha.
            minutos = (t / 60000) % 60;     // 60000   = 60 * 1000
            horas = t / 3600000;            // 3600000 = 60 * 60 * 1000

            return horas.ToString() + ":" + minutos.ToString() + ":" + segundos.ToString();
        }
    }
}
