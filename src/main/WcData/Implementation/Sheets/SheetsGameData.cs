using System;
using System.Collections.Generic;
using System.Text;
using WcCore.Domain;
using WcData.Sheets;

namespace WcData.Implementation.Sheets
{
    /// <summary>
    /// An object that encapsulate all of the various data sheets in one injectable class
    /// </summary>
    public class SheetsGameData : IGameData
    {
        private readonly IOfferData offerData;
        private readonly IUnitData unitData;

        public SheetsGameData(IOfferData offerData, IUnitData unitData)
        {
            this.offerData = offerData;
            this.unitData = unitData;
        }

        public IEnumerable<Unit> Units
        {
            get { return unitData.Units; }
        }
    }
}
