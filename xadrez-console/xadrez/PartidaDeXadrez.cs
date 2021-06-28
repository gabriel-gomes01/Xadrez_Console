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
        public bool xeque { get; private set; }

        //Construtor
        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            xeque = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        /*O nome do método é auto-descritivo. 
         * Ele retira peça do local, adiciona +1 no contador de jogadas
         * captura a peça (se tiver peça lá) e cria a mesma peça que retirou, só que em outro local*/
        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQtdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.ColocarPeca(p, destino);
            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQtdMovimentos();
           if(pecaCapturada != null)
            {
                tab.ColocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.ColocarPeca(p, origem);
        }

        //Metodo que aciona o executarMovimento, só que conta +1 no turno e muda jogador
        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em Xeque!");
            }

            if (estaEmXeque(adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (testeXequemate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                mudaJogador();
            }
            
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

        private Cor adversaria(Cor cor)
        {
            if(cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }
        private Peca rei(Cor cor) 
        {
            foreach(Peca x in pecasEmJogo(cor))
            {
                if(x is Rei)
                {
                    return x;
                }                
            }
            return null;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca R = new Rei(tab, cor);
           
            if(R == null)
            {
                throw new TabuleiroException ("Não tem rei da cor " + cor + " no tabuleiro");
            }

            foreach(Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if(mat[R.posicao.linha, R.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool testeXequemate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }

            foreach(Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for(int i =0; i<tab.linhas; i++)
                {
                    for(int j=0; j<tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
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

            colocarNovaPeca('d', 8, new Rei(tab, Cor.Preta));
            colocarNovaPeca('c', 7, new Torre(tab, Cor.Preta));
            colocarNovaPeca('c', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('d', 7, new Torre(tab, Cor.Preta));
            colocarNovaPeca('e', 7, new Torre(tab, Cor.Preta));
            colocarNovaPeca('e', 8, new Torre(tab, Cor.Preta));
            

        }
    }
}