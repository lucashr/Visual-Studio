using localizaEDestroi_CLI.Deletar;
using localizaEDestroi_CLI.Log;
using localizaEDestroi_CLI.Pesquisa;
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

        private static List<string> listaPorDiretorio = new List<string>();     //List contendo o resultado da pesquisa por nome
        private static List<string> listaPorExtensao = new List<string>(); //List contendo o resultado da pesquisa por extensão
        private static List<string> logtxt = new List<string>();           //Juncao da lista por nome e lista por extensao
        private static List<string> listaExclusaoEspec = new List<string>();
        private static long qtdDir, qtdArqExt, tempoDeExecucao;
        private static string tamTotArqExt, tamTotArqDir;
        private static CriaArquivosConfiguracao dirRaiz = new CriaArquivosConfiguracao();

        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            int milliseconds = 3000;

            dirRaiz.CriacaoArqParametrosPesquisa(); //Cria diretorio de parametros de pesquisa do programa

            Console.WriteLine("------ Pesquisa iniciada ------");

            // dispara uma nova thread para executar 
            Thread tPe = new Thread(PesquisaPorExtensao);
            tPe.Start();
            // dispara uma nova thread para executar 
            Thread tPn = new Thread(  PesquisaPorDiretorio);
            tPn.Start();

            while (tPe.IsAlive || tPn.IsAlive)
            {
                Console.Write(".");

                Thread.Sleep(milliseconds);
            }

            Console.WriteLine("------ Pesquisa concluida ------");

            Console.WriteLine("------ Exclusao iniciada ------");
            // dispara uma nova thread para executar 
            Thread tDe = new Thread(DeletarPorExtensao);
            tDe.Start();
            //dispara uma nova thread para executar
            Thread tdn = new Thread(DeletarPorDiretorio);
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

            GeraTxt geraLog = new GeraTxt();

            Console.WriteLine(geraLog.TempoTotalDeExecucao(tempoDeExecucao));

            geraLog.Geratxt();

            geraLog.GeraLog(listaPorDiretorio,listaPorExtensao, qtdDir,tamTotArqDir,
                            qtdArqExt,tamTotArqExt,tempoDeExecucao, listaExclusaoEspec);

        }

        //Metodo para exclusao de arquivos pontuais
        public static void exclusaoExpecifica()
        {
            Deletar deletar = new Deletar();
            //var lst = deletar.ExclusaoExpecifica().Item1;
            listaExclusaoEspec.AddRange(deletar.ExclusaoExpecifica().Item1);
            //if (lst != null)
            //{
                
            //}            

        }

        //Metodo para pesquisa por extensao de arquivos
        public static void PesquisaPorExtensao()
        {
            Pesquisa pesquisa = new Pesquisa();
            var ret = pesquisa.PesquisaPorExtensao();
            tamTotArqExt = ret.Item2;
            listaPorExtensao = ret.Item1;
            logtxt.AddRange(ret.Item1);
            qtdArqExt = ret.Item1.Count();
        }

        //Metodo para pesquisa por diretorio de arquivos
        public static void   PesquisaPorDiretorio()
        {
            Pesquisa pesquisa = new Pesquisa();
            var ret = pesquisa.PesquisaPorDiretorio();
            tamTotArqDir = ret.Item2;
            listaPorDiretorio = ret.Item1;
            logtxt.AddRange(ret.Item1);
            qtdDir = ret.Item3;
        }

        private static void DeletarPorDiretorio()
        {
            Deletar deletar = new Deletar();
            deletar.DeletarPorDiretorio(listaPorDiretorio);
        }

        private static void DeletarPorExtensao()
        {
            Deletar deletar = new Deletar();
            deletar.DeletarPorExtensao(listaPorExtensao);
        }


    }

}