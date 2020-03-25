//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace localizaEDestroi_CLI.Pesquisa
//{
//    class GeraLogTxt
//    {
//        //Metodo responsavel por gravar as informacoes no arquivo txt
//        public static void geraLog()
//        {
//            int tamLog = logtxt.Count;

//            if (tamLog == 0)
//            {
//                string[] vetorListaDiretorio2 = File.ReadAllLines(localArqListExDiretorio());
//                string[] vetorListaExtensao2 = File.ReadAllLines(localArqListExExtensao());

//                string listaDiretorio2 = "";
//                string listaExtensao2 = "";

//                foreach (var linha in vetorListaDiretorio2)
//                {
//                    listaDiretorio2 += linha;
//                }

//                foreach (var linha in vetorListaExtensao2)
//                {
//                    listaExtensao2 += linha;
//                }

//                File.AppendAllText(localLog(), "*********************************************************************************************************" + "\r\n");
//                File.AppendAllText(localLog(), "* Quantidade de diretorios: " + qtdDir + "\r\n");
//                File.AppendAllText(localLog(), "* Tamanho total dos diretorios: " + tamTotArqDir + "\r\n");
//                File.AppendAllText(localLog(), "* Quantidade de arquivos: " + qtdArqExt + "\r\n");
//                File.AppendAllText(localLog(), "* Tamanho total dos arquivos: " + tamTotArqExt + "\r\n");
//                File.AppendAllText(localLog(), "* Tempo decorrido em HMSms: " + tempoTotalDeExecucao() + "\r\n");
//                File.AppendAllText(localLog(), "* ------------------------------------------------------------------------------------------------------- " + "\r\n");
//                File.AppendAllText(localLog(), "* Arquivos incluidos na pesquisa: " + "\r\n");
//                File.AppendAllText(localLog(), "* <<< Por pastas >>>" + "\r\n");
//                File.AppendAllText(localLog(), "* " + listaDiretorio2 + "\r\n");
//                File.AppendAllText(localLog(), "* <<< Por extensao de arquivo >>>" + "\r\n");
//                File.AppendAllText(localLog(), "* " + listaExtensao2 + "\r\n");
//                File.AppendAllText(localLog(), "*********************************************************************************************************" + "\r\n\r\n");
//                File.AppendAllText(localLog(), "=== Clean ===" + "\r\n"); //Grava em um arquivo externo   

//                return;
//            }

//            string[] vetorListaDiretorio = File.ReadAllLines(localArqListExDiretorio());
//            string[] vetorListaExtensao = File.ReadAllLines(localArqListExExtensao());

//            string listaDiretorio = "";
//            string listaExtensao = "";

//            foreach (var linha in vetorListaDiretorio)
//            {
//                listaDiretorio += linha;
//            }

//            foreach (var linha in vetorListaExtensao)
//            {
//                listaExtensao += linha;
//            }

//            File.AppendAllText(localLog(), "*********************************************************************************************************" + "\r\n");
//            File.AppendAllText(localLog(), "* Quantidade de diretorios: " + qtdDir + "\r\n");
//            File.AppendAllText(localLog(), "* Tamanho total dos diretorios: " + tamTotArqDir + "\r\n");
//            File.AppendAllText(localLog(), "* Quantidade de arquivos: " + qtdArqExt + "\r\n");
//            File.AppendAllText(localLog(), "* Tamanho total dos arquivos: " + tamTotArqExt + "\r\n");
//            File.AppendAllText(localLog(), "* Tempo decorrido em HMS: " + tempoTotalDeExecucao() + "\r\n");
//            File.AppendAllText(localLog(), "* ------------------------------------------------------------------------------------------------------- " + "\r\n");
//            File.AppendAllText(localLog(), "* Arquivos incluidos na pesquisa: " + "\r\n");
//            File.AppendAllText(localLog(), "* <<< Por pastas >>>" + "\r\n");
//            File.AppendAllText(localLog(), "* " + listaDiretorio + "\r\n");
//            File.AppendAllText(localLog(), "* <<< Por extensao de arquivo >>>" + "\r\n");
//            File.AppendAllText(localLog(), "* " + listaExtensao + "\r\n");
//            File.AppendAllText(localLog(), "*********************************************************************************************************" + "\r\n\r\n");

//            foreach (var dir in logtxt)
//            {
//                File.AppendAllText(localLog(), dir + "\r\n"); //Grava em um arquivo externo
//            }

//            tamLog = 0;

//        }
//    }
//}
