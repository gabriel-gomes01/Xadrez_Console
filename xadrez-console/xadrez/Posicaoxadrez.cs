using System;
using tabuleiro;
using System.Text;

namespace xadrez
{
    class Posicaoxadrez
    {
        //Atributos
        public char coluna { get; set; }
        public int linha{ get; set; }

        //Construtor
        public Posicaoxadrez(char coluna, int linha)
        {
            this.coluna = coluna;
            this.linha = linha;
        }

        //Metodo que transforma Linha e Coluna com a numeração do Xadrez
        public Posicao toPosicao()
        {
            return new Posicao(8 - linha, coluna - 'a');
        }

        //Sobreinscrição enviando linha e coluna
        public override string ToString()
        {
            return "" + coluna + linha;
        }
    }
}