using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class StringExtensions
{
    public static string ToBase64(this string source)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(source);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string Right(this string value, int numeroCaracteres)
    {
        return value.Substring(value.Length - numeroCaracteres, numeroCaracteres);
    }

    public static string GenerateRandomCode(this string s, int lenght = 5)
    {
        Random r = new Random();

        for (int j = 0; j < lenght; j++)
        {
            int i = r.Next(3);
            int ch;
            switch (i)
            {
                case 1:
                    ch = r.Next(0, 9);
                    s = s + ch.ToString();
                    break;
                case 2:
                    ch = r.Next(65, 90);
                    s = s + Convert.ToChar(ch).ToString();
                    break;
                case 3:
                    ch = r.Next(97, 122);
                    s = s + Convert.ToChar(ch).ToString();
                    break;
                default:
                    ch = r.Next(97, 122);
                    s = s + Convert.ToChar(ch).ToString();
                    break;
            }
            r.NextDouble();
            r.Next(100, 1999);
        }
        return s;
    }

    public static string RemoveEspacamentoDuploEntreNomes(this string nomePessoa)
    {
        if (nomePessoa.IsNotSent())
            return nomePessoa;

        var arrayDeNomes = nomePessoa.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        var nomeAjustado = String.Join(" ", arrayDeNomes);

        return nomeAjustado;
    }

    public static string FormataCNPJ(this string cnpj)
    {
        try
        {
            return string.Format("{0}.{1}.{2}/{3}-{4}", cnpj.Substring(0, 2), cnpj.Substring(2, 3), cnpj.Substring(5, 3), cnpj.Substring(8, 4), cnpj.Substring(12, 2));
        }
        catch
        {
            return cnpj;
        }
    }
}
