using IgiCore.SDK.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Roleplay.Vehicles.Core.Models
{
    public class VehicleColor
    {
        public VehicleStockColor StockColor { get; set; }
        public Color CustomColor { get; set; } = new Color();
		public bool IsCustom { get; set; } = false;
		[NotMapped]
	    public bool IsStock => !this.IsCustom;
    }
}
