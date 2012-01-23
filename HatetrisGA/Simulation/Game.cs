using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HatetrisGA.Simulation
{
    class Game
    {
        private Replay replay;
        private Well liveWell;
        private Piece livePiece;
        private static readonly Pieces pieces = new Pieces();
        private Dictionary<PieceType, List<Orientation>> orientations;
        private int searchDepth;

        public int Score
        {
            get
            {
                return liveWell.Score;
            }
        }

        public Game()
        {
            CreatePlayingField();
        } 

        void ClearField()
        {
            liveWell = new Well();
            livePiece = WorstPiece(liveWell);
        }

        public int FilledSpaces()
        {
            return liveWell.GetFilledSpaces();
        }

        public int Start(Replay replay)
        {
            this.replay = replay;

            ClearField();
            InputReplayStep();

            return Score;
        }

        void InputReplayStep()
        {
            var transformId = replay.Shift();

            var gameContinues = InputHandler(transformId);
            if (gameContinues)
            {
                if (replay.Count() > 0)
                {
                    InputReplayStep();
                }
            }
        }

        bool InputHandler(Transform transformId)
        {
            var newPiece = TryTransform(liveWell, livePiece, transformId);
            if (newPiece == null)
            {
                if (transformId == Transform.Down)
                {
                    AddPiece(liveWell, livePiece);
                    livePiece = null;
                }
            }
            else
            {
                livePiece = newPiece;
            }

            if (liveWell.Content[liveWell.Bar - 1] > 0)
            {
                return false;
            }

            if (livePiece == null)
            {
                livePiece = WorstPiece(liveWell);
            }

            return true;
        }

        void CreatePlayingField()
        {
            orientations = new Dictionary<PieceType, List<Orientation>>();
            foreach (var piece in pieces)
            {
                var bits = piece.Bits;
                var i = piece.ID;

                orientations[i] = new List<Orientation>();
                for (int o = 0; o < 4; o++)
                {
                    orientations[i].Add(new Orientation());
                }

                foreach (var j in bits)
                {
                    var bit = new Point(j.X, j.Y);
                    for (int o = 0; o < 4; o++)
                    {
                        orientations[i][o].Rows[bit.Y] += 1 << bit.X;

                        // update extens
                        if (orientations[i][o].XMin == null || bit.X < orientations[i][o].XMin) // original is  XMin == null, not 0
                        {
                            orientations[i][o].XMin = bit.X;
                        }

                        if (orientations[i][o].YMin == null || bit.Y < orientations[i][o].YMin)
                        {
                            orientations[i][o].YMin = bit.Y;
                        }

                        // starts as xMax but we recalculate later
                        if (orientations[i][o].XDim == null || bit.X > orientations[i][o].XDim)
                        {
                            orientations[i][o].XDim = bit.X;
                        }

                        // starts as yMax but we recalculate later
                        if (orientations[i][o].YDim == null || bit.Y > orientations[i][o].YDim)
                        {
                            orientations[i][o].YDim = bit.Y;
                        }

                        // rotate this bit around the middle of the 4x4 grid
                        bit = new Point(3 - bit.Y, bit.X);
                    }
                }

                for (int o = 0; o < 4; o++)
                {
                    // turn Maxes into Dims
                    orientations[i][o].XDim = orientations[i][o].XDim - orientations[i][o].XMin + 1;
                    orientations[i][o].YDim = orientations[i][o].YDim - orientations[i][o].YMin + 1;

                    // reduce that list of rows to the minimum
                    // truncate top rows
                    while (orientations[i][o].Rows[0] == 0)
                    {
                        orientations[i][o].Rows.RemoveAt(0); // shift
                    }

                    // truncate bottom rows
                    while (orientations[i][o].Rows[orientations[i][o].Rows.Count - 1] == 0)
                    {
                        orientations[i][o].Rows.RemoveAt(orientations[i][o].Rows.Count - 1); // pop
                    }

                    for (int row = 0; row < orientations[i][o].YDim; row++)
                    {
                        orientations[i][o].Rows[row] >>= orientations[i][o].XMin;
                    }
                }
            }
        }

        Piece WorstPiece(Well thisWell)
        {
            int? worstRating = null;
            PieceType? worstId = null;

            foreach (var piece in pieces)
            {
                var currentRating = BestWellRating(thisWell, piece.ID, searchDepth);

                if (worstRating == null || currentRating < worstRating)
                {
                    worstRating = currentRating;
                    worstId = piece.ID;
                }

                if (worstRating.Value == 0)
                {
                    break;
                }
            }

            return new Piece(worstId.Value, new Point((int)Math.Floor(((double)(thisWell.Width - 4) / 2)),0),0);
        }

        int BestWellRating(Well thisWell, PieceType pieceId, int thisSearchDepth)
        {
            var thisPiece = new Piece(pieceId, new Point(0, 0), 0);
            int? bestRating = null;

            while (thisPiece.Position.Y + 4 < thisWell.Depth // piece is above the bottom
                && thisWell.Content[thisPiece.Position.Y + 4] == 0 // nothing immediately below it
                )
            {
                thisPiece = TryTransform(thisWell, thisPiece, Transform.Down);
            }

            var piecePositions = new List<Piece>();
            piecePositions.Add(thisPiece);

            var ints = new Dictionary<int, int>();
            ints[Hashcode(thisPiece.Position, thisPiece.O)] = 1;

            var i = 0;
            while (i < piecePositions.Count)
            {
                thisPiece = piecePositions[i];

                // apply all possible transforms
                foreach (Transform j in Enum.GetValues(typeof(Transform)))
                {
                    var newPiece = TryTransform(thisWell, thisPiece, j);

                    if (newPiece == null)
                    {
                        // piece locked? better add that to the list
                        // do NOT check locations, they aren't significant here
                        if (j == Transform.Down)
                        {
                            var newWell = new Well(thisWell.Score, thisWell.HighestBlue);
                            for (int row2 = 0; row2 < thisWell.Depth; row2++)
                            {
                                newWell.Content[row2] = thisWell.Content[row2];
                            }

                            AddPiece(newWell, thisPiece);

                            var currentRating = newWell.HighestBlue + (
                                                                          thisSearchDepth == 0 ? 
                                                                          0
                                                                              : 
                                                                              WorstPieceRating(newWell, thisSearchDepth - 1)/ 100
                                                                      );
                            if (bestRating == null || currentRating > bestRating)
                            {
                                bestRating = currentRating;
                            }
                        }
                    }
                    else // transform succeeded?
                    {
                        var newHashcode = Hashcode(newPiece.Position, newPiece.O);
                        if (!ints.ContainsKey(newHashcode))
                        {
                            piecePositions.Add(newPiece);
                            ints[newHashcode] = 1;
                        }
                    }
                }
                i++;
            }

            return bestRating.Value;
        }

        int WorstPieceRating(Well thisWell, int thisSearchDepth)
        {
            int? worstRating = null;

            foreach(var piece in pieces)
            {
                var currentRating = BestWellRating(thisWell, piece.ID, thisSearchDepth);
                if (worstRating == null || currentRating < worstRating)
                {
                    worstRating = currentRating;
                }

                if (worstRating.Value == 0)
                {
                    return 0;
                }
            }

            return worstRating.Value;
        }

        void AddPiece(Well thisWell, Piece thisPiece)
        {
            var orientation = orientations[thisPiece.ID][thisPiece.O];

            // this is the top left point in the bounding box of this orientation of this piece
            var xActual = thisPiece.Position.X + orientation.XMin;
            var yActual = thisPiece.Position.Y + orientation.YMin;

            // update the "highestBlue" value to account for newly-placed piece
            thisWell.HighestBlue = Math.Min(thisWell.HighestBlue, yActual.Value);

            // row by row bitwise line alteration
            // because we do this from the top down, we can remove lines as we go
            for (int row = 0; row < orientation.YDim; row++)
            {
                // can't negative bit-shift, but alas X can be negative
                thisWell.Content[yActual.Value + row] |= (orientation.Rows[row] << xActual.Value);

                // check for a complete line now
                // NOTE: completed lines don't count if you've lost
                if (yActual >= thisWell.Bar
                    && thisWell.Content[yActual.Value+row] == (1 << thisWell.Width) - 1
                    )
                {
                    // move all lines above this point down
                    for (int k = yActual.Value + row; k > 1; k--)
                    {
                        thisWell.Content[k] = thisWell.Content[k - 1];
                    }

                    // insert a new blank line at the top
                    // though of course the top line will always be blank anyway
                    thisWell.Content[0] = 0;

                    thisWell.Score++;
                    thisWell.HighestBlue++;
                }
            }

        }

        int Hashcode(Point location, int orientation)
        {
            return 4*((liveWell.Depth + 3)*location.X + location.Y) + orientation;
        }

        Piece TryTransform(Well thisWell, Piece thisPiece, Transform transformId)
        {
            var id = thisPiece.ID;
            var x = thisPiece.Position.X;
            var y = thisPiece.Position.Y;
            var o = thisPiece.O;

            // apply transform (very fast now)
            switch (transformId)
            {
                case Transform.Left: x--; break;
                case Transform.Right: x++; break;
                case Transform.Down: y++; break;
                case Transform.Up: o = (o + 1) % 4; break;
            }

            var orientation = orientations[id][o];
            var xActual = x + orientation.XMin;
            var yActual = y + orientation.YMin;

            if (xActual < 0 // make sure not off left side
                || xActual + orientation.XDim > thisWell.Width // make sure not off right side
                || yActual + orientation.YDim > thisWell.Depth // make sure not off bottom
                )
            {
                return null;
            }

            // make sure there is NOTHING IN THE WAY
            // we do this by hunting for bit collisions
            for (int row = 0; row < orientation.Rows.Count; row++) // 0 to 0, 1, 2 or 3 depending on vertical size of piece
            {
                if((thisWell.Content[yActual.Value+row] & (orientation.Rows[row] << xActual)) != 0) // [altered it]
                {
                    return null;
                }
            }

            return new Piece(id,new Point(x,y),o);
        }
    }
}
