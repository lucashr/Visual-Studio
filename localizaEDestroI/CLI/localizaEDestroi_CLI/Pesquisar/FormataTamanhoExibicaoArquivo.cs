namespace localizaEDestroi_CLI.Pesquisa
{
    public class FormataTamanhoExibicaoArquivo
    {

        // O formato padrão é "0.### XB", Ex: "4.2 KB" ou "1.434 GB"
        public string Formata(long i)
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
    }
}
