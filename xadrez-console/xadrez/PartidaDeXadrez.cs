using System;
using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        //Atributos
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual;
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        //Construtor
        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        /*O nome do método é auto-descritivo. 
         * Ele retira peça do local, adiciona +1 no contador de jogadas
         * captura a peça (se tiver peça lá) e cria a mesma peça que retirou, só que em outro local*/
        public void executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQtdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.ColocarPeca(p, destino);
            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
        }

        //Metodo que aciona o executarMovimento, só que conta +1 no turno e muda jogador
        public void realizaJogada(Posicao origem, Posicao destino)
        {
            executaMovimento(origem, destino);
            turno++;
            mudaJogador();
        }

        /*Policial do programa.
         *Metodo responsavel por enviar avisos caso o jogador faça coisa errada*/
        public void validarPosicaoOrigem(Posicao pos)
        {
            //Aviso de escolher posição sem peça
            if(tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de Origem escolhida");
            }
            /*Aviso de escolha de peça que não é sua. */
            if(jogadorAtual != tab.peca(pos).cor)
            {
                throw new TabuleiroException("A peça de Origem escolhida não é sua");
            }
            //Caso a peça escolhida esteja "Trancada".
            if (!tab.peca(pos).existemovimentosPossiveis())
            {
                throw new TabuleiroException("Não ha movimentos possiveis para a peça escolhida");
            }
        }

        //Caso a peça escolhida tente ir a uma posição que não pode
        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).podeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        //Metodo que muda o jogador, checando se a cor dele é atual ou não
        private void mudaJogador()
        {
            if(jogadorAtual == Cor.Branca)
            {
                jogadorAtual = Cor.Preta;
            }
            else
            {
                jogadorAtual = Cor.Branca;
            }
        }

        //TODO Colocar descrição
        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach(Peca x in capturadas){
                if(x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        //TODO Colocar descrição
        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }
        //TODO Colocar descrição
        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.ColocarPeca(peca, new Posicaoxadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        //Metodo que coloca as peças no tabuleiro
        private void colocarPecas()
        {
           colocarNovaPeca('c', 1, new Torre(tab, Cor.Branca));
           colocarNovaPeca('c', 2, new Torre(tab, Cor.Branca));
           colocarNovaPeca('d', 2, new Torre(tab, Cor.Branca));
           colocarNovaPeca('e', 2, new Torre(tab, Cor.Branca));
           colocarNovaPeca('e', 1, new Torre(tab, Cor.Branca));
           colocarNovaPeca('d', 1, new Rei(tab, Cor.Branca));

            colocarNovaPeca('c', 7, new Torre(tab, Cor.Preta));
            colocarNovaPeca('c', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('d', 7, new Torre(tab, Cor.Preta));
            colocarNovaPeca('e', 7, new Torre(tab, Cor.Preta));
            colocarNovaPeca('e', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('d', 8, new Rei(tab, Cor.Preta));

        }
    }
}