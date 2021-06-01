using System;
namespace tabuleiro

    /* Clase que cria uma exceção personalizada.
    Herda da classe Exception (é a classe master de exceção) */
{
    class TabuleiroException: Exception
    {
        public TabuleiroException(string msg) : base(msg)
        {

        }
    }
}