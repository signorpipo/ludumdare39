
public enum F_Direction
{
    RIGHT,
    LEFT,
    UP,
    DOWN
}
public static class F_DirectionExtensions
{
    public static F_Direction Turn(F_Direction i_From, F_Direction i_To)
    {
        switch (i_To)
        {
            case F_Direction.RIGHT:
                switch (i_From)
                {
                    case F_Direction.RIGHT:
                        return F_Direction.DOWN;
                    case F_Direction.LEFT:
                        return F_Direction.UP;
                    case F_Direction.UP:
                        return F_Direction.RIGHT;
                    case F_Direction.DOWN:
                        return F_Direction.LEFT;
                }
                break;

            case F_Direction.LEFT:
                switch (i_From)
                {
                    case F_Direction.RIGHT:
                        return F_Direction.UP;
                    case F_Direction.LEFT:
                        return F_Direction.DOWN;
                    case F_Direction.UP:
                        return F_Direction.LEFT;
                    case F_Direction.DOWN:
                        return F_Direction.RIGHT;
                }
                break;
        }

        return F_Direction.UP;

    }
}