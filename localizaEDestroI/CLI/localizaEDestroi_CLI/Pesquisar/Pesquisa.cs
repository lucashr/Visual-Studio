using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace localizaEDestroi_CLI.Pesquisa
{
    class Pesquisa
    {
        readonly CriaArquivosConfiguracao dirRaiz = new CriaArquivosConfiguracao();

        //Metodo para pesquisa por extensao de arquivos
        public Tuple<List<string>, string, long> PesquisaPorExtensao()
        {
            List<string> listaPorExtensao = new List<string>(); //List contendo o resultado da pesquisa por extensão
            long tamArqExt, qtdArqExt, tamArqExt_verificador;
            string tamTotArqExt;
            
            string[] allfiles;  //Lista de diretorios pela pesquisa por extensão
            string[] linhas = File.ReadAllLines(LocalArqListExExtensao()); //Efetua a leitura do arquivo externo com os parametros da pesquisa

            string extensaoArq = "";

            tamTotArqExt = "";
            tamArqExt = qtdArqExt = tamArqExt_verificador = 0;

            int contadorLinhas = linhas.Count();

            //Verifica se o arquivo esta vazio
            if (contadorLinhas == 0)
            {
                return Tuple.Create(listaPorExtensao, tamTotArqExt, qtdArqExt);
            }

            foreach (string item in linhas)
            {
                extensaoArq += item;
            }

            string[] listaDeArqPorExtensao = extensaoArq.Split(';'); //Quebra o texto para obter os parametros de pesquisa por extensao

            foreach (var item in listaDeArqPorExtensao)
            {
                //allfiles recebe o caminho absoluto de cada arquivo encontrado a cada iteração do foreach                    
                allfiles = Directory.GetFiles(dirRaiz.DiretorioRaiz(), "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith("." + item.ToString(), StringComparison.CurrentCulture)).ToArray();

                foreach (var file in allfiles)
                {
                    string[] teste = Convert.ToString(file).Split('\\');
                    bool verificador = false; //Usado para verificar se o caminho possui algum dos parametros do filtro

                    foreach (var ch in teste)
                    {
                        //Filtro usado para ignorar determinadas pastas na pesquisa
                        if (ch == "target" || ch == ".jazz5" || ch == ".git" || ch == ".metadata" || ch == "Parametros pesquisa localizaEDestroi" || ch == "LogDeletados")
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

        //Metodo para pesquisa por diretorio de arquivos
        public Tuple<List<string>, string, long> PesquisaPorDiretorio()
        {
            List<string> listaPorDiretorio = new List<string>();     //List contendo o resultado da pesquisa por nome
            List<string> tempAllfiles2 = new List<string>(); //Armazena partes de um caminho para iteracao de arquivos
            string[] allfiles2; //Lista de diretorios pela pesquisa por nome            
            string DirArq = "";
            string tamTotArqDir = "";
            DirectoryInfo tamDir_verificador;
            long tamArqDir_somador, qtdDir;

            qtdDir = tamArqDir_somador = 0;

            string[] linhas = File.ReadAllLines(dirRaiz.LocalArqListExDiretorio());

            int contadorLinhas = linhas.Count();

            if (contadorLinhas == 0)
            {
                return Tuple.Create(listaPorDiretorio, tamTotArqDir, qtdDir);
            }

            foreach (string item in linhas)
            {
                DirArq += item;
            }

            string[] listaDeArqPorExtensao = DirArq.Split(';');
            // 

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
                string[] teste = Convert.ToString(file2).Split('\\');
                bool verificador = false;

                foreach (var ch in teste)
                {
                    if (ch == "target" || ch == ".jazz5" || ch == ".git" || ch == ".metadata" || ch == "Parametros pesquisa localizaEDestroi" || ch == "LogDeletados")
                    {
                        verificador = true;
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
