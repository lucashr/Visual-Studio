using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localizaEDestroi_CLI.Pesquisa
{
    class CriaArquivosConfiguracao
    {
        //Cria diretorio e arquivos com parametros de pesquisa padrao
        public void CriacaoArqParametrosPesquisa()
        {
            if (!File.Exists(DirParametrosDePesquisa())) //Se o diretorio de configuracao nao existir, entao ele sera criado
            {
                Directory.CreateDirectory(DirParametrosDePesquisa());
            }

            if (!File.Exists(LocalArqListExEspecificas())) //Verifica se o arquivo lista de exclusoes especificas existe
            {
                File.Create(LocalArqListExEspecificas()).Close();
            }

            if (!File.Exists(LocalArqListExDiretorio())) //Verifica se o arquivo lista de exclusoes por diretorio existe
            {
                File.Create(LocalArqListExDiretorio()).Close();
            }

            if (!File.Exists(LocalArqListExExtensao())) //Verifica se o arquivo lista de exclusoes por extensao existe
            {
                File.Create(LocalArqListExExtensao()).Close();
            }

        }

        //Caminho do diretorio do parametro de pesquisa completo mais o nome da pasta
        public string DirParametrosDePesquisa()
        {
            string pastaArqDeConf = "Parametros pesquisa localizaEDestroi";
            string DirLog = DiretorioRaiz() + "\\" + pastaArqDeConf;
            return DirLog;
        }

        //Diretorio do arquivo de lista de exclusoes especificas completo mais o nome do arquivo
        public string LocalArqListExEspecificas()
        {
            string nomeArq = "lista de exclusoes especificas.txt";
            string localArq = DirParametrosDePesquisa() + "\\" + nomeArq;
            return localArq;
        }

        //Diretorio do arquivo de lista de exclusoes por diretorio completo mais o nome do arquivo
        public string LocalArqListExDiretorio()
        {
            string nomeArq = "lista de exclusoes por diretorio.txt";
            string localArq = DirParametrosDePesquisa() + "\\" + nomeArq;
            return localArq;
        }

        //Diretorio do arquivo de lista de exclusoes por extensao completo mais o nome do arquivo
        public string LocalArqListExExtensao()
        {
            string nomeArq = "lista de exclusoes por extensao.txt";
            string localArq = DirParametrosDePesquisa() + "\\" + nomeArq;
            return localArq;
        }

        //Obtem o diretorio de execucao do programa
        public string DiretorioRaiz()
        {
            string diretorio = Environment.CurrentDirectory;
            return diretorio;
        }
    }
}
