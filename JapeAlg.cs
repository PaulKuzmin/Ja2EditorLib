namespace Ja2EditorLib;

public class JapeAlg
{
    public static byte[] Encode(byte[] plaintext, int textLength, int[] table)
    {
        byte[] ciphertext = new byte[textLength];
        byte lastchar = 0;
        int offset = 0;
        for (int i = 0; i < textLength; ++i)
        {
            byte salt = (byte)table[offset];
            ciphertext[i] = (byte)(plaintext[i] + lastchar + salt);
            offset = (offset + 1) % 49;
            lastchar = ciphertext[i];
        }

        return ciphertext;
    }

    public static byte[] Decode(byte[] ciphertext, int textLength, int[] table)
    {
        byte[] plaintext = new byte[textLength];
        byte lastchar = 0;
        int offset = 0;
        for (int i = 0; i < textLength; ++i)
        {
            byte salt = (byte)table[offset];
            plaintext[i] = (byte)(ciphertext[i] - lastchar - salt);
            offset = (offset + 1) % 49;
            lastchar = ciphertext[i];
        }

        return plaintext;
    }

    private static int sx(byte b)
    {
        return (sbyte)b; // знак сохраняем
    }

    private static int u(byte b)
    {
        return b & 0xFF;
    }

    private static int word_ptr(byte[] data, int offset)
    {
        byte b0 = data[offset];
        byte b2 = data[offset + 1];
        return ((b0 & 0xFF) | (b2 & 0xFF) << 8) & 0xFFFF;
    }

    public static int ActorChecksum(byte[] data)
    {
        int eax = sx(data[297]);
        int edx = sx(data[334]);
        ++eax;
        edx += 2;
        eax *= edx;
        edx = sx(data[405]);
        int edi = 416;
        eax = eax + edx + 1;
        edx = sx(data[335]);
        ++edx;
        eax *= edx;
        edx = sx(data[296]);
        eax = eax + edx + 1;
        edx = sx(data[353]);
        ++edx;
        eax *= edx;
        edx = sx(data[261]);
        eax = eax + edx + 1;
        edx = sx(data[411]);
        ++edx;
        eax *= edx;
        edx = sx(data[339]);
        int esi = eax + edx + 1;
        eax = sx(data[352]);
        ++eax;
        esi *= eax;
        edx = 0;
        do
        {
            int ebx = 0;
            eax = 0;
            ebx = word_ptr(data, edi);
            eax = u(data[377 + edx]);
            ebx += esi;
            ++edx;
            edi += 2;
            esi = ebx + eax;
        } while (edx < 19);

        eax = esi;
        return eax;
    }

    public static int MercChecksum(byte[] data)
    {
        int esi = 19;
        int eax = sx(data[917]);
        int edx = sx(data[868]);
        ++eax;
        edx += 2;
        eax *= edx;
        edx = sx(data[880]);
        eax = eax + edx + 1;
        edx = sx(data[840]);
        ++edx;
        eax *= edx;
        edx = sx(data[886]);
        eax = eax + edx + 1;
        edx = sx(data[1377]);
        ++edx;
        eax *= edx;
        edx = sx(data[1372]);
        eax = eax + edx + 1;
        edx = sx(data[916]);
        ++edx;
        eax *= edx;
        edx = sx(data[1378]);
        eax = eax + edx + 1;
        edx = sx(data[849]);
        ++edx;
        eax *= edx;
        edx = u(data[1825]);
        int offset = 12;
        eax = eax + edx + 1;
        do
        {
            int edi = 0;
            edi = word_ptr(data, offset);
            edx = u(data[offset + 2]);
            edi += eax;
            offset += 36;
            --esi;
            eax = edi + edx;
        } while (esi != 0);

        return eax;
    }
}