using localizaEDestroi_CLI.Pesquisa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace localizaEDestroi_CLI.Deletar
{
    class Deletar
    {
        public void DeletarPorDiretorio(List<string> listaPorDiretorio)
        {
            //Se a string allfiles2 estiver null então retorna do metodo
            if (listaPorDiretorio == null)
            {
                return;
            }

            int cont = listaPorDiretorio.Count;

            foreach (var loc in listaPorDiretorio)
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

        public void DeletarPorExtensao(List<string> listaPorExtensao)
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

        //Metodo para exclusao de arquivos pontuais
        public Tuple<List<string>> ExclusaoExpecifica()
        {
           CriaArquivosConfiguracao dirRaiz = new CriaArquivosConfiguracao();

            string[] dirCamSplit = dirRaiz.DiretorioRaiz().Split('\\'); //Quebra a string do diretorio de execucao do programa
            string[] linhas = File.ReadAllLines(dirRaiz.LocalArqListExEspecificas()); //Efetua a leitura de todas as linhas do arquivo externo txt
            string nomeDaPasta = dirCamSplit.Last();            //Obtem o nome da sigla (pasta)
            List<string> lstExclusaEspec = new List<string>();

            bool flag_titulo = false; //Flag responsavel por informar se o titulo foi imprimido no log ou nao

            int contadorLinhas = linhas.Count();

            //Verifica se o arquivo esta vazio ou nao
            if (contadorLinhas == 0)
            {
                return Tuple.Create(lstExclusaEspec);
            }

            foreach (string linha in linhas)
            {
                string[] tmp = linha.Split('\\');

                foreach (string ex in tmp)
                {
                    if (ex == nomeDaPasta) //Verifica se o diretorio atual (pasta) faz parte da exclusao especifica
                    {
                        if (!flag_titulo)
                        {
                            lstExclusaEspec.Add("\r\n" + "***** Exclusao especifica *****" + "\r\n");
                            flag_titulo = true;
                        }

                        string[] tmp2 = linha.Split('\\'); //Usado para verificar o arquivo da pasta especifica

                        foreach (string verificaArquivo in tmp2)
                        {

                            if (verificaArquivo.Contains("@@@")) //No final da string indica que e um arquivo
                            {
                                string caminho = linha.Replace("@@@", "");

                                bool arqPresente = true; //Usada para verificar se o arquivo estava no diretorio ou nao

                                try
                                {
                                    //File.OpenRead(caminho); //Usado para tentar efetuar a leitura de um arquivo, com o objetivo de gerar uma exception caso ele nao esteja presente
                                    arqPresente = File.Exists(caminho);
                                    File.Delete(caminho);   //Deleta o arquivo através do caminho absoluto                                    
                                }
                                catch (FileNotFoundException)
                                {
                                    arqPresente = false;
                                }
                                catch (IOException)
                                {
                                    arqPresente = false;
                                }
                                catch (Exception)
                                {
                                    arqPresente = false;
                                }

                                if (arqPresente) //Verifica se o arquivo esta presente ou nao no direotorio
                                {
                                    Console.WriteLine(caminho);
                                    lstExclusaEspec.Add(caminho); //Armazena o caminho completo para ser mostrado no arquivo de log
                                }

                            }

                            else if (verificaArquivo.Contains("###")) //No final da string indica que e uma pasta
                            {
                                string caminho = linha.Replace("###", "");

                                bool arqPresente = true; //Usada para verificar se o arquivo estava no diretorio ou nao

                                try
                                {
                                    Directory.Delete(caminho, true); //Deleta o diretorio recursivamente através do caminho absoluto                                    
                                }
                                catch (DirectoryNotFoundException)
                                {
                                    arqPresente = false;
                                }
                                catch (IOException)
                                {
                                    arqPresente = false;
                                }
                                catch (Exception)
                                {
                                    arqPresente = false;
                                }

                                if (arqPresente) //Verifica se o arquivo esta presente ou nao no direotorio
                                {
                                    Console.WriteLine(caminho);
                                    lstExclusaEspec.Add(caminho); //Armazena o caminho completo para ser mostrado no arquivo de log
                                }

                            }
                        }

                    }

                }

            }

            return Tuple.Create(lstExclusaEspec);

        }

    }
}
