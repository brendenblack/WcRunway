using System;
using System.Collections.Generic;
using System.Text;

namespace WcCore.Domain.Battles
{
    public class PveBase
    {
        private PveBase() { }

        public PveBase(int sector, int xCoordinate, int yCoordinate, string type, int level)
        {
            this.Sector = sector;
            this.Type = type;
            this.Level = level;
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
        }


        public int Id
        {
            get
            {
                return 0;
            }
        }

        public string Type { get; private set; }

        public int Level { get; private set; }

        public int Sector { get; private set; }

        public int XCoordinate { get; private set; }

        public int YCoordinate { get; private set; }

        public static IRequireSector OfType(string type)
        {
            var builder = new PveBaseBuilder();
            builder.Type = type;
            return builder;
        }

        public interface IRequireSector
        {
            IRequireCoordinates InSector(int sector);
        }

        public interface IRequireCoordinates
        {
            IRequireLevel AtCoordinates(int x, int y);
        }



        public interface IRequireLevel
        {
            IBuildable AtLevel(int level);
        }

        public interface IBuildable
        {
            PveBase Build();
        }


        public class PveBaseBuilder : IRequireSector, IRequireCoordinates, IRequireLevel, IBuildable
        {
            public int Sector { get; internal set; }
            public int X { get; internal set; }
            public int Y { get; internal set; }
            public string Type { get; internal set; }
            public int Level { get; internal set; }

            public IRequireLevel AtCoordinates(int x, int y)
            {
                this.X = x;
                this.Y = y;
                return this;
            }

            public PveBase Build()
            {
                return new PveBase(Sector, X, Y, Type, 1);
            }

            public IRequireLevel OfBaseType(string type)
            {
                this.Type = type;
                return this;
            }

            public IBuildable AtLevel(int level)
            {
                this.Level = level;
                return this;
            }

            public IRequireCoordinates InSector(int sector)
            {
                this.Sector = sector;
                return this;
            }
        }
    }
}
