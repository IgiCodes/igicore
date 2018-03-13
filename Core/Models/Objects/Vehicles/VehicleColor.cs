using System.Drawing;

namespace IgiCore.Core.Models.Objects.Vehicles
{
    public class VehicleColor
    {
        private Color? _customColor;
        public Color? CustomColor
        {
            get => this._customColor;
            set
            {
                this._customColor = value;
                Color = null;
            }
        }

        private VehicleStockColor? _color;
        public VehicleStockColor? Color
        {
            get => this._color;
            set
            {
                this._color = value;
                CustomColor = null;
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