namespace Loita.UI.UIElements
{
    internal interface IScrollbarBase
    {
        public abstract float WheelValue { get; set; }
        public abstract float WheelValueMult { get; set; }
    }
}