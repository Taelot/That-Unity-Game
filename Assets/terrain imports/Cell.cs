public class Cell
{
    public bool isWater;
    public bool isMud;
    public bool hasObject;
    public bool isSand;

    public Cell(bool isWater, bool isMud, bool hasObject, bool isSand)
    {
        this.isWater = isWater;
        this.isMud = isMud;
        this.hasObject = hasObject;
        this.isSand = isSand;
    }
}