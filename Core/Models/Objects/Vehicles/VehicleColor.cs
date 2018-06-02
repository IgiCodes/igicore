using System.ComponentModel.DataAnnotations.Schema;

namespace IgiCore.Core.Models.Objects.Vehicles
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
