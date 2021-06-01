namespace tabuleiro
{
    class Posicao
    {
        //Atributos
        public int linha { get; set; }
        public int coluna { get; set; }

        //Construtor
        public Posicao(int linha, int coluna) {

            this.linha = linha;
            this.coluna = coluna;
        }

        //Metodo que passa valor da linha e coluna para outras classes
        public void definirValores(int linha, int coluna)
        {
            this.linha = linha;
            this.coluna = coluna;
        }

        //Sobreinscrição Da classe, passando linha e coluna
        public override string ToString()
        {
            return linha
                + ", "
                + coluna;
        }
    }
}