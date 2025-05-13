// ReSharper disable InconsistentNaming
namespace Ja2EditorLib;

public interface Structure
{
    void chain(Structure p0);
    string get(string p0);
    int getInt(string p0);
    void set(string p0, string p1);
    void setInt(string p0, int p1);
}