using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace localizaEDestroi_CLI.Pesquisa
{
    class clsPesquisa
    {
        readonly CriaArquivosConfiguracao dirRaiz = new CriaArquivosConfiguracao();

        //Metodo para pesquisa por extensao de arquivos
        public Tuple<List<string>, string, long> PesquisaPorExtensao()
        {
            List<string> listaPorExtensao = new List<string>(); //List contendo o resultado da pesquisa por extensão
            long tamArqExt, qtdArqExt, tamArqExt_verificador;
            string tamTotArqExt;
            
            string[] arquivosEncontrados;  //Lista de diretorios pela pesquisa por extensão
            string[] listaPesquisaExt = File.ReadAllLines(LocalArqListExExtensao()); //Efetua a leitura do arquivo externo com os parametros da pesquisa
            string[] diretoriosIgnorados = File.ReadAllLines(LocalArqListDiretoriosIgnorados());
            
            string extensaoArq = "";
            string diretoriosIg = "";

            tamTotArqExt = "";
            tamArqExt = qtdArqExt = tamArqExt_verificador = 0;

            int contadorLinhas = listaPesquisaExt.Count();

            //Verifica se o arquivo esta vazio
            if (contadorLinhas == 0)
            {
                return Tuple.Create(listaPorExtensao, tamTotArqExt, qtdArqExt);
            }

            foreach (var dir in diretoriosIgnorados)
            {
                diretoriosIg += dir;
            }

            foreach (string item in listaPesquisaExt)
            {
                extensaoArq += item;
            }

            string[] listaDeArqPorExtensao = extensaoArq.Split(';');
            string[] diretoriosIgnoradosSplit = diretoriosIg.Split(';'); //Quebra o texto para obter os parametros de pesquisa por extensao

            foreach (var item in listaDeArqPorExtensao)
            {
                //'arquivosEncontrados' recebe o caminho absoluto de cada arquivo encontrado a cada iteração do foreach                    
                arquivosEncontrados = Directory.GetFiles(dirRaiz.DiretorioRaiz(), "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith("." + item.ToString(), StringComparison.CurrentCulture)).ToArray();

                foreach (var file in arquivosEncontrados)
                {
                    string[] arqEncontradosSplit = Convert.ToString(file).Split('\\');     
                    bool verificador = false; //Usado para verificar se o caminho possui algum dos parametros do filtro

                    foreach (var ch in arqEncontradosSplit)
                    {
                        foreach (var dirIgnorado in diretoriosIgnoradosSplit)
                        {
                            //Filtro usado para ignorar determinadas pastas na pesquisa
                            if (ch == dirIgnorado)
                            {
                                verificador = true;
                                break;
                            }
                        }

                        if (verificador)
                        {
                            break;
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

            foreach (var arqExtTam in listaPorExtensao)
            {
                tamArqExt_verificador = new FileInfo(arqExtTam.ToString()).Length;
                tamArqExt += tamArqExt_verificador;
            }

            FormataTamanhoExibicaoArquivo format = new FormataTamanhoExibicaoArquivo();
            
            qtdArqExt = listaPorExtensao.Count; //Obtem a quantidade de arquivos por extensao encontrados na pesquisa
            tamTotArqExt = format.Formata(tamArqExt); //Obtem o tamanho total dos arquivos encontrados na pesquisa

            //Apos o termino da pesquisa, a lista de pesquisa atual e adicionada na lista central para gerar as informacoes no arquivo de log
            return Tuple.Create(listaPorExtensao, tamTotArqExt, qtdArqExt);

        }

        //Responsavel por calcular o tamanho dos diretorios encontrados na pesquisa
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

        //Diretorio do arquivo de lista de exclusoes por extensao completo mais o nome do arquivo
        public string LocalArqListExExtensao()
        {
            string localArq = dirRaiz.LocalArqListExExtensao();
            return localArq;
        }

        //Diretorio do arquivo de lista de exclusoes por extensao completo mais o nome do arquivo
        public string LocalArqListDiretoriosIgnorados()
        {
            string localArq = dirRaiz.LocalArqListDiretoriosIgnorados();
            return localArq;
        }

        //Metodo para pesquisa por diretorio de arquivos
        public Tuple<List<string>, string, long> PesquisaPorDiretorio()
        {
            List<string> listaPorDiretorio = new List<string>();     //List contendo o resultado da pesquisa por nome
            List<string> tempAllfiles2 = new List<string>(); //Armazena partes de um caminho para iteracao de arquivos
            
            string[] diretoriosIgnorados = File.ReadAllLines(LocalArqListDiretoriosIgnorados());
            string[] allfiles2; //Lista de diretorios pela pesquisa por nome            
            
            string DirArq = "";
            string tamTotArqDir = "";
            string diretoriosIg = "";
            
            DirectoryInfo tamDir_verificador;
            long tamArqDir_somador, qtdDir;

            qtdDir = tamArqDir_somador = 0;

            string[] linhas = File.ReadAllLines(dirRaiz.LocalArqListExDiretorio());

            int contadorLinhas = linhas.Count();

            if (contadorLinhas == 0)
            {
                return Tuple.Create(listaPorDiretorio, tamTotArqDir, qtdDir);
            }

            foreach (var dir in diretoriosIgnorados)
            {
                diretoriosIg += dir;
            }

            foreach (string item in linhas)
            {
                DirArq += item;
            }

            string[] listaDeArqPorExtensao = DirArq.Split(';');
            string[] diretoriosIgnoradosSplit = diretoriosIg.Split(';'); //Lista diretorios ignorados

            foreach (var item2 in listaDeArqPorExtensao)
            {
                //allfiles2 recebe o caminho absoluto de cada diretorio encontrado a cada iteração do foreach
                allfiles2 = Directory.GetDirectories(dirRaiz.DiretorioRaiz(), item2.ToString(), SearchOption.AllDirectories);

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
                string[] dirEncontradosSplit = Convert.ToString(file2).Split('\\');
                bool verificador = false;

                foreach (var dirIgnorado in diretoriosIgnoradosSplit)
                {
                    foreach (var ch in dirEncontradosSplit)
                    {
                        if (ch == dirIgnorado)
                        {
                            verificador = true;
                        }
                    }

                    if (verificador)
                    {
                        break;
                    }

                }

                if (!verificador)
                {
                    listaPorDiretorio.Add(Convert.ToString(file2)); //listaPorNome recebe a variavel com o caminho absoluto
                    Console.WriteLine(file2);
                }

            }
            
            foreach (var arqDirTam in listaPorDiretorio)
            {
                tamDir_verificador = new DirectoryInfo(arqDirTam);
                tamArqDir_somador = TamanhoTotalDiretorio(tamDir_verificador, true);
            }

            FormataTamanhoExibicaoArquivo format = new FormataTamanhoExibicaoArquivo();

            qtdDir = listaPorDiretorio.Count;
            tamTotArqDir = format.Formata(tamArqDir_somador);

            return Tuple.Create(listaPorDiretorio, tamTotArqDir, qtdDir);

        }

    }
}
