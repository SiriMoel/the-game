using Godot;
public static partial class Utils {
    public partial class SignalWrapper<T> : RefCounted where T: class
    {
        public T Value;
        public SignalWrapper(T value) => Value = value;
    }

    public static SignalWrapper<T> WrapClass<T>(T cls) where T: class{
        return new SignalWrapper<T>(cls);
    }

    public static T UnwrapClass<T>(SignalWrapper<T> a) where T: class {
        return a.Value;
    }
}
