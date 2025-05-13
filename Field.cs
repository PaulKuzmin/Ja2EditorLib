// ReSharper disable InconsistentNaming
namespace Ja2EditorLib;

public interface Field
{
    string get(byte[] p0);
    int getInt(byte[] p0);
    void set(byte[] p0, string p1);
    void setInt(byte[] p0, int p1);
}