using System.Collections.Generic;

namespace Ahorcado
{
    public class MotorViborita : IMotorJuego
    {
        public int Ancho { get; } = 20;
        public int Alto { get; } = 15;

        private readonly LinkedList<(int x, int y)> _cuerpo =
            new LinkedList<(int x, int y)>();

        private (int x, int y) _direccion = (1, 0);
        private (int x, int y) _comida;
        private bool _perdido = false;

        public int Puntos { get; private set; } = 0;

        public IEnumerable<(int x, int y)> Cuerpo => _cuerpo;

        public (int x, int y) Comida => _comida;

        public MotorViborita()
        {
            _cuerpo.AddFirst((Ancho / 2, Alto / 2));
            _cuerpo.AddFirst((Ancho / 2 + 1, Alto / 2));
            _cuerpo.AddFirst((Ancho / 2 + 2, Alto / 2));

            GenerarComida();
        }

        public void CambiarDireccion(ConsoleKey tecla)
        {
            switch (tecla)
            {
                case ConsoleKey.UpArrow:
                    if (_direccion.y != 1)
                        _direccion = (0, -1);
                    break;

                case ConsoleKey.DownArrow:
                    if (_direccion.y != -1)
                        _direccion = (0, 1);
                    break;

                case ConsoleKey.LeftArrow:
                    if (_direccion.x != 1)
                        _direccion = (-1, 0);
                    break;

                case ConsoleKey.RightArrow:
                    if (_direccion.x != -1)
                        _direccion = (1, 0);
                    break;
            }
        }

        public void Avanzar()
        {
            if (_perdido)
                return;

            var cabeza = _cuerpo.First!.Value;

            var nueva = (
                x: cabeza.x + _direccion.x,
                y: cabeza.y + _direccion.y
            );

            if (nueva.x < 0 || nueva.x >= Ancho || nueva.y < 0 || nueva.y >= Alto)
            {
                _perdido = true;
                return;
            }

            if (_cuerpo.Contains(nueva))
            {
                _perdido = true;
                return;
            }

            _cuerpo.AddFirst(nueva);

            if (nueva == _comida)
            {
                Puntos++;
                GenerarComida();
            }
            else
            {
                _cuerpo.RemoveLast();
            }
        }

        private void GenerarComida()
        {
            Random random = new Random();

            do
            {
                _comida = (random.Next(Ancho), random.Next(Alto));
            }
            while (_cuerpo.Contains(_comida));
        }

        public bool Ganado()
        {
            return Puntos >= 10;
        }

        public bool Perdido()
        {
            return _perdido;
        }
    }
}