using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA.Simulation
{
    class Pieces:IEnumerable<Piece>
    {
        private readonly Dictionary<PieceType, Piece> allowedPieces;

        public Pieces()
        {
            allowedPieces = new Dictionary<PieceType, Piece>();
            GeneratePieces();
        }

        public Piece this[PieceType type]
        {
            get
            {
                return allowedPieces[type];
            }
        }

        public IEnumerator<Piece> GetEnumerator()
        {
            return allowedPieces.Select(ap => ap.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void GeneratePieces()
        {
            allowedPieces.Add(PieceType.S, new Piece(PieceType.S, new Point(1, 2), new Point(2, 1), new Point(2, 2), new Point(3, 1)));
            allowedPieces.Add(PieceType.Z, new Piece(PieceType.Z, new Point(1, 1), new Point(2, 1), new Point(2, 2), new Point(3, 2)));
            allowedPieces.Add(PieceType.O, new Piece(PieceType.O, new Point(1, 1), new Point(1, 2), new Point(2, 1), new Point(2, 2)));
            allowedPieces.Add(PieceType.I, new Piece(PieceType.I, new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1)));
            allowedPieces.Add(PieceType.L, new Piece(PieceType.L, new Point(1, 1), new Point(1, 2), new Point(2, 1), new Point(3, 1)));
            allowedPieces.Add(PieceType.J, new Piece(PieceType.J, new Point(1, 1), new Point(1, 2), new Point(1, 3), new Point(2, 1)));
            allowedPieces.Add(PieceType.T, new Piece(PieceType.T, new Point(1, 1), new Point(2, 1), new Point(2, 2), new Point(3, 1)));
        }
    }
}
