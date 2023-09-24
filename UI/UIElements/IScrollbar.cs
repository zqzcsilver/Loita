namespace Loita.UI.UIElements
{
    internal interface IScrollbar
    {
        public abstract float WheelValue { get; set; }
        public abstract float WheelValueMult { get; set; }
    }
}