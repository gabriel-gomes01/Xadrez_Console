namespace tabuleiro
{
    class Tabuleiro
    {
        //Atributos
        public int linhas { get; set; }
        public int colunas { get; set; }
        private Peca[,] pecas;

        //Construtor
        public Tabuleiro(int linhas, int colunas)
        {
            this.linhas = linhas;
            this.colunas = colunas;
            pecas = new Peca[linhas, colunas];
        }

        //Metodo de peça passando linha e coluna
        public Peca peca(int linha, int coluna)
        {
            return pecas[linha, coluna];
        }

        //Sobreincrição do metodo, passando posição(Linha e coluna)...Mesma coisa,  só que mais organizado
        public Peca peca(Posicao pos)
        {
            return pecas[pos.linha, pos.coluna];
        }

        //Metodo coloca peça em uma posição. Caso tenha uma peça ele envia mensagem 
        public void ColocarPeca(Peca p, Posicao pos)
        {
            if (existePeca(pos))
            {
                throw new TabuleiroException("Já existe uma peça nessa posição");
            }
            pecas[pos.linha, pos.coluna] = p;
            p.posicao = pos;
        }

        //Metodo de retirar peça, transforma a peça na posição nula
        public Peca retirarPeca(Posicao pos)
        {
            if(peca(pos) == null)
            {
                return null;
            }
            Peca aux = peca(pos);
            aux.posicao = null;
            pecas[pos.linha, pos.coluna] = null;
            return aux;
        }

        //Metodo que checa se existe peça em uma posição, retornado algo que não seja nulo
        public bool existePeca(Posicao pos)
        {
            validarPosicao(pos);
            return peca(pos) != null;
        }

        /*Metodo que checa se a posição é valida, posição não pode ser menor que 0
         * ou maior que o numero que o programador(eu ou você que pegou esse código) determinou*/
        public bool posicaoValida(Posicao pos)
        {
            if(pos.linha<0 || pos.linha>=linhas || pos.coluna<0 || pos.coluna >= colunas)
            {
                return false;
            }

            return true;
        }

        //Metodo que valida possição, se não existir manda um aviso
        public void validarPosicao(Posicao pos)
        {
            if (!posicaoValida(pos))
            {
                throw new TabuleiroException("Posição invalida");
            }
        }
    }
}