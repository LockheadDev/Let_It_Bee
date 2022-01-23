using UnityEngine;
public static class FlowerEnum
{
    public static Color GetColor(FlowerColor clr)
    {
        switch (clr)
        {
            case FlowerColor.red:
                return Color.red;

            case FlowerColor.green:
                return Color.green;

            case FlowerColor.blue:
                return Color.blue;
            default:
                break;
        }
        return Color.black;
    }
}

public enum FlowerColor
{
    red, green,blue
};
