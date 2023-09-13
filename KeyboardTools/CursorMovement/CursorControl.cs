namespace KeyboardTools.CursorMovement;

public abstract class CursorControl
{
    public static void MoveCursor(Direction direction, int speed)
    {
        var yMod = 0;
        var xMod = 0;

        var currentCursorPosition = CursorHook.GetCursorPosition();

        switch (direction)
        {
            case Direction.Right:
                xMod = speed;
                break;
            case Direction.Left:
                xMod = (speed / -1);
                break;
            case Direction.Down:
                yMod = speed;
                break;
            case Direction.Up:
                yMod = (speed / -1);
                break;
        }

        //TODO: Interpolation
        CursorHook.SetCursorPos(currentCursorPosition.x + xMod, currentCursorPosition.y + yMod);
        
    }
}

public enum Direction
{
    Right, 
    Left,
    Up,
    Down
}