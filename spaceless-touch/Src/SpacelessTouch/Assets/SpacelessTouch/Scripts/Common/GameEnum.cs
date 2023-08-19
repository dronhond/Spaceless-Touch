namespace SpacelessTouch.Scripts.Common
{
    public enum BoxType
    {
        普通,
        沉重,
        传送,
        闪烁,
        互吸,
        互斥,
        重力
    }

    public enum BoxColorType
    {
        红色,
        黄色,
        白色,
        蓝色
    }
    
    public enum ElementType
    {
        None,
        Ground, //Only for standing
        Touchable, //Touchable
    }
}