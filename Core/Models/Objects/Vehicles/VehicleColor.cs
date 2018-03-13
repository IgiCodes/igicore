namespace IgiCore.Core.Models.Objects.Vehicles
{
    public class VehicleColor
    {
        private Color? customColor;
        public Color? CustomColor
        {
            get => this.customColor;
            set
            {
                this.customColor = value;
                this.Color = null;
            }
        }

        private VehicleStockColor? color;
        public VehicleStockColor? Color
        {
            get => this.color;
            set
            {
                this.color = value;
                this.CustomColor = null;
            }
        }

        public bool IsCustom => this.Color == null;

        public static implicit operator Color? (VehicleColor c)
        {
            return c.CustomColor;
        }

        public static implicit operator VehicleStockColor? (VehicleColor c)
        {
            return c.Color;
        }
    }
}