using System;
namespace tabuleiro
{
    abstract class Peca
    {
        //Atributos
        public Posicao posicao { get; set; }
        public Cor cor { get; protected set; }
        public int qtdMovimentos { get; protected set; }
        public Tabuleiro tab { get; protected set; }

        //Construtor
        public Peca(Tabuleiro tab, Cor cor) {

           this.posicao = null;
           this.tab = tab;
           this.cor = cor;
           this.qtdMovimentos = 0;    
        }

        //Contador de Movimentos
        public void incrementarQtdMovimentos()
        {
            qtdMovimentos++;
        }

        //TODO: 
        public bool existemovimentosPossiveis()
        {
            bool[,] mat = movimentosPossiveis();
            for(int i=0; i<tab.linhas; i++)
            {
                for(int j =0; j<tab.colunas; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //TODO
        public bool podeMoverPara(Posicao pos)
        {
            return movimentosPossiveis()[pos.linha, pos.coluna];
        }

        //Metodo abstrato para Movimentos Possiveis em cada peça que herdar
        public abstract bool[,] movimentosPossiveis();
    }
}